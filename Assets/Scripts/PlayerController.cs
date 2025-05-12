using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float timer;
    float hp;

    
    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        hp = 100;

        GameObject.Find("Buddy.HP").GetComponent<UnityEngine.UI.Text>().text = hp.ToString();

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(timer);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Enemy") 
        {          
            timer += Time.deltaTime;
            if (timer > 1) {
                hp -= 1;
                GameObject.Find("Buddy.HP").GetComponent<UnityEngine.UI.Text>().text = hp.ToString();
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
