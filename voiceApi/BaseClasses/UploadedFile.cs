using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace voiceApi.BaseClasses
{
    public class UploadedFile
    {
        public Id id { get; set; }
        public bool completed { get; set; }
        public List<object> metadata { get; set; }
        public string warning { get; set; }
    }
    public class Id
    {
        public string job { get; set; }
        public string input { get; set; }
    }

}
