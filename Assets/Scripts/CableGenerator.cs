using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CableGenerator : MonoBehaviour
{
    public GameObject ropeSegmentPrefab;
    public int segmentCount = 10;
    public float segmentLength = 0.5f;
    public Rigidbody movingObject; // Reference to the moving object's Rigidbody
    public Collider characterCollider; // Reference to the character's collider

    void Start()
    {
        GenerateRope();
    }

    public void GenerateRope()
    {
        ClearRope(); // Clear existing rope before generating a new one

        GameObject previousSegment = null;

        for (int i = 0; i < segmentCount; i++)
        {
            GameObject newSegment = Instantiate(ropeSegmentPrefab, transform);
            newSegment.transform.position = transform.position + Vector3.back * segmentLength * i;

            Rigidbody rb = newSegment.GetComponent<Rigidbody>();

            if (i == 0)
            {
                // Use a FixedJoint for the first segment
                FixedJoint joint = newSegment.AddComponent<FixedJoint>();
                joint.connectedBody = movingObject;
            }
            else
            {
                // Make the rest of the segments non-kinematic
                rb.isKinematic = false;

                HingeJoint joint = newSegment.AddComponent<HingeJoint>();
                joint.connectedBody = previousSegment.GetComponent<Rigidbody>();
                joint.anchor = new Vector3(0, segmentLength / 2, 0);
                joint.axis = Vector3.forward;
            }

            // Disable collision between the cable segment and the character
            Collider segmentCollider = newSegment.GetComponent<Collider>();
            if (segmentCollider != null && characterCollider != null)
            {
                Physics.IgnoreCollision(segmentCollider, characterCollider);
            }

            previousSegment = newSegment;
        }
    }

    public void ClearRope()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}