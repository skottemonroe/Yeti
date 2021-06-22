using System.IO;
using System.Configuration;
using Amazon.S3;
using Amazon.S3.Transfer;

namespace Yeti {
    class UploadController {

        public string PrepareToCopy(string path) {
            bool okay = false;
            string s3FileName = Path.GetFileName(path);
            string s3DirectoryName = "";

            using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read)) {
                okay = SendFileToS3(fileStream, ConfigurationManager.AppSettings.Get("BucketName"), s3DirectoryName, s3FileName);
            }

            if (okay) {
                return "https://s3.us-east-2.amazonaws.com/house.of.cheeze/" + s3FileName.Replace(" ", "+");
            } else return "!! FAILED TO UPLOAD CORRECTLY !!";

        }


        static public bool SendFileToS3(
                Stream dataStream,
                string bucketName,
                string subDirectoryInBucket,
                string fileNameInS3) {

            IAmazonS3 client = new AmazonS3Client(
                ConfigurationManager.AppSettings.Get("AccessKeyId"),
                ConfigurationManager.AppSettings.Get("SecretAccessKey"),
                Amazon.RegionEndpoint.USEast2);

            TransferUtility utility = new TransferUtility(client);

            TransferUtilityUploadRequest request = new TransferUtilityUploadRequest();

            //We are currently only allowing things to go in a single root.
            //No subfolders.
            //But this stuff would make it so we COULD add subfolders.
            if (subDirectoryInBucket == "" || subDirectoryInBucket == null) {
                request.BucketName = bucketName;
            } else {
                request.BucketName = bucketName + @"/" + subDirectoryInBucket;
            }

            //fFile name, up in S3
            request.Key = fileNameInS3;

            //The local fFile has been turned into a stream.
            request.InputStream = dataStream;

            //S3 gives out a lot of warnings about this.
            //If we were making an app fFor someone else, this might be a security risk.
            //But the whole point of this app is to make somewhere we can quickly toss things, in the cloud.
            //The whole point is to be globally public.
            request.CannedACL = "public-read";

            //DO IT!
            utility.Upload(request);

            //indicate that the file was sent
            return true;
        }

    }
}
