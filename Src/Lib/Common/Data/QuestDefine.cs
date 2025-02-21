using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Data
{
    public enum QuestType
    {
        MAIN,
        BRANCH
    }
    public enum TargetType
    {
        None,
        Kill,
        Item
    }
    public class QuestDefine
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int LimitLevel { get; set; }
        public int LimitClass { get; set; }
        public int PreQuset { get; set; }
        public int PostQuest { get; set; }
        public QuestType Type { get; set; }
        public int AceptNPC { get; set; }
        public int SubmitNPC { get; set; }
        public string Overview { get; set; }
        public string Dialog {  get; set; }
        public string DialogDeny { get; set; }
        public string DialogIncomplete { get; set; }
        public string DialogFinish { get; set; }
        public TargetType Target1 { get; set; }
        public int Target1ID { get; set; }
        public int Target1Num { get; set; }
        public TargetType Target2 { get; set; }
        public int Target2ID { get; set; }
        public int Target2Num { get; set; }
        public TargetType Target3 { get; set; }
        public int Target3ID { get; set; }
        public int Target3Num { get; set; }
        public int RewardGold { get; set; }
        public int RewardExp { get; set; }
        public int RewardItem1 { get; set; }
        public int RewardItem1Count { get; set; }
        public int RewardItem2 { get;set; }
        public int RewardItem2Count { get;set; }
        public int RewardItem3 { get;set; }
        public int RewardItem3Count { get; set; }
    }
}
