using GameServer.Entities;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Models
{
    internal class Guild
    {
        public TGuild Data;
        public Character Leader;
        public List<Character> members = new List<Character>();
        public double timeStamp;
        public int Id { get { return this.Data.Id; } }
        public string Name { get { return this.Data.Name; } }

        public Guild(TGuild guild)
        {
            this.Data = guild;
        }
    }
}
