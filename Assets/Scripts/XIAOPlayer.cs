using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class XIAOPlayer : MonoBehaviour
{
    public TextMesh descText;
    public TextMesh titleText;

    public SpriteRenderer borderSprite;

    private PlayerDeck _currentPlayer = null;

    private bool _playerSelected = false;

    private bool _playerMine = false;

    public bool PlayerSelected
    {
        get
        {
            return _playerSelected;
        }

        set
        {
            _playerSelected = value;

            ChangeBorderColor();
        }
    }

    public bool PlayerMine
    {
        get
        {
            return _playerMine;
        }

        set
        {
            _playerMine = value;

            ChangeBorderColor();
        }
    }

    public PlayerDeck Player
    {
        get
        {
            return _currentPlayer;
        }
    }

    public event Action<XIAOPlayer> OnClick;

    // Start is called before the first frame update
    void Start()
    {
        //cardBack.SetActive(cardHide);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMouseDown()
    {
        PlayerSelected = !PlayerSelected;

        if (OnClick != null)
        {
            OnClick.Invoke(this);
        }
    }

    public void SetPlayer(PlayerDeck player)
    {
        _currentPlayer = player;

        descText.text = player.Identity.cardTypeDesc;
        titleText.text = player.Identity.title;
    }

    private void ChangeBorderColor()
    {
        if (_playerSelected)
        {
            borderSprite.color = Color.green;
        }
        else
        {
            if (_playerMine)
            {
                borderSprite.color = Color.red;
            }
            else
            {
                borderSprite.color = Color.white;
            }
        }
    }
}
