using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerDeck : MonoBehaviour
{
    private List<XIAOCard> _currentHandCards = null;

    private List<Card> _commonCards = null;
    private List<Card> _specialCards = null;
    private int _specialCardsRemain = 0;

    private int _currentXIAOCards = 0;

    private bool _aiHandle = false;

    public bool AIHandle
    {
        get
        {
            return _aiHandle;
        }
    }

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

        _currentHandCards = new List<XIAOCard>();

        _specialCardsRemain = playerCount;
    }

    public void PickCard(int num)
    {
        var startPos = Camera.main.ViewportToWorldPoint(new Vector2(.5f, .2f));
        startPos.z = 0;

        while (num > 0)
        {
            var card = Deck.GetRandomCard();

            if (card != null)
            {
                GameManager.Instance.DisplayOnBoard($"{Identity.title} 抽取了一张卡牌");
                GameManager.Instance.DisplayOnBoard($"卡牌信息：{card.identity} {card.title} {card.effect01} {card.effect02}");

                _commonCards.Add(card);

                var go = GameManager.Instance.CreateXIAOCard(card, startPos, CardSelected, new Vector3(1.5f, 1.5f, 1.0f), false, false);

                _currentHandCards.Add(go);
            }

            num--;
        }

        RefreshHandCards();
    }

    public void XIAOCard(XIAOPlayer xiaoPlayer)
    {
        var selectedCard = _currentHandCards.Where(t => t.CardSelected).ToList();

        if (selectedCard.Count > 0)
        {
            GameManager.Instance.DisplayOnBoard($"不能同时使用两张卡牌");
            return;
        }
        
        if (selectedCard.Count == 0)
        {
            GameManager.Instance.DisplayOnBoard($"没有选中任何卡牌");
            return;
        }

        var xiaoCard = selectedCard.First();

        var card = xiaoCard.Card;

        if (card.identity == Card.IdentityType.Common)
        {
            if (card.cardType == Card.CardType.Attack)
            {

            }
            else if (card.cardType == Card.CardType.Defend)
            {

            }
            else if (card.cardType == Card.CardType.Mission)
            {

            }
            else if (card.cardType == Card.CardType.Scene)
            {

            }

            _commonCards.Remove(card);
        }
        else
        {
            // Use special card
            _specialCards.Remove(card);
        }

        GameManager.Instance.DisplayOnBoard($"{Identity.title} 使用了一张卡牌");
        GameManager.Instance.DisplayOnBoard($"卡牌信息：{card.identity} {card.title} {card.effect01} {card.effect02}");

        _currentXIAOCards++;

        _currentHandCards.Remove(xiaoCard);

        Destroy(xiaoCard.gameObject);

        RefreshHandCards();
    }

    public void Discard()
    {
        var selectedCard = _currentHandCards.Where(t => t.CardSelected).ToList();

        if (selectedCard.Count > 0)
        {
            GameManager.Instance.DisplayOnBoard($"丢弃了{selectedCard.Count}张卡牌...");

            for(var i = 0; i < selectedCard.Count; i++)
            {
                var card = selectedCard[i];

                if (card.Card.identity == Card.IdentityType.Common)
                {
                    _commonCards.Remove(card.Card);
                }
                else if(card.Card.identity == Card.IdentityType.Special)
                {
                    _specialCards.Remove(card.Card);
                }

                _currentHandCards.Remove(card);

                Destroy(card.gameObject);
            }

            RefreshHandCards();
        }
    }

    public int GetMINERemain()
    {
        return _specialCardsRemain;
    }

    public void RoundStarted()
    {
        _currentXIAOCards = 0;

        GameManager.Instance.DisplayOnBoard($"{Identity.title} 的回合开始...");

        GameManager.Instance.DisplayOnBoard($"从普通牌堆自动获取两张卡牌...");

        PickCard(2);
    }

    public void SkipRound()
    {
        if (_currentXIAOCards == 0)
        {
            GameManager.Instance.DisplayOnBoard($"{Identity.title} 本回合未出牌...");

            GameManager.Instance.DisplayOnBoard($"从普通牌堆自动获取一张卡牌...");

            PickCard(1);
        }

        GameManager.Instance.NextPlayer();
    }

    public void SetAIHandle(bool openAI)
    {
        _aiHandle = openAI;

        if (_aiHandle)
        {
            GameManager.Instance.DisplayOnBoard($"{Identity.title} 当前设置为 < AI > 模式");
        }
        else
        {
            GameManager.Instance.DisplayOnBoard($"{Identity.title} 当前设置为 <手动> 模式");
        }
    }

    public void CardSelected(XIAOCard xiaoCard)
    {
        xiaoCard.Choose(!xiaoCard.CardSelected);
    }

    public void RefreshHandCards()
    {
        var startPos = Camera.main.ViewportToWorldPoint(new Vector2(.5f, .2f));
        startPos.z = 0;

        var cardWidth = 2.2f;
        var startX = (_currentHandCards.Count - 1) / 2 * -1 * cardWidth;

        var visible = GameManager.Instance.currentPlayer == this;

        if (visible)
        {
            for (var i = 0; i < _currentHandCards.Count; i++)
            {
                var go = _currentHandCards[i];

                var v3 = startPos;
                v3.x += i * cardWidth + startX;

                go.transform.position = v3;
                go.gameObject.SetActive(visible);
            }
        }
    }

    public void PlayerSelected(XIAOPlayer xiaoPlayer)
    {

    }
}
