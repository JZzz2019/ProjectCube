using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed;
    private Rigidbody rb;
    private Vector3 movement;

    [Header("Ground Detection")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundRadius;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private bool isGrounded;

    [Header("Fallen Cube Detection")]
    [SerializeField] private Transform headCheck;
    [SerializeField] private float headRadius;
    [SerializeField] private LayerMask headLayer;
    [SerializeField] private bool isHit = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundRadius, groundLayer);
        if (isGrounded != true)
        {
            GameManager.Instance.ResetPlayerPosition();
            GameManager.Instance.CountDown -= 2;
        }
        else
        {
            movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        }

        isHit = Physics.CheckSphere(headCheck.position, headRadius, headLayer);
        if (isHit != false)
        {
            GameManager.Instance.ResetPlayerPosition();
        }
    }

    private void FixedUpdate()
    {
        movePlayer(movement);
    }

    private void movePlayer(Vector3 _movement)
    {
        rb.MovePosition(transform.position + (_movement * speed * Time.fixedDeltaTime));
    }
}
