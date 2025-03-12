using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Data
{
    public class SpawnRuleDefine
    {
        public int ID {  get; set; }
        public string MapID { get; set; }
        public int SpawnMonID { get; set; }
        public int SpawnLevel { get; set; }
        public int SpawnPeriod { get; set; }
        public int SpawnPoint { get; set; }
    }
}
