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

    public void Buy(){
        if(EnoughMana()){
            gameManager.Mana -= 100;
            SpawnCard();
        }
    }

    public void SpawnCard(){

        audio.PlayOneShot(drawCard);
        card = (Card) Instantiate(cardPrefab, transform.position, Quaternion.identity);
        card.transform.SetParent(handPanel);
        card.RandomCard();
        card.transform.localScale = new Vector3(1, 1, 1);
    }

    private bool EnoughMana()
    {
        if(gameManager.Mana >= 100){
            return true;
        }
        else{
            return false;
        }
    }

}
