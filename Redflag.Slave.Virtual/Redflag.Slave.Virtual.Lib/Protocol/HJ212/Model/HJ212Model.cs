using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redflag.Slave.Virtual.Lib.Protocol.HJ212.Model
{
    public class HJ212Model
    {
        public string QN { get; set; }
        public string ST { get; set; }
        public int CN { get; set; }
        public string MN { get; set; }
        public string PW { get; set; }
        public int Flag_V { get; set; }
        public int Flag_D { get; set; }
        public int Flag_A { get; set; }
        public string PNUM { get; set; }
        public string PNO { get; set; }
        public string CP { get; set; }
        public string CRC { get; set; }
        public string Body { get; set; }

        public string Flag { get; set; }
    }
}
