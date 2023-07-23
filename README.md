# Triggering AWS Lambda with S3 Events - for .NET Developers!

Imagine this scenario: your .NET web application is thriving, and users are uploading images like never before. In the backend, you are generating thumbnails for each of the uploaded images. But with increasing user activity comes the challenge of handling image processing efficiently, which may end up as a bottleneck in your entire application's design. This is where AWS Lambda steps in as a game-changer. Your application code should allow the users to upload their images to S3 as usual, and not worry about the thumbnail conversion part. This will be asynchronously handled by the Serverless Lambda!

![Triggering AWS Lambda with S3 Events for .NET Developers!](https://codewithmukesh.com/wp-content/uploads/2023/07/Trigger-AWS-Lambda-with-S3-Events-.NET_.png)

Here is my new article, where I built a practical system using the following components.
- ASP.NET Core WebAPI (.NET 8) - To upload an image to a particular S3 Location.
- C# Lambda - That uses ImageSharp to resize the incoming images and upload the thumbnails back to Amazon S3.
- S3 Events - To Trigger the AWS Lambda whenever there is a new object uploaded.

You will asynchronously delegate this task to a Serverless Lambda that scales automatically. By triggering Lambda functions with S3 events, you can effortlessly create image thumbnails, providing a smooth user experience and optimizing storage costs. You get the problem we are trying to solve right?  This is the case for almost every application once there is a spike in traffic. Thumbnail Creation seems to be the ideal candidate scenario to explain the S3 and Lambda integration!

Read - https://codewithmukesh.com/blog/trigger-aws-lambda-with-s3-events-dotnet/
