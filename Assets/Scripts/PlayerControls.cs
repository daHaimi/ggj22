using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    [SerializeField] public GameObject otherPlayer;
    [SerializeField] public float speed = 100;

    public bool RightStick;

    private GameControls controls;
    // Start is called before the first frame update
    void Start()
    {
        controls = Camera.main.GetComponent<GameControls>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        
        // Raumwechsel nur von RightStick und wenn der andre in der Nähe (collision) ist
        if (other.isTrigger && RightStick && GetComponent<Rigidbody2D>().IsTouching(otherPlayer.GetComponent<CircleCollider2D>()))
        {
            Vector2 delta = (controls.curRoomBounds.center - transform.position).normalized;
            Vector2 direction = new Vector2(Mathf.Round(-delta.x), Mathf.Round(delta.y));
            controls.rooms[controls.curRoom].GetComponent<ChangeRoom>().SwitchRoom(direction, gameObject);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 vec;
        if (RightStick)
        {
            vec = new Vector2(Input.GetAxis("RightHorizontal"), Input.GetAxis("RightVertical"));
        } else { 
            vec = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        }
        GetComponent<Rigidbody2D>().AddForce(vec * speed);
    }
}
