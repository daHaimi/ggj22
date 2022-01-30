using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using SuperTiled2Unity;
using UnityEngine;

public class Pfeil : MonoBehaviour
{
    public float damage;
    public float ttl;

    private Collider2D _collider2D;
    private GameControls controls;
    
    // Start is called before the first frame update
    void Start()
    {
        gameObject.tag = "Bullet";
        
        _collider2D = GetComponent<Collider2D>();
        controls = Camera.main.GetComponent<GameControls>();

        GameObject room = controls.GetCurRoomGameObject();
        foreach (Collider2D collider in room.GetComponentsInChildren<Collider2D>())
        {
            SuperObject sObject = collider.gameObject.GetComponent<SuperObject>();
            if (sObject && sObject.m_Type == "hole")
            {
                Physics2D.IgnoreCollision(this._collider2D, collider);
            }
        }
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
        // if (sObject && sObject.m_Type == "hole")
        // {
        //     Physics2D.IgnoreCollision(collision.collider, collision.otherCollider);
        // }
    }
}
