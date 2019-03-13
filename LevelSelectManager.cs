using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectManager : MonoBehaviour
{
    public Sprite[] levelPreviews;
    public GameObject levelPreview;
    private int selectedLevel;
    public UnityEngine.UI.Button buttonPreviousLevel;
    public UnityEngine.UI.Button buttonNextLevel;
    public UnityEngine.UI.Button buttonLoadLevel;
    
    // Start is called before the first frame update
    void Start()
    {
        selectedLevel = 1;
        buttonPreviousLevel.onClick.AddListener(() => {changeSelectedLevel(false);});
        buttonNextLevel.onClick.AddListener(() => {changeSelectedLevel(true);});
        buttonLoadLevel.onClick.AddListener(() => {loadLevel();});
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void changeSelectedLevel(bool cycleForward){
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
    }

    public void loadLevel(){
        Debug.Log("load scene:" + selectedLevel);
        SceneManager.LoadScene(selectedLevel);
    }

}
