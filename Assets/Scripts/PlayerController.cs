using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float damageTimer;
    float hp;

    Animator sparkAnim;
    
    public Transform shootingPoint;
    public GameObject bulletGO;
    public float bulletSpeed;

    public float walkSpeed;
    public bool isTrapped;

    void Start()
    {
        damageTimer = 0;
        hp = 100;
        isTrapped = false;
        GameObject.Find("Buddy.HP").GetComponent<UnityEngine.UI.Text>().text = hp.ToString();
        sparkAnim = gameObject.GetComponentInChildren<Animator>();
    }

    void Update()
    {
        //Debug.Log(damageTimer);

        // PLAYER ATTACK - SHOOTING
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var bullet = Instantiate(bulletGO, shootingPoint.position, bulletGO.transform.rotation);
            bullet.GetComponent<Rigidbody>().velocity = shootingPoint.forward * bulletSpeed;
        }

        // PLAYER MOVEMENT
        if (!isTrapped)
        {
            Vector3 moveVec = new Vector3(Input.GetAxis("Vertical"), 0, -Input.GetAxis("Horizontal"));
            transform.position += moveVec * walkSpeed * Time.deltaTime;
        }
    }

    // HP DEDUCTION FROM ENEMY ATTACK
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Enemy") 
        {
            isTrapped = true;
            damageTimer += Time.deltaTime;
            if (damageTimer > 1) {
                hp -= 5;
                GameObject.Find("Buddy.HP").GetComponent<UnityEngine.UI.Text>().text = hp.ToString();
                sparkAnim.SetTrigger("Spark");
                damageTimer = 0;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            damageTimer = 0;
            isTrapped = false;
        }
    }
}
