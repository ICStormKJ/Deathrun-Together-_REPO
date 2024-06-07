using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class TutorialPlayer : MonoBehaviour
{
    #region KeyCodes
    private KeyCode jumpKey = KeyCode.Space;
    private KeyCode sprintKey = KeyCode.LeftShift;
    private KeyCode powerKey = KeyCode.Q;
    #endregion

    #region camera movement
    float xRotation;
    float yRotation;

    [Header("Camera Settings")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float sensitivity;
    #endregion

    #region movement
    float horizontalInput;
    float verticalInput;
    Vector3 moveDirection;

    public float playerHeight;
    public LayerMask WhatIsGround;

    [Header("Player Movement Stats")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float sprintSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float groundDrag;
    [SerializeField] private float airDrag;
    #endregion

    #region player stats
    [Header("Player Attributes")]
    //HP
    public float maxHP;
    private float currentHP;
    //STAMINA
    public float maxStamina;
    private float currentStamina;
    [SerializeField] private float stamDecayRate;
    [SerializeField] private float stamRegenRate;

    #endregion

    #region checking bools
    private bool isGrounded;
    private bool isSprinting;
    #endregion

    #region UI
    [SerializeField] private Image healthBar;
    [SerializeField] private Image staminaBar;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text staminaText;
    [SerializeField] private TMP_Text cooldownText;
    #endregion

    #region Power things
    private Power currentPower;
    private float powerCooldown;
    private bool canUsePower;
    public bool dashing;
    public Vector3 targetpos;
    #endregion

    private Rigidbody rb;

    public Transform checkpoint;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        currentHP = maxHP;
        currentStamina = maxStamina;

        healthText.text = currentHP + "/" + maxHP;
        staminaText.text = currentStamina + "/" + maxStamina;
    }

    void Update()
    {
        //-----GROUNDED CHECK-----
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.1f, WhatIsGround);
        if (isGrounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;

        //-----INPUTS-----
        StaminaController(Input.GetKey(sprintKey));
        moveCamera();
        MovePlayer();

        if (isGrounded && Input.GetKeyDown(jumpKey)) //jumping
        {
            Jump();
        }

        if (!isGrounded)
        {
            AirSpeedControl();
        }

        //-----Manage dashing-----
        if (dashing)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetpos, 0.6f);
            if (Vector3.Distance(transform.position, targetpos) <= 0.1f)
            {
                dashing = false;
            }
        }

        //-----Managing power cooldowns and displayed text-----
        if (powerCooldown <= 0)  
        { 
            canUsePower = true;
            cooldownText.text = powerKey.ToString();
        }
        else
        {
            powerCooldown -= Time.deltaTime;
            canUsePower = false;
            cooldownText.text = ""+Mathf.Floor(powerCooldown+1);
        }

        if (canUsePower && isGrounded && Input.GetKeyDown(powerKey))
        {
            TutorialPower.DashTutorial(this);
        }
    }
    //----------Player camera rotation----------
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
    //----------Moving the player and adding force with conditions----------
    private void MovePlayer()
    {
        //inputs
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        //moving
        moveDirection = transform.forward * verticalInput + transform.right * horizontalInput;
        rb.AddForce(moveDirection.normalized *
            (isSprinting ? sprintSpeed : walkSpeed) *
            (!isGrounded ? airDrag : 1)
            , ForceMode.Force);

        if (horizontalInput == 0 && verticalInput == 0 && isGrounded) { rb.velocity = new Vector3(0,rb.velocity.y,0); }
    }

    //-----Method used to control the airspeed cuz airdrag is stupid-----
    private void AirSpeedControl()
    {
        Vector3 currentSpeed = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        float mod = isSprinting?sprintSpeed: walkSpeed;
        if (currentSpeed.magnitude >= mod)
        {
            rb.velocity = new Vector3(currentSpeed.normalized.x * mod, rb.velocity.y, currentSpeed.normalized.z * mod);
        }
    }
    //----------Jumping----------
    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
    //----------Controls the stamina----------
    private void StaminaController(bool input)
    {
        if (input && (rb.velocity.x > 0 || rb.velocity.z > 0))
        {
            if (currentStamina <= 0f)
            {
                isSprinting = false;
                return;
            }
            currentStamina -= stamDecayRate * Time.deltaTime;

        }
        else if (!input && currentStamina < maxStamina)
        {
            currentStamina += stamRegenRate * Time.deltaTime;
            if (currentStamina > maxStamina)
                currentStamina = 100f;
        }
        isSprinting = input;
        if (currentStamina < 0f)
            currentStamina = 0;
        staminaText.text = Mathf.Floor(currentStamina) + "/" + maxStamina;
        staminaBar.transform.localScale = new Vector3(currentStamina / maxStamina, 1f, 1f);
    }
    //----------When hit by a projectile or by void----------
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Projectile>(out Projectile proj))
        {
            DamagePlayer(proj.damage);
            KnockbackPlayer(proj.knockback, collision.transform.forward);
        }
        else if (collision.gameObject.tag == "Void")
        {
            ReturnToCheckpoint();
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.TryGetComponent<TutorialPlatforms>(out  TutorialPlatforms tp))
        {
            transform.parent = other.transform;
        }
    }

    //----------Damaging the player by any projectile----------
    private void DamagePlayer(float damage)
    {
        currentHP -= damage;
        healthBar.transform.localScale = new Vector3(currentHP / maxHP, 1f, 1f);
        healthText.text = Mathf.Floor(currentHP) + "/" + maxHP;
        if (currentHP <= 0f)
        {
            ReturnToCheckpoint();
        }
    }

    private void ReturnToCheckpoint()
    {
        currentHP = maxHP;
        currentStamina = maxStamina;
        transform.position = checkpoint.position;
    }

    //----------Knocking the player back from any sources----------
    public void KnockbackPlayer(float magnitude, Vector3 direction)
    {
        if (currentPower == Power.Daredevil) { return; }
        rb.velocity = Vector3.zero;
        rb.AddForce(direction * magnitude, ForceMode.Impulse);
    }

    public bool Grounded()
    {
        return isGrounded;
    }
    public void addPowerCooldown(float duration)
    {
        powerCooldown += duration;
    }
}
