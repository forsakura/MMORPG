﻿using System.Threading;

using Network;
using GameServer.Services;
using GameServer.Managers;
namespace GameServer
{
    class GameServer
    {
        NetService network;
        Thread thread;
        bool running = false;
        public bool Init()
        {
            int Port = Properties.Settings.Default.ServerPort;
            network = new NetService();
            network.Init(Port);
            DataManager.Instance.Load();
            UserService.Instance.Init();
            DBService.Instance.Init();
            MapService.Instance.Init();
            ItemService.Instance.Init();
            QuestService.Instance.Init();
            FriendService.Instance.Init();
            TeamService.Instance.Init();
            GuildService.Instance.Init();
            ChatService.Instance.Init();
            thread = new Thread(new ThreadStart(this.Update));

            return true;
        }

        public void Start()
        {
            network.Start();
            running = true;
            thread.Start();
        }


        public void Stop()
        {
            running = false;
            thread.Join();
            network.Stop();
        }

        public void Update()
        {
            var mapManager = MapManager.Instance;
            while (running)
            {
                Time.Tick();
                Thread.Sleep(100);
                //Console.WriteLine("{0} {1} {2} {3} {4}", Time.deltaTime, Time.frameCount, Time.ticks, Time.time, Time.realtimeSinceStartup);
                mapManager.Updata();
            }
        }
    }
}
