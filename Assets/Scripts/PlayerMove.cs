using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private CharacterController controller;
    [SerializeField] private float speed = 12f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float jumpHeight = 3f;
    private readonly float gravity = -9.18f;
    private Vector3 velocity;
    private bool isGrounded = false;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        // if (isGrounded && velocity.y < 0f)
        // {
        //     velocity.y = -2f;
        // }

        float x = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
        float z = Input.GetAxis("Vertical") * Time.deltaTime * speed;
        float y = Input.GetAxis("Depth") * Time.deltaTime * speed;

        Vector3 move = transform.right * x + transform.forward * z + transform.up * y;
        controller.Move(move);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * - 2 * gravity);
        }
        
        // velocity.y += gravity * Time.deltaTime;
        // controller.Move(velocity * Time.deltaTime);

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            UIManager.instance.ShowQuitMenu();
        }
    }
}
