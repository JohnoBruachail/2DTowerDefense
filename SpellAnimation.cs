using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellAnimation : MonoBehaviour
{
    //list of objects with each spell animation
    //each animation has a state from passive to active
    //on click the animation will trigger its active animation and then turn off
    public GameObject[] spellAnimations;

    //takes the spell type and activates the approperate animation

    // Update is called once per frame
    void Update(){
        var screenPoint = Input.mousePosition;
        screenPoint.z = 10.0f; //distance of the plane from the camera
        transform.position = Camera.main.ScreenToWorldPoint(screenPoint);
    }

    public void ActivateAnimation(int spell){

        spellAnimations[spell].SetActive(true);
    }
}
