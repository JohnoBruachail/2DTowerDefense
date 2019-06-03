using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManagerBehaviour : MonoBehaviour
{
    public int manaModifier;
    public int speedModifier;
    public int rangeModifier;
    private int mana;
    public int heroesSlain;
    public int spellActive;
    private int wave;
    private int treasure;
    public bool gameOver = false;
    public int startingTreasure;
    public GameObject dropZone;
    public GameObject[] cardSlots;
    public GameObject[] nextWaveLabels;
    public GameObject[] treasureIndicator;
    public List<Spawner> spawners;
    public GameObject beginButton;
    public GameObject timeLabel;
    public GameObject manaLabel;
    public GameObject treasureLabel;
    public GameObject waveLabel;
    public GameObject gameOverPanel;
    public GameObject gameOverLabel;
    public GameObject treasureResultsLabel;
    public GameObject enemysKilledResultsLabel;
    public SpellAnimation spellAnimator;
    public TouchCamera touchCamera;
    public BuyCard buyCardButton;

    public int Mana
    {
        get
        {
            return mana;
        }
        set
        {
            //Debug.Log("updating mana value");
            mana = value;
            if(manaModifier == 2){
                manaLabel.GetComponent<Text>().text = "Mana: " + mana + " x2";
                manaLabel.GetComponent<Text>().color = new Color32(70,215,250,255);
            }else{
                manaLabel.GetComponent<Text>().text = "Mana: " + mana;
                manaLabel.GetComponent<Text>().color = new Color32(255,255,255,255);
            }
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
            waveLabel.GetComponent<Text>().text = "Wave: " + (wave + 1);
        }
    }
 
    public int Treasure
    {
        get { return treasure; }
        set
        {

            if (value < treasure)
            {
                Camera.main.GetComponent<CameraShake>().Shake();
            }

            treasure = value;
            treasureLabel.GetComponent<Text>().text = "Treasure: " + treasure;

            if (treasure <= 0 && !gameOver)
            {
                GameOverLose();
            }


        }
    }

    // Use this for initialization
    void Start()
    {
        beginButton   = GameObject.Find("BeginButton");
        timeLabel     = GameObject.Find("TimeLabel");
        manaLabel     = GameObject.Find("ManaLabel");
        treasureLabel = GameObject.Find("TreasureLabel");
        waveLabel     = GameObject.Find("WaveLabel");
        gameOverLabel = GameObject.Find("GameOverLabel");

        treasureResultsLabel = GameObject.Find("TreasureResultsLabel");
        enemysKilledResultsLabel = GameObject.Find("EnemysKilledResultsLabel");

        gameOverPanel = GameObject.FindGameObjectWithTag("GameOverPanel");
        gameOverPanel.SetActive(false);
        
        Mana        = 300;
        Wave        = 0;
        Treasure    = startingTreasure;
        heroesSlain = 0;

        manaModifier  = 1;
        speedModifier = 1;
        rangeModifier = 1;

        FindAllCardSlots();

        InvokeRepeating("UpdateTime",1.0f, 1.0f);
        Time.timeScale = 0f;
    }

    public void Begin(){
        Time.timeScale = 1f;
        beginButton.SetActive(false);
    }

    //certain events trigger an end game bool set to true. This will then determin if its a win or a loss
    //either way a win condition menu like a pause menu should load, giving the player the option to restart the level, go to the main menu and
    //display the score of the level.

    // Update is called once per frame
    void Update(){
        //need to add case if tower is deselected.
    }

    void UpdateTime(){
        timeLabel.GetComponent<Text>().text = "Time: " + Time.time.ToString("0");
    }

    //Collects all the card slots in the scene and stores them in the array cardSlots
    public void FindAllCardSlots(){
        cardSlots = GameObject.FindGameObjectsWithTag("CardSlot");

        foreach(GameObject card in cardSlots) {
            card.SetActive(false);
        }
    }

    //also change the text on game over panel to "you lose".
    //play a negitive sound on losing.

    public void GameOverWin(){
        gameOver = true;
        gameOverPanel.SetActive(true);
        gameOverPanel.GetComponent<Animator>().SetBool("gameOver", true);
        gameOverLabel.GetComponent<Text>().text = "Victory";
        treasureResultsLabel.GetComponent<Text>().text = "Treasure Saved: " + treasure + "/500";
        enemysKilledResultsLabel.GetComponent<Text>().text = "Heroes Slain: " + heroesSlain;
    }

    public void GameOverLose(){
        Debug.Log("Games over man, games over");
        gameOver = true;
        gameOverPanel.SetActive(true);
        gameOverPanel.GetComponent<Animator>().SetBool("gameOver", true);
        gameOverLabel.GetComponent<Text>().text = "Defeat";
        treasureResultsLabel.GetComponent<Text>().text = "Treasure Saved: " + treasure + "/500";
        enemysKilledResultsLabel.GetComponent<Text>().text = "Heroes Slain: " + heroesSlain;
    }


    //each wave is a collection of enemy types.
    //a wave will have an array and as the wave is deployed that enemy type will be spawned based 


    //spawners work independently
    //game manager has a difficulty stat
    //spawners consult difficulty stat when spawning
    
    
    //once a spawner has completed its ful spawn it wont go again till next wave is triggered
    //this is done once all spaweners are done spawning. 

    public void CardInHand(bool cardInHand, int cardType){
        if(cardInHand){
            //disable the camera in any case, depending on card type either activate card slots or the background to catch the spell card.
            touchCamera.enabled = false;

            if(cardType >= 0 && cardType <= 1){
                foreach(GameObject card in cardSlots) {
                    card.SetActive(true);
                }
            }
            else{
                dropZone.SetActive(true);
            }
            
        }else{
            touchCamera.enabled = true;
            foreach(GameObject card in cardSlots) {
                    card.SetActive(false);
            }
        }
    }

    public void ActivateCard(int cardFace){
        if(cardFace == 6){
            //Debug.Log("Tower Speed Booster activated");
            speedModifier = 2;
            Invoke("NormaliseSpeed", 10);
        }
        else if(cardFace == 7){
            //Debug.Log("Mana Booster activated");
            manaModifier = 2;
            manaLabel.GetComponent<Text>().text = "Mana: " + mana + " x2";
            manaLabel.GetComponent<Text>().color = new Color32(70,215,250,255);
            Invoke("NormaliseMana", 10);
        }
        else if(cardFace == 8){
            //Debug.Log("Give 3 cards Booster activated");
            GiveCard();
            Invoke("GiveCard", 0.3f);
            Invoke("GiveCard", 0.6f);
        }
        else if(cardFace == 9){
            //Debug.Log("Armor spell card activated");
            spellActive = 1;
            spellAnimator.ActivateAnimation(spellActive - 1);
            //need to add some indication that the spell is now active in the players hand
        }
        else if(cardFace == 10){
            //Debug.Log("Freeze spell card activated");
            spellActive = 2;
            spellAnimator.ActivateAnimation(spellActive - 1);

        }
        else if(cardFace == 11){
            //Debug.Log("Teleport spell card activated");
            spellActive = 3;
            spellAnimator.ActivateAnimation(spellActive - 1);
        }
        touchCamera.enabled = true;
    }

    public void NormaliseSpeed(){
        //Debug.Log("Tower Speed Booster deactivated");
        speedModifier = 1;
    }

    public void NormaliseMana(){
        //Debug.Log("mana Booster deactivated");
        manaModifier = 1;
        manaLabel.GetComponent<Text>().text = "Mana: " + mana;
        manaLabel.GetComponent<Text>().color = new Color32(255,255,255,255);
    }
    public void GiveCard(){
        buyCardButton.SpawnCard();
    }

    public void UpdateWave(){

        //foreach(Spawner spawner in spawners){

        //}
    }
}
