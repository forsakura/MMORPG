using Common;
using GameServer.Entities;
using GameServer.Managers;
using Network;
using SkillBridge.Message;

namespace GameServer.Services
{
    internal class GuildService : Singleton<GuildService>
    {
        public void Init()
        {
            GuildManager.Instance.Init();
        }
        public GuildService()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<GuildCreateRequest>(OnGuildCreate);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<GuildJoinRequest>(OnGuildJoinReq);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<GuildJoinResponse>(OnGuildJoinRes);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<GuildListRequest>(OnGuildList);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<GuildLeaveRequest>(OnGuildLeave);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<GuildSearchRequest>(OnGuildSearch);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<GuildAdminRequest>(OnGuildAdmin);
        }

        private void OnGuildCreate(NetConnection<NetSession> sender, GuildCreateRequest message)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("OnGuildCreate:: GuildName: {0} character: {1} : {2}", message.GuildName, character.Name, character.Id);
            sender.Session.Response.guildCreate = new GuildCreateResponse();
            if(character.Guild != null)
            {
                sender.Session.Response.guildCreate.Result = Result.Failed;
                sender.Session.Response.guildCreate.Errormsg = "已经有公会";
                sender.SendResponse();
                return;
            }
            if (GuildManager.Instance.CheckNameExisted(character.Name))
            {
                sender.Session.Response.guildCreate.Result = Result.Failed;
                sender.Session.Response.guildCreate.Errormsg = "公会名称已存在";
                sender.SendResponse();
                return;
            }
            if(!GuildManager.Instance.CreateGuild(message.GuildName, message.GuildNotice, character))
            {
                sender.Session.Response.guildCreate.Result = Result.Failed;
                sender.Session.Response.guildCreate.Errormsg = "金币不足,创建公会需要5000金币";
                sender.SendResponse();
                return;
            }
            sender.Session.Response.guildCreate.guildInfo = character.Guild.GuildInfo(character);
            sender.Session.Response.guildCreate.Result = Result.Success;
            sender.SendResponse();
        }

        private void OnGuildJoinReq(NetConnection<NetSession> sender, GuildJoinRequest message)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("OnGuildJoinReq:: character: {0} guild: {1}", character.Id, message.Apply.GuildId);
            var guild = GuildManager.Instance.GetGuild(message.Apply.GuildId);
            if (guild == null)
            {
                sender.Session.Response.guildJoinRes = new GuildJoinResponse();
                sender.Session.Response.guildJoinRes.Result = Result.Failed;
                sender.Session.Response.guildJoinRes.Errormsg = "公会不存在";
                sender.SendResponse();
            }
            message.Apply.characterId = character.Id;
            message.Apply.Name = character.Data.Name;
            message.Apply.Class = character.Data.Class;
            message.Apply.Level = character.Data.Level;
            if (guild.JoinApply(message.Apply))
            {
                var leader = SessionManager.Instance.GetSession(guild.Data.LeaderID);
                if (leader != null)
                {
                    leader.Session.Response.guildJoinReq = message;
                    leader.SendResponse();
                }
            }
            else
            {
                sender.Session.Response.guildJoinRes = new GuildJoinResponse();
                sender.Session.Response.guildJoinRes.Result = Result.Failed;
                sender.Session.Response.guildJoinRes.Errormsg = "请勿重复申请";
                sender.SendResponse();
            }
        }

        private void OnGuildJoinRes(NetConnection<NetSession> sender, GuildJoinResponse message)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("OnGuildJoinRes:: Guild: {0} character: {1}", message.Apply.GuildId, character.Id);
            var guild = GuildManager.Instance.GetGuild(message.Apply.GuildId);
            if (message.Result == Result.Success)
            {
                guild.JoinAppove(message.Apply);
            }

            var requester = SessionManager.Instance.GetSession(message.Apply.characterId);
            if (requester != null)
            {
                requester.Session.Character.Guild = guild;
                requester.Session.Response.guildJoinRes = message;
                requester.Session.Response.guildJoinRes.Result = Result.Success;
                requester.Session.Response.guildJoinRes.Errormsg = "加入公会成功";
                requester.SendResponse();
            }
        }

        private void OnGuildList(NetConnection<NetSession> sender, GuildListRequest message)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("OnGuildList: Character{0} {1}", character.Id, character.Name);

            sender.Session.Response.guildList = new GuildListResponse();
            sender.Session.Response.guildList.Guilds.AddRange(GuildManager.Instance.GetGuildInfos());
            sender.Session.Response.guildList.Result = Result.Success;
            sender.SendResponse();
        }

        private void OnGuildLeave(NetConnection<NetSession> sender, GuildLeaveRequest message)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("OnGuildLeave:: guild: {0} character : {1}", character.Guild.Id, character.Id);
            sender.Session.Response.guildLeave = new GuildLeaveResponse();


            if(!character.Guild.Leave(character))
            {
                sender.Session.Response.guildLeave.Result = Result.Failed;
                sender.Session.Response.guildLeave.Errormsg = "由于您是会长，不能直接离开公会，需要转让会长职务";
                sender.SendResponse();
                return;
            }
            sender.Session.Response.guildLeave.Result = Result.Success;
            sender.SendResponse();
        }

        private void OnGuildSearch(NetConnection<NetSession> sender, GuildSearchRequest message)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("OnGuildSearch:: guildId: {0} character: {1}", message.guildId, character.Id);
            var guild = GuildManager.Instance.GetGuild(message.guildId);
            sender.Session.Response.guildSearch = new GuildSearchResponse();
            if(guild == null)
            {
                sender.Session.Response.guildSearch.Result = Result.Failed;
                sender.Session.Response.guildSearch.Errormsg = "公会不存在";
                sender.SendResponse();
                return;
            }
            sender.Session.Response.guildSearch.Result = Result.Success;
            sender.Session.Response.guildSearch.Guilds.Add(guild.GuildInfo(null));
            sender.Session.Response.guildSearch.Errormsg = "None";
            sender.SendResponse();
        }

        private void OnGuildAdmin(NetConnection<NetSession> sender, GuildAdminRequest message)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("OnGuildAdminRequest:: command: {0} targetId: {1}", message.Command, message.Target);
            sender.Session.Response.guildAdmin = new GuildAdminResponse();
            if(character.Guild == null)
            {
                sender.Session.Response.guildAdmin.Result = Result.Failed;
                sender.Session.Response.guildAdmin.Errormsg = "你没公会";
                sender.SendResponse();
                return;
            }

            if(!character.Guild.ExcuteAdmin(message.Command, message.Target, character.Id))
            {
                sender.Session.Response.guildAdmin.Result = Result.Failed;
                sender.Session.Response.guildAdmin.Command = message;
                sender.Session.Response.guildAdmin.Errormsg = "执行失败";
                sender.SendResponse();
                return;
            }

            var target = SessionManager.Instance.GetSession(message.Target);
            if (target != null)
            {
                target.Session.Response.guildAdmin = new GuildAdminResponse();
                target.Session.Response.guildAdmin.Result = Result.Success;
                target.Session.Response.guildAdmin.Command = message;
                target.SendResponse();
            }

            sender.Session.Response.guildAdmin.Result = Result.Success;
            sender.Session.Response.guildAdmin.Command = message;
            sender.SendResponse();
        }
    }
}
