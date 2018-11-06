using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kinematic : SteeringBehavior {

    public Vector3 velocity = new Vector3(0,0,0);
    public float verticalVelocity = 0f;
    public float rotation = 0f;
    public float maxSpeed = 30f;

    public bool jumping = false;
    public float gravity = 1f;

    private float ground = 0;

	// Use this for initialization
	void Start () {
	}

    // Update is called once per frame
    void Update()
    {
        bool onPlatform = (transform.position.x < -6 && transform.position.z > -1) || (transform.position.z > 6) || (transform.position.z > -6 && transform.position.x > -1 && transform.position.x < 3.5) || (transform.position.x > 6.5 && transform.position.z > -1);

        if (onPlatform)
        {
            ground = 2;
        }
        else
        {
            ground = 0;
        }


        if (!jumping)
        {
            transform.position = new Vector3(transform.position.x, ground, transform.position.z);
        }
                

    }

    public void Move () {

        // Update position and orientation
        transform.position += velocity * Time.deltaTime;
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, 
                                              transform.rotation.eulerAngles.y + rotation * Time.deltaTime,
                                              transform.rotation.eulerAngles.z);

        // Update velocity and rotation
        velocity += linearSteering * Time.deltaTime;
        rotation += angularSteering * Time.deltaTime;

        if (velocity.magnitude > maxSpeed) {
            velocity = Vector3.Normalize(velocity);
            velocity *= maxSpeed;
        }


	}

    public void Jump()
    {
        // Position check
        float verticalPosition;

        // Jumping actions

        verticalPosition = transform.position.y + verticalVelocity * Time.deltaTime;

        verticalVelocity += verticalSteering * Time.deltaTime;


        if (verticalPosition < ground)
        {
            verticalPosition = ground;
            verticalVelocity = 0;
            verticalSteering = 0;
            jumping = false;
        }
        else
        {
            verticalSteering -= gravity;
        }

        transform.position = new Vector3(transform.position.x, verticalPosition, transform.position.z);

        

    }

    public void Stop()
    {
        velocity = new Vector3(0, 0, 0);
        rotation = 0;
        linearSteering = Vector3.zero;
        angularSteering = 0;
    }

}
