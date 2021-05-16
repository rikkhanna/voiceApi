using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace voiceApi.BaseClasses
{
    public class ConvertToWavFile
    {
        public class Input
        {
            public string type { get; set; }
            public string source { get; set; }
        }
        public class Options
        {
            public string channels { get; set; }
            public int frequency { get; set; }
            public int audio_bitdepth { get; set; }
        }

        public class ConvertWAVKeys
        {
            public string target { get; set; }
            public Options options { get; set; }
        }

        public class ConvertWAV
        {
            public List<Input> input { get; set; }
            public List<ConvertWAVKeys> conversion { get; set; }
        }
    }
    

}
