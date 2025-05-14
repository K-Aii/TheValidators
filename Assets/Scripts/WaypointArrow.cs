using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrow_waypoints : MonoBehaviour
{
    public Transform target;
    public float arrowspeed;

    LayerMask targetLayer;
    public float visibleDistance;

    private void Start()
    {
        targetLayer = LayerMask.GetMask("Enemy");
    }

    void Update()
    {
        // ARROW POINTING TOWARDS ENEMY
        Vector3 relativePos = target.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
        transform.rotation = rotation;

        // SHOW ARROW ONLY WHEN ENEMY NOT NEARBY
        if (Physics.CheckSphere(transform.position, visibleDistance, targetLayer))
            GetComponentInChildren<MeshRenderer>().enabled = false;
        else
            GetComponentInChildren<MeshRenderer>().enabled = true;
    }
}