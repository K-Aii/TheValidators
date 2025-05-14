using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrow_waypoints : MonoBehaviour
{
    public Transform target;
    public float arrowspeed;

    LayerMask targetLayer;
    public float visibleDistance;
    GameObject enemy;

    private void Start()
    {
        targetLayer = LayerMask.GetMask("Enemy");
        enemy = GameObject.Find("Drone_B");
    }

    void Update()
    {
        // ARROW POINTING TOWARDS ENEMY
        Vector3 relativePos = target.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
        transform.rotation = rotation;

        // HIDE ARROW WHEN ENEMY NEARBY AND DEACTIVATED
        if (Physics.CheckSphere(transform.position, visibleDistance, targetLayer))
            GetComponentInChildren<MeshRenderer>().enabled = false;     // CLOSE
        else
        {
            if(!enemy.activeSelf)
                GetComponentInChildren<MeshRenderer>().enabled = false; // DEACTIVATED
            else
                GetComponentInChildren<MeshRenderer>().enabled = true;      // FAR
        }
    }
}