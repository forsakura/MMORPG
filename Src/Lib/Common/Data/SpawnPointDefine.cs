﻿using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Data
{
    public class SpawnPointDefine
    {
        public int ID {  get; set; }
        public int MapID { get; set; }
        public NVector3 Position { get; set; }
        public NVector3 Direction { get; set; }
    }
}
