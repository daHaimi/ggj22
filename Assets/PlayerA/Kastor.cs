using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kastor : MonoBehaviour
{
    public GameObject pfeilPrefab;
    
    private GameControls controls;
    private float sleep = 0;
    // Start is called before the first frame update
    void Start()
    {
        
        controls = Camera.main.GetComponent<GameControls>();
    }

    // Update is called once per frame
    void Update()
    {
        sleep -= Time.deltaTime;
        GameObject nearestEnemy = Tools.GetClosest(gameObject, GameObject.FindGameObjectsWithTag("Enemy"));
        if (nearestEnemy && InRange(nearestEnemy))
        {
            if (sleep <= 0)
            {
                ShootAt(nearestEnemy);
                sleep = controls.playerCapabilities.reloadPause;
            }
        }
    }

    private bool InRange(GameObject enemy)
    {
        return controls.playerCapabilities.range >= Vector2.Distance(transform.position, enemy.transform.position);
    }

    private void ShootAt(GameObject enemy)
    {
        GameObject pfeil = Instantiate(pfeilPrefab, transform.position, Quaternion.identity);

        Collider2D pfeilCollider = pfeil.GetComponent<Collider2D>();
        Physics2D.IgnoreCollision(pfeilCollider, GetComponent<Collider2D>());
        Physics2D.IgnoreCollision(pfeilCollider, GetComponent<PlayerControls>().otherPlayer.GetComponent<Collider2D>());
        Tools.LookTowards(pfeil.transform, enemy.transform);
        Pfeil p = pfeil.GetComponent<Pfeil>();
        p.damage = controls.playerCapabilities.damage;
        p.ttl = controls.playerCapabilities.shotTTL;
        Rigidbody2D body = pfeil.GetComponent<Rigidbody2D>();
        body.freezeRotation = true;
        body.AddForce((enemy.transform.position - transform.position).normalized * controls.playerCapabilities.shotSpeed);
    }
}
