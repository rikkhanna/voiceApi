using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace voiceApi.BaseClasses
{
    public class UploadOutput
    {
        public string id { get; set; }
        public string token { get; set; }
        public string type { get; set; }
        public List<Output> output { get; set; }
    }
    public class Output
    {
        public string id { get; set; }
        public string filename { get; set; }
        public string uri { get; set; }
        public int size { get; set; }
        public string status { get; set; }
        public string content_type { get; set; }
        public int downloads_counter { get; set; }
        public string checksum { get; set; }
        public DateTime created_at { get; set; }
    }
}
