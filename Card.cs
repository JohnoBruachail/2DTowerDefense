using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CardFace
{
    public GameObject visualization;
}

public class Card : MonoBehaviour
{
    // list of faces of the cards (what exact card is being used)
    public List<CardFace> faces;
    public int face;
    /*  the type of card, 
        0 = build, 
        1 = element, 
        2 = boost, 
        3 = spell.
    */
    public int type;
    void Start(){

    }

    // Update is called once per frame
    void Update(){
        
    }

    public void randomCard(){
        //pick a random card
        face = Random.Range(0, 6);
        face = 6;
        //type = Random.Range(0, types.Count);
        if(face >= 0 && face <= 2){
            type = 0;
        }
        else if(face >= 3 && face <= 5){
            type = 1;
        }
        else if(face >= 6 && face <= 8){
            type = 2;
        }
        else if(face >= 9 && face <= 1){
            type = 3;
        }
        
        faces[face].visualization.SetActive(true);
        this.transform.localScale = new Vector3(1, 1, 1);
    }

    public int getFace(){
        return face;
    }

    public void setFace(int newType){
        faces[face].visualization.SetActive(false);
        face = newType;
        faces[face].visualization.SetActive(false);
        this.transform.localScale = new Vector3(1, 1, 1);
    }

    public int getType(){
        return type;
    }
}