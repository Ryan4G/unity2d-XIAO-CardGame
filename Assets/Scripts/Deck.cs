
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Deck
{
    private static List<Card> _cardDeck = new List<Card>();

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
                effect02 = e1
            });
        }
    }

    public static int GetDeckCount(Card.IdentityType id = Card.IdentityType.Common, Card.CardType ct = Card.CardType.None)
    {
        if (ct != Card.CardType.None)
        {
            return _cardDeck.Where(t => t.identity == id && t.cardType == ct).Count();
        }
        else
        {
            return _cardDeck.Where(t => t.identity == id).Count();
        }
    }

    public static Card GetRandomCard(Card.IdentityType id = Card.IdentityType.Common, Card.CardType ct = Card.CardType.None)
    {
        var idCards = new List<Card>();
        
        if (ct != Card.CardType.None)
        {
            idCards = _cardDeck.Where(t => t.identity == id && t.cardType == ct).ToList();
        }
        else
        {
            idCards = _cardDeck.Where(t => t.identity == id).ToList();
        }

        if (idCards.Count < 1)
        {
            return null;
        }

        var random = Random.Range(0, idCards.Count - 1);

        //Debug.Log(random);

        var theCard = idCards[random];

        if (theCard != null)
        {
            _cardDeck.Remove(theCard);
        }

        return theCard;
    }

    public static void EmptyDeck()
    {
        _cardDeck.Clear();
    }
}
