using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManagerBehaviour : MonoBehaviour
{
    public GameObject[] cardSlots;
    public List<Spawner> spawners;
    public Text goldLabel;
    public Text bankedGoldLabel;
    private int gold;
    private int bankedGold;
    private int nextBankGoldCost;
    public int difficulty;
    public TouchCamera touchCamera;
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
            goldLabel.GetComponent<Text>().text = "GOLD: " + gold;
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

    public Text waveLabel;
    public GameObject[] nextWaveLabels;

    public bool gameOver = false;

    private int wave;
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

    public Text healthLabel;
    public GameObject[] healthIndicator;

    private int health;
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

    public void cardInHand(bool cardInHand){
        if(cardInHand){
            for(int x = 0; x < cardSlots.Length; x++){
                touchCamera.enabled = false;
                cardSlots[x].SetActive(true);
            }
        }else{
            for(int x = 0; x < cardSlots.Length; x++){
                touchCamera.enabled = true;
                cardSlots[x].SetActive(false);
            }
        }
    }

    public void UpdateWave(){

        //foreach(Spawner spawner in spawners){

        //}
    }
}
