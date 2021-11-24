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

    public IdentityType identity { get; set; }
    public CardType cardType { get; set; }
    public string cardTypeDesc { get; set; }
    public string title { get; set; }
    public string effect01 { get; set; }
    public string effect02 { get; set; }
}
