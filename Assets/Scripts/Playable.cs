using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playable : SteeringBehavior {

    private Vector3 target;
    public float maxAcceleration = 50f;

    public float targetRadius = 1.4f;
    public float slowRadius = 10f;
    public float timeToTarget = 0.1f;

    public float movementSize = 0.2f;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        bool moves = false;
        bool pressed = false;
        target = transform.position;
		
        if (Input.GetKey("w"))
        {
            target += new Vector3(0, 0, movementSize);
            pressed = true;
        }

        if (Input.GetKey("s"))
        {
            target += new Vector3(0, 0, -movementSize);
            pressed = true;
        }

        if (Input.GetKey("a"))
        {
            target += new Vector3(-movementSize, 0, 0);
            pressed = true;
        }

        if (Input.GetKey("d"))
        {
            target += new Vector3(movementSize, 0, 0);
            pressed = true;
        }

        if (pressed)
        {
            moves = getSteering();
        }

        if (moves)
        {
            moveCharacter();
        }
        else
        {
            linearStop();
        }
    }

    public override bool getSteering()
    {
        // Declaration of internal variables
        Vector3 direction;
        float distance;
        float targetSpeed;
        Vector3 targetVelocity;

        // Target direction and distance
        direction = target - transform.position;
        distance = direction.magnitude;

        // Target Radius to stop
        if (distance < targetRadius)
        {
            // Return no movement
            linearSteering = new Vector3(0, 0, 0);
            angularSteering = 0f;
            return false;
        }

        // Speed for slow radius
        if (distance > slowRadius)
        {
            targetSpeed = characterKinematic.maxSpeed;
        }
        else
        {
            targetSpeed = characterKinematic.maxSpeed * distance / slowRadius;
        }

        targetVelocity = Vector3.Normalize(direction);
        targetVelocity *= targetSpeed;

        linearSteering = targetVelocity - characterKinematic.velocity;
        linearSteering /= timeToTarget;

        // If too fast
        if (linearSteering.magnitude > maxAcceleration)
        {
            linearSteering = Vector3.Normalize(linearSteering);
            linearSteering *= maxAcceleration;
        }

        return true;

    }
    
}
