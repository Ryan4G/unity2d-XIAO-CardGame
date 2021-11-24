using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XIAOCard : MonoBehaviour
{
    public GameObject cardBack;

    public bool cardHide = true;

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
}
