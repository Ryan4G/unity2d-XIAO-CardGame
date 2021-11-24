using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class XIAOCard : MonoBehaviour
{
    public GameObject cardBack;

    public bool cardHide = true;

    public TextMesh descText;
    public TextMesh titleText;
    public TextMesh effect01Text;
    public TextMesh effect02Text;

    private Card _currentCard = null;

    public Card Card
    {
        get
        {
            return _currentCard;
        }
    }

    public event Action<Card> OnClick;

    // Start is called before the first frame update
    void Start()
    {
        cardBack.SetActive(cardHide);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMouseDown()
    {
        cardBack.SetActive(false);

        if (OnClick != null)
        {
            OnClick.Invoke(_currentCard);
        }
    }

    public void SetCard(Card card)
    {
        _currentCard = card;

        descText.text = convertTextMesh(card.cardTypeDesc);
        titleText.text = convertTextMesh(card.title);
        effect01Text.text = convertTextMesh(card.effect01);
        effect02Text.text = convertTextMesh(card.effect02);
    }

    private string convertTextMesh(string msg)
    {
        int width = 8;

        if (msg.Length <= 8)
        {
            return msg;
        }

        var level = Convert.ToInt32(Math.Ceiling(msg.Length * 1.0f / width));

        var shortMsgs = new List<string>();

        for(var i = 0; i < level; i++)
        {
            var subW = Math.Min(width, msg.Length - i * width);

            shortMsgs.Add(msg.Substring(i * width, subW));
        }

        return string.Join("\n", shortMsgs);
    }
}
