using Amazon.S3;
using Amazon.S3.Model;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions());
builder.Services.AddAWSService<IAmazonS3>();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();


app.MapPost("upload", async (IFormFile file, IAmazonS3 _s3Client) =>
{
    if (!file.ContentType.StartsWith("image/")) return Results.BadRequest("Invalid File Format. We support only Images.");
    var request = new PutObjectRequest()
    {
        BucketName = "cwm-image-storage",
        Key = "images/" + file.FileName,
        InputStream = file.OpenReadStream()
    };
    request.Metadata.Add("Content-Type", file.ContentType);
    await _s3Client.PutObjectAsync(request);
    return Results.Accepted($"Image {file.FileName} uploaded to S3 successfully!");
});

app.UseHttpsRedirection();

app.Run();
