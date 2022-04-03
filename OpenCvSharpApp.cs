using System;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using OpenCvSharp;

namespace opencvsharp_app_docker
{
    public class OpenCvSharpApp
    {
        [FunctionName("Grayscale")]
        public static void Run(
            [BlobTrigger("input/{name}", Connection = "")] Stream myBlob,
            string name,
            [Blob("output/{name}", FileAccess.Write)] Stream outBlob,
            ILogger log)
        {
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");


            using (var mat = Mat.FromStream(myBlob, ImreadModes.Color))
            {
                log.LogInformation($"Height: {mat.Height}, Width: {mat.Width}");

                using (var grayMat = mat.CvtColor(ColorConversionCodes.BGR2GRAY))
                {
                    grayMat.WriteToStream(outBlob);

                    log.LogInformation($"Converted.");
                }
            }
        }
    }
}
