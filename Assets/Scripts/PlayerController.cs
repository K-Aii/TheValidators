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
    public float rotateSpeed;
    public bool isTrapped;
    Rigidbody rb;

    public int test;

    void Start()
    {
        damageTimer = 0;
        hp = 100;
        isTrapped = false;
        GameObject.Find("Buddy.HP").GetComponent<UnityEngine.UI.Text>().text = hp.ToString();
        sparkAnim = gameObject.GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack(test);
        }

        // PLAYER MOVEMENT
        if (!isTrapped)
        {
            Vector3 moveVec = new Vector3(Input.GetAxis("Vertical"), 0, -Input.GetAxis("Horizontal"));
            transform.position += moveVec * walkSpeed * Time.deltaTime;
            if(Input.GetAxis("Vertical") != 0)
                transform.rotation = Quaternion.LookRotation(new Vector3(Input.GetAxis("Vertical"), 0, 0));
        }

        if (Input.GetKeyDown(KeyCode.H))
            //Move(test, 1);
            Move(GameObject.Find("EnemyFront"));

    }

    // PLAYER ATTACK - SHOOTING
    public void Attack(int duration = 1) {
        IEnumerator AttackTimer() {
            for (int i = 0; i < duration; i++) { 
                var bullet = Instantiate(bulletGO, shootingPoint.position, bulletGO.transform.rotation);
                bullet.GetComponent<Rigidbody>().velocity = shootingPoint.forward * bulletSpeed;
                yield return new WaitForSeconds(0.2f);
            }
        }
        StartCoroutine(AttackTimer());
        
    }

    // PLAYER MOVEMENT
    public void Move(GameObject target)
    {
        Vector3 targetPos = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
        IEnumerator MoveTo()
        {           
            while (Vector3.Distance(targetPos, transform.position) > 0)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPos, walkSpeed * Time.deltaTime);
                Vector3 moveDirection = (targetPos - transform.position).normalized;
                if(moveDirection != Vector3.zero)
                    transform.rotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0, 0));
                else
                    transform.rotation = Quaternion.Euler(0, 90, 0);
                yield return null;
            }
        }
        if(!isTrapped)
            StartCoroutine(MoveTo());
    }
    public void Move(string direction, int unit)
    {
        IEnumerator MoveTimer(Vector3 direction, float _unit)
        {
            if (!isTrapped)
            {
                float i = 0f;
                while (i < _unit)
                {
                    i += Time.deltaTime;
                    rb.velocity = direction * walkSpeed;
                    yield return new WaitForSeconds(Time.deltaTime);
                }
                rb.velocity = Vector3.zero;
                yield break;
            }
        }

        switch (direction) {
            case "forward":
                transform.rotation = Quaternion.Euler(0, 90, 0);
                StartCoroutine(MoveTimer(Vector3.right, unit));
                break;
            case "back":
                transform.rotation = Quaternion.Euler(0, -90, 0);
                StartCoroutine(MoveTimer(Vector3.left, unit));
                break;
            case "left":
                StartCoroutine(MoveTimer(Vector3.forward, unit));
                break;
            case "right":
                StartCoroutine(MoveTimer(Vector3.back, unit));
                break;
            default:    
                break;
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
                hp -= 6;
                GameObject.Find("Buddy.HP").GetComponent<UnityEngine.UI.Text>().text = hp.ToString();
                sparkAnim.SetTrigger("Spark");
                damageTimer = 0;

                // PLAYER LOSE - OWN HP ZERO
                if (hp <= 0)
                {
                    Time.timeScale = 0;
                    GameObject.Find("BattleResult").GetComponent<UnityEngine.UI.Text>().text = "LOSE";
                }
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
