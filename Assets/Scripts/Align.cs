using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Align : SteeringBehavior {

    // Data for target
    public Transform target;


    // Acceleration and rotation
    public float maxAnglarAcceleration = 360f;
    public float maxRotation = 360f;

    // Radii
    public float targetRadius = 1.5f;
    public float slowRadius = 180f;
    
    // Time to achieve target speed
    public float timeToTarget = 0.01f;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        bool moves = getSteering(target.rotation.eulerAngles.y);
        if (moves)
        {
            moveCharacter();
        }
        else
        {
            angularStop();
        }
    }

    float MapToRange(float rotation)
    {
        // Get degrees in range
        rotation = rotation % 360;

        // Change orientation to avoid spinning
        if (rotation > 180)
        {
            rotation = rotation - 360;
        }

        else if (rotation < -180)
        {
            rotation = rotation + 360;
        }

        return rotation;

    }

    public bool getSteering(float targetRotation)
    {   
        float rotation;
        float rotationSize;
        float rotationSpeed;
        float angularAcceleration;

        // Distance between target and character
        rotation = targetRotation - transform.rotation.eulerAngles.y;
        rotation = MapToRange(rotation);

        rotationSize = Mathf.Abs(rotation);

        // Target radius to stop
        if (rotationSize < targetRadius)
        {
            // Return no movement
            linearSteering = new Vector3(0, 0, 0);
            angularSteering = 0f;
            return false;
        }

        // Outside slowRadius to go full speed
        if (rotationSize > slowRadius)
        {
            rotationSpeed = maxRotation;
        }

        // Inside slowRadius decrease speed
        else
        {
            rotationSpeed = maxRotation * rotationSize / slowRadius;
        }

        // Add direction to speed
        rotationSpeed *= rotation / rotationSize;

        // Get steering needed
        angularSteering = rotationSpeed - characterKinematic.rotation;
        angularSteering /= timeToTarget;

        // If too fast
        angularAcceleration = Mathf.Abs(angularSteering);

        if (angularAcceleration > maxAnglarAcceleration)
        {
            angularSteering /= angularAcceleration;
            angularSteering *= maxAnglarAcceleration;
        }

        linearSteering = new Vector3(0, 0, 0);

        return true;
    }

}
