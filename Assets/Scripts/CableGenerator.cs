using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CableGenerator : MonoBehaviour
{
    public GameObject ropeSegmentPrefab;
    public int segmentCount = 10;
    public float segmentLength = 0.5f;
    public Rigidbody movingObject; // Reference to the moving object's Rigidbody

    void Start()
    {
        GenerateRope();
    }

    void GenerateRope()
    {
        GameObject previousSegment = null;

        for (int i = 0; i < segmentCount; i++)
        {
            GameObject newSegment = Instantiate(ropeSegmentPrefab, transform);
            newSegment.transform.position = transform.position + Vector3.back * segmentLength * i;

            HingeJoint joint = newSegment.GetComponent<HingeJoint>();

            if (i == 0)
            {
                // Attach the first segment to the moving object
                joint.connectedBody = movingObject;
            }
            else
            {
                joint.connectedBody = previousSegment.GetComponent<Rigidbody>();
            }

            joint.anchor = new Vector3(0, segmentLength / 2, 0);
            joint.axis = Vector3.forward;

            previousSegment = newSegment;
        }
    }
}
