using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using voiceApi.BaseClasses;
using voiceApi.Utilities;

namespace voiceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }
        public class CustomModel3
        {
            public string Url { get; set; }
            public string filename { get; set; }
            public string filepath { get; set; }
        }
        [HttpPost]
        [Route("download")]
        public async Task<string> DownloadFile([FromBody] CustomModel3 convertedFile )
        {

            
            var filename = new FileInfo(convertedFile.filename);
            var client = new HttpClient();
            var response = await client.GetAsync(convertedFile.Url);
            //response.EnsureSuccessStatusCode();
            using (var stream = await response.Content.ReadAsStreamAsync())
            {
                
                var fileInfo = new FileInfo(convertedFile.filepath + filename);
                using (var fileStream = fileInfo.OpenWrite())
                {
                    await stream.CopyToAsync(fileStream);
                }
            }
            return convertedFile.filepath + filename;
        }

        // GET api/values/3
        [HttpGet("getinfo/{jobId}")]
        public async Task<List<Output>> GetJobInfo(string jobId)
        {
            var client = new HttpClient();
            var url = "https://api2.online-convert.com/jobs/" + jobId;
            client.DefaultRequestHeaders.Add("x-oc-api-key", "744be7d281d6b3b6871baef24cf58052");
            var response = await client.GetAsync(url);
            var strResponse = await response.Content.ReadAsStringAsync();
            UploadOutput getJob = JsonConvert.DeserializeObject<UploadOutput>(strResponse);
            return getJob.output;
        }

        // POST api/values
        [HttpPost]
        [Route("server")]
        public async Task<string> PostCreateServer()
        {
            var conv = new Conversion();
            conv.category = "audio";
            conv.target = "wav";
            var rootList = new List<Conversion>();
            rootList.Add(conv);
            var root = new ConversionRoot();
            root.conversion = rootList;

            var client = new HttpClient();
            var json = JsonConvert.SerializeObject(root);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var url = "https://api2.online-convert.com/jobs";

            client.DefaultRequestHeaders.Add("x-oc-api-key", "744be7d281d6b3b6871baef24cf58052");
            var response = await client.PostAsync(url, data);

            var strResponse = await response.Content.ReadAsStringAsync();
            var server = JsonConvert.DeserializeObject<ConversionServer>(strResponse);

            string serverID = server.server + "/upload-file/" + server.id;
            return serverID;
        }

        public class CustomModel
        {
            public string server { get; set; }
            public string filepath { get; set; }
        }

        [HttpPost]
        [Route("upload")]
        public async Task<string> UploadFileServer([FromBody] CustomModel data )
        {
            using (var httpClient = new HttpClient())
            {
                using (var form = new MultipartFormDataContent())
                {
                    using (var fs = System.IO.File.OpenRead(data.filepath))
                    {
                        using (var streamContent = new StreamContent(fs))
                        {
                            using (var fileContent = new ByteArrayContent(await streamContent.ReadAsByteArrayAsync()))
                            {
                                //fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");

                                // "file" parameter name should be the same as the server side input parameter name
                                form.Add(fileContent, "file", Path.GetFileName(data.filepath));
                                httpClient.DefaultRequestHeaders.Add("x-oc-api-key", "744be7d281d6b3b6871baef24cf58052");
                                HttpResponseMessage response = await httpClient.PostAsync(data.server, form);
                                var strResponse = await response.Content.ReadAsStringAsync();
                                var rootId = JsonConvert.DeserializeObject<UploadedFile>(strResponse);
                                return rootId.id.job;

                            }
                        }
                    }
                }
            }
            
        }

        public class CustomModel2
        {
            public string uploadedFileUrl { get; set; }
        }
        [HttpPost]
        [Route("convert")]
        public async Task<string> convertToWAV([FromBody] CustomModel2 fileURL)
        {

            var input = new ConvertToWavFile.Input();
            input.type = "remote";
            input.source = fileURL.uploadedFileUrl;

            var inputList = new List<ConvertToWavFile.Input>();
            inputList.Add(input);

            var options = new ConvertToWavFile.Options();
            options.audio_bitdepth = 16;
            options.frequency = 16000;
            options.channels = "mono";

            var convertWAVKeys = new ConvertToWavFile.ConvertWAVKeys();
            convertWAVKeys.target = "wav";
            convertWAVKeys.options = options;

            var convertWAVKeysList = new List<ConvertToWavFile.ConvertWAVKeys>();
            convertWAVKeysList.Add(convertWAVKeys);

            var convertWAV = new ConvertToWavFile.ConvertWAV();
            convertWAV.input = inputList;
            convertWAV.conversion = convertWAVKeysList;


            var client = new HttpClient();
            var json = JsonConvert.SerializeObject(convertWAV);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var url = "https://api2.online-convert.com/jobs";

            client.DefaultRequestHeaders.Add("x-oc-api-key", "744be7d281d6b3b6871baef24cf58052");
            var response = await client.PostAsync(url, data);

            var strResponse = await response.Content.ReadAsStringAsync();
            var server = JsonConvert.DeserializeObject<IdAfterConvert>(strResponse);
            return server.id;
        }
        class IdAfterConvert
        {
            public string id { get; set; }
        }
        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
