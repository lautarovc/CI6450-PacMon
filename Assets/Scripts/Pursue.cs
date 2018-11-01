using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pursue : Arrive {

    // Kinematic data for target
    public Kinematic targetKinematic;

    // Maximum prediction time
    public float maxPrediction = 2f;

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

    public override bool getSteering()
    {   
        // Prediction data
        Vector3 futureTarget;
        float prediction;

        Vector3 direction = target.position - transform.position;
        float distance = direction.magnitude;

        float speed = characterKinematic.velocity.magnitude;

        if (speed <= distance/maxPrediction)
        {
            prediction = maxPrediction;
        }
        else
        {
            prediction = distance / speed;
        }

        futureTarget = target.position + targetKinematic.velocity * prediction;

        return getSteering(futureTarget);
    }
}
