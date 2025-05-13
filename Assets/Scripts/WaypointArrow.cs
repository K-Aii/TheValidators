using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrow_waypoints : MonoBehaviour
{
    public Transform target;
    public float arrowspeed;

    void Update()
    {
        Vector3 relativePos = target.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
        transform.rotation = rotation;
    }
}