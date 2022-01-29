using System;
using System.Collections;
using System.Collections.Generic;
using Models;
using UnityEngine;
using Random = System.Random;

public class RoomBehaviour : MonoBehaviour
{
    public RoomType type;
    public bool virgin = true;

    private GameControls controls;
    
    // Start is called before the first frame update
    void Start()
    {
        controls = Camera.main.GetComponent<GameControls>();
    }

    public void SetRoomType(RoomType type)
    {
        this.type = type;
        if (type == RoomType.Start)
        {
            virgin = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnterRoom()
    {
        if (virgin)
        {
            var random = new Random(controls.seed);
            foreach (var col in GetComponentsInChildren<BoxCollider2D>())
            {
                if (col.gameObject.name.StartsWith("spawn"))
                {
                    Vector3 center = col.transform.position + (Vector3)(col.offset + col.size / 2 * new Vector2(1, -1));
                    virgin = false; // defloration
                    // Spawn enemies or pickups (3/1)
                    if (random.Next(0, 3) == 1)
                    {
                        // Spawn pickups; 50% coin, 25% heart, 25% key
                        PickupType puType;
                        int put = random.Next(0, 3);
                        put = 5;
                        if (put == 0) puType = PickupType.Heart;
                        else if (put == 1) puType = PickupType.Key;
                        else puType = PickupType.Coin;
                
                        Instantiate(controls.pickupPrefabs[(int) puType], center, Quaternion.identity);
                    }
                    else
                    {
                        // Spawn enemies
                        Instantiate(controls.enemyPrefabs[0], center, Quaternion.identity);
                    }
                    Destroy(col);
                }
            }
        }
        
    }
}
