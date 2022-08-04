using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenFigmaToMTA
{
    internal class Functions
    {
        public static async Task<string> getFigmaAsync(string url, string key)
        {
            //Layout should be like: https://www.figma.com/file/<projectId>/<projectName>?node-id=0%3A1
            var figmaUrlSplit = url.Split('/');
            var projectId = string.Empty;
            bool isNextElement = false;

            foreach (var figma in figmaUrlSplit)
            {
                if(isNextElement)
                {
                    projectId = figma;
                    break;
                }

                if(figma.ToLower().Equals("file"))
                {
                    isNextElement = true;
                    continue;
                }
            }

            using (var httpClient = new HttpClient())
            {
                string figmaApiEndPoint = String.Format("https://api.figma.com/v1/files/{0}", projectId);

                var httpRequest = (HttpWebRequest)WebRequest.Create(figmaApiEndPoint);

                httpRequest.Headers["X-FIGMA-TOKEN"] = key;


                var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                var result = "";
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }

                return result;
            }
        }

        public static async Task<Structs.Root> getFigmaContent(string url, string key)
        {
            //Download the json with all content from the figma window from the figma API
            string web_content = await getFigmaAsync(url, key);

            if (web_content == null)
                return null;

            //Deserialize the downloaded content into a readable struct
            Structs.Root figmaData = JsonConvert.DeserializeObject<Structs.Root>(web_content);

            //Return the struct to proceed with file generation
            return figmaData;
        }
    }
}
