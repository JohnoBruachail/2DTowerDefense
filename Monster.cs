﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

    /*
        Monster Type denotes what prefab to use for monster
        0 Purple monster
        1 Fire monster
        2 Fire speed monster
        3 Fire range monster
        4 Ice monster
        5 Ice speed monster
        6 Ice range monster
        7 Poison monster
        8 Poison speed monster
        9 Poison range monster
    */

[System.Serializable]
public class MonsterType
{
    public GameObject visualization;
    public PowerTier[] powerTier;
    public ElementTier[] elementTier;
}

[System.Serializable]
public class PowerTier
{
    public float fireRate;
    public int range;
}

[System.Serializable]
public class ElementTier
{
    public GameObject bullet;
}

public class Monster : MonoBehaviour
{
    public List<MonsterType> types;
    //[HideInInspector]
    public int currentPowerTier;
    //[HideInInspector]
    public int currentElementTier;
    public CircleCollider2D rangeCollider;
    private MonsterType currentType;
    [HideInInspector]
    public string powerType;
    [HideInInspector]
    public string elementType;

    public MonsterType CurrentType
    {
        get
        {
            return currentType;
        }
        set
        {
            currentType = value;
            int currentTypeIndex = types.IndexOf(currentType);

            GameObject levelVisualization = types[currentTypeIndex].visualization;
            for (int i = 0; i < types.Count; i++)
            {
                if (levelVisualization != null)
                {
                    if (i == currentTypeIndex)
                    {
                        types[i].visualization.SetActive(true);
                    }
                    else
                    {
                        types[i].visualization.SetActive(false);
                    }
                }
            }
        }
    }

    // Use this for initialization
    void Start()
    {
        currentPowerTier = 0;
        currentElementTier = 0;
        elementType = "None";
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetType(int set){
        //Debug.Log("Change to type: " + set);
        CurrentType = types[set];
    }

    public int GetMonsterType(){
        return types.IndexOf(currentType);
    }

    public int GetMonsterRange(){
        return types[types.IndexOf(currentType)].powerTier[currentPowerTier].range;
    }

    public void UpdateMonsterRange(){
        //when a new type is set this should update the colliders radius to the correct range
        rangeCollider.radius = types[types.IndexOf(currentType)].powerTier[currentPowerTier].range;
    }

    public bool IncreasePowerTier()
    {
        Debug.Log("currentPowerTier: " + currentPowerTier);
        if(currentPowerTier < 2)
        {
            currentPowerTier++;
            return true;
        }
        else{
            return false;
        }
    }

    public bool IncreaseElementTier()
    {
        if(currentElementTier < CurrentType.elementTier.Length)
        {
            currentElementTier++;
            return true;
        }
        else{
            return false;
        }
    }
}
