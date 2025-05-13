using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroy : MonoBehaviour
{

    public float lifetime;

    void Awake()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnDestroy()
    {
        // ALLOW PLAYER MOVEMENT WHEN DAMAGEZONE DESTROYED
        if (this.gameObject.tag == "Enemy")
        {
            GameObject.Find("Drone_W").GetComponent<PlayerController>().isTrapped = false;
        }

    }

}
