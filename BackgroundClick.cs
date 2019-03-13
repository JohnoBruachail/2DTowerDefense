using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundClick : MonoBehaviour
{
    // Start is called before the first frame update
    public StatsPanel statsPanel;
    
    void OnMouseUp()
    {
        Debug.Log("Background pressed");
        //may need a circle to indicate this is selected
        statsPanel.SelectTower();
    }
}
