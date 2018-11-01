using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlendedSteering : SteeringBehavior {

    public List<SteeringBehavior> behaviors;

    public float maxAcceleration = 50f;
    public float maxRotation = 360f;

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
        //  Local steering variables
        Vector3 localLinear = Vector3.zero;
        float localAngular = 0f;

        float angularAcceleration;
        bool moves;
        bool anyMovement = false;

        foreach (SteeringBehavior behavior in behaviors)
        {
            moves = behavior.getSteering();

            if (moves)
            {
                localLinear += behavior.weight * behavior.linearSteering;
                localAngular += behavior.weight * behavior.angularSteering;
                anyMovement = true;
            }

        }

        // If too fast
        if (localLinear.magnitude > maxAcceleration)
        {
            localLinear = Vector3.Normalize(localLinear);
            localLinear *= maxAcceleration;
        }

        // If too fast
        angularAcceleration = Mathf.Abs(localAngular);

        if (angularAcceleration > maxRotation)
        {
            localAngular /= angularAcceleration;
            localAngular *= maxRotation;
        }

        linearSteering = localLinear;
        angularSteering = localAngular;

        return anyMovement;
    }

}
