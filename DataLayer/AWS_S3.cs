using System;
using System.IO;
using Amazon.S3;
using Amazon.S3.Model;
using avSV = ApplicationVariables.ApplicationVariables.SystemValues;
using dataIDs = ApplicationVariables.ApplicationVariables.DataIDs;
using CsvPath = ApplicationVariables.ApplicationVariables.SystemSettings.CsvPaths;
using mcl = MovieClassLayer.MovieClasses;
using LumenWorks.Framework.IO.Csv;
using System.Collections.Generic;

namespace MovieDataLayer
{
    public class AWS_S3:Shared
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
            string accessKey;
            string secretKey;
            getKeys(out accessKey, out secretKey);
            AmazonS3Client client = new AmazonS3Client(accessKey, secretKey, Amazon.RegionEndpoint.EUWest2);
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

        private void getKeys(out string accessKey, out string secretKey)
        {
            using (CsvReader csv = new CsvReader(new StreamReader(CsvPath.credentialsAWS), true))
            {
                int fieldCount = csv.FieldCount;
                string[] headers = csv.GetFieldHeaders();
                Dictionary<string, int> dict = headerDict(headers);
                csv.ReadNextRecord();
                accessKey = csv[dict[dataIDs.AWS_Keys.accessKey]];
                secretKey = csv[dict[dataIDs.AWS_Keys.secretKey]];
            }
        }
        
    }
}
