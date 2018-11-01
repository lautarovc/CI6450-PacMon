using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seek : SteeringBehavior
{
    public Transform target;

    public float maxAcceleration = 10f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        getSteering();
        moveCharacter();
    }

    public override bool getSteering()
    {
        return getSteering(target.position);

    }

    public bool getSteering(Vector3 target)
    {
        // Target direction
        linearSteering = target - transform.position;

        // Accelerate
        linearSteering = Vector3.Normalize(linearSteering);
        linearSteering *= maxAcceleration;

        angularSteering = 0f;

        return true;
    }
}