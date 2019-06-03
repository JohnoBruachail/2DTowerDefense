using UnityEngine;
using System.Collections;

public class Tower : MonoBehaviour
{
    public Monster monster;
    public GameObject selectionCircle;
    public GameObject rangeIndicator;
    AudioSource audio;
    public AudioClip placeStandardMonster;
    public AudioClip upgradeStandardMonster;
    public AudioClip placeSpeedMonster;
    public AudioClip upgradeSpeedMonster;
    public AudioClip placeRangeMonster;
    public AudioClip upgradeRangeMonster;
    private StatsPanel statsPanel;
    private int currentMonsterType;
    public GameObject[] powerStars;
    public GameObject[] elementStars;
    private Color32 fire;
    private Color32 ice;
    private Color32 poison;
    


    // Use this for initialization
    void Start()
    {
        statsPanel = GameObject.Find("StatsPanel").GetComponent<StatsPanel>();
        audio = gameObject.GetComponent<AudioSource>();

        fire = new Color32(255,90,25,255);
        ice = new Color32(25,200,255,255);
        poison = new Color32(25,255,30,255);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    

    //HOW BUTTONS WORK
    //clicking selects the current turret

    //1
    //Clicking on a tower makes each button visable + adds a reference of the open spot to each button script.

    //2
    //pressing the button script activates the button and triggers the open spot script function from the button.

    //3
    //THE BUTTONS WILL THEN CALL THE GAME MANAGER SCRIPT WHICH HAS THE REFERENCE TO THE TOWER AND TRIGGERS ITS FUNCTION TO UPGRADE.

    //Click will select this spawnpoint as the active spawnpoint,
    //This will make the UI buttons for upgrades visiable
    //Pressing these buttons will trigger the upgrade process with the related upgrade type as input
    //Ill need a deselect process to make the buttons disapear
    //If an upgrade is selected the buttons also disappear


    //1. click, spawn monster.
    //2. click, spawn upgrade options for fire, ice, poison.
    //3. click, spawn upgrade options for power, range, aoe.

    public void SelectMe()
    {
        Debug.Log("Tower Selected");
        //may need a circle to indicate this is selected
        statsPanel.SelectTower(this);
        selectionCircle.SetActive(true);
        if(monster){

            rangeIndicator.SetActive(true);
            rangeIndicator.transform.localScale = new Vector3(monster.GetMonsterRange() * 2, monster.GetMonsterRange() * 2, 0);

        }
    }

    public void DeselectMe()
    {
        Debug.Log("Tower Deselected");
        //may need a circle to indicate this is selected
        statsPanel.SelectTower();
        selectionCircle.SetActive(false);
        rangeIndicator.SetActive(false);

    }

    //build based on what card was just dropped into the tower slot by the player
    public bool Build(int cardType){
        //Debug.Log("build using cardType: " + cardType);

        /*
            Card    Monster
            0       Standard
            1       Standard Speed
            2       Standard Range
            3       Element Fire
            4       Element Ice
            5       Element Poison
        */

        //check what monster currently exists.
        //do what card wants based on current monster type.
        currentMonsterType = monster.GetMonsterType();

        switch (cardType){
        case 0:
            //Standard Monster
            if(currentMonsterType == 0 || currentMonsterType == 1 || currentMonsterType == 2 || currentMonsterType == 3){
                Debug.Log("upgrade a standard monster");
                if(monster.IncreasePowerTier()){
                    powerStars[monster.currentPowerTier].SetActive(true);
                    monster.UpdateMonsterRange();
                    audio.PlayOneShot(upgradeStandardMonster);
                    return true;
                }else{
                    return false;
                }
            }
            else{
                powerStars[0].SetActive(false);
                powerStars[0].SetActive(false);
                powerStars[0].SetActive(false);
                monster.powerType = "Standard";
                Debug.Log("Build a standard monster");
                powerStars[monster.currentPowerTier].SetActive(true);
                monster.gameObject.SetActive(true);
                monster.GetComponent<Monster>().SetType(0);
                monster.UpdateMonsterRange();
                audio.PlayOneShot(placeStandardMonster);

                gameObject.GetComponent<SpriteRenderer>().enabled = false;
                return true;
            }
        break;

        case 1:
            //Standard Speed
            if(currentMonsterType == 4 || currentMonsterType == 5 || currentMonsterType == 6 || currentMonsterType == 7){
                if(monster.IncreasePowerTier()){
                    powerStars[monster.currentPowerTier].SetActive(true);
                    monster.UpdateMonsterRange();
                    audio.PlayOneShot(upgradeSpeedMonster);
                    return true;
                }else{
                    return false;
                }
            }
            else{
                powerStars[0].SetActive(false);
                powerStars[1].SetActive(false);
                powerStars[2].SetActive(false);
                monster.powerType = "Speed";
                powerStars[monster.currentPowerTier].SetActive(true);
                monster.gameObject.SetActive(true);
                monster.GetComponent<Monster>().SetType(4);
                monster.UpdateMonsterRange();
                audio.PlayOneShot(placeSpeedMonster);

                gameObject.GetComponent<SpriteRenderer>().enabled = false;
                return true;
            }
        break;

        case 2:
            //Standard Range
            if(currentMonsterType == 8 || currentMonsterType == 9 || currentMonsterType == 10 || currentMonsterType == 11){
                if(monster.IncreasePowerTier()){
                    powerStars[monster.currentPowerTier].SetActive(true);
                    monster.UpdateMonsterRange();
                    audio.PlayOneShot(upgradeRangeMonster);
                    return true;
                }else{
                    return false;
                }
            }
            else{
                powerStars[0].SetActive(false);
                powerStars[1].SetActive(false);
                powerStars[2].SetActive(false);
                monster.powerType = "Range";
                powerStars[monster.currentPowerTier].SetActive(true);
                monster.gameObject.SetActive(true);
                monster.GetComponent<Monster>().SetType(8);
                monster.UpdateMonsterRange();
                audio.PlayOneShot(placeRangeMonster);

                gameObject.GetComponent<SpriteRenderer>().enabled = false;
                return true;
            }
        break;

        case 3:
            //Element Fire
            if(currentMonsterType == 1 || currentMonsterType == 5 || currentMonsterType == 9){
                if(monster.IncreaseElementTier()){
                    elementStars[monster.currentElementTier].SetActive(true);
                    monster.UpdateMonsterRange();
                    elementStars[monster.currentElementTier].GetComponent<SpriteRenderer>().color = fire;
                    return true;
                }else{
                    return false;
                }
            }
            else if(currentMonsterType >= 0 && currentMonsterType <= 3){
                elementStars[0].SetActive(false);
                elementStars[1].SetActive(false);
                elementStars[2].SetActive(false);
                monster.elementType = "Fire";
                monster.gameObject.SetActive(true);
                monster.GetComponent<Monster>().SetType(1);
                monster.UpdateMonsterRange();
                audio.PlayOneShot(upgradeStandardMonster);
                elementStars[monster.currentElementTier].SetActive(true);
                Debug.Log("monsterelementtier: " + monster.currentElementTier);
                Debug.Log("change the star color from: " + elementStars[monster.currentElementTier].GetComponent<SpriteRenderer>().color + " to the color of: " + fire);
                elementStars[monster.currentElementTier].GetComponent<SpriteRenderer>().color = fire;
                Debug.Log("star color is now: " + elementStars[monster.currentElementTier].GetComponent<SpriteRenderer>().color);
                
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
                return true;
            }
            else if(currentMonsterType >= 4 && currentMonsterType <= 7){
                elementStars[0].SetActive(false);
                elementStars[1].SetActive(false);
                elementStars[2].SetActive(false);
                monster.elementType = "Fire";
                monster.gameObject.SetActive(true);
                monster.GetComponent<Monster>().SetType(5);
                monster.UpdateMonsterRange();
                audio.PlayOneShot(upgradeStandardMonster);
                elementStars[monster.currentElementTier].SetActive(true);
                elementStars[monster.currentElementTier].GetComponent<SpriteRenderer>().color = fire;

                gameObject.GetComponent<SpriteRenderer>().enabled = false;
                return true;
            }
            else if(currentMonsterType >= 8 && currentMonsterType <= 11){
                elementStars[0].SetActive(false);
                elementStars[1].SetActive(false);
                elementStars[2].SetActive(false);
                monster.elementType = "Fire";
                monster.gameObject.SetActive(true);
                monster.GetComponent<Monster>().SetType(9);
                monster.UpdateMonsterRange();
                audio.PlayOneShot(upgradeStandardMonster);
                elementStars[monster.currentElementTier].SetActive(true);
                elementStars[monster.currentElementTier].GetComponent<SpriteRenderer>().color = fire;

                gameObject.GetComponent<SpriteRenderer>().enabled = false;
                return true;
            }
        break;

            case 4:
            //Element Ice
            if(currentMonsterType == 2 || currentMonsterType == 6 || currentMonsterType == 10){
                if(monster.IncreaseElementTier()){
                    elementStars[monster.currentElementTier].SetActive(true);
                    monster.UpdateMonsterRange();
                    elementStars[monster.currentElementTier].GetComponent<SpriteRenderer>().color = ice;
                    return true;
                }else{
                    return false;
                }
            }
            else if(currentMonsterType >= 0 && currentMonsterType <= 3){
                elementStars[0].SetActive(false);
                elementStars[1].SetActive(false);
                elementStars[2].SetActive(false);
                monster.elementType = "Ice";
                monster.gameObject.SetActive(true);
                monster.GetComponent<Monster>().SetType(2);
                monster.UpdateMonsterRange();
                audio.PlayOneShot(upgradeStandardMonster);
                elementStars[monster.currentElementTier].SetActive(true);
                elementStars[monster.currentElementTier].GetComponent<SpriteRenderer>().color = ice;

                gameObject.GetComponent<SpriteRenderer>().enabled = false;
                return true;
            }
            else if(currentMonsterType >= 4 && currentMonsterType <= 7){
                elementStars[0].SetActive(false);
                elementStars[1].SetActive(false);
                elementStars[2].SetActive(false);
                monster.elementType = "Ice";
                monster.gameObject.SetActive(true);
                monster.GetComponent<Monster>().SetType(6);
                monster.UpdateMonsterRange();
                audio.PlayOneShot(upgradeStandardMonster);
                elementStars[monster.currentElementTier].SetActive(true);
                elementStars[monster.currentElementTier].GetComponent<SpriteRenderer>().color = ice;

                gameObject.GetComponent<SpriteRenderer>().enabled = false;
                return true;
            }
            else if(currentMonsterType >= 8 && currentMonsterType <= 11){
                elementStars[0].SetActive(false);
                elementStars[1].SetActive(false);
                elementStars[2].SetActive(false);
                monster.elementType = "Ice";
                monster.gameObject.SetActive(true);
                monster.GetComponent<Monster>().SetType(10);
                monster.UpdateMonsterRange();
                audio.PlayOneShot(upgradeStandardMonster);
                elementStars[monster.currentElementTier].SetActive(true);
                elementStars[monster.currentElementTier].GetComponent<SpriteRenderer>().color = ice;

                gameObject.GetComponent<SpriteRenderer>().enabled = false;
                return true;
            }
        break;

            case 5:
            //Element Poison
            if(currentMonsterType == 3 || currentMonsterType == 7 || currentMonsterType == 11){
                if(monster.IncreaseElementTier()){
                    elementStars[monster.currentElementTier].SetActive(true);
                    monster.UpdateMonsterRange();
                    elementStars[monster.currentElementTier].GetComponent<SpriteRenderer>().color = poison;
                    return true;
                }else{
                    return false;
                }
            }
            else if(currentMonsterType >= 0 && currentMonsterType <= 3){
                elementStars[0].SetActive(false);
                elementStars[1].SetActive(false);
                elementStars[2].SetActive(false);
                monster.elementType = "Poison";
                monster.gameObject.SetActive(true);
                monster.GetComponent<Monster>().SetType(3);
                monster.UpdateMonsterRange();
                audio.PlayOneShot(upgradeStandardMonster);
                elementStars[monster.currentElementTier].SetActive(true);
                elementStars[monster.currentElementTier].GetComponent<SpriteRenderer>().color = poison;

                gameObject.GetComponent<SpriteRenderer>().enabled = false;
                return true;
            }
            else if(currentMonsterType >= 4 && currentMonsterType <= 7){
                elementStars[0].SetActive(false);
                elementStars[1].SetActive(false);
                elementStars[2].SetActive(false);
                monster.elementType = "Poison";
                monster.gameObject.SetActive(true);
                monster.GetComponent<Monster>().SetType(7);
                monster.UpdateMonsterRange();
                audio.PlayOneShot(upgradeStandardMonster);
                elementStars[monster.currentElementTier].SetActive(true);
                elementStars[monster.currentElementTier].GetComponent<SpriteRenderer>().color = poison;

                gameObject.GetComponent<SpriteRenderer>().enabled = false;
                return true;
            }
            else if(currentMonsterType >= 8 && currentMonsterType <= 11){
                elementStars[0].SetActive(false);
                elementStars[1].SetActive(false);
                elementStars[2].SetActive(false);
                monster.elementType = "Poison";
                monster.gameObject.SetActive(true);
                monster.GetComponent<Monster>().SetType(11);
                monster.UpdateMonsterRange();
                audio.PlayOneShot(upgradeStandardMonster);
                elementStars[monster.currentElementTier].SetActive(true);
                elementStars[monster.currentElementTier].GetComponent<SpriteRenderer>().color = poison;

                gameObject.GetComponent<SpriteRenderer>().enabled = false;
                return true;
            }
        break;
        }

        return false;
    }
}
