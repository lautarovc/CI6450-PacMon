using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastSeek : SteeringBehavior {

    public Transform target;

    public float maxSpeed = 20f;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public bool getSteering(Vector3 target)
    {
        Vector3 velocity = target - transform.position;
        velocity.Normalize();
        velocity *= maxSpeed;

        characterKinematic.velocity = velocity;
        return true;
    }

    public override bool getSteering()
    {
        return getSteering(target.position);
    }
}
