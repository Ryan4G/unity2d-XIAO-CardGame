using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card
{
    public enum IdentityType
    {
        /// <summary>
        /// 通用牌
        /// </summary>
        Common,
        /// <summary>
        /// 身份牌
        /// </summary>
        MINE,
        /// <summary>
        /// 特殊牌
        /// </summary>
        Special,
        /// <summary>
        /// 专属技能
        /// </summary>
        Skill,
    }

    public enum CardType
    {
        /// <summary>
        /// 无
        /// </summary>
        None,
        /// <summary>
        /// 攻击牌
        /// </summary>
        Attack,
        /// <summary>
        /// 防御牌
        /// </summary>
        Defend,
        /// <summary>
        /// 任务牌
        /// </summary>
        Mission,
        /// <summary>
        /// 场景牌
        /// </summary>
        Scene
    }

    public enum SceneType
    {
        /// <summary>
        /// 无
        /// </summary>
        None,
        /// <summary>
        /// 游戏场景
        /// </summary>
        Game,
        /// <summary>
        /// 次元场景
        /// </summary>
        Dimension,
        /// <summary>
        /// 聚会场景
        /// </summary>
        Party
    }

    public enum TargetType
    {
        /// <summary>
        /// 无
        /// </summary>
        None,
        /// <summary>
        /// 单人
        /// </summary>
        One,
        /// <summary>
        /// 双人
        /// </summary>
        Two,
        /// <summary>
        /// 所有人
        /// </summary>
        Each,
    }

    /// <summary>
    /// 行为类型
    /// </summary>
    public enum ActionType
    {
        None,
        PickCommon,
        PickSpecial,
        PickSkill,
        Grab,
        Discard,
        ClearScene,
        Stop,
        ForceUse,
    }

    /// <summary>
    /// 卡牌种类
    /// </summary>
    public IdentityType identity { get; set; }
    /// <summary>
    /// 通用牌类型
    /// </summary>
    public CardType cardType { get; set; }
    /// <summary>
    /// 卡牌类型描述
    /// </summary>
    public string cardTypeDesc { get; set; }
    /// <summary>
    /// 卡牌名称
    /// </summary>
    public string title { get; set; }
    /// <summary>
    /// 效果1描述
    /// </summary>
    public string effect01 { get; set; }
    /// <summary>
    /// 效果2描述（身份/技能卡）
    /// </summary>
    public string effect02 { get; set; }
    /// <summary>
    /// 是否已使用（XIAO 或 DISCARD）
    /// </summary>
    public bool used { get; set; }
    /// <summary>
    /// 是否已派发
    /// </summary>
    public bool dealed { get; set; }

    /// <summary>
    /// 获取卡牌场景
    /// </summary>
    /// <returns></returns>
    public SceneType GetSceneType()
    {
        if (cardType == CardType.Scene)
        {
            var sub = effect01.Substring(0, 6);

            if (sub.Contains("游戏"))
            {
                return SceneType.Game;
            }
            else if (sub.Contains("次元"))
            {
                return SceneType.Dimension;
            }
            else if(sub.Contains("聚会"))
            {
                return SceneType.Party;
            }
        }

        return SceneType.None;
    }

    public Dictionary<ActionType, int> GetActions()
    {
        var dic = new Dictionary<ActionType, int>();

        if (identity == IdentityType.Common)
        {
            var count = 0;

            if (effect01.Contains("弃掉"))
            {
                var match = System.Text.RegularExpressions.Regex.Match(effect01, @"\S+(\d+)张\S+");

                if (match.Groups.Count > 1)
                {
                    int.TryParse(match.Groups[1].Value, out count);
                }
                
                if (effect01.Contains("弃掉所有"))
                {
                    count = 99;
                }

                dic.Add(ActionType.Discard, count);
            }

            if (effect01.Contains("抽取"))
            {
                var match = System.Text.RegularExpressions.Regex.Match(effect01, @"\S+(\d+)张\S+");

                if (match.Groups.Count > 1)
                {
                    int.TryParse(match.Groups[1].Value, out count);
                }

                dic.Add(ActionType.PickCommon, count);
            }

            if (effect01.Contains("抓取"))
            {

                var match = System.Text.RegularExpressions.Regex.Match(effect01, @"\S+(\d+)张\S+");

                if (match.Groups.Count > 1)
                {
                    int.TryParse(match.Groups[1].Value, out count);
                }

                dic.Add(ActionType.Grab, count);
            }
        }
        else if (identity == IdentityType.Special)
        {

        }
        else if (identity == IdentityType.Skill)
        {

        }

        return dic;
    }
}
