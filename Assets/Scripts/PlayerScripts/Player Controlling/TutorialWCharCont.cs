using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class TutorialWCharCont : MonoBehaviour
{
    float xRotation;
    float yRotation;

    [Header("Camera Settings")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float sensitivity;

    private CharacterController cont;
    private Vector3 moveInput;

    [SerializeField] private float jumpForce;
    [SerializeField] private float moveSpeed;

    private float horizontalInput;
    private float verticalInput;

    private bool isGrounded;
    public LayerMask WhatIsGround;

    Vector3 move = Vector3.zero;
    private void Start()
    {
        cont = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update() {
        moveCamera();
        isGrounded = Physics.Raycast(transform.position, Vector3.down, cont.height * 0.5f + 0.1f, WhatIsGround);

        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        moveInput = transform.forward * verticalInput + transform.right * horizontalInput;
        move = moveInput * Time.deltaTime * moveSpeed;
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            move.y = Mathf.Sqrt(jumpForce * -2 * Physics.gravity.y);
        }
        move += Physics.gravity;
        
    }

    private void FixedUpdate()
    {
        cont.Move(move);
    }
    private void moveCamera()
    {
        //inputs
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * sensitivity;

        //rotating stuff
        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }

}
