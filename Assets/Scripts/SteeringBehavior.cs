using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SteeringBehavior : MonoBehaviour {

    // Character Data
    public Kinematic characterKinematic;

    // Weight for blending
    public float weight = 1f;

    // Output steering
    public Vector3 linearSteering;
    public float angularSteering;
    public float verticalSteering;

    public virtual bool getSteering()
    {
        throw new System.NotImplementedException();
    }

    public void moveCharacter()
    {
        // Return movement to character
        characterKinematic.linearSteering = linearSteering;
        characterKinematic.angularSteering = angularSteering;

        // Move character
        characterKinematic.Move();
    }

    public void linearStop()
    {
        characterKinematic.linearSteering = Vector3.zero;
        characterKinematic.velocity = Vector3.zero;
        linearSteering = Vector3.zero;
    }

    public void verticalStop()
    {
        characterKinematic.verticalSteering = 0;
        characterKinematic.verticalVelocity = 0;
        verticalSteering = 0;
    }

    public void angularStop()
    {
        characterKinematic.angularSteering = 0;
        characterKinematic.rotation = 0;
        angularSteering = 0;
    }
}
