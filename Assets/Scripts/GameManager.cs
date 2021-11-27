using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    public GameObject titleUI;
    public GameObject pausedUI;
    public GameObject gameOverUI;
    public GameObject mainUI;
    public GameObject initUI;

    public Text memberText;

    public ScrollRect scrollBoard;

    public Text boardText;

    public Text deckRemainText;
    public Text sceneStateText;
    public Text mineRemainText;

    public bool gameIsPlaying
    {
        get;
        private set;
    }

    public bool paused;

    // UI

    private PlayerDeck _currentPlayer = null;

    public GameObject cardPrefab;

    private GameObject[] _identityCards;

    private GameObject[] _identityPlayers;

    private bool _chooseIdentityCard = false;

    public GameObject playerPrefab;

    private PlayerDeck _targetPlayer = null;

    private int _actionPlayerIndex = 0;

    private int _gameRound = 0;

    private bool _currentPlayerRound = false;

    private Card.SceneType _currentScene = Card.SceneType.None;

    public Card.SceneType CurrentScene
    {
        get
        {
            return _currentScene;
        }
    }

    private int _memberNumber = 0;

    private XIAOPlayer[] _xiaoPlayers = null;

    private void ShowUI(GameObject newUI)
    {
        GameObject[] allUI = { titleUI, pausedUI, gameOverUI, mainUI, initUI};

        foreach (var go in allUI)
        {
            go.SetActive(false);
        }

        newUI.SetActive(true);
    }

    public PlayerDeck currentPlayer
    {
        get
        {
            return _currentPlayer;
        }
    }

    public void InitDeck(int memberCount)
    {
        _memberNumber = memberCount;

        Deck.EmptyDeck();

        // Common Cards
        Deck.AddCommonCards(1, "欢迎新人", "指定一名玩家进行自我介绍，弃掉1张手牌", null, Card.CardType.Attack, "01");
        Deck.AddCommonCards(1, "原画复盘", "指定一名玩家进行作品分享，弃掉1张手牌", null, Card.CardType.Attack, "02");
        Deck.AddCommonCards(1, "画不完了", "让一名玩家画不完项目，弃掉2张手牌", null, Card.CardType.Attack, "03");
        Deck.AddCommonCards(1, "填写绩效", "让一名玩家填写绩效表，弃掉2张手牌", null, Card.CardType.Attack, "04");
        Deck.AddCommonCards(1, "周六加班", "指定一名玩家周六加班，弃掉3张手牌", null, Card.CardType.Attack, "05");
        Deck.AddCommonCards(1, "部门加班", "今天全组加班！其他玩家弃掉所有手牌", null, Card.CardType.Attack, "06");
        Deck.AddCommonCards(1, "坏女人罪", "指定一名玩家成为坏女人，弃掉所有专属牌", null, Card.CardType.Attack, "07");
        Deck.AddCommonCards(1, "中转崩溃", "中转又双叒叕崩溃了，其他玩家强制弃掉2张专属牌", null, Card.CardType.Attack, "08");
        Deck.AddCommonCards(1, "群内禁言", "我以HR的名义禁止所有人发言，指定玩家停止行动一回合", null, Card.CardType.Attack, "09");
        Deck.AddCommonCards(1, "部门搬迁", "准备搬家我们要换地方了，清除当前场景", null, Card.CardType.Attack, "10");

        Deck.AddCommonCards(1, "申请签卡", "今天迟到了但我能签卡嘿嘿，抽取2张通用牌", null, Card.CardType.Defend, "01");
        Deck.AddCommonCards(1, "去取奶茶", "奶茶到了，我去拿！抽取2张通用牌", null, Card.CardType.Defend, "02");
        Deck.AddCommonCards(1, "食堂午饭", "终于等到午饭了，抽取1张专属牌", null, Card.CardType.Defend, "03");
        Deck.AddCommonCards(1, "吃下午茶", "不要抢每个人都有，所有玩家抽取1张专属牌", null, Card.CardType.Defend, "04");
        Deck.AddCommonCards(1, "准点下班", "五十九分了我得走了，抽取3张通用牌", null, Card.CardType.Defend, "05");
        Deck.AddCommonCards(1, "得开会了", "大伙们该开会去了，所有玩家抽取1张通用牌", null, Card.CardType.Defend, "06");
        Deck.AddCommonCards(1, "我不会画", "我摊牌了我开摆了，抽取4张通用牌后选择其中1张弃掉", null, Card.CardType.Defend, "07");
        Deck.AddCommonCards(1, "电脑蓝屏", "电脑蓝了我也没办法，弃掉手中的通用牌后抽取相同数量的通用牌", null, Card.CardType.Defend, "08");
        Deck.AddCommonCards(1, "今天调休", "不好意思我今天调休了！本回合内无法受到攻击牌攻击", null, Card.CardType.Defend, "09");
        Deck.AddCommonCards(1, "留守八楼", "我不用搬我留在八楼，清除当前场景", null, Card.CardType.Defend, "10");

        Deck.AddCommonCards(1, "来画封面", "你！来画封面，选择一名玩家抓取其1张手牌", null, Card.CardType.Mission, "01");
        Deck.AddCommonCards(1, "英语插图", "这季度的插图都交给你了，选择一名玩家抓取其2张手牌", null, Card.CardType.Mission, "02");
        Deck.AddCommonCards(1, "新奇冒险", "准备好和艳迷一起冒险了吗？选择一名玩家抓取其1张专属牌", null, Card.CardType.Mission, "03");
        Deck.AddCommonCards(1, "古典上色", "瑶琪绝对能教会你的放心，选择一名玩家抓取其2张手牌", null, Card.CardType.Mission, "04");
        Deck.AddCommonCards(1, "分点草稿", "我让偌穗分点草稿给你，选择一名玩家抓取其1张手牌", null, Card.CardType.Mission, "05");
        Deck.AddCommonCards(1, "画面审图", "日星看看图，抓取一名玩家1张手牌弃掉", null, Card.CardType.Mission, "06");
        Deck.AddCommonCards(1, "不@不1", "我没艾特就不准1，抓取一名玩家1张手牌弃掉", null, Card.CardType.Mission, "07");
        Deck.AddCommonCards(1, "你去交流", "你去和那个老师沟通吧，抓取一名玩家1张手牌并置与牌堆顶部", null, Card.CardType.Mission, "08");
        Deck.AddCommonCards(1, "整理素材", "你今天不用画就只用整理素材，抽取其他玩家1张专属牌", null, Card.CardType.Mission, "09");
        Deck.AddCommonCards(1, "调去AI", "你以后就属于AI部门了，清除当前场景", null, Card.CardType.Mission, "10");

        Deck.AddCommonCards(1, "这周桌游", "进入游戏回合，在游戏回合内，所有的攻击牌等同于游戏牌", null, Card.CardType.Scene, "");
        Deck.AddCommonCards(1, "铁咩吖咯", "进入次元回合，在次元回合内，所有的防御牌等同于次元牌", null, Card.CardType.Scene, "");
        Deck.AddCommonCards(1, "正佳集合", "进入聚会回合，在聚会回合内，所有的任务牌等同于聚会牌", null, Card.CardType.Scene, "");

        // MINE Cards
        Deck.AddMINECards(1, "小肥", "当打出游戏牌时，抽取1张专属牌。", "被抓取手牌时，可打出1张专属牌。", "A");
        Deck.AddMINECards(1, "子源", "当打出次元牌时，抽取1张专属牌。", "当手牌数量≥4时，可打出1张专属牌。", "B");
        Deck.AddMINECards(1, "日星", "当打出聚会牌时，抽取1张专属牌。", "本回合无攻击时，可打出1张专属牌。", "C");

        // Special Cards
        Deck.AddSpecialCards(memberCount, "够了！", "本回合玩家只能出攻击牌", "其他玩家弃掉1张牌", "A");
        Deck.AddSpecialCards(memberCount, "~~~~", "本回合玩家只能出防御牌", "额外放置1张防御牌", "B");
        Deck.AddSpecialCards(memberCount, "笨女人 ", "本回合玩家只能出1张牌", "抽取2张通用牌", "C");

        var idCards = new List<Card>();

        // init global deck
        for(var i = 0; i < _memberNumber; i++)
        {
            var idCard = Deck.GetRandomCard(Card.IdentityType.MINE);

            if (idCard == null)
            {
                DisplayOnBoard($"专属卡牌数量不足，无法开始游戏...");
                return;
            }

            idCards.Add(idCard);
        }

        _identityCards = new GameObject[_memberNumber];
        _identityPlayers = new GameObject[_memberNumber];
        _xiaoPlayers = new XIAOPlayer[_memberNumber];

        PickIdentityCards(idCards);
    }

    public void OnPickCard()
    {
        if (!_chooseIdentityCard)
        {
            DisplayOnBoard($"尚未选择身份，无法使用 <Pick>");
            return;
        }

        if (!_currentPlayerRound)
        {
            DisplayOnBoard($"当前并不是您的回合，无法使用 <Pick>");
            return;
        }

        if (_currentPlayer != null)
        {
            _currentPlayer.PickCard(1);
        }
    }

    public void OnXIAOCard()
    {

        if (!_chooseIdentityCard)
        {
            DisplayOnBoard($"尚未选择身份，无法使用 <XIAO>");
            return;
        }

        if (!_currentPlayerRound)
        {
            DisplayOnBoard($"当前并不是您的回合，无法使用 <XIAO>");
            return;
        }

        if (_currentPlayer != null)
        {
            _currentPlayer.XIAOCard(_targetPlayer);
        }
    }

    public void OnDiscard()
    {
        if (!_chooseIdentityCard)
        {
            DisplayOnBoard($"尚未选择身份，无法使用 <Discard>");
            return;
        }

        if (!_currentPlayerRound)
        {
            DisplayOnBoard($"当前并不是您的回合，无法使用 <Discard>");
            return;
        }

        if (_currentPlayer != null)
        {
            _currentPlayer.Discard();
        }
    }

    public void OnSkipRound()
    {
        if (!_chooseIdentityCard)
        {
            DisplayOnBoard($"尚未选择身份，无法使用 <Skip>");
            return;
        }

        if (!_currentPlayerRound)
        {
            DisplayOnBoard($"当前并不是您的回合，无法使用 <Skip>");
            return;
        }

        if (_currentPlayer != null)
        {
            _currentPlayer.SkipRound();
        }
    }

    public void OnStart()
    {
        gameIsPlaying = true;

        ShowUI(initUI);
    }

    public void OnMemberComfirm()
    {
        ShowUI(mainUI);

        var count = int.Parse(memberText.text);

        InitDeck(count);
    }

    public void OnPause(bool paused)
    {

    }

    public void OnMemberChange(int nums)
    {
        var count = int.Parse(memberText.text);

        count += nums;

        count = Mathf.Min(Mathf.Max(3, count), 9);

        memberText.text = $"{count}";
    }

    // Start is called before the first frame update
    void Start()
    {
        ShowUI(titleUI);
    }

    // Update is called once per frame
    void Update()
    {
        deckRemainText.text = $"Deck Remain:{Deck.GetDeckCount()}";
        sceneStateText.text = $"Scene State:{_currentScene}";

        if (_chooseIdentityCard)
        {
            mineRemainText.text = $"MINE Remain:{_currentPlayer.GetMINERemain()}";
        }
        else
        {
            mineRemainText.text = $"MINE Remain: None";
        }
    }

    public void DisplayOnBoard(string msg)
    {
        Debug.Log(msg);

        var parent = scrollBoard.content;

        Transform tmp = Instantiate(boardText, parent).transform;
        tmp.localPosition = Vector3.zero;
        tmp.localRotation = Quaternion.identity;
        tmp.localScale = Vector3.one;

        tmp.GetComponent<Text>().text = $"{msg}\n";

        StartCoroutine(DelayAction(0.2f, () => {
            scrollBoard.verticalNormalizedPosition = 0f;
        }));
    }

    private IEnumerator DelayAction(float delay, Action action)
    {
        yield return new WaitForSeconds(delay);

        action.Invoke();
    }

    private void PickIdentityCards(List<Card> cards)
    {
        DisplayOnBoard("请玩家选择一张身份牌");

        var index = 0;
        var cardWidth = 3.0f;
        var startX = (cards.Count - 1) / 2 * -1 * cardWidth;

        if (cards.Count % 2 == 0)
        {
            startX -= cardWidth * 0.5f;
        }

        foreach(var card in cards)
        {
            var v3 = Vector3.zero;
            v3.x += index * cardWidth + startX;

            _identityCards[index] = CreateXIAOCard(card, v3, this.ChooseIdentityCard, new Vector3(1.8f, 1.8f, 1.0f)).gameObject;

            index++;
        }
    }

    private void ChooseIdentityCard(XIAOCard xiaoCard)
    {
        if (!_chooseIdentityCard)
        {
            Card chooseCard = null;

            foreach (GameObject go in _identityCards)
            {
                var xCard = go.GetComponent<XIAOCard>();

                if (xiaoCard.Card.cardTypeDesc == xCard.Card.cardTypeDesc)
                {
                    chooseCard = xCard.Card;
                    _chooseIdentityCard = true;
                }
                else
                {
                    go.SetActive(false);
                }
            }

            if (_chooseIdentityCard)
            {
                StartCoroutine(DelayAction(3.0f, () => {
                    for (int i = _identityCards.Length - 1; i >= 0; i--)
                    {
                        _identityCards[i].SetActive(false);
                    }
                }));

                DisplayOnBoard($"您的身份是：{chooseCard.title}");

                InitIdentityPlayers(chooseCard);
            }
        }
    }

    private void InitIdentityPlayers(Card card)
    {
        var playerHeight = -1.25f;

        var startPos = Camera.main.ViewportToWorldPoint(new Vector2(.1f, .7f));
        startPos.z = 0;

        for (var i = 0; i < _memberNumber; i++)
        {
            var v3 = startPos;
            v3.y += i * playerHeight;

            var card_go = _identityCards[i];
            var xiaoCard = card_go.GetComponent<XIAOCard>();

            var go = Instantiate(playerPrefab, v3, cardPrefab.transform.rotation);

            var playerDeck = go.GetComponent<PlayerDeck>();
            playerDeck.Init(xiaoCard.Card, _memberNumber);

            var xiaoPlayer = go.GetComponent<XIAOPlayer>();
            xiaoPlayer.SetPlayer(playerDeck);
            xiaoPlayer.OnClick += this.ChoosePlayer;

            if (xiaoCard.Card.cardTypeDesc == card.cardTypeDesc)
            {
                _currentPlayer = playerDeck;
                xiaoPlayer.PlayerMine = true;
                playerDeck.SetAIHandle(false);
            }
            else
            {
                playerDeck.SetAIHandle(true);
            }

            _identityPlayers[i] = go;
            _xiaoPlayers[i] = xiaoPlayer;
        }

        DisplayOnBoard($"玩家身份已初始化完毕");

        DisplayOnBoard($"游戏开始...");

        StartCoroutine(DelayAction(1.0f, () => {
            for (int i = _identityCards.Length - 1; i >= 0; i--)
            {
                Destroy(_identityCards[i]);
            }
        }));

        // Begin new round
        NextRound();
    }

    private void ChoosePlayer(XIAOPlayer xiaoPlayer)
    {
        var player = xiaoPlayer.Player;

        if (player != _targetPlayer)
        {
            DisplayOnBoard($"当前选中了身份为 {player.Identity.title} 的玩家");

            _targetPlayer = player;
        }
    }

    public void NextPlayer()
    {
        if (_actionPlayerIndex > _identityPlayers.Length - 1)
        {
            _actionPlayerIndex = 0;

            NextRound();
        }
        else
        {
            var player = _identityPlayers[_actionPlayerIndex].GetComponent<XIAOPlayer>().Player;
            player.RoundStarted();

            _currentPlayerRound = player.Identity.cardTypeDesc == _currentPlayer.Identity.cardTypeDesc;

            _actionPlayerIndex++;

            if (player.AIHandle)
            {
                DisplayOnBoard($"{player.Identity.title} 为AI模式，于<5>秒后自动跳过出牌阶段...");

                StartCoroutine(DelayAction(5.0f, () =>
                {
                    player.SkipRound();
                }));
            }
        }

        // clear selected player
        if (_currentPlayerRound)
        {
            foreach(var go in _identityPlayers)
            {
                var xiaoPlayer = go.GetComponent<XIAOPlayer>();

                xiaoPlayer.PlayerSelected = false;
            }

            _targetPlayer = null;
        }

    }

    private void NextRound()
    {
        _gameRound++;

        DisplayOnBoard($"当前回合： {_gameRound:D2} 回合");

        // player start round
        NextPlayer();
    }

    public XIAOCard CreateXIAOCard(Card card, Vector3 position, Action<XIAOCard> action, Vector3 scale, bool cardHide = true, bool visible = true)
    {
        var go = Instantiate(cardPrefab, position, cardPrefab.transform.rotation);
        go.transform.localScale = scale;
        go.SetActive(visible);

        var xiaoCard = go.GetComponent<XIAOCard>();
        xiaoCard.SetCard(card);
        xiaoCard.CardHide = cardHide;
        xiaoCard.OnClick += action;

        return xiaoCard;
    }

    public void ChangeScene(Card.SceneType scene)
    {
        _currentScene = scene;
    }
}
