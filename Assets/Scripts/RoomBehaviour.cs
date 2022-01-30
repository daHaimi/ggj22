using System;
using System.Collections;
using System.Collections.Generic;
using Models;
using SuperTiled2Unity;
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
        // Tilemap tm1 = this.gameObject.GetComponentsInChildren<Tilemap>()[0];
        Tilemap tm2 = this.gameObject.GetComponentsInChildren<Tilemap>()[1];
        
        controls = Camera.main.GetComponent<GameControls>();
        // SuperTileset tileSet = controls.roomTileset;
        // tileSet.TryGetTile(166, out SuperTile door_o_n);
        // tileSet.TryGetTile(201, out SuperTile door_o_e);
        // tileSet.TryGetTile(198, out SuperTile door_o_w); 
        // if (tm2.GetTile(new Vector3Int(8, 0, 0)))
            // for (int i = 8; i <= 9; i++) tm2.SetTile(new Vector3Int(i, 0, 0), door_o_n);
        // if (tm2.GetTile(new Vector3Int(8, -13, 0)))
            // for (int i = 8; i <= 9; i++) tm2.SetTile(new Vector3Int(i, -13, 0), door_o_n);
        // if (tm2.GetTile(new Vector3Int(0, -7, 0)))
            // for (int i = -7; i <= -6; i++) tm2.SetTile(new Vector3Int(0, i, 0), door_o_w);
        // if (tm2.GetTile(new Vector3Int(17, -7, 0)))
            // for (int i = -7; i <= -6; i++) tm2.SetTile(new Vector3Int(17, i, 0), door_o_e);
        foreach (Collider2D col in tm2.gameObject.GetComponents<Collider2D>())
        {
            Destroy(col);
        }

    }
    

    public void EnterRoom()
    {
        if (!virgin) return;
        foreach (var col in GetComponentsInChildren<BoxCollider2D>())
        {
            if (!col.gameObject.name.StartsWith("spawn")) continue;
            Vector3 center = col.transform.position + (Vector3)(col.offset + col.size / 2 * new Vector2(1, -1));
            Debug.Log(center);
            virgin = false; // defloration
            // Spawn enemies or pickups (3/1)
            Debug.Log(type);
            Debug.Log(controls.bossPrefabs[0]);
            
            if (type == RoomType.Boss || controls.GetRandom(4) > 1)
            {
                GameObject prefab;
                prefab = type == RoomType.Boss ? controls.bossPrefabs[0] : controls.enemyPrefabs[controls.GetRandom(controls.enemyPrefabs.Count) - 1];
                // Spawn enemies
                //Instantiate(controls.enemyPrefabs[1], center, Quaternion.identity);
                
                var enemy = Instantiate(prefab, center, Quaternion.identity);
                enemyCount += 1;
                enemy.GetComponent<EnemyBase>().room = this.gameObject;
            } else {
                // Spawn pickups; 50% coin, 25% heart, 25% key
                PickupType puType;
                int put = controls.GetRandom(3);
                if (put == 0 || put == 1) puType = PickupType.Coin;
                else if (put == 2) puType = PickupType.Heart;
                else puType = PickupType.Key;
                
                Instantiate(controls.pickupPrefabs[(int) puType], center, Quaternion.identity);
            }
            Destroy(col);
        }

        if (enemyCount == 0)
        {
            OpenRoom();
        }
    }
}
