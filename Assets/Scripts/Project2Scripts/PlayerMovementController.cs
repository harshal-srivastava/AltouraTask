using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField]
    private float speed = 5.0f;

    [SerializeField]
    private float mouseSensitivity = 2.0f;

    [SerializeField]
    private Transform playerCamera;

    [SerializeField]
    private float maxLookAngle = 80f;

    private float verticalLookRotation = 0;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Move();
        Look();
    }

    void Move()
    {
        float moveDirectionX = Input.GetAxis("Horizontal");
        float moveDirectionZ = Input.GetAxis("Vertical");

        Vector3 moveDirection = transform.right * moveDirectionX + transform.forward * moveDirectionZ;
        rb.velocity = new Vector3(moveDirection.x * speed, rb.velocity.y, moveDirection.z * speed);
    }

    void Look()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        verticalLookRotation -= mouseY;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -maxLookAngle, maxLookAngle);

        playerCamera.localRotation = Quaternion.Euler(verticalLookRotation, 0, 0);
        transform.Rotate(Vector3.up * mouseX);
    }
}
