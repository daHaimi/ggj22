using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pfeil : MonoBehaviour
{
    public float damage;
    public float ttl;
    
    // Start is called before the first frame update
    void Start()
    {
        gameObject.tag = "Bullet";
    }

    // Update is called once per frame
    void Update()
    {
        ttl -= Time.deltaTime;
        if (ttl <= 0) Destroy(gameObject);
    }
}
