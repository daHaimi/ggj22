using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorManager : MonoBehaviour
{
    public float rootDensity = 7;
    public float obstacleDensity = 3;

    public List<GameObject> roots;
    public List<GameObject> possObstacles;

    public GameObject rootsContainer;
    public GameObject obstaclesContainer;

    private BoxCollider bc;
    
    public void Populate()
    {
        foreach (Transform child in rootsContainer.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in obstaclesContainer.transform)
        {
            Destroy(child.gameObject);
        }

        var s = bc.size / 2;
        for (int i = 0; i < rootDensity; i++)
        {
            var inst = Instantiate(roots[Random.Range(0, roots.Count)], rootsContainer.transform, true);
            inst.transform.localPosition = new Vector3(Random.Range(-s.x, s.x), 0, Random.Range(-s.z, s.z));
            inst.transform.Rotate(Vector3.up, Random.Range(0, 360));
            
        }
        for (int i = 0; i < obstacleDensity; i++)
        {
            var inst = Instantiate(possObstacles[Random.Range(0, possObstacles.Count)], obstaclesContainer.transform, true);
            inst.transform.localPosition = new Vector3(Random.Range(-s.x, s.x), 0, Random.Range(-s.z, s.z));
            inst.transform.Rotate(Vector3.up, Random.Range(0, 360));
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        bc = GetComponent<BoxCollider>();
        Populate();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
