using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrive : SteeringBehavior {

    public Transform target;

    public float maxAcceleration = 50f;

    public float targetRadius = 1.4f;
    public float slowRadius = 10f;

    public float timeToTarget = 0.1f;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        bool moves = getSteering();
        if (moves)
        {
            moveCharacter();
        }
        else
        {
            linearStop();
        }
    }

    public bool getSteering(Vector3 target)
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

        angularSteering = 0f;

        return true;

    }

    public override bool getSteering()
    {
        return getSteering(target.position);
    }
}