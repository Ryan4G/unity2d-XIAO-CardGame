using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class PlayerDeck
{
    private int _id = 0;

    private Card _identity = null;

    private List<Card> _specialCards = null;
    private int _specialCardsRemain = 0;

    public PlayerDeck(Card id, int playerCount)
    {
        _identity = id;

        _specialCards = new List<Card>();

        _specialCardsRemain = playerCount;
    }

    public void PickCard()
    {

    }

    public void XIAOCard()
    {

    }

    public void Discard()
    {

    }

    public int GetMINERemain()
    {
        return _specialCardsRemain;
    }
}
