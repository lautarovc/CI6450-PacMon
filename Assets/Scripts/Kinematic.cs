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

    public float ground = 0;
    public List<int> walls = new List<int>();
    public string normal = "ninguna";

    private Graph graph;

	// Use this for initialization
	void Start () {
        GameObject graphObj = GameObject.FindGameObjectsWithTag("Graph")[0];
        graph = graphObj.GetComponent<Graph>();
    }

    // Update is called once per frame
    void Update()
    {   // Small island and top platform
        bool onPlatform = (transform.position.x < 4 && transform.position.z > 9) || (transform.position.z > 16) || (transform.position.z > 4 && transform.position.x > 9 && transform.position.x < 13.5 && transform.position.z < 13 ) || (transform.position.x > 16.5 && transform.position.z > 9);
        // Big island
        onPlatform = onPlatform || (transform.position.x > 7 && transform.position.x < 20 && transform.position.z > -3 && transform.position.z < 1) || (transform.position.x > 16.5 && transform.position.x < 20 && transform.position.z > -3 && transform.position.z < 5);

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

        // Crash walls
        walls = nearWalls();
        Vector3 futurePosition = transform.position + (velocity.normalized * 1.01f);

        if (outsideMap(futurePosition))
        {
            Stop();
        }

        Node wall = graph.posicionEnNodo(futurePosition, walls);

        if (wall != null && transform.position.y < 2)
        {
            if (wall.normal == "horizontal")
            {
                Stop();
            }
            else if (wall.normal == "vertical")
            {
                Stop();
            }
            normal = wall.normal;
        }
        else
        {   
            normal = "ninguna";
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

    public void FastMove()
    {
        transform.position += velocity * Time.deltaTime;

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

    public void xStop()
    {
        linearSteering = new Vector3(0, linearSteering.y, linearSteering.z);
        velocity = new Vector3(0, velocity.y, velocity.z);
    }

    public void zStop()
    {
        linearSteering = new Vector3(linearSteering.x, linearSteering.y, 0);
        velocity = new Vector3(linearSteering.x, velocity.y, 0);
    }

    public List<int> nearWalls()
    {
        List<int> wallIds = new List<int>();

        if (transform.position.z > 3 && transform.position.x > 8 && transform.position.x < 14.5 && transform.position.z < 14)
        {
            for (int i=40; i<64; i++)
            {
                wallIds.Add(i);
            }
        }

        else if ((transform.position.x < 5 && transform.position.z > 8) || (transform.position.z > 15) || (transform.position.x > 15.5 && transform.position.z > 8))
        {
            for (int i=0; i<40; i++)
            {
                wallIds.Add(i);
            }
        }

        else if ((transform.position.x > 6 && transform.position.x < 21 && transform.position.z > -4 && transform.position.z < 2) || (transform.position.x > 15.5 && transform.position.x < 21 && transform.position.z > -4 && transform.position.z < 6))
        {
            for (int i=64; i<108; i++)
            {
                wallIds.Add(i);
            }
        }

        return wallIds;
    }

    public bool outsideMap(Vector3 position)
    {
        return (position.x > 23.5 || position.z < -6.5 || position.z > 19 || (position.x < 1 && position.z > 6) || (position.x < 3.5 && position.z <= 6));
    }
}
