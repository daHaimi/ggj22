using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rigidbody2d;
    Animator animator;

    public float walkSpeed = 5.0f;
    public float rollSpeed = 10.0f;
    public float rollDuration = 0.8f;
    public TilemapRenderer renderingLayer;

    Vector2 moveDirection = new Vector2(0,0);
    Vector2 lookDirection = new Vector2(1,0);
    bool isRolling = false;
    float rollingTime = 0.0f;

    // Start is called before the first frame update
    private void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        if (this.renderingLayer) {
            GetComponent<SpriteRenderer>().sortingOrder = this.renderingLayer.sortingOrder;
        }
    }

    private void Update() {
        bool doRole = Input.GetButtonDown("Fire1");

        if (doRole && !isRolling) {
            isRolling = true;
            moveDirection = lookDirection;
            animator.speed = rollSpeed / walkSpeed;
        }
    }

    private void FixedUpdate()
    {
        if (isRolling) {
            rollingTime += Time.deltaTime;
            if (rollingTime >= rollDuration) {
                rollingTime = 0.0f;
                isRolling = false;
            }
        }

        SetMoveDirection();
        SetLookDirection();

        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", moveDirection.magnitude);

        Vector2 position = rigidbody2d.position;
        position = position + moveDirection * GetSpeed() * Time.deltaTime;
        rigidbody2d.MovePosition(position);
    }

    private void SetMoveDirection()
    {
        if (isRolling) return;

        moveDirection.x = Input.GetAxisRaw("Horizontal");
        moveDirection.y = Input.GetAxisRaw("Vertical");
        if(moveDirection.magnitude > 1) {
            moveDirection.Normalize();
        }
        animator.speed = moveDirection.magnitude;
    }

    private void SetLookDirection()
    {
        if (Mathf.Approximately(moveDirection.x, 0.0f) && Mathf.Approximately(moveDirection.y, 0.0f)) return;

        lookDirection.Set(moveDirection.x, moveDirection.y);
        lookDirection.Normalize();
    }
    

    private float GetSpeed()
    {
        return isRolling ? rollSpeed : walkSpeed;
    }
}
