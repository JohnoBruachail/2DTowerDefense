using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

    Transform handPanel = null;
    bool inCardSlot;
    bool blockRaycasts;
    private Tower tower;
    private GameManagerBehaviour gameManager;
    public AudioClip cardPickup;
    public AudioClip cardDrop;
    AudioSource audio;

    void Start(){
        gameManager = GameObject.Find("GameManager").GetComponent<GameManagerBehaviour>();
        audio = GetComponent<AudioSource>();
        blockRaycasts = GetComponent<CanvasGroup>().blocksRaycasts;
    }

    public void OnBeginDrag(PointerEventData pEventData){
        //Debug.Log("OnBeginDrag");

        audio.PlayOneShot(cardPickup);

        gameManager.cardInHand(true);

        handPanel = this.transform.parent;
        this.transform.SetParent(this.transform.parent.parent);
        this.transform.localScale = new Vector3(2f, 2f, 1);

        GetComponent<Collider2D>().enabled = true;
        blockRaycasts = false;
    }
    public void OnDrag(PointerEventData pEventData){
        //Debug.Log("OnDrag");

        var screenPoint = Input.mousePosition;
        screenPoint.z = 10.0f; //distance of the plane from the camera
        transform.position = Camera.main.ScreenToWorldPoint(screenPoint);
    }

    public void OnEndDrag(PointerEventData pEventData){
        //Debug.Log("OnEndDrag");

        audio.PlayOneShot(cardDrop);

        gameManager.cardInHand(false);

        //if the card is hovering over a part of the map you can place a tower then highlight it.
        this.transform.localScale = new Vector3(1, 1, 1);
        blockRaycasts = true;
        //Debug.Log("inCardSlot: " + inCardSlot);
        //this will be triggered if the object doesnt hit a collder
        if(inCardSlot){
            //THIS IS THE TRIGGER TO DESTROY THIS CARD AND UPGRADE THE TOWER APPROPRIATELY;
            int cardType = this.transform.GetComponent<Card>().getType();
            if(tower.build(cardType)){
                Destroy(gameObject);
            }
            else{
                GetComponent<Collider2D>().enabled = false;
            this.transform.SetParent(handPanel);
            }
        }
        else{
            GetComponent<Collider2D>().enabled = false;
            this.transform.SetParent(handPanel);  
        }
        
    }

    void OnTriggerEnter2D(Collider2D col){
        Debug.Log("OnTriggerEnter2D");
        if (col.gameObject.tag == "CardViewer"){
            Debug.Log(this.name + " has collided with the card viewer");
            this.transform.localScale = new Vector3(2f, 2f, 1);
        }
        else if (col.gameObject.tag == "CardSlot"){
            Debug.Log(this.name + " has collided with the card slot");
            tower = col.GetComponentInParent<Tower>();
            inCardSlot = true;
        }
    }
    void OnTriggerExit2D(Collider2D col){
        Debug.Log("OnTriggerExit2D");
        if (col.gameObject.tag == "CardViewer"){
            Debug.Log(this.name + " has left collision with the card viewer");
            this.transform.localScale = new Vector3(0.5f, 0.5f, 1);
        }
        else if (col.gameObject.tag == "CardSlot"){
            Debug.Log(this.name + " has left collision with the card slot");
            tower = col.GetComponentInParent<Tower>();
            inCardSlot = true;
        }
    }
}
