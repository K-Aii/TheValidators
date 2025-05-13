using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float timer;
    float hp;

    Animator sparkAnim;
    
    public Transform shootingPoint;
    public GameObject bulletGO;
    public float bulletSpeed;
    
    void Start()
    {
        timer = 0;
        hp = 100;
        GameObject.Find("Buddy.HP").GetComponent<UnityEngine.UI.Text>().text = hp.ToString();
        sparkAnim = gameObject.GetComponentInChildren<Animator>();
    }

    void Update()
    {
        Debug.Log(timer);

        // PLAYER ATTACK - SHOOTING
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var bullet = Instantiate(bulletGO, shootingPoint.position, bulletGO.transform.rotation);
            bullet.GetComponent<Rigidbody>().velocity = shootingPoint.forward * bulletSpeed;
        }
    }

    // HP DEDUCTION FROM ENEMY ATTACK
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Enemy") 
        {          
            timer += Time.deltaTime;
            if (timer > 1) {
                hp -= 1;
                GameObject.Find("Buddy.HP").GetComponent<UnityEngine.UI.Text>().text = hp.ToString();
                sparkAnim.SetTrigger("Spark");
                timer = 0;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            timer = 0;
        }
    }
}
