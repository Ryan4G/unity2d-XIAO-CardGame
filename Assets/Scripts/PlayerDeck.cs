using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class PlayerDeck
{
    private List<Card> _commonCards = null;
    private List<Card> _specialCards = null;
    private int _specialCardsRemain = 0;

    public Card Identity
    {
        private set;

        get;
    }

    public PlayerDeck(Card id, int playerCount)
    {
        Identity = id;

        _commonCards = new List<Card>();

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
