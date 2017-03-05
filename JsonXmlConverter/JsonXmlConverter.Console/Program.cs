using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml;

namespace JsonXmlConverter.ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            //for testing convenience and less typing of file paths, this app will look in a pre-set directory
            string localDir = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "TestFiles");
            Console.WriteLine("Make sure your files are saved to: " + localDir);

            //begin control loop to enable repeated trials
            bool quit = false;
            while(quit != true)
            {
                Console.WriteLine();
                Console.Write("Enter the name of the file you wish to convert, or x to quit: ");

                string fileName = Console.ReadLine();
                if(fileName == "x") //then set flag to quit
                {
                    quit = true;
                    continue;
                }

                Console.WriteLine();
                string filePath = Path.Combine(localDir, fileName);
                if(!File.Exists(filePath))
                {
                    Console.WriteLine("File not found.");
                    continue;
                }

                //get and output results
                Console.WriteLine(getConversion(filePath));
            }
        }

        //use the existing webapi to get the conversion
        static string getConversion(string filePath)
        {
            string result = "";

            using (HttpClient client = new HttpClient())
            {
                //build the request content to send to the web api
                using (var content = new MultipartFormDataContent())
                {
                    ByteArrayContent fileContent = new ByteArrayContent(File.ReadAllBytes(filePath));
                    fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") { FileName = Path.GetFileName(filePath) };
                    content.Add(fileContent);

                    var response = client.PostAsync("http://localhost:50745/api/JsonXmlConverter/GetJsonXmlTranslation", content).Result;

                    if(response.StatusCode == HttpStatusCode.OK)
                    {
                        result = response.Content.ReadAsStringAsync().Result;
                    }
                    else
                    {
                        result = "Error " + response.StatusCode.ToString() + " " + response.ReasonPhrase;
                    }
                }
            }

            return result;
        }
    }
}
