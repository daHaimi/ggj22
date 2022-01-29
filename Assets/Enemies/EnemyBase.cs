using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public float hitpoints;
    public GameObject drop;

    protected void Initialize()
    {
        gameObject.tag = "Enemy";
    }

    // Update is called once per frame
    void Update()
    {
        
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

    private void Die()
    {
        if (drop)
        {
            Instantiate(drop, transform.position, transform.rotation);
        }
        Destroy(gameObject);
    }
}
