using Unity.Netcode;
using UnityEngine;
public class MouseLook : NetworkBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform playerTransform;
    
    Camera playerCamera;
    float xRotation = 0f;

    public override void OnNetworkSpawn()
    {
        playerCamera = GetComponent<Camera>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        
        if (!IsOwner)
        {
            playerCamera.enabled = false;
            GetComponentInChildren<AudioListener>().enabled = false;
        }
    }
    void Update()
    {
        if (!IsOwner)
        {
            return;
        }
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerTransform.Rotate(Vector3.up * mouseX);
    }
}
