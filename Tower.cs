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

    void OnMouseUp()
    {
        //may need a circle to indicate this is selected
        statsPanel.SelectTower(this);
        selectionCircle.SetActive(true);
        if(monster){
            rangeIndicator.SetActive(true);
            rangeIndicator.transform.localScale = new Vector3(monster.getMonsterRange() + 5, monster.getMonsterRange() + 5, 0);
/*             
            if(monster.GetComponent<CircleCollider2D>().radius == 5){
                rangeIndicator.transform.localScale = new Vector3(10, 10, 0);
            }
            else{
                rangeIndicator.transform.localScale = new Vector3(20, 20, 0);
            }
*/
        }
    }

    //build based on what card was just dropped into the tower slot by the player
    public bool build(int cardType){
        Debug.Log("build using cardType: " + cardType);

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
        currentMonsterType = monster.getMonsterType();

        switch (cardType){
        case 0:
            //Standard Monster
            if(currentMonsterType == 0 || currentMonsterType == 1 || currentMonsterType == 2 || currentMonsterType == 3){
                Debug.Log("upgrade a standard monster");
                if(monster.increasePowerTier()){
                    powerStars[monster.currentPowerTier].SetActive(true);
                    audio.PlayOneShot(upgradeStandardMonster);
                    return true;
                }else{
                    return false;
                }
            }
            else{
                monster.powerType = "Standard";
                Debug.Log("Build a standard monster");
                powerStars[monster.currentPowerTier].SetActive(true);
                monster.gameObject.SetActive(true);
                monster.GetComponent<Monster>().setType(0);
                audio.PlayOneShot(placeStandardMonster);

                gameObject.GetComponent<SpriteRenderer>().enabled = false;
                return true;
            }
        break;

        case 1:
            //Standard Speed
            if(currentMonsterType == 4 || currentMonsterType == 5 || currentMonsterType == 6 || currentMonsterType == 7){
                if(monster.increasePowerTier()){
                    powerStars[monster.currentPowerTier].SetActive(true);
                    audio.PlayOneShot(upgradeSpeedMonster);
                    return true;
                }else{
                    return false;
                }
            }
            else{
                monster.powerType = "Speed";
                powerStars[monster.currentPowerTier].SetActive(true);
                monster.gameObject.SetActive(true);
                monster.GetComponent<Monster>().setType(4);
                audio.PlayOneShot(placeSpeedMonster);

                gameObject.GetComponent<SpriteRenderer>().enabled = false;
                return true;
            }
        break;

        case 2:
            //Standard Range
            if(currentMonsterType == 8 || currentMonsterType == 9 || currentMonsterType == 10 || currentMonsterType == 11){
                if(monster.increasePowerTier()){
                    powerStars[monster.currentPowerTier].SetActive(true);
                    audio.PlayOneShot(upgradeRangeMonster);
                    return true;
                }else{
                    return false;
                }
            }
            else{
                monster.powerType = "Range";
                powerStars[monster.currentPowerTier].SetActive(true);
                monster.gameObject.SetActive(true);
                monster.GetComponent<Monster>().setType(8);
                audio.PlayOneShot(placeRangeMonster);

                gameObject.GetComponent<SpriteRenderer>().enabled = false;
                return true;
            }
        break;

        case 3:
            //Element Fire
            if(currentMonsterType == 1 || currentMonsterType == 5 || currentMonsterType == 9){
                if(monster.increaseElementTier()){
                    elementStars[monster.currentElementTier].SetActive(true);
                    elementStars[monster.currentElementTier].GetComponent<SpriteRenderer>().color = fire;
                    return true;
                }else{
                    return false;
                }
            }
            else if(currentMonsterType >= 0 && currentMonsterType <= 3){
                
                monster.elementType = "Fire";
                monster.gameObject.SetActive(true);
                monster.GetComponent<Monster>().setType(1);
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
                
                monster.elementType = "Fire";
                monster.gameObject.SetActive(true);
                monster.GetComponent<Monster>().setType(5);
                audio.PlayOneShot(upgradeStandardMonster);
                elementStars[monster.currentElementTier].SetActive(true);
                elementStars[monster.currentElementTier].GetComponent<SpriteRenderer>().color = fire;

                gameObject.GetComponent<SpriteRenderer>().enabled = false;
                return true;
            }
            else if(currentMonsterType >= 8 && currentMonsterType <= 11){
                
                monster.elementType = "Fire";
                monster.gameObject.SetActive(true);
                monster.GetComponent<Monster>().setType(9);
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
                if(monster.increaseElementTier()){
                    elementStars[monster.currentElementTier].SetActive(true);
                    elementStars[monster.currentElementTier].GetComponent<SpriteRenderer>().color = ice;
                    return true;
                }else{
                    return false;
                }
            }
            else if(currentMonsterType >= 0 && currentMonsterType <= 3){
                
                monster.elementType = "Ice";
                monster.gameObject.SetActive(true);
                monster.GetComponent<Monster>().setType(2);
                audio.PlayOneShot(upgradeStandardMonster);
                elementStars[monster.currentElementTier].SetActive(true);
                elementStars[monster.currentElementTier].GetComponent<SpriteRenderer>().color = ice;

                gameObject.GetComponent<SpriteRenderer>().enabled = false;
                return true;
            }
            else if(currentMonsterType >= 4 && currentMonsterType <= 7){

                monster.elementType = "Ice";
                monster.gameObject.SetActive(true);
                monster.GetComponent<Monster>().setType(6);
                audio.PlayOneShot(upgradeStandardMonster);
                elementStars[monster.currentElementTier].SetActive(true);
                elementStars[monster.currentElementTier].GetComponent<SpriteRenderer>().color = ice;

                gameObject.GetComponent<SpriteRenderer>().enabled = false;
                return true;
            }
            else if(currentMonsterType >= 8 && currentMonsterType <= 11){

                monster.elementType = "Ice";
                monster.gameObject.SetActive(true);
                monster.GetComponent<Monster>().setType(10);
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
                if(monster.increaseElementTier()){
                    elementStars[monster.currentElementTier].SetActive(true);
                    elementStars[monster.currentElementTier].GetComponent<SpriteRenderer>().color = poison;
                    return true;
                }else{
                    return false;
                }
            }
            else if(currentMonsterType >= 0 && currentMonsterType <= 3){

                monster.elementType = "Poison";
                monster.gameObject.SetActive(true);
                monster.GetComponent<Monster>().setType(3);
                audio.PlayOneShot(upgradeStandardMonster);
                elementStars[monster.currentElementTier].SetActive(true);
                elementStars[monster.currentElementTier].GetComponent<SpriteRenderer>().color = poison;

                gameObject.GetComponent<SpriteRenderer>().enabled = false;
                return true;
            }
            else if(currentMonsterType >= 4 && currentMonsterType <= 7){

                monster.elementType = "Poison";
                monster.gameObject.SetActive(true);
                monster.GetComponent<Monster>().setType(7);
                audio.PlayOneShot(upgradeStandardMonster);
                elementStars[monster.currentElementTier].SetActive(true);
                elementStars[monster.currentElementTier].GetComponent<SpriteRenderer>().color = poison;

                gameObject.GetComponent<SpriteRenderer>().enabled = false;
                return true;
            }
            else if(currentMonsterType >= 8 && currentMonsterType <= 11){

                monster.elementType = "Poison";
                monster.gameObject.SetActive(true);
                monster.GetComponent<Monster>().setType(11);
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
