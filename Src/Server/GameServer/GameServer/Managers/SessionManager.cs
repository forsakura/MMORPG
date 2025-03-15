using Common;
using Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Managers
{
    public class SessionManager : Singleton<SessionManager>
    {
        public Dictionary<int, NetConnection<NetSession>> Sessions = new Dictionary<int, NetConnection<NetSession>>();

        public void AddSession(int id, NetConnection<NetSession> session)
        {
            Sessions[id] = session;
        }

        public void RemoveSession(int chaID)
        {
            if(Sessions.ContainsKey(chaID))
                Sessions.Remove(chaID);
        }
        public NetConnection<NetSession> GetSession(int toId)
        {
            this.Sessions.TryGetValue(toId, out NetConnection<NetSession> session);
            return session;
        }
    }
}
