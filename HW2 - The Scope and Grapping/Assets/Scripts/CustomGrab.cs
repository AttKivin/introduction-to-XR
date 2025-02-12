using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CustomGrab : MonoBehaviour
{
    // This script should be attached to both controller objects in the scene
    // Make sure to define the input in the editor (LeftHand/Grip and RightHand/Grip recommended respectively)
    CustomGrab otherHand = null;
    public List<Transform> nearObjects = new List<Transform>();
    public Transform grabbedObject = null;
    public InputActionReference action;
    bool grabbing = false;
    Vector3 lastPosition;
    Quaternion lastRotation;
    
    

    private void Start()
    {
        action.action.Enable();

        // Find the other hand
        foreach(CustomGrab c in transform.parent.GetComponentsInChildren<CustomGrab>())
        {
            if (c != this)
                otherHand = c;
        }
        
        lastPosition = transform.position;
        lastRotation = transform.rotation;
    }

    void Update()
    {
        grabbing = action.action.IsPressed();
        if (grabbing)
        {
            // Grab nearby object or the object in the other hand
            if (!grabbedObject)
                grabbedObject = nearObjects.Count > 0 ? nearObjects[0] : otherHand.grabbedObject;

            if (grabbedObject)
            {
                Rigidbody rb = grabbedObject.GetComponent<Rigidbody>();
                rb.isKinematic = true;
                Vector3 currentPosition = transform.position;
                Quaternion currentRotation = transform.rotation;
                Vector3 deltaPosition = currentPosition - lastPosition;
                Quaternion deltaRotation = currentRotation * Quaternion.Inverse(lastRotation);

                // When grabbed by one hand
                if (!otherHand.grabbing || otherHand.grabbedObject != grabbedObject)
                {
                    grabbedObject.position += deltaPosition;
                    RotateAroundController(deltaRotation);
                }
                else // When grabbed by both hands
                {
                    // Combine transformations from both hands
                    Vector3 combinedDeltaPosition = deltaPosition + otherHand.CalculateDeltaPosition();
                    Quaternion combinedDeltaRotation = deltaRotation * otherHand.CalculateDeltaRotation();

                    grabbedObject.position += combinedDeltaPosition;
                    RotateAroundController(combinedDeltaRotation);
                }
            }
        }
        // If let go of button, release object
        else if (grabbedObject) 
        
        {
            Rigidbody rb = grabbedObject.GetComponent<Rigidbody>();
            grabbedObject = null;
            rb.isKinematic = false;
        }
        // Save the current position and rotation here
        lastPosition = transform.position;
        lastRotation = transform.rotation;
    }

    private Quaternion CalculateDeltaRotation()
    {
        Quaternion currentRotation = transform.rotation;
        return currentRotation * Quaternion.Inverse(lastRotation);
    }

    private Vector3 CalculateDeltaPosition()
    {
        Vector3 currentPosition = transform.position;
        return currentPosition - lastPosition;
    }

    private void RotateAroundController(Quaternion deltaRotation)
    {
        Vector3 toObject = grabbedObject.position - transform.position;
        toObject = deltaRotation * toObject;
        grabbedObject.position = transform.position + toObject;
        grabbedObject.rotation = deltaRotation * grabbedObject.rotation;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        // Make sure to tag grabbable objects with the "grabbable" tag
        // You also need to make sure to have colliders for the grabbable objects and the controllers
        // Make sure to set the controller colliders as triggers or they will get misplaced
        // You also need to add Rigidbody to the controllers for these functions to be triggered
        // Make sure gravity is disabled though, or your controllers will (virtually) fall to the ground

        Transform t = other.transform;
        if(t && t.tag.ToLower()=="grabbable")
            nearObjects.Add(t);
    }

    private void OnTriggerExit(Collider other)
    {
        Transform t = other.transform;
        if( t && t.tag.ToLower()=="grabbable")
            nearObjects.Remove(t);
    }
}