using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class XIAOCard : MonoBehaviour
{
    public GameObject cardBack;

    public TextMesh descText;
    public TextMesh titleText;
    public TextMesh effect01Text;
    public TextMesh effect02Text;

    private Card _currentCard = null;
    private bool _cardHide = true;
    private bool _cardSelected = false;

    public Card Card
    {
        get
        {
            return _currentCard;
        }
    }

    public bool CardHide
    {
        get
        {
            return _cardHide;
        }

        set
        {
            _cardHide = value;

            cardBack.SetActive(!_cardHide);
        }
    }

    public bool CardSelected
    {
        get
        {
            return _cardSelected;
        }
    }

    public event Action<XIAOCard> OnClick;

    private Vector3 _originPos;

    // Start is called before the first frame update
    void Start()
    {
        cardBack.SetActive(_cardHide);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMouseDown()
    {
        if (_cardHide)
        {
            _cardHide = false;

            cardBack.SetActive(false);
        }

        if (OnClick != null)
        {
            OnClick.Invoke(this);
        }
    }

    public void SetCard(Card card)
    {
        _currentCard = card;

        descText.text = convertTextMesh(card.cardTypeDesc);
        titleText.text = convertTextMesh(card.title);
        effect01Text.text = convertTextMesh(card.effect01);
        effect02Text.text = convertTextMesh(card.effect02);

        _originPos = transform.position;
    }

    private string convertTextMesh(string msg)
    {
        int width = 8;

        if (string.IsNullOrEmpty(msg))
        {
            return "";
        }

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

    public void Choose(bool state)
    {
        _cardSelected = state;

        chooseCardEffect();
    }

    private void chooseCardEffect()
    {
        Vector3 pos = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        if (_cardSelected)
        {
            pos.y += 0.1f;
            pos.z += -0.15f;
        }
        else
        {
            pos.y = _originPos.y;
            pos.z = _originPos.z;
        }

        gameObject.transform.position = pos;
    }

    public void SetOriginPos(Vector3 pos)
    {
        _originPos = pos;
    }
}
