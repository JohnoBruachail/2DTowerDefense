using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CardType
{
    public GameObject visualization;
}

public class Card : MonoBehaviour
{
    // Start is called before the first frame update
    public List<CardType> types;
    public int type = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void randomCard(){
        //pick a random card
        type = Random.Range(0, 5);
        //type = Random.Range(0, types.Count);
        types[type].visualization.SetActive(true);
        this.transform.localScale = new Vector3(1, 1, 1);
    }

    public int getType(){
        return type;
    }

    public void setType(int newType){
        types[type].visualization.SetActive(false);
        type = newType;
        types[type].visualization.SetActive(false);
        this.transform.localScale = new Vector3(1, 1, 1);
    }
}