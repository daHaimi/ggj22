using System;
using System.Collections;
using System.Collections.Generic;
using SuperTiled2Unity;
using UnityEngine;

public class Pfeil : MonoBehaviour
{
    public float damage;
    public float ttl;

    private Collider2D _collider2D;
    
    // Start is called before the first frame update
    void Start()
    {
        gameObject.tag = "Bullet";
        _collider2D = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        ttl -= Time.deltaTime;
        if (ttl <= 0) Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        SuperObject sObject = collision.gameObject.GetComponent<SuperObject>();
        if (sObject && sObject.m_Type == "hole")
        {
            Physics2D.IgnoreCollision(collision.collider, collision.otherCollider);
        }
    }
}
