using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    [SerializeField] public float dampen = 300;

    public bool RightStick;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 vec;
        if (RightStick)
        {
            vec = new Vector2(Input.GetAxis("RightHorizontal"), Input.GetAxis("RightVertical"));
        } else { 
            vec = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        }
        GetComponent<Rigidbody2D>().AddForce(vec / dampen);
    }
}
