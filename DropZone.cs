using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IPointerEnterHandler, IDropHandler, IPointerExitHandler
{
    private GameManagerBehaviour gameManager;
    void Start(){
        gameManager = GameObject.Find("GameManager").GetComponent<GameManagerBehaviour>();
    }

    public void OnPointerEnter(PointerEventData pEventData){
        Debug.Log("OnPointerEnter");
    }

    public void OnDrop(PointerEventData pEventData){
        Debug.Log(pEventData.pointerDrag.name + "was dropped on " + gameObject.name);

        Card card = pEventData.pointerDrag.GetComponent<Card>();
        
        gameManager.ActivateCard(card.GetFace());
        Destroy(card.gameObject);
        gameObject.SetActive(false);
    }
    public void OnPointerExit(PointerEventData pEventData){
        Debug.Log("OnPointerExit");
    }
}
