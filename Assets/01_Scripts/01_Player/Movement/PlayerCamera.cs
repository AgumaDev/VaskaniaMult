using System.Collections.Generic;
using DG.Tweening;
using Unity.Netcode;
using UnityEngine;
public class PlayerCamera : NetworkBehaviour
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
            GetComponent<AudioListener>().enabled = false;
        }
    }
    void Update()
    {
        if (!IsOwner) return;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerTransform.Rotate(Vector3.up * mouseX);
    }
    public void ChangeCameraFov(float newFov)
    {
        // if (!IsOwner) return;

        playerCamera.DOFieldOfView(newFov, 0.1f);
    }
}
