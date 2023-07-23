using Amazon.Lambda.Core;
using Amazon.Lambda.S3Events;
using Amazon.S3;
using Amazon.S3.Model;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace ThumbnailConverter;

public class Function
{
    IAmazonS3 S3Client { get; set; }
    public Function()
    {
        S3Client = new AmazonS3Client();
    }
    public Function(IAmazonS3 s3Client)
    {
        this.S3Client = s3Client;
    }
    public async Task FunctionHandler(S3Event @event, ILambdaContext context)
    {
        var eventRecords = @event.Records ?? new List<S3Event.S3EventNotificationRecord>();
        var thumbnailFolder = "thumbnails/";
        foreach (var record in eventRecords)
        {
            var s3Event = record.S3;
            if (s3Event == null)
            {
                continue;
            }
            try
            {
                var bucketName = s3Event.Bucket.Name;
                var key = s3Event.Object.Key;

                var response = await this.S3Client.GetObjectAsync(bucketName, key);
                context.Logger.LogLine($"Original Size of {key}: {response.ContentLength}");
                using (var image = Image.Load(response.ResponseStream))
                {
                    int maxWidth = 500;
                    int maxHeight = 500;

                    image.Mutate(x => x.Resize(new ResizeOptions
                    {
                        Mode = ResizeMode.Max,
                        Size = new Size(maxWidth, maxHeight)
                    }));
                    using (MemoryStream stream = new MemoryStream())
                    {
                        image.Save(stream, new SixLabors.ImageSharp.Formats.Jpeg.JpegEncoder());
                        context.Logger.LogLine($"Thumbnail Size of {key}: {stream.Length}");
                        var thumbnailKey = thumbnailFolder + key.Replace("images/", "");
                        var uploadRequest = new PutObjectRequest
                        {
                            BucketName = bucketName,
                            Key = thumbnailKey,
                            InputStream = stream
                        };
                        await this.S3Client.PutObjectAsync(uploadRequest);
                        context.Logger.LogLine($"Uploaded Thumbnail to {thumbnailKey}");
                    }
                    //delete original image
                    await this.S3Client.DeleteObjectAsync(bucketName, s3Event.Object.Key);
                    context.Logger.LogLine($"Deleted Original Image from {s3Event.Object.Key}");
                }
            }
            catch (Exception e)
            {
                context.Logger.LogError(e.Message);
                context.Logger.LogError(e.StackTrace);
                throw;
            }
        }
    }
}