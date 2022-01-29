using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlateHandler : MonoBehaviour
{
    public RoomTileController rtc;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        rtc.StepOnPressurePlate();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        rtc.StepOffPressurePlate();
    }
}
