using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    float hp;

    float teleportTimer;
    public Transform[] spawnPoints;
    public GameObject damageZone;
    LayerMask zoneLayer;

    void Start()
    {
        hp = 100;
        teleportTimer = 0;
        GameObject.Find("Enemy.HP").GetComponent<UnityEngine.UI.Text>().text = hp.ToString();

        zoneLayer = LayerMask.GetMask("Damage");
    }

    void Update()
    {
        
    }

    // HP DEDUCTION FROM PLAYER ATTACK
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "PlayerAttack")
        {
            hp -= 3;
            GameObject.Find("Enemy.HP").GetComponent<UnityEngine.UI.Text>().text = hp.ToString();
            Destroy(collision.gameObject);
        }
    }

    // TELEPORT AND ATTACK WHEN PLAYER NEARBY
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            teleportTimer += Time.deltaTime;
            //Debug.Log(teleportTimer);
            if (teleportTimer > 2) 
            {
                // ATTACK - SPAWN IF NOT EXIST
                Vector3 zonePos = new Vector3(transform.position.x, 0.5f, transform.position.z);
                if (!Physics.CheckSphere(zonePos, 0.5f, zoneLayer))
                    Instantiate(damageZone, zonePos , Quaternion.identity); 
                
                // TELEPORT
                transform.position = spawnPoints[Random.Range(0, 3)].position; 

            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
            teleportTimer = 0;
    }   
}
