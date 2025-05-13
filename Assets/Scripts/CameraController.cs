using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Transform player;
    Vector3 offset;
    
    void Start()
    {
        player = GameObject.Find("Drone_W").transform;
        offset = transform.position - player.position;
    }

    void Update()
    {
        Vector3 newPos = player.position + offset;
        transform.position = Vector3.Slerp(transform.position, newPos, 0.5f);
    }
}
