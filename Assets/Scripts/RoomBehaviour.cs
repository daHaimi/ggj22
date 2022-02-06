using System;
using System.Collections;
using System.Collections.Generic;
using Models;
using SuperTiled2Unity;
using UnityEngine;
using UnityEngine.Tilemaps;

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
        TileBase door_o_n = controls.GetHackTile(4);
        TileBase door_o_e = controls.GetHackTile(5);
        TileBase door_o_w = controls.GetHackTile(6);
        if (tm2.GetTile(new Vector3Int(8, 0, 0)))
            for (int i = 8; i <= 9; i++) tm2.SetTile(new Vector3Int(i, 0, 0), door_o_n);
        if (tm2.GetTile(new Vector3Int(8, -13, 0)))
            for (int i = 8; i <= 9; i++) tm2.SetTile(new Vector3Int(i, -13, 0), door_o_n);
        if (tm2.GetTile(new Vector3Int(0, -7, 0)))
            for (int i = -7; i <= -6; i++) tm2.SetTile(new Vector3Int(0, i, 0), door_o_w);
        if (tm2.GetTile(new Vector3Int(17, -7, 0)))
            for (int i = -7; i <= -6; i++) tm2.SetTile(new Vector3Int(17, i, 0), door_o_e);
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
            var props = col.gameObject.GetComponent<SuperCustomProperties>();
            int num = 1;
            CustomProperty numProp;
            if (props.TryGetCustomProperty("count", out numProp))
            {
                num = numProp.GetValueAsInt();
            }
            for (int i = 0; i < num; i++)
            {
                Vector3 center = col.transform.position + new Vector3(
                    controls.GetRandom((int)col.size.x),
                    controls.GetRandom((int)col.size.y) * -1,
                    0
                );
                virgin = false; // defloration
                
                // Spawn enemies or pickups (7/1)
                if (type == RoomType.Boss || controls.GetRandom(6) > 1)
                {
                    GameObject prefab;
                    prefab = type == RoomType.Boss
                        ? controls.bossPrefabs[0]
                        : controls.enemyPrefabs[controls.GetRandom(controls.enemyPrefabs.Count)];

                    var enemy = Instantiate(prefab, center, Quaternion.identity);
                    enemyCount += 1;
                    enemy.GetComponent<EnemyBase>().room = this.gameObject;
                }
                else
                {
                    // Spawn pickups; 50% coin, 25% heart, 25% key
                    PickupType puType;
                    int put = controls.GetRandom(4);
                    if (put == 0 || put == 1) puType = PickupType.Coin;
                    else puType = PickupType.Heart;

                    Instantiate(controls.pickupPrefabs[(int) puType], center, Quaternion.identity);
                }

                Destroy(col);
            }
        }

        if (enemyCount == 0)
        {
            OpenRoom();
        }
    }
}
