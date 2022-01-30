using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
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
            // Wait if prev. Action was active
            if (curAction != BossAction.Wait)
            {
                gameObject.GetComponent<LineRenderer>().enabled = false;
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
                curAction = random.Next(0, 1) == 0 ? BossAction.Jump : BossAction.Spawn;
            }
            actionTimer = 1.5f;
        }
    }

    Vector2 GetRandomTarget()
    {
        Vector3 result;
        GameObject curRoom = controls.rooms[controls.curRoom];
        Collider2D[] roomColliders = curRoom.GetComponentsInChildren<Collider2D>();
        var center = curRoom.transform.position + (Vector3)(controls.roomSize / 2 * new Vector2(1, -1));
        do
        {
            float angle = random.Next(0, 359);
            float distX = (float) (random.NextDouble() + 1f);
            float distY = (float) (random.NextDouble() + 1f);
            result = center + new Vector3(distX, distY);
        } while (roomColliders.Any(col => col.bounds.Contains(result)));
        return result;
    }
    
    void PointToTarget()
    {
        LineRenderer l = gameObject.GetComponent<LineRenderer>();
        float angle = Vector3.Angle(transform.position, targetPosition);
        ParticleSystem pSys = GetComponent<ParticleSystem>();
        ParticleSystem.ShapeModule shape = pSys.shape;
        shape.rotation += Vector3.forward * angle;
        pSys.Play();
    }

    protected override void DieCallback()
    {
        SceneManager.LoadScene("Scenes/WinScene");
    }
}

internal enum BossAction
{
    None,
    Jump,
    Spawn,
    Wait
}
