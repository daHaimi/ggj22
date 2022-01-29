using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyFollower : EnemyBase
{
    public float speed = 1.0f;

    private GameControls game;
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
        game = Camera.main.GetComponent<GameControls>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = (Tools.GetClosest(gameObject, game.player).transform.position - transform.position).normalized;
        GetComponent<Rigidbody2D>().MovePosition(transform.position + (dir * (speed * Time.deltaTime)));
    }
}
