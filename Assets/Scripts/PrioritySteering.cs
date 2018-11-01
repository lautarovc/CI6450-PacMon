using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrioritySteering : SteeringBehavior {

    public List<BlendedSteering> groups;

    public float epsilon = 0.02f;

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
        bool moves = false;

        foreach (BlendedSteering group in groups)
        {
            moves = group.getSteering();
            linearSteering = group.linearSteering;
            angularSteering = group.angularSteering;

            if (moves && (group.linearSteering.magnitude > epsilon || Mathf.Abs(angularSteering) > epsilon)) {
                return moves;
            }
        }

        // Nobody has enough acceleration, return last one if it moves
        return moves;
    }
}
