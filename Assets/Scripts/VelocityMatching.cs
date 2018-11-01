using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityMatching : SteeringBehavior {

    // Data for target
    public Transform target;
    public Kinematic targetKinematic;

    // Maximum acceleration
    public float maxAcceleration = 25f;

    // Time to achieve target speed
    public float timeToTarget = 0.1f;

    
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        getSteering();
        moveCharacter();
	}

    public override bool getSteering()
    {
        linearSteering = targetKinematic.velocity - characterKinematic.velocity;
        linearSteering /= timeToTarget;

        if (linearSteering.magnitude > maxAcceleration) {

            linearSteering.Normalize();
            linearSteering *= maxAcceleration;
        }

        return true;
    }
}
