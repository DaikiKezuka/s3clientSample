using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

using Amazon;
using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;
using Amazon.S3;
using Amazon.S3.Model;

namespace S3ClientSample
{

    // cf:https://docs.aws.amazon.com/ja_jp/AmazonS3/latest/dev/UploadObjSingleOpNET.html
    class Program
    {
        private const string bucketName = "d-kezuka";

        private const string keyName1 = "firstObject";
        private const string keyName2 = "secondObject";

        private const string filePath = @"D:\work\S3ClientSampleFile.txt";

        private static readonly RegionEndpoint bucketRegion = RegionEndpoint.APNortheast1;

        private static IAmazonS3 client;
        public static void Main()
        {
            
            client = new AmazonS3Client(bucketRegion);
            WritingAnObjectAsync().Wait();

        }

        static async Task WritingAnObjectAsync()
        {
            try
            {
                var putRequest1 = new PutObjectRequest
                {
                    BucketName = bucketName,
                    Key = keyName1,
                    ContentBody = "sample text"
                };

                {
                    PutObjectResponse response1 = await client.PutObjectAsync(putRequest1);
                    Console.WriteLine($"Status Code is {response1.HttpStatusCode}.");
                }

                var putRequest2 = new PutObjectRequest
                {
                    BucketName = bucketName,
                    Key = keyName2,
                    FilePath = filePath,
                    ContentType = "text/plain"
                };
                putRequest2.Metadata.Add("x-amz-meta-title", "someTitle");

                PutObjectResponse response2 = await client.PutObjectAsync(putRequest2);
                Console.WriteLine($"Status Code is {response2.HttpStatusCode}.");
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine(
                    "Error encoutered ***. Message:'{0}' when writing an object"
                    , e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(
                    "Unknownr encoutered on server. Message:'{0}' when writing an object"
                    , e.Message);
            }
        }
    }
}