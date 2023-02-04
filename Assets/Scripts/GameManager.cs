using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float speed = .5f;

    private Queue<Transform> floors = new();
    private float delta = 0;
    private Vector3 direction = Vector3.back;
    
    // Start is called before the first frame update
    void Start()
    {
        foreach (var child in transform)
        {
            floors.Enqueue(child as Transform);
        }

    }

    // Update is called once per frame
    void Update()
    {
        var size = 3;
        var movement = Time.deltaTime * speed;
        delta += movement;
        foreach (var floor in floors)
        {
            floor.position += movement * direction;
        }

        if (delta > size)
        {
            var last = floors.Dequeue();
            last.position -= direction * (size * (floors.Count + 1));
            last.gameObject.GetComponent<FloorManager>().Populate();
            floors.Enqueue(last);
            delta = 0;
        }
    }
}
