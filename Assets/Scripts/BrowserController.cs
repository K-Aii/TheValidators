using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class BrowserController : MonoBehaviour
{
    public string test;
    
    [System.Obsolete]
    void Start()
    {
        Application.ExternalEval(@"
        window.addEventListener('message', function(event){
            if(event.data.type === 'BROWSER_TO_UNITY'){
                SendMessage('BrowserController', 'ReceiveData', event.data.value);
            }
        });");

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
            ReceiveData(test);
    }

    void ReceiveData(string value)
    {
        Debug.Log("Received from web app: " + value);
        value = value.Replace(" ", "");         // DELETE ALL SPACE FROM RECEIVED CODE
        string[] commands = value.Split(';') ;

        for (int i = 0; i < commands.Length; i++)
        {
            string methodName = commands[i];
            Debug.Log("Calling method: " + methodName);
            //GameObject.Find("Drone_W").GetComponent<PlayerController>().Invoke(methodName, 0);

            if (methodName.StartsWith("Attack"))
            {
                if (methodName.Length > 8)   // INCLUDES PARAMETERS
                {
                    int duration = int.Parse(Regex.Match(methodName, @"\d+").Value);    // EXTRACT NUMBERS FROM STRING
                    GameObject.Find("Drone_W").GetComponent<PlayerController>().Attack(duration);
                }else
                    GameObject.Find("Drone_W").GetComponent<PlayerController>().Attack();
            }

            if (methodName.StartsWith("Move")) 
            {
                int unit = 0;
                if (Regex.IsMatch(methodName, @"\d+"))
                    unit = int.Parse(Regex.Match(methodName, @"\d+").Value);    // EXTRACT NUMBERS FROM STRING

                if (methodName.Contains("forward"))
                    GameObject.Find("Drone_W").GetComponent<PlayerController>().Move("forward", unit);
                else if (methodName.Contains("back"))
                    GameObject.Find("Drone_W").GetComponent<PlayerController>().Move("back", unit);
                else if (methodName.Contains("left"))
                    GameObject.Find("Drone_W").GetComponent<PlayerController>().Move("left", unit);
                else if (methodName.Contains("right"))
                    GameObject.Find("Drone_W").GetComponent<PlayerController>().Move("right", unit);
                else {
                    string target = Regex.Match(methodName, @"[\(](.*?)[\)]").Groups[1].Value;
                    GameObject.Find("Drone_W").GetComponent<PlayerController>().Move(GameObject.Find(target));
                }



            }
        }

    }
}
