using System;
using UnityEngine;

public class HeadBobController : MonoBehaviour
{
    [SerializeField] private new bool enabled = true;
    
    [SerializeField, Range(0, 0.1f)] private float amplitude = 0.015f;
    [SerializeField, Range(0, 30f)] private float frequency = 10.0f;
    
    [SerializeField] private Transform cam = null;

    private Vector3 startPos;
    private CharacterController characterController;
    private PlayerController playerController;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        playerController = GetComponent<PlayerController>();
        startPos = cam.localPosition;
    }

    private void Update()
    {
        if(!enabled) return;

        CheckMotion();
        ResetPosition();
        cam.LookAt(FocusTarget());
    }
    private void PlayMotion(Vector3 motion)
    {
        cam.localPosition += motion; 
    }

    private void CheckMotion()
    {
        float speed = new Vector3(playerController.x, 0, playerController.z).magnitude;
        
        if (speed < 0.1) return;
        if(!characterController.isGrounded) return;
        PlayMotion(FootStepMotion());
    }

    private Vector3 FocusTarget()
    {
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + cam.localPosition.y, transform.position.z);
        pos += cam.forward * 15.0f;
        return pos;
    }
    private Vector3 FootStepMotion()
    {
        Vector3 pos = Vector3.zero;
        pos.y += Mathf.Sin(Time.time * frequency) * amplitude;
        pos.x += Mathf.Cos(Time.time * frequency / 2) * amplitude * 2;
        return pos;
    }

    private void ResetPosition()
    {
        if (cam.localPosition == startPos) return;
        cam.localPosition = Vector3.Lerp(cam.localPosition, startPos,1 * Time.deltaTime);
    }
}
