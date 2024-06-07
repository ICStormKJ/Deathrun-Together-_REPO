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

    Vector3 move = Vector3.zero;
    private void Start()
    {
        cont = GetComponent<CharacterController>();

    }

    private void Update() {
        moveInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        move = moveInput * Time.deltaTime * moveSpeed;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            move.y = jumpForce;
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
