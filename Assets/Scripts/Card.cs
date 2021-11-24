using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card
{
    public enum IdentityType
    {
        Common,
        MINE,
        Special
    }

    public enum CardType
    {
        None,
        Attack,
        Defend,
        Mission,
        Scene
    }

    public IdentityType Identity { get; set; }
    public CardType cardType { get; set; }
    public string cardTypeDesc { get; set; }
    public string Title { get; set; }
    public string Effect0 { get; set; }
    public string Effect1 { get; set; }
}
