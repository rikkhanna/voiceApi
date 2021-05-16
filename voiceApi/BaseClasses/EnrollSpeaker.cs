using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace voiceApi.BaseClasses
{
    public class EnrollSpeaker
    {
        public string enrollFile1 { get; set; }
        public string enrollFile2 { get; set; }
        public string enrollFile3 { get; set; }
    }

    public class VerifySpeaker
    {
        public string file { get; set; }
    }
}
