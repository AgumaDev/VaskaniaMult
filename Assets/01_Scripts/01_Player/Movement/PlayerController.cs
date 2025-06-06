using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;
public class PlayerController : NetworkBehaviour
{
    [Header("PLAYER MOVEMENT")]
    [Space(5)]

    public PlayerState playerState;
    public enum PlayerState
    {
        Air,
        Walking,
        Running,
        Menu
    }

    public Vector3 velocity;
    [SerializeField] private float walkingSpeed;
    [SerializeField] private float runningSpeed;
    public float gravity;
    public float x;
    public float z;

    [Header("STAMINA")]
    [Space(5)]

    [SerializeField] private float stamina;
    [SerializeField] private float maxStamina;
    [SerializeField] private float minStaminaToRun;
    [SerializeField] private float staminaDecreaseSpeed = 20f;
    [SerializeField] private float staminaIncreaseSpeed = 20f;


    [Header("GROUND CHECK")]
    [Space(5)]

    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance;
    [SerializeField] private LayerMask groundMask;
    public bool isGrounded;


    [Header("PLAYER CULLING MASK")]
    [Space(5)]

    [SerializeField] private GameObject playerModel;
    [SerializeField] private string localPlayerLayer = "LocalPlayer";
    
    [Header("OTHER")]
    [Space(5)]

    public bool insideMainArea;
    [SerializeField] private Animator playerAnim;

    public GameObject UIMenu;

    public bool isMenuOpened;
    
    float baseSpeed;

    #region REFERENCES
    PlayerCamera playerCam;
    CharacterController characterController;
    #endregion

    public override void OnNetworkSpawn()
    { 
        characterController = GetComponent<CharacterController>();
        playerCam = GetComponentInChildren<PlayerCamera>();
    }
    private void Start()
    {
        if (IsLocalPlayer)
        {
            int localLayer = LayerMask.NameToLayer(localPlayerLayer);
            CullingLayer(playerModel, localLayer);
        }

        UIMenu = GameObject.FindGameObjectWithTag("Menu/InGameMenu");
        stamina = maxStamina;
        GameObject.FindGameObjectWithTag("Menu/InGameMenu").GetComponent<InGameMenuHandler>().LookForPlayers();
    }
    private void Update()
    {
        if (!IsOwner) return;
        Movement();

        if (UIMenu.GetComponent<InGameMenuHandler>().isMenuEnabled)
        {
            ChangePlayerState(PlayerState.Menu);
            isMenuOpened = true;
        }
        else
        {
            isMenuOpened = false;
        }
    }
    private void Movement()
    {
        //CHANGING STATE

        if ( isGrounded && Input.GetKey(KeyCode.LeftShift) && isMenuOpened == false)
        {
            if(playerState != PlayerState.Running && stamina >= minStaminaToRun) 
            {
                ChangePlayerState(PlayerState.Running);
            }
        }
        else if (isGrounded && isMenuOpened == false)
        {
            if(playerState != PlayerState.Walking)
            {
                ChangePlayerState(PlayerState.Walking);
            }
        }
        else playerState = PlayerState.Air;
        
        switch (playerState)
        {
            case PlayerState.Running:
                stamina -= Time.deltaTime * staminaDecreaseSpeed;
                if (stamina <= 0)
                {
                    ChangePlayerState(PlayerState.Walking);
                    stamina = 0;
                }
                break;

            case PlayerState.Walking:
                stamina += Time.deltaTime * staminaIncreaseSpeed;
                if (stamina >= maxStamina) stamina = maxStamina;
                break;
        }

        //MOVEMENT

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");
        
        if (x != 0 || z != 0)
             playerAnim.SetBool("isMoving", true);
        else 
             playerAnim.SetBool("isMoving", false);
        
        Vector3 move = transform.right * x + transform.forward * z;
        characterController.Move(move * (baseSpeed * Time.deltaTime));
        
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }
    void ChangePlayerState(PlayerState newState)
    {
        playerState = newState;
        switch (playerState)
        {
            case PlayerState.Walking:
                baseSpeed = walkingSpeed;
                playerCam.ChangeCameraFov(60);
                break;

            case PlayerState.Running:
                baseSpeed = runningSpeed;
                playerCam.ChangeCameraFov(70);
                break;
            case PlayerState.Menu:
                baseSpeed = 0;
                break;
        }
    }
    public void ResetVerticalVelocity()
    {
        velocity.y = 0f;
    }
    private void CullingLayer(GameObject obj, int newLayer)
    {
        if (obj == null) return;

        obj.layer = newLayer;

        foreach (Transform child in obj.transform)
        {
            if (child == null) continue;
            CullingLayer(child.gameObject, newLayer);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("MainArea"))
        {
            insideMainArea = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("MainArea"))
        {
            insideMainArea = false;
        }
    }
}
