using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public float hitpoints;
    public GameObject drop;
    public Animator anim;
    public GameObject room;

    protected void Initialize()
    {
        gameObject.tag = "Enemy";
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (anim)
        {
            Vector2 lookDirection = GetComponent<Rigidbody2D>().velocity;
            anim.SetFloat("Look X", lookDirection.x);
            anim.SetFloat("Look Y", lookDirection.y);
            anim.SetFloat("Speed", lookDirection.magnitude);
        }
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            Destroy(other.gameObject);
            hitpoints -= other.gameObject.GetComponent<Pfeil>().damage;
            if (hitpoints <= 0)
            {
                Die();
            }
        }
    }

    protected virtual void DieCallback()
    {
        
    }
    
    protected virtual void Die()
    {
        if (drop)
        {
            Instantiate(drop, transform.position, transform.rotation);
        }
        room.GetComponent<RoomBehaviour>().RemoveEnemy(this.gameObject);
        Destroy(gameObject);
        DieCallback();
    }
}
