using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Separation : SteeringBehavior {

    // Data for targets
    public List<GameObject> targets;

    public float threshold = 1.6f;
    public float decayCoefficient = 1f;

    public float maxAcceleration = 5f;

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
        Vector3 direction;
        float distance;
        float strength;
        bool anyMovement = false;

        foreach (GameObject target in targets)
        {
            direction = transform.position - target.GetComponent<Transform>().position;
            distance = direction.magnitude;

            if (distance < threshold)
            {   
                strength = Mathf.Min((decayCoefficient / (distance * distance)), maxAcceleration);

                direction = Vector3.Normalize(direction);
                linearSteering += strength * direction;

                anyMovement = true;
            }
        }

        return anyMovement;
    }

}
