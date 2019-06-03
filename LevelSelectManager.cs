using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectManager : MonoBehaviour
{
    
    public Sprite[] levelPreviews;
    public GameObject levelLabel;
    public GameObject levelPreview;
    private int selectedLevel;
    public UnityEngine.UI.Button buttonPreviousLevel;
    public UnityEngine.UI.Button buttonNextLevel;
    public UnityEngine.UI.Button buttonLoadLevel;
    
    // Start is called before the first frame update
    void Start()
    {
        selectedLevel = 1;
        buttonPreviousLevel.onClick.AddListener(() => {ChangeSelectedLevel(false);});
        buttonNextLevel.onClick.AddListener(() => {ChangeSelectedLevel(true);});
        buttonLoadLevel.onClick.AddListener(() => {LoadLevel();});
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    //I require a method to swap the level label between selections based on the level changed too.

    //levelLabel.GetComponentInChildren<Text>().text = "Level 1: " + New Beginnings;
    void ChangeLevelName(int level){
        switch (level)
        {
            case 1:
                levelLabel.GetComponentInChildren<Text>().text = "Level 1: Just around the bend";
            break;
            case 2:
                levelLabel.GetComponentInChildren<Text>().text = "Level 2: Split";
            break;
            case 3:
                levelLabel.GetComponentInChildren<Text>().text = "Level 3: Corkscrew";
            break;
            case 4:
                levelLabel.GetComponentInChildren<Text>().text = "Level 4: Divided Rush";
            break;
            case 5:
                levelLabel.GetComponentInChildren<Text>().text = "Level 5: Crossroads";
            break;
            case 6:
                levelLabel.GetComponentInChildren<Text>().text = "Level 6: Peasant Push";
            break;
            case 7:
                levelLabel.GetComponentInChildren<Text>().text = "Level 7: Convergence";
            break;
            case 8:
                 levelLabel.GetComponentInChildren<Text>().text = "Level 8: I'm lovin it";
            break;
            case 9:
                 levelLabel.GetComponentInChildren<Text>().text = "Level 9: Twisted Meeting";
            break;
            case 10:
                 levelLabel.GetComponentInChildren<Text>().text = "Level 10: All good things";
            break;
        }
    }

    public void ChangeSelectedLevel(bool cycleForward){
        //cycleForward is true for cycle forward through levels and false to cycle backwards.
        //selected lvl cannot be 0 as thats the main menu
        //it has to cycle between 1 and 20 if their are 20 levels

        if(cycleForward == true){
            selectedLevel++;
        }else{
            selectedLevel--;
        }
        Debug.Log("selected lvl is: " + selectedLevel);
        if(selectedLevel <= 0){
            selectedLevel = levelPreviews.Length;
        }
        else if(selectedLevel > levelPreviews.Length){
            selectedLevel = 1;
        }
        Debug.Log("selected lvl is now changed too: " + selectedLevel);
        levelPreview.GetComponent<Image>().sprite = levelPreviews[selectedLevel-1];
        ChangeLevelName(selectedLevel);
    }

    public void LoadLevel(){
        Debug.Log("load scene:" + selectedLevel);
        SceneManager.LoadScene(selectedLevel);
    }

}
