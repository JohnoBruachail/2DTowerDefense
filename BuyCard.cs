using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyCard : MonoBehaviour
{
    public Transform handPanel;
    public Card cardPrefab;
    private Card card;
    public GameManagerBehaviour gameManager;

    AudioSource audio;
    public AudioClip drawCard;

    void Start(){
        audio = gameObject.GetComponent<AudioSource>();
    }

    public void buy(){
        if(EnoughGold()){
            audio.PlayOneShot(drawCard);
            gameManager.Gold -= 100;
            card = (Card) Instantiate(cardPrefab, transform.position, Quaternion.identity);
            card.transform.SetParent(handPanel);
            card.randomCard();
            card.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    private bool EnoughGold()
    {
        if(gameManager.Gold >= 100){
            return true;
        }
        else{
            return false;
        }
    }

}
