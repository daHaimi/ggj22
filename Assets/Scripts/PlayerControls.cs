using System;
using System.Collections;
using System.Collections.Generic;
using Models;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerControls : MonoBehaviour
{
    [SerializeField] public GameObject otherPlayer;
    [SerializeField] public float speed = 100;
    public GameObject hitpointsText;

    public bool RightStick;

    private GameControls controls;
    private Animator animator;
    private AudioSource player;
    private float iFrames;

    private Vector2 lookDirection = new Vector2(1, 0);
    // Start is called before the first frame update
    void Start()
    {
        controls = Camera.main.GetComponent<GameControls>();
        animator = GetComponent<Animator>();
        player = Camera.main.GetComponent<AudioSource>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Pickup"))
        {
            Pickup p = other.gameObject.GetComponent<Pickup>();
            if (p.type == PickupType.Heart) GainLife();
            else if (p.type == PickupType.Coin) GainStat();
            else controls.playerCapabilities.pickups[p.type]++;
            player.clip = p.pickupSound;
            player.Play();
            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("Enemy") && iFrames <= 0)
        {
            LoseLife();
            hitpointsText.GetComponent<Text>().text = "" + controls.playerCapabilities.life[RightStick ? 1 : 0].current;
            iFrames = .5f;
        }
    }

    private void LoseLife()
    {
        controls.playerCapabilities.life[RightStick ? 1 : 0].current--;
        if (controls.playerCapabilities.life[RightStick ? 1 : 0].current < 1)
        {
            SceneManager.LoadScene("Scenes/LoseScene");
        }
    }

    private void GainStat()
    {
        // Todo: random stat
        controls.playerCapabilities.damage ++;
    }
    
    private void GainLife()
    {
        if (controls.playerCapabilities.life[RightStick ? 1 : 0].current < controls.playerCapabilities.life[RightStick ? 1 : 0].capacity)
            controls.playerCapabilities.life[RightStick ? 1 : 0].current++;
    }
    
    private void OnTriggerStay2D(Collider2D other)
    {
        // Raumwechsel nur von RightStick und wenn der andre in der Nähe (collision) ist
        if (other.isTrigger && other.gameObject.name.StartsWith("room") && RightStick && GetComponent<Rigidbody2D>().IsTouching(otherPlayer.GetComponent<Collider2D>()))
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

        if (iFrames > 0)
        {
            iFrames -= Time.deltaTime;
        }
        
        UpdateLookDirection();

        // float distance = Vector2.Distance(transform.position, otherPlayer.transform.position);
        // RaycastHit2D hit = Physics2D.Raycast(transform.position,
        //     otherPlayer.transform.position,
        //     distance);
        // if (hit.collider != null)
        // {
        //     Debug.Log("hitting: " + hit.collider.name);
        // }
        // Debug.DrawRay(transform.position, otherPlayer.transform.position * distance, Color.blue);
    }

    void UpdateLookDirection()
    {
        lookDirection = GetComponent<Rigidbody2D>().velocity;
        lookDirection.Normalize();
        if (animator)
        {
            animator.SetFloat("Look X", lookDirection.x);
            animator.SetFloat("Look Y", lookDirection.y);
            animator.SetFloat("Speed", lookDirection.magnitude);
        }
    }
}
