using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSlotTrigger : MonoBehaviour
{
    public Tower selectedTower;
    private Color normal;
    private Color highlight;
    // Start is called before the first frame update
    void Start()
    {
        normal = this.GetComponent<SpriteRenderer>().color;
        highlight = new Color(145,255,120,50);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
 
    void OnTriggerEnter2D(Collider2D col){
        //Debug.Log("OnTriggerEnter2D");
        if (col.gameObject.tag == "Card"){
            Debug.Log(this.name + " has collided with the card");
            this.GetComponent<SpriteRenderer>().color = highlight;
            
        }
    }
    void OnTriggerExit2D(Collider2D col){
        //Debug.Log("OnTriggerExit2D");
        if (col.gameObject.tag == "Card"){
            Debug.Log(this.name + " has exited with the card");
            this.GetComponent<SpriteRenderer>().color = normal;
        }
    }
}
