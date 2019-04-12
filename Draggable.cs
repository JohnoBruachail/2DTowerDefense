using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

    Transform handPanel = null;
    bool inCardSlot;
    bool inDropZone;
    private Tower tower;
    private GameManagerBehaviour gameManager;
    public AudioClip cardPickup;
    public AudioClip cardDrop;
    AudioSource audio;
    int cardFace;
    int cardType;
    private int offset = 50;

    void Start(){
        gameManager = GameObject.Find("GameManager").GetComponent<GameManagerBehaviour>();
        audio = GetComponent<AudioSource>();
    }

    public void OnBeginDrag(PointerEventData pEventData){

        cardFace = this.transform.GetComponent<Card>().GetFace();
        cardType = this.transform.GetComponent<Card>().GetType();
        //Debug.Log("OnBeginDrag");

        audio.PlayOneShot(cardPickup);
        gameManager.CardInHand(true, cardType);

        handPanel = this.transform.parent;
        this.transform.SetParent(this.transform.parent.parent);
        this.transform.localScale = new Vector3(2f, 2f, 1);

        GetComponent<Collider2D>().enabled = true;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }
    public void OnDrag(PointerEventData pEventData){
        //Debug.Log("OnDrag");

        var screenPoint = Input.mousePosition;
        screenPoint.z = 10.0f; //distance of the plane from the camera

        //just a basic IF the screenpoint is outside of a certain x y coordinate then move, other then that, dont.
        
        //if(Input.mousePosition.x >= offset && Input.mousePosition.x <= (Screen.width - offset) && Input.mousePosition.y >= offset && Input.mousePosition.y >= (Screen.width - offset)){
            Debug.Log("Inside movement zone, Now moving the card");
            Debug.Log("Input.mousePosition.x" + Input.mousePosition.x);
            Debug.Log("Input.mousePosition.y" + Input.mousePosition.y);
            Debug.Log("Camera.main.ScreenToWorldPoint(screenPoint)" + Camera.main.ScreenToWorldPoint(screenPoint));
            Debug.Log("offset" + offset);
            Debug.Log("Screen.width" + Screen.width);
        //}
        //transform.position = Camera.main.ScreenToWorldPoint(screenPoint);

        if(Input.mousePosition.y <= 170){
            this.transform.localScale = new Vector3(2f, 2f, 1);
            screenPoint.y = 170.0f;
            transform.position = Camera.main.ScreenToWorldPoint(screenPoint);
        }
        else{
            this.transform.localScale = new Vector3(0.5f, 0.5f, 1);
            transform.position = Camera.main.ScreenToWorldPoint(screenPoint);
        }

    }

    public void OnEndDrag(PointerEventData pEventData){
        //Debug.Log("OnEndDrag");

        audio.PlayOneShot(cardDrop);
        gameManager.CardInHand(false, 0);

        //if the card is hovering over a part of the map you can place a tower then highlight it.
        this.transform.localScale = new Vector3(1, 1, 1);
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        //Debug.Log("inCardSlot: " + inCardSlot);
        //this will be triggered if the object doesnt hit a collder
        if(cardType == 0 || cardType == 1){
            if(inCardSlot){
                //THIS IS THE TRIGGER TO DESTROY THIS CARD AND UPGRADE THE TOWER APPROPRIATELY;
                if(tower.Build(cardFace)){
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
    }

    void OnTriggerEnter2D(Collider2D col){
        Debug.Log("OnTriggerEnter2D");
        if (col.gameObject.tag == "CardSlot"){
            Debug.Log(this.name + " has collided with the card slot");
            tower = col.GetComponentInParent<Tower>();
            inCardSlot = true;
        }
    }
    void OnTriggerExit2D(Collider2D col){
        Debug.Log("OnTriggerExit2D");
        if (col.gameObject.tag == "CardSlot"){
            Debug.Log(this.name + " has left collision with the card slot");
            tower = col.GetComponentInParent<Tower>();
            inCardSlot = true;
        }
    }
}
