using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redflag.Slave.Virtual.Lib.Protocol.HJ212.Model
{
    public class SetSampleTimeIntervalModel
    {
        public string PolId { get; set; }

        public string CstartTime { get; set; }

        public int? CTime { get; set; }
    }
}
