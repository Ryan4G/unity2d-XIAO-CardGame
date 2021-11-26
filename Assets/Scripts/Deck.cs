
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Deck
{
    private static List<Card> _cardDeck = new List<Card>();

    private static int _recycleCount = 0;

    public static void AddCommonCards(int n, string t, string e, string e1 = null, Card.CardType ct = Card.CardType.Attack, string cardDesc = "")
    {
        if (ct == Card.CardType.Scene)
        {
            cardDesc = $"场景牌{cardDesc}";
        }
        else if (ct == Card.CardType.Attack)
        {
            cardDesc = $"攻击牌{cardDesc}";
        }
        else if (ct == Card.CardType.Defend)
        {
            cardDesc = $"防御牌{cardDesc}";
        }
        else if (ct == Card.CardType.Mission)
        {
            cardDesc = $"任务牌{cardDesc}";
        }

        AddCards(n, t, e, e1, Card.IdentityType.Common, ct, cardDesc: cardDesc);
    }

    public static void AddMINECards(int n, string t, string e, string e1 = null, string cardDesc = "")
    {
        AddCards(n, t, e, e1, Card.IdentityType.MINE, cardDesc: cardDesc);
    }

    public static void AddSpecialCards(int n, string t, string e, string e1 = null, string cardDesc = "")
    {
        AddCards(n, t, e, e1, Card.IdentityType.Special, cardDesc: cardDesc);
    }

    public static void AddCards(int n, string t, string e, string e1 = null, Card.IdentityType id = Card.IdentityType.Common, Card.CardType ct = Card.CardType.None, string cardDesc = "")
    {
        for (var i = 0; i < n; i++)
        {
            _cardDeck.Add(new Card
            {
                identity = id,
                cardType = ct,
                cardTypeDesc = cardDesc,
                title = t,
                effect01 = e,
                effect02 = e1,
                used = false,
                dealed = false
            });
        }
    }

    public static int GetDeckCount(Card.IdentityType id = Card.IdentityType.Common, Card.CardType ct = Card.CardType.None)
    {
        if (ct != Card.CardType.None)
        {
            return _cardDeck.Where(t => t.identity == id && t.cardType == ct && !t.used && !t.dealed).Count();
        }
        else
        {
            return _cardDeck.Where(t => t.identity == id && !t.used && !t.dealed).Count();
        }
    }

    public static Card GetRandomCard(Card.IdentityType id = Card.IdentityType.Common, Card.CardType ct = Card.CardType.None)
    {
        var idCards = new List<Card>();
        
        if (ct != Card.CardType.None)
        {
            idCards = _cardDeck.Where(t => t.identity == id && t.cardType == ct && !t.used && !t.dealed).ToList();
        }
        else
        {
            idCards = _cardDeck.Where(t => t.identity == id && !t.used && !t.dealed).ToList();
        }

        if (idCards.Count < 1)
        {
            var msg = "";

            if (id == Card.IdentityType.Common)
            {
                msg = "通用牌堆";
            }
            else if (id == Card.IdentityType.Special)
            {
                msg = "专属牌堆";
            }

            GameManager.Instance.DisplayOnBoard($"{msg}已无可用卡牌，将重新回收卡牌...");

            // 3 times recycle
            if (DeckRecycle(id))
            {
                return GetRandomCard(id, ct);
            }

            return null;
        }

        var random = Random.Range(0, idCards.Count - 1);

        //Debug.Log(random);

        var theCard = idCards[random];

        if (theCard != null)
        {
            theCard.dealed = true;
        }

        return theCard;
    }

    public static void EmptyDeck()
    {
        _cardDeck.Clear();
    }

    public static bool DeckRecycle(Card.IdentityType id = Card.IdentityType.Common)
    {
        var cards = _cardDeck.Where(t => t.identity == id && t.used && t.dealed).ToList();

        foreach(var card in cards)
        {
            card.used = false;
            card.dealed = false;
        }

        _recycleCount++;

        if (_recycleCount > 2)
        {
            _recycleCount = 0;
            return false;
        }

        return true;
    }
}
