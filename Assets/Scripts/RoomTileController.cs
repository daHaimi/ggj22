using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomTileController : MonoBehaviour
{

    public Tilemap laserTilemap;

    private float progress;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        progress += Time.deltaTime;
        Color color = laserTilemap.color;
        color.a = (float) Math.Sin(progress * 30) * 0.1f + 0.9f;
        laserTilemap.color = color;
    }
}
