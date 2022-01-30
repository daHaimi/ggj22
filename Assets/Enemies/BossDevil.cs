using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Random = System.Random;

public class BossDevil : EnemyBase
{
    private Random random;
    private GameControls controls;

    private Vector2 targetPosition;
    private BossAction curAction;
    private float actionTimer;
    
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
        controls = Camera.main.GetComponent<GameControls>();
        random = new Random(controls.seed);
    }

    // Update is called once per frame
    void Update()
    {
        actionTimer -= Time.deltaTime;
        if (actionTimer <= 0)
        {
            // Wait if prev. ACtion was active
            if (curAction != BossAction.Wait)
            {
                Destroy(gameObject.GetComponent<LineRenderer>());
                if (curAction == BossAction.Jump)
                {
                    GetComponent<Rigidbody2D>().MovePosition(targetPosition);
                }
                else
                {
                    GameObject add = Instantiate(controls.enemyPrefabs[random.Next(0, controls.enemyPrefabs.Count)]);
                    add.GetComponent<Rigidbody2D>().MovePosition(targetPosition);
                }
                curAction = BossAction.Wait;
            }
            else
            {
                targetPosition = GetRandomTarget();
                PointToTarget();
            }
            actionTimer = 1.5f;
        }
    }

    Vector2 GetRandomTarget()
    {
        Vector3 result;
        Collider2D[] roomColliders = controls.rooms[controls.curRoom].GetComponentsInChildren<Collider2D>();
        do
        {
            float angle = random.Next(0, 359);
            float dist = (float) (random.NextDouble() * 2f + 1f);
            result = Quaternion.Euler(0, 0, angle).eulerAngles.normalized * dist;
        } while (roomColliders.Any(col => col.bounds.Contains(result)));
        return result;
    }
    
    void PointToTarget()
    {
        LineRenderer l = gameObject.AddComponent<LineRenderer>();
        List<Vector3> pos = new List<Vector3>();
        pos.Add(transform.position);
        pos.Add(targetPosition);
        l.startWidth = 0.1f;
        l.endWidth = 0.1f;
        l.SetPositions(pos.ToArray());
        l.useWorldSpace = true;
    }
}

internal enum BossAction
{
    Jump,
    Spawn,
    Wait
}
