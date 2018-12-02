using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionAvoidance2 : SteeringBehavior {

    // Data for Three Rays collision detector
    public ThreeRays collisionDetector;

    // Data for targets
    public List<GameObject> targets;

    // Threatened Ray
    private Vector3 threatenedRay;

    // Max force to apply
    public Vector3 maxForce = new Vector3(10, 0, 10);


    // Use this for initialization
    void Awake()
    {
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
        foreach (GameObject monster in monsters)
        {
            if (!GameObject.ReferenceEquals(monster, gameObject))
            {
                targets.Add(monster);
            }
        }
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
        GameObject mostThreatening = findMostThreateningObstacle();

        if (mostThreatening != null)
        {
            linearSteering.x = threatenedRay.x - mostThreatening.GetComponent<Transform>().position.x;
            linearSteering.z = threatenedRay.z - mostThreatening.GetComponent<Transform>().position.z;

            linearSteering.Normalize();
            linearSteering.Scale(maxForce);

            return true;
        }
        else
        {
            linearSteering.Scale(Vector3.zero);

            return false;
        }
    } 

    private GameObject findMostThreateningObstacle()
    {
        GameObject mostThreatening = null;
        Vector3 mostThreateningPos = Vector3.zero;

        foreach (GameObject target in targets)
        {
            Vector3 targetPosition = target.GetComponent<Transform>().position;
            Vector3 ray = collisionDetector.intersectsRays(targetPosition);

            bool collides = ray.magnitude < Vector3.positiveInfinity.magnitude;

            if (collides && (mostThreatening == null || Vector3.Distance(transform.position, targetPosition) < Vector3.Distance(transform.position, mostThreateningPos)))
            {
                mostThreatening = target;
                mostThreateningPos = mostThreatening.GetComponent<Transform>().position;
                threatenedRay = ray;
            }
        }
        return mostThreatening;
    }
}
