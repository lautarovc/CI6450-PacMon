using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Face : Align {

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
        else
        {
            angularStop();
        }
    }

    public bool getTarget()
    {
        float targetRotation;
        Vector3 direction = target.position - transform.position;

        if (direction.magnitude == 0)
        {
            // Return no movement
            return false; // Return target?
        }

        targetRotation = Mathf.Atan2(-direction.x, direction.z);

        // Direct rotation
        //transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, -(targetRotation * 180 / Mathf.PI), transform.rotation.eulerAngles.z);
        //return false;

        // Align rotation
        targetRotation = -(targetRotation * 180) / Mathf.PI;
        return getSteering(targetRotation);
    }
}
