using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.Netcode;
using Unity.VisualScripting;
using Newtonsoft.Json.Bson;

public class PlayerManager : NetworkBehaviour
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

    [Header ("Player Movement Stats")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float sprintSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float groundDrag;
    [SerializeField] private float airDrag;
    #endregion

    #region player stats
    [Header("Player Attributes")]
    //HP
    NetworkVariable<float> maxHP = new NetworkVariable<float>(100f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    NetworkVariable<float> currentHP = new NetworkVariable<float>(100f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    //STAMINA
    public float maxStamina;
    private float currentStamina;
    [SerializeField] private float stamDecayRate;
    [SerializeField] private float stamRegenRate;

    #endregion

    #region checking bools
    private bool isGrounded;
    private bool isSprinting;
    private NetworkVariable<bool> isEliminated = new NetworkVariable<bool>();
    private NetworkVariable<bool> isGoaled = new NetworkVariable<bool>();
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
    private PlayerData data;
    private NetworkVariable<bool> isTrapmaster = new NetworkVariable<bool>();
    private NetworkVariable<bool> victory = new NetworkVariable<bool>();

    void Start()
    {
        if (!IsOwner) enabled = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        data = FindFirstObjectByType<PlayerData>();

        currentHP.Value = maxHP.Value;
        currentStamina = maxStamina;

        healthText.text = currentHP.Value + "/" + maxHP.Value;
        staminaText.text = currentStamina + "/" + maxStamina;

        GameManager.SetPlayerManager(this);
    }

    public override void OnNetworkSpawn()
    {
        if (isTrapmaster.Value)
        {
            currentPower = data.trapPower;
        }
        else
        {
            currentPower = data.runnerPower;
        }
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

        //-----Managing power cooldown and UI-----
        if (powerCooldown <= 0) 
        {
            cooldownText.text = powerKey.ToString();
            canUsePower = true; 
        }
        else 
        {
            powerCooldown -= Time.deltaTime;
            cooldownText.text = "" + Mathf.Floor(powerCooldown+1);
            canUsePower = false; 
        }

        if (canUsePower && Input.GetKeyDown(powerKey))
        {
            PowerManager.ActivatePower(currentPower, this);
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
            (!isGrounded? airDrag : 1)
            , ForceMode.Force);
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
            currentHP.Value = 0;
            GoToSpectatorMode();
        }

    }
    //----------For the speed power----------
    public void UpdateSpeeds(float walkMult, float runMult)
    {
        walkSpeed *= walkMult;
        sprintSpeed *= runMult;
    }

    //----------For the drain power----------
    [ClientRpc(RequireOwnership = false)]
    public void UpdateHealthsClientRPC(float newHP)
    {
        maxHP.Value = newHP;
        currentHP.Value = maxHP.Value;
    }

    //----------Damaging the player by any projectile----------
    private void DamagePlayer(float damage)
    {
        currentHP.Value -= damage;
        healthBar.transform.localScale = new Vector3(currentHP.Value / maxHP.Value, 1f, 1f);
        healthText.text = Mathf.Floor(currentHP.Value) + "/" + maxHP.Value;
        if (currentHP.Value <= 0f)
            GoToSpectatorMode();
    }

    //----------Knocking the player back from any sources----------
    public void KnockbackPlayer(float magnitude, Vector3 direction)
    {
        if (currentPower == Power.Daredevil) { return; }
        rb.velocity = Vector3.zero;
        rb.AddForce(direction * magnitude, ForceMode.Impulse);
    }

    //----------Method to control putting the player into spectator mode----------
    private void GoToSpectatorMode()
    {
        healthText.enabled = false;
        staminaText.enabled = false;
        healthBar.enabled = false;
        staminaBar.enabled = false;
        rb.useGravity = false;
        gameObject.GetComponentInChildren<Collider>().enabled = false;
        isEliminated.Value = true;
        FindFirstObjectByType<GameManager>().playersEliminated.Value++;
    }

    public void Goaled()
    {
        isGoaled.Value = true;
    }
    public bool Grounded()
    {
        return isGrounded;
    }
    public void addPowerCooldown(float duration)
    {
        powerCooldown += duration;
    }
    public void MakeTrapmaster()
    {
        isTrapmaster.Value = true;
    }
    public void MakePlayerWin()
    {
        victory.Value = true;
    }
    public bool PlayerWinCondition()
    {
        return victory.Value;
    }
}
