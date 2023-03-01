using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    public float movementSpeed = 6f;
    public float gravity = -9.81f * 3f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public bool isGrounded;

    public float jumpHeight = 2f;
    private float jumpForce;
    private Vector3 velocity;
    private Vector3 jumpDirection; 

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        float InputX = Input.GetAxis("Horizontal"),
              InputZ = Input.GetAxis("Vertical");
        float absInputX = Mathf.Abs(InputX),
              absInputZ = Mathf.Abs(InputZ);
        Vector3 move;  

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0) {
            velocity.y = -2f;
            if (absInputX > absInputZ) {
                jumpForce = InputX;
                jumpDirection = transform.right;
            } else {
                jumpForce = InputZ;
                jumpDirection = transform.forward;
            } 
        } 

        if (!isGrounded) {
            // Player is floating
            move = jumpDirection * jumpForce;
        } else {
            move = transform.right * InputX + transform.forward * InputZ;
        }

        controller.Move(move * movementSpeed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded) {
            velocity.y += Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

    }
}
