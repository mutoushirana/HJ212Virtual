using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redflag.Slave.Virtual.Lib.Protocol.HJ212.Model
{
    public class DataModel
    {
       
        public double? Rtd { get; set; }

        public double? Min { get; set; }

        public double? Max { get; set; }

        public string Flag { get; set; }

        public double? ZsRtd { get; set; }

        public double? ZsMin { get; set; }

        public double? ZsAvg { get; set; }

        public double? ZsMax { get; set; }

        public string EFlag { get; set; }

        public double? Cou { get; set; }

    }
}
