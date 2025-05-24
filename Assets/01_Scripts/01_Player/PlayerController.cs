using Unity.Netcode;
using UnityEngine;
public class PlayerController : NetworkBehaviour
{
    public float speed;
    public float gravity;
    
    public Transform groundCheck;
    public float groundDistance;
    public LayerMask groundMask;

    public bool insideMainArea;
    public Animator playerAnim;
    
    
    
    CharacterController characterController;
    bool isGrounded;
    Vector3 velocity;
    
    public override void OnNetworkSpawn()
    { 
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (!IsOwner) return;
        Movement(); 
    }
    void Movement()
    {
        
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        
        if (x > 0 || z > 0)
             playerAnim.SetBool("isMoving", true);
        else 
             playerAnim.SetBool("isMoving", false);
        
        Vector3 move = transform.right * x + transform.forward * z;
        characterController.Move(move * (speed * Time.deltaTime));
        
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
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
