﻿using System;
using System.IO;
using Amazon.S3;
using Amazon.S3.Model;
using avSS = ApplicationVariables.ApplicationVariables.SystemSettings;
using avSV = ApplicationVariables.ApplicationVariables.SystemValues;
using mcl = MovieClassLayer.MovieClasses;


namespace MovieDataLayer
{
    public class AWS_S3
    {

        public void UpdateAllFromS3()
        {
            downloadLatestFile();
            archiveLatestFile();
            CSVData dl = new CSVData();
            mcl.Films films = dl.GetCsvData();
            SQLData dl2 = new SQLData();
            dl2.UpdateDBFromS3(films);
        }

        private void downloadLatestFile()
        {
            //-- download from S3 bucket
            AmazonS3Client client = new AmazonS3Client(avSS.AWS.AccessKey, avSS.AWS.SecretKey, Amazon.RegionEndpoint.EUWest2);
            GetObjectResponse file = client.GetObject(avSV.S3_Storage.S3Paths.bucket, avSV.S3_Storage.S3Paths.fileKey);
            file.WriteResponseStreamToFile(avSV.S3_Storage.LocalPaths.download);
        }

        private void archiveLatestFile()
        {
            //-- copy from download to CSV folder for use
            File.Copy(avSV.S3_Storage.LocalPaths.download, avSV.S3_Storage.LocalPaths.active, true);

            //-- get date and time, append to file name and copy to Archive
            DateTime now = DateTime.Now;
            File.Copy(avSV.S3_Storage.LocalPaths.download,
                String.Format(avSV.S3_Storage.LocalPaths.archive, now.ToString(avSV.S3_Storage.timeFormat)));

            //-- delete file from Download
            File.Delete(avSV.S3_Storage.LocalPaths.download);
        }
    }
}
