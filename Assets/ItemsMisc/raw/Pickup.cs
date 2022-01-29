using System.Collections;
using System.Collections.Generic;
using Models;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public PickupType type;
    public AudioClip pickupSound;
    
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<CircleCollider2D>().tag = "Pickup";
    }

    // Update is called once per frame
    void Update()
    {
        
    }   
}
