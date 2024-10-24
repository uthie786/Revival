using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    private Rigidbody rb;
    public bool isGrounded;
    public Camera mainCamera; // Reference to the main camera
    public Material tyreMaterial; // Reference to the tyre material
    public float textureScrollSpeed = 0.1f; // Speed at which the texture scrolls

    public TrailRenderer leftTireTrail; // Reference to the left tire trail
    public TrailRenderer rightTireTrail; // Reference to the right tire trail
    public float trailWidth = 0.2f; // Width of the tire trails
    public Color trailColor = Color.black; // Color of the tire trails

    private float textureOffsetY = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Initialize the trail renderers
        InitializeTrailRenderer(leftTireTrail);
        InitializeTrailRenderer(rightTireTrail);
    }

    private void Update()
    {
        Jump();
        UpdateTrails();
        UpdateTyreTexture();
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        float moveX = Input.GetAxis("Horizontal") * moveSpeed * Time.fixedDeltaTime;
        float moveZ = Input.GetAxis("Vertical") * moveSpeed * Time.fixedDeltaTime;

        Vector3 forward = mainCamera.transform.forward;
        forward.y = 0; // Keep the movement horizontal
        forward.Normalize();

        Vector3 right = mainCamera.transform.right;
        right.y = 0; // Keep the movement horizontal
        right.Normalize();

        Vector3 move = forward * moveZ + right * moveX;

        if (move != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, Time.fixedDeltaTime * moveSpeed));
        }

        rb.MovePosition(rb.position + move);
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void UpdateTyreTexture()
    {
        if(Input.GetAxis("Horizontal" ) != 0 || Input.GetAxis("Vertical") != 0)
        {
            textureOffsetY += textureScrollSpeed * Time.fixedDeltaTime;
            tyreMaterial.mainTextureOffset = new Vector2(0, textureOffsetY);
        }
    }

    void UpdateTrails()
    {
        leftTireTrail.emitting = isGrounded;
        rightTireTrail.emitting = isGrounded;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
       
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Lethal"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    void InitializeTrailRenderer(TrailRenderer trail)
    {
        if (trail != null)
        {
            trail.startWidth = trailWidth;
            trail.endWidth = trailWidth;
            trail.material.color = trailColor;
            trail.alignment = LineAlignment.View; // Ensure the trail stays parallel to the ground
        }
    }
}