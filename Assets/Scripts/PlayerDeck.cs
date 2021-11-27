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

    private RoundCardManager _roundCardManager = new RoundCardManager();

    public void Init(Card id, int playerCount)
    {
        _commonCards = new List<Card>();

        _specialCards = new List<Card>();

        _currentHandCards = new List<XIAOCard>();

        Identity = id;

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

                var go = GameManager.Instance.CreateXIAOCard(card, startPos, CardSelected, new Vector3(1.4f, 1.4f, 1.0f), false, false);

                _currentHandCards.Add(go);
            }
            else
            {
                GameManager.Instance.DisplayOnBoard($"通用牌堆已无更多可用卡牌回收...");
            }

            num--;
        }

        RefreshHandCards();
    }

    public void XIAOCard(PlayerDeck player)
    {
        var selectedCard = _currentHandCards.Where(t => t.CardSelected).ToList();

        if (selectedCard.Count > 1)
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

        if (!_roundCardManager.XIAOLimit(card))
        {
            return;
        }

        if (card.identity == Card.IdentityType.Common)
        {
            if (card.cardType == Card.CardType.Attack)
            {
                if (player == null)
                {
                    GameManager.Instance.DisplayOnBoard($"请先选择攻击牌的攻击对象");
                    return;
                }

                if (player.Identity.cardTypeDesc == Identity.cardTypeDesc)
                {
                    GameManager.Instance.DisplayOnBoard($"攻击牌的攻击对象不能是出牌者");
                    return;
                }

                // attack card also will be game card
                if (GameManager.Instance.CurrentScene == Card.SceneType.Game)
                {
                    GameManager.Instance.DisplayOnBoard($"当前场景<游戏>，攻击牌同时被视为游戏牌");
                }
            }
            else if (card.cardType == Card.CardType.Defend)
            {
                // defend card also will be dimension card
                if (GameManager.Instance.CurrentScene == Card.SceneType.Dimension)
                {
                    GameManager.Instance.DisplayOnBoard($"当前场景<次元>，攻击牌同时被视为次元牌");
                }
            }
            else if (card.cardType == Card.CardType.Mission)
            {
                // mission card also will be party card
                if (GameManager.Instance.CurrentScene == Card.SceneType.Party)
                {
                    GameManager.Instance.DisplayOnBoard($"当前场景<聚会>，攻击牌同时被视为聚会牌");
                }
            }
            else if (card.cardType == Card.CardType.Scene)
            {
                GameManager.Instance.ChangeScene(card.GetSceneType());
            }

            _commonCards.Remove(card);
        }
        else
        {
            // Use special card
            _specialCards.Remove(card);
        }

        // add card use count
        _roundCardManager.XIAOCard(card);

        // mark the card used
        card.used = true;

        GameManager.Instance.DisplayOnBoard($"{Identity.title} {(player != null ? $"对{player.Identity.title}": "")}使用了一张卡牌");
        GameManager.Instance.DisplayOnBoard($"卡牌信息：{card.identity} {card.title} {card.effect01} {card.effect02}");

        _currentHandCards.Remove(xiaoCard);

        ShowCardAndDestory(xiaoCard);

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

                // mark the card used
                card.Card.used = true;

                if (card.Card.identity == Card.IdentityType.Common)
                {
                    _commonCards.Remove(card.Card);
                }
                else if(card.Card.identity == Card.IdentityType.Special)
                {
                    _specialCards.Remove(card.Card);
                }

                _currentHandCards.Remove(card);

                ShowCardAndDestory(card);
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
        GameManager.Instance.DisplayOnBoard($"{Identity.title} 的回合开始...");

        GameManager.Instance.DisplayOnBoard($"从普通牌堆自动获取两张卡牌...");

        PickCard(2);

        _roundCardManager.Reset();
    }

    public void SkipRound()
    {
        if (_roundCardManager.XIAOCardCount == 0)
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

        var cardWidth = 1.1f;
        var startX = (_currentHandCards.Count - 1) / 2 * -1 * cardWidth;
        var startZ = (_currentHandCards.Count + 1) * 0.1f;

        if (_currentHandCards.Count % 2 == 0)
        {
            startX -= cardWidth * 0.5f;
        }

        var visible = GameManager.Instance.currentPlayer.Identity.cardTypeDesc == Identity.cardTypeDesc;

        if (visible)
        {
            for (var i = 0; i < _currentHandCards.Count; i++)
            {
                var go = _currentHandCards[i];

                var v3 = startPos;
                v3.x += i * cardWidth + startX;
                v3.z += i * -0.1f + startZ;

                if (go.CardSelected)
                {
                    go.Choose(false);
                }

                go.SetOriginPos(v3);
                go.transform.position = v3;
                go.gameObject.SetActive(visible);
            }
        }
    }

    public void Reaction(Card card)
    {
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
        }
        else
        {
        }
    }

    private void ShowCardAndDestory(XIAOCard xiaoCard)
    {
        xiaoCard.OnClick -= this.CardSelected;
        xiaoCard.transform.position = new Vector3(Vector3.zero.x, Vector3.zero.y, xiaoCard.transform.position.z);

        StartCoroutine(DelayAction(1.0f, () => {
            Destroy(xiaoCard.gameObject);
        }));
    }

    private IEnumerator DelayAction(float delay, Action action)
    {
        yield return new WaitForSeconds(delay);

        action.Invoke();
    }
}
