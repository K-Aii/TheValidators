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

            // PLAYER WIN - ENEMY HP ZERO
            if (hp <= 0)
            {
                this.gameObject.SetActive(false);
                GameObject.Find("BattleResult").GetComponent<UnityEngine.UI.Text>().text = "WIN";
                GameObject.Find("Gate").GetComponent<SphereCollider>().isTrigger = true;
            }
        }
    }

    // TELEPORT AND ATTACK WHEN PLAYER NEARBY
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            teleportTimer += Time.deltaTime;
            if (teleportTimer > 3) 
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
