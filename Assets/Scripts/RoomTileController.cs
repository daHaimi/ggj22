using System;
using System.Collections;
using System.Collections.Generic;
using SuperTiled2Unity;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomTileController : MonoBehaviour
{

    private Tilemap laserTilemap;
    private TilemapRenderer laserTilemapRenderer;
    private Array laserColiders;
    private bool laserOn = true;
    private int activePlates = 0;
    
    private float progress;
    
    // Start is called before the first frame update
    void Start()
    {
        Transform tLaser = transform.Find("Grid").Find("laser");
        if (tLaser)
        {
            laserTilemap = tLaser.GetComponent<Tilemap>();
            laserTilemapRenderer = tLaser.GetComponent<TilemapRenderer>();
        }

        Transform tLaserCollision = transform.Find("Grid").Find("collision_laser");
        if (tLaserCollision)
        {
            laserColiders = tLaserCollision.GetComponentsInChildren<Collider2D>();
        }

        Transform tPressurePlates = transform.Find("Grid").Find("pressure_plates");
        if (tPressurePlates)
        {
            foreach (Collider2D col in tPressurePlates.GetComponentsInChildren<Collider2D>())
            {
                col.isTrigger = true;
                col.gameObject.AddComponent<PressurePlateHandler>().rtc = this;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other);
    }

    // Update is called once per frame
    void Update()
    {
        progress += Time.deltaTime;
        if (laserTilemap)
        {
            Color color = laserTilemap.color;
            color.a = (float)Math.Sin(progress * 30) * 0.1f + 0.9f;
            laserTilemap.color = color;
        }
    }

    void DisableLasers()
    {
        if (!laserTilemapRenderer) return;
        laserTilemapRenderer.enabled = false;
        foreach(Collider2D lCollider in laserColiders)
        {
            lCollider.enabled = false;
        }

        laserOn = false;
    }

    void EnableLasers()
    {
        if (!laserTilemapRenderer) return;
        laserTilemapRenderer.enabled = true;
        foreach (Collider2D lCollider in laserColiders)
        {
            lCollider.enabled = true;
        }

        laserOn = true;
    }

    public void StepOnPressurePlate()
    {
        if (activePlates < 1)
        {
            DisableLasers();
        } 
        activePlates += 1;
        
        Debug.Log(activePlates);
    }

    public void StepOffPressurePlate()
    {
        activePlates -= 1;
        if (activePlates < 1)
        {
            EnableLasers();
        }
        
        Debug.Log(activePlates);
    }
}
