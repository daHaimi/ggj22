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
        if (InRange(nearestEnemy))
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
        Tools.LookTowards(pfeil.transform, enemy.transform);
        pfeil.GetComponent<Pfeil>().damage = controls.playerCapabilities.damage;
        pfeil.GetComponent<Pfeil>().ttl = controls.playerCapabilities.shotTTL;
        pfeil.GetComponent<Rigidbody2D>().AddForce((enemy.transform.position - transform.position).normalized * controls.playerCapabilities.shotSpeed);
    }
}
