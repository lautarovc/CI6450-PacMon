using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookWhereYoureGoing : Align {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        bool moves = getTarget();
        if (moves)
        {
            moveCharacter();
        }

    }

    public bool getTarget()
    {
        float targetRotation;
  
        if (characterKinematic.velocity.magnitude == 0)
        {
            // Return no movement
            characterKinematic.linearSteering = new Vector3(0, 0, 0);
            characterKinematic.angularSteering = 0f;
            return false;
        }

        targetRotation = Mathf.Atan2(-characterKinematic.velocity.x, characterKinematic.velocity.z);

        // Direct rotation
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, -(targetRotation * 180 / Mathf.PI), transform.rotation.eulerAngles.z);
        return false;

        // Align rotation (Character loses control)
        //targetRotation = -(targetRotation * 180) / Mathf.PI;
        //return getSteering(targetRotation);
    }
}
