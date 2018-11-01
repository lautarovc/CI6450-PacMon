using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionAvoidance : SteeringBehavior {

    // Data for character and targets
    public List<GameObject> targets;

    // Maximum acceleration and collision radius
    public float maxAcceleration = 5f;
    public float radius = 0.8f;



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
        // Collision time
        float shortestTime = Mathf.Infinity;

        // First target data
        Transform firstTarget = null;
        float firstMinSeparation = Mathf.Infinity;
        float firstDistance = Mathf.Infinity;
        Vector3 firstRelativePos = new Vector3(0, 0, 0);
        Vector3 firstRelativeVel = new Vector3(0, 0, 0);

        // Data for each target in loop
        Vector3 relativePos;
        Vector3 relativeVel;
        float relativeSpeed;
        float timeToCollision;
        float distance;
        float minSeparation;


        foreach (GameObject target in targets)
        {
            // Get time to collision
            relativePos = target.GetComponent<Transform>().position - transform.position;
            relativeVel = target.GetComponent<Kinematic>().velocity - characterKinematic.velocity;
            relativeSpeed = relativeVel.magnitude;
            timeToCollision = - Vector3.Dot(relativePos, relativeVel) / (relativeSpeed * relativeSpeed);

            // Check for collision
            distance = relativePos.magnitude;
            minSeparation = distance - relativeSpeed * timeToCollision;

            if (minSeparation > 2 * radius)
            {
                continue;
            }

            // Check if it is first target to collide with
            if (timeToCollision > 0 && timeToCollision < shortestTime)
            {
                shortestTime = timeToCollision;
                firstTarget = target.GetComponent<Transform>();
                firstMinSeparation = minSeparation;
                firstDistance = distance;
                firstRelativePos = relativePos;
                firstRelativeVel = relativeVel;
            }
        }

        // If there is no target to collide
        if (firstTarget == null)
        {
            return false;
        }

        // Do steering based on current position if we are colliding
        if (firstMinSeparation <= 0 || firstDistance < 2 * radius)
        {
            relativePos = firstTarget.position - transform.position;

        }
        // Calculate future relative position of target
        else
        {
            relativePos = firstRelativePos + firstRelativeVel * shortestTime;
        }

        // Avoid target
        relativePos = Vector3.Normalize(relativePos);
        linearSteering = -relativePos * maxAcceleration;

        return true;
        
    }

}
