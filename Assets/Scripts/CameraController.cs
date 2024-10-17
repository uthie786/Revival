using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform cameraFocusPoint;
    [SerializeField] private Transform player;
    [SerializeField] private Transform playerObject;
    [SerializeField] private float rotationSpeed;

    private Vector3 viewDir;
    private float horizontalInput;
    private float verticalInput;
    private Vector3 inputDir;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (Time.timeScale <= 0f)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true; 
        }
        else if (Time.timeScale >= 1f)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        cameraFocusPoint.forward = viewDir.normalized;
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        inputDir = cameraFocusPoint.forward * -horizontalInput + cameraFocusPoint.right * verticalInput;

        if (inputDir != Vector3.zero)
        {
            player.forward = Vector3.Slerp(player.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
        }
    }
}
