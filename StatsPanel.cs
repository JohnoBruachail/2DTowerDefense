using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsPanel : MonoBehaviour
{
    [HideInInspector]
    public Tower selectedTower;
    public GameObject monsterType;
    public GameObject monsterImage;
    public GameObject monsterStats;
    private Animator panelAnimator;
    private bool isVisible;


    // Start is called before the first frame update
    void Start()
    {
        panelAnimator = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectTower(){
        if(selectedTower != null){
            //turn off selection circle and range indicator
            selectedTower.selectionCircle.SetActive(false);
            selectedTower.rangeIndicator.SetActive(false);
            selectedTower = null;
            panelAnimator.SetBool("MakeVisible", false);
        }
    }

    public void SelectTower(Tower tower){

        //Debug.Log("firerate: " + tower.monster.CurrentType.powerTier[tower.monster.currentPowerTier].fireRate);
        //Debug.Log("range: " + tower.monster.CurrentType.powerTier[tower.monster.currentPowerTier].range);
        //Debug.Log("elementtype: " + tower.monster.CurrentType.elementType);


        monsterType.GetComponentInChildren<Text>().text = "Monster: " + tower.monster.powerType;
        //selecting a tower will now show its range and stats in a side panel that comes into view from the left.
        monsterStats.GetComponentInChildren<Text>().text = "APS: " + tower.monster.CurrentType.powerTier[tower.monster.currentPowerTier].fireRate + " \n"
                                                       + "Range: " + tower.monster.CurrentType.powerTier[tower.monster.currentPowerTier].range + " \n"
                                                       + "Element: " + tower.monster.elementType + " \n"
        ;
        //based on element we need either
        //extra fire damage
        //frozen time
        //poison damage per second
        if(tower.monster.elementType == "Fire"){
            monsterStats.GetComponentInChildren<Text>().text += "Extra Damage";
        }else if(tower.monster.elementType == "Ice"){
            monsterStats.GetComponentInChildren<Text>().text += "Freeze Time";
        }else if(tower.monster.elementType == "Poison"){
            monsterStats.GetComponentInChildren<Text>().text += "Poison Damage Per Second";
        }




        //cleanup on old selected tower
        if(selectedTower != null){
            SelectTower();
        }
            selectedTower = tower;

            isVisible = panelAnimator.GetBool("MakeVisible");
            panelAnimator.SetBool("MakeVisible", true);
            //must make certain buttons invisable based on tier lvl of tower.
    }
}
