using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSlotTrigger : MonoBehaviour
{
    private Color32 normal;
    private Color32 highlight;
    // Start is called before the first frame update
    void Start()
    {
        normal = new Color32(255,255,255,50);
        highlight = new Color32(255,255,255,180);

        this.GetComponent<SpriteRenderer>().color = normal;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
 
    void OnTriggerEnter2D(Collider2D col){
        //Debug.Log("OnTriggerEnter2D");
        if (col.gameObject.tag == "Card"){
            //Debug.Log(this.name + " has collided with the card");
            //Debug.Log("Change Color too: " + highlight);
            this.GetComponent<SpriteRenderer>().color = highlight;
        }
    }
    void OnTriggerExit2D(Collider2D col){
        //Debug.Log("OnTriggerExit2D");
        if (col.gameObject.tag == "Card"){
            //Debug.Log(this.name + " has exited with the card");
            //Debug.Log("Change Color too: " + normal);
            this.GetComponent<SpriteRenderer>().color = normal;
        }
    }
}
