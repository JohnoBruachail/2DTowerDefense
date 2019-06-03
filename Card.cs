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

    public void RandomCard(){

        //pick a random type, we do this to even card selection distrubution by type
        type = Random.Range(0, 3);

        if(type == 0){
            face = Random.Range(0, 3);

        }else if(type == 1){
            face = Random.Range(3, 6);

        }else if(type == 2){
            face = Random.Range(6, 9);

        }else if(type == 3){
            face = Random.Range(9, 13);

        }
    
        faces[face].visualization.SetActive(true);
        this.transform.localScale = new Vector3(1, 1, 1);
    }

    public int GetFace(){
        return face;
    }

    public void SetFace(int newType){
        faces[face].visualization.SetActive(false);
        face = newType;
        faces[face].visualization.SetActive(false);
        this.transform.localScale = new Vector3(1, 1, 1);
    }

    public int GetType(){
        return type;
    }
}