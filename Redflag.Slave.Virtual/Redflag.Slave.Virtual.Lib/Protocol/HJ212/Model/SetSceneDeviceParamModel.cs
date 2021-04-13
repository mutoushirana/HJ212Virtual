using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redflag.Slave.Virtual.Lib.Protocol.HJ212.Model
{
    public class SetSceneDeviceParamModel
    {
        public string PolId { get; set; }

        public string InfoId { get; set; }

        public object Value { get; set; }
    }
}
