using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class RoundCardManager
{
    private int _discardTotal = 0;

    private int _roundAttackCount = 0;
    private int _roundDefendCount = 0;
    private int _roundMissionCount = 0;
    private int _roundSceneCount = 0;
    private int _roundSpecialCount = 0;

    public RoundCardManager()
    {

    }

    public int XIAOCardCount
    {
        get
        {
            return _roundAttackCount + _roundDefendCount + _roundMissionCount + _roundSceneCount + _roundSpecialCount;
        }
    }

    public bool XIAOLimit(Card card, bool showMsg = true)
    {
        if (card.identity == Card.IdentityType.Common)
        {
            if (card.cardType == Card.CardType.Attack)
            {
                if (_roundAttackCount > 1)
                {
                    if (showMsg)
                    {
                        GameManager.Instance.DisplayOnBoard($"一回合最多只能使用一张攻击牌");
                    }
                    return false;
                }
            }
            else if (card.cardType == Card.CardType.Defend)
            {
                if (_roundDefendCount > 1)
                {
                    if (showMsg)
                    {
                        GameManager.Instance.DisplayOnBoard($"一回合最多只能使用一张防御牌");
                    }
                    return false;
                }
            }
            else if (card.cardType == Card.CardType.Mission)
            {
                if (_roundMissionCount > 2)
                {
                    if (showMsg)
                    {
                        GameManager.Instance.DisplayOnBoard($"一回合最多只能使用两张任务牌");
                    }
                    return false;
                }
            }
            else if (card.cardType == Card.CardType.Scene)
            {
            }
        }

        return true;
    }

    public bool XIAOCard(Card card)
    {
        if (!XIAOLimit(card, false))
        {
            return false;
        }

        if (card.identity == Card.IdentityType.Common)
        {
            if (card.cardType == Card.CardType.Attack)
            {
                _roundAttackCount++;
            }
            else if (card.cardType == Card.CardType.Defend)
            {
                _roundDefendCount++;
            }
            else if (card.cardType == Card.CardType.Mission)
            {
                _roundMissionCount++;
            }
            else if (card.cardType == Card.CardType.Scene)
            {
                _roundSceneCount++;
            }
        }
        else
        {
            _roundSpecialCount++;
        }

        return true;
    }

    public void Reset()
    {
        _discardTotal = 0;

        _roundAttackCount = 0;
        _roundDefendCount = 0;
        _roundMissionCount = 0;
        _roundSceneCount = 0;
        _roundSpecialCount = 0;
    }
}
