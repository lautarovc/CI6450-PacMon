using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ThreeRays : MonoBehaviour {

    // Target for testing
    public Transform target;
    public float targetRadius = 2f;

    // Rays definition
    public Vector3 ray1;
    public Vector3 ray2;
    public Vector3 ray3;

    // Endpoint of rays
    public Vector3 rayEnd1;
    public Vector3 rayEnd2;
    public Vector3 rayEnd3;

    // rays Distance
    public float mainDistance = 2f;
    public float sideDistance = 2f;
    public float sideAngle = 30f;

	// Use this for initialization
	void Start () {

    }

    // Update is called once per frame
    void Update () {

        defineRays();
        rotateRays();
        drawRays();

        Vector3 intersection = intersectsRays(target.position);
        if (intersection.magnitude < Vector3.positiveInfinity.magnitude)
        {
            //Debug.Log(intersection.ToString());
        }
    }

    private void defineRays()
    {
        // Define points
        Vector3 mainPoint = transform.position + (Vector3.forward * mainDistance);

        Vector3 sidePoint = transform.position + (Vector3.forward * sideDistance);

        // Define rays
        Vector3 sideRay = sidePoint - transform.position;

        ray1 = mainPoint - transform.position;

        // Rotate side rays
        ray2 = Quaternion.AngleAxis(-sideAngle, Vector3.up) * sideRay;

        ray3 = Quaternion.AngleAxis(sideAngle, Vector3.up) * sideRay;
    }

    private void rotateRays()
    {
        // Rotate rays to where the character is looking
        float characterAngle = transform.rotation.eulerAngles.y;

        ray1 = Quaternion.AngleAxis(characterAngle, Vector3.up) * ray1;

        ray2 = Quaternion.AngleAxis(characterAngle, Vector3.up) * ray2;

        ray3 = Quaternion.AngleAxis(characterAngle, Vector3.up) * ray3;

        rayEnd1 = transform.position + ray1;
        rayEnd2 = transform.position + ray2;
        rayEnd3 = transform.position + ray3;
    }

    private void drawRays()
    {
        Debug.DrawRay(transform.position, ray1, Color.green, 0.0f, false);
        Debug.DrawRay(transform.position, ray2, Color.green, 0.0f, false);
        Debug.DrawRay(transform.position, ray3, Color.green, 0.0f, false);
    }

    private bool intersectsRay(Vector3 ray, Vector3 point)
    {
        Vector3 toPoint = point - transform.position;

        Vector3 product = Vector3.Cross(ray, toPoint);

        // Check colinearity
        if (product != Vector3.zero)
        {
            return false;
        }

        // Check range
        float kToPoint = Vector3.Dot(transform.position, toPoint);
        float kPosition = Vector3.Dot(transform.position, transform.position);

        if ((kToPoint < 0) || (kToPoint > kPosition))
        {   
            return false;
        }

        return true;

    }

    private bool intersectsSphere(Vector3 rayStart, Vector3 rayEnd, Vector3 center, float radius)
    {

        float d = HandleUtility.DistancePointLine(center, rayStart, rayEnd);

        return (Mathf.Abs(d) <= radius);
    }

    public Vector3 intersectsRays(Vector3 center)
    {
        if (intersectsSphere(transform.position, rayEnd1, center, targetRadius))
        {
            return rayEnd1;
        }

        if (intersectsSphere(transform.position, rayEnd2, center, targetRadius))
        {
            return rayEnd2;
        }

        if (intersectsSphere(transform.position, rayEnd3, center, targetRadius))
        {
            return rayEnd3;
        }

        return Vector3.positiveInfinity;
    }

    private void avoid(Vector3 ray, float degrees)
    {
        Vector3 falseTarget = Quaternion.AngleAxis(degrees, Vector3.up) * ray;

        falseTarget = transform.position + falseTarget;


    }
}
