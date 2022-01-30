using System;
using System.Collections;
using System.Collections.Generic;
using Models;
using SuperTiled2Unity;
using SuperTiled2Unity.Editor;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = System.Random;

public class RoomBehaviour : MonoBehaviour
{
    public RoomType type;
    public bool virgin = true;

    private int enemyCount;
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

    public void RemoveEnemy(GameObject enemyGo)
    {
        enemyCount -= 1;
        if (enemyCount < 1)
        {
            OpenRoom();
        }
    }


    public void OpenRoom()
    {
        Tilemap tm = this.gameObject.GetComponentsInChildren<Tilemap>()[1];
        controls = Camera.main.GetComponent<GameControls>();
        SuperTileset tileSet = controls.roomTileset;
        tileSet.TryGetTile(166, out SuperTile door_o_n);
        tileSet.TryGetTile(201, out SuperTile door_o_e);
        tileSet.TryGetTile(198, out SuperTile door_o_w);
        for (int i = 8; i <= 9; i++) tm.SetTile(new Vector3Int(i, 0, 0), door_o_n);
        for (int i = 8; i <= 9; i++) tm.SetTile(new Vector3Int(i, -13, 0), door_o_n);
        for (int i = -7; i <= -6; i++) tm.SetTile(new Vector3Int(0, i, 0), door_o_w);
        for (int i = -7; i <= -6; i++) tm.SetTile(new Vector3Int(17, i, 0), door_o_e);
        foreach (Collider2D col in tm.gameObject.GetComponents<Collider2D>())
        {
            Destroy(col);
        }

    }
    

    public void EnterRoom()
    {
        if (virgin)
        {
            if (type == RoomType.Boss)
            {
                Vector3 center = transform.position + (Vector3)(controls.roomSize / 2 * new Vector2(1, -1));
                Instantiate(controls.bossPrefabs[0], center, Quaternion.identity);
            }
            else
            {
                var random = new Random(controls.seed);
                foreach (var col in GetComponentsInChildren<BoxCollider2D>())
                {
                    if (col.gameObject.name.StartsWith("spawn"))
                    {
                        Vector3 center = col.transform.position + (Vector3)(col.offset + col.size / 2 * new Vector2(1, -1));
                        virgin = false; // defloration
                        // Spawn enemies or pickups (3/1)
                        if (random.Next(0, 3) == 0)
                        {
                            // Spawn pickups; 50% coin, 25% heart, 25% key
                            PickupType puType;
                            int put = random.Next(0, 3);
                            put = 5;
                            if (put == 0 || put == 1) puType = PickupType.Heart;
                            else if (put == 1) puType = PickupType.Key;
                            else puType = PickupType.Coin;
                    
                            var pickup = Instantiate(controls.pickupPrefabs[(int) puType], center, Quaternion.identity);
                        }
                        else
                        {
                            // Spawn enemies
                            //Instantiate(controls.enemyPrefabs[1], center, Quaternion.identity);
                            var enemy = Instantiate(controls.enemyPrefabs[random.Next(0, controls.enemyPrefabs.Count)], center, Quaternion.identity);
                            enemyCount += 1;
                            enemy.GetComponent<EnemyBase>().room = this.gameObject;
                        }
                        Destroy(col);
                    }
                }
            }
        }
    }
}
