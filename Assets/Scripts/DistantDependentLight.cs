using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistantDependentLight : MonoBehaviour
{
    [SerializeField] public GameObject other;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float dist = Vector2.Distance(transform.position, other.transform.position);
        GetComponent<Light>().range = 5 - dist;
    }
}
