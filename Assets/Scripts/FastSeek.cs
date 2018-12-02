using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastSeek : SteeringBehavior {

    public Transform target;

    public float maxSpeed = 5f;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public bool getSteering(Vector3 target)
    {   
        Vector3 velocity = new Vector3(target.x,0,target.z) - new Vector3(transform.position.x, 0, transform.position.z);
        float distance = velocity.magnitude;
        velocity.Normalize();
        velocity *= maxSpeed;

        //if (distance < 1)
        //{
        //    return false;
        //}
        characterKinematic.velocity = velocity;
        return true;
    }

    public override bool getSteering()
    {
        return getSteering(target.position);
    }
}
