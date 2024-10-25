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
    public AudioSource jumpSound;
    public AudioSource impactSound;
    private float textureOffsetY = 0f;
    public GameObject lvl2SpawnPoint;
    public CableGenerator cableGenerator;
    public Camera winCamera;
    public GameObject windTurbine;
    public GameObject winScreen;
    public AudioSource btnSFX;

    void Start()
    {
        if (winCamera != null)
        {
            winCamera.enabled = false;
        }

        if (mainCamera != mainCamera)
        {
            mainCamera.enabled = true;
        }
        
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
            jumpSound.Play();
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
            impactSound.Play();
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

        if (other.gameObject.CompareTag("NewLevel"))
        {
            gameObject.transform.position = lvl2SpawnPoint.transform.position;
            gameObject.transform.rotation = lvl2SpawnPoint.transform.rotation;
            cableGenerator.ClearRope();
            cableGenerator.GenerateRope();
        }

        if (other.gameObject.CompareTag("Windmill"))
        {
            btnSFX.Play();
            // Connect the last segment of the cable to an empty GameObject
            GameObject emptyObject = GameObject.Find("Connector");
            emptyObject.transform.position = cableGenerator.transform.position + Vector3.back * cableGenerator.segmentLength * (cableGenerator.segmentCount - 1);
            Rigidbody emptyRb = emptyObject.AddComponent<Rigidbody>();
            Debug.Log(emptyRb , emptyObject);
            emptyRb.isKinematic = true;

            HingeJoint lastSegmentJoint = cableGenerator.transform.GetChild(cableGenerator.segmentCount - 1).gameObject.AddComponent<HingeJoint>();
            lastSegmentJoint.connectedBody = emptyRb;
            lastSegmentJoint.anchor = new Vector3(0, cableGenerator.segmentLength / 2, 0);
            lastSegmentJoint.axis = Vector3.forward;

            // Disable player control
            this.enabled = false;

            // Start coroutine to show win screen
            StartCoroutine(Victory());
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

    IEnumerator Victory()
    {
       
        // Switch to a new camera
        mainCamera.gameObject.SetActive(false);
        winCamera.gameObject.SetActive(true);
        winCamera.GetComponent<Camera>().enabled = true;

        // Wait for 1 second
        yield return new WaitForSeconds(1f);

        // Start spinning the windmill turbine
        float maxSpeed = 90f; // degrees per second
        float currentSpeed = 0f;
        float acceleration = maxSpeed / 2f; // speed increase per second

        float elapsedTime = 0f;
        while (elapsedTime < 5f)
        {
            elapsedTime += Time.deltaTime;
            currentSpeed = Mathf.Lerp(0, maxSpeed, elapsedTime / 3f);
            windTurbine.transform.Rotate(Vector3.forward * currentSpeed * Time.deltaTime);
            yield return null;
        }

        btnSFX.Play();
        // Make the Winscreen gameobject active
        winScreen.SetActive(true); 
    }
}