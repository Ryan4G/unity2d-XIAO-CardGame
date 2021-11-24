using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XIAOCard : MonoBehaviour
{
    public GameObject cardBack;

    public bool cardHide = true;

    public TextMesh descText;
    public TextMesh titleText;
    public TextMesh effect01Text;
    public TextMesh effect02Text;

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
    }

    public void SetCard(Card card)
    {
        descText.text = card.cardTypeDesc;
        titleText.text = card.title;
        effect01Text.text = card.effect01;
        effect02Text.text = card.effect02;
    }
}
