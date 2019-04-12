using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManagerBehaviour : MonoBehaviour
{
    public int goldModifier;
    public int speedModifier;
    public int rangeModifier;
    private int gold;
    private int bankedGold;
    private int nextBankGoldCost;
    public int difficulty;
    public int spellActive;
    private int wave;
    private int health;

    public bool gameOver = false;

    public GameObject dropZone;
    
    public GameObject[] cardSlots;
    public GameObject[] nextWaveLabels;
    public GameObject[] healthIndicator;

    public List<Spawner> spawners;

    public Text healthLabel;
    public Text goldLabel;
    public Text waveLabel;
    public Text bankedGoldLabel;

    public SpellAnimation spellAnimator;

    public TouchCamera touchCamera;

    public BuyCard buyCardButton;

    public int Gold
    {
        get
        {
            return gold;
        }
        set
        {
            Debug.Log("updating gold value");
            gold = value;
            if(goldModifier == 2){
                goldLabel.GetComponent<Text>().text = "GOLD: " + gold + " x2";
            }else{
                goldLabel.GetComponent<Text>().text = "GOLD: " + gold;
            }
        }
    }

    public int BankedGold
    {
        get
        {
            return bankedGold;
        }
        set
        {
            Debug.Log("updating banked gold value");
            Gold -= value;
            bankedGold += value;
            bankedGoldLabel.GetComponent<Text>().text = "TREASURE: " + bankedGold;
        }
    }

    public int Wave
    {
        get { return wave; }
        set
        {
            wave = value;
            if (!gameOver)
            {
                for (int i = 0; i < nextWaveLabels.Length; i++)
                {
                    nextWaveLabels[i].GetComponent<Animator>().SetTrigger("nextWave");
                }
            }
            waveLabel.text = "WAVE: " + (wave + 1);
        }
    }
 
    public int Health
    {
        get { return health; }
        set
        {

            if (value < health)
            {
                Camera.main.GetComponent<CameraShake>().Shake();
            }

            health = value;

            if (health <= 0 && !gameOver)
            {
                gameOver = true;
                GameObject gameOverText = GameObject.FindGameObjectWithTag("GameOver");
                gameOverText.GetComponent<Animator>().SetBool("gameOver", true);
            }

            for (int i = 0; i < healthIndicator.Length; i++)
            {
                if (i < Health)
                {
                    healthIndicator[i].SetActive(true);
                }
                else
                {
                    healthIndicator[i].SetActive(false);
                }
            }
        }
    }

    // Use this for initialization
    void Start()
    {
        Gold = 2000;
        nextBankGoldCost = 250;
        difficulty = 0;
        Wave = 0;
        Health = 5;

        goldModifier = 1;
        speedModifier = 1;
        rangeModifier = 1;
    }

    // Update is called once per frame
    void Update(){
        //need to add case if tower is deselected.
    }


    //press bank gold button
    //checks if enough gold is available to bank
    //updates gold and banked gold
    //wave spawn types are fixed

    //each wave is a collection of enemy types.
    //a wave will have an array and as the wave is deployed that enemy type will be spawned based 


    //spawners work independently
    //game manager has a difficulty stat
    //spawners consult difficulty stat when spawning
    
    
    //once a spawner has completed its ful spawn it wont go again till next wave is triggered
    //this is done once all spaweners are done spawning. 

    public void CardInHand(bool cardInHand, int cardType){
        if(cardInHand){
            if(cardType >= 0 && cardType <= 1){
                for(int x = 0; x < cardSlots.Length; x++){
                    touchCamera.enabled = false;
                    cardSlots[x].SetActive(true);
                }
            }
            else{
                dropZone.SetActive(true);
            }
            
        }else{
            for(int x = 0; x < cardSlots.Length; x++){
                touchCamera.enabled = true;
                cardSlots[x].SetActive(false);
            }
        }
    }

    public void ActivateCard(int cardFace){
        if(cardFace == 6){
            Debug.Log("Tower Speed Booster activated");
            speedModifier = 2;
            Invoke("NormaliseSpeed", 10);
        }
        else if(cardFace == 7){
            Debug.Log("Gold Booster activated");
            goldModifier = 2;
            goldLabel.GetComponent<Text>().text = "GOLD: " + gold + " x2";
            Invoke("NormaliseGold", 10);
        }
        else if(cardFace == 8){
            Debug.Log("Give 3 cards Booster activated");
            GiveCard();
            Invoke("giveCard", 0.3f);
            Invoke("giveCard", 0.6f);
        }
        else if(cardFace == 9){
            Debug.Log("Armor spell card activated");
            spellActive = 1;
            spellAnimator.ActivateAnimation(spellActive - 1);
            //need to add some indication that the spell is now active in the players hand
        }
        else if(cardFace == 10){
            Debug.Log("Freeze spell card activated");
            spellActive = 2;
            spellAnimator.ActivateAnimation(spellActive - 1);

        }
        else if(cardFace == 11){
            Debug.Log("Teleport spell card activated");
            spellActive = 3;
            spellAnimator.ActivateAnimation(spellActive - 1);
        }
    }

    public void NormaliseSpeed(){
        Debug.Log("Tower Speed Booster deactivated");
        speedModifier = 1;
    }

    public void NormaliseGold(){
        Debug.Log("gold Booster deactivated");
        goldModifier = 1;
        goldLabel.GetComponent<Text>().text = "GOLD: " + gold;
    }
    public void GiveCard(){
        buyCardButton.SpawnCard();
    }

    public void UpdateWave(){

        //foreach(Spawner spawner in spawners){

        //}
    }
}
