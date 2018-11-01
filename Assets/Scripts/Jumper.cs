using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumper : SteeringBehavior {

    public float jumpAcceleration = 20f;

    public float movementSize = 0.2f;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKey("space") && !characterKinematic.jumping)
        {
            characterKinematic.jumping = true;
            characterKinematic.verticalSteering = jumpAcceleration;
        }

        if (characterKinematic.jumping)
        {   
            characterKinematic.Jump();
        }
		
	}


}
