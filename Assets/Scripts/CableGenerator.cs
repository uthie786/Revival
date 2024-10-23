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

    void GenerateRope()
    {
        GameObject previousSegment = null;

        for (int i = 0; i < segmentCount; i++)
        {
            GameObject newSegment = Instantiate(ropeSegmentPrefab, transform);
            newSegment.transform.position = transform.position + Vector3.back * segmentLength * i;

            Rigidbody rb = newSegment.GetComponent<Rigidbody>();

            if (i == 0)
            {
                // Use a ConfigurableJoint for the first segment
                ConfigurableJoint joint = newSegment.AddComponent<ConfigurableJoint>();
                joint.connectedBody = movingObject;
                joint.xMotion = ConfigurableJointMotion.Locked;
                joint.yMotion = ConfigurableJointMotion.Locked;
                joint.zMotion = ConfigurableJointMotion.Locked;
                joint.angularXMotion = ConfigurableJointMotion.Free;
                joint.angularYMotion = ConfigurableJointMotion.Free;
                joint.angularZMotion = ConfigurableJointMotion.Free;
            }
            else
            {
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
}