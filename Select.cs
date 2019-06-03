using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Select : MonoBehaviour
{
    [SerializeField]
    private LayerMask selectablesLayer;
    GameObject selectedObject;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)){
            Debug.Log("Mouse button pressed");
            RaycastHit rayHit;

            if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out rayHit, Mathf.Infinity, selectablesLayer)){

                Debug.Log("Raycast has hit");
                if(selectedObject != null){
                    selectedObject.GetComponent<Tower>().DeselectMe();
                }
                selectedObject = rayHit.collider.transform.parent.gameObject;
                rayHit.collider.transform.parent.GetComponent<Tower>().SelectMe();
            }
            else{
                selectedObject.GetComponent<Tower>().DeselectMe();
            }
        }
    }
}
