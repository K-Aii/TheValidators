using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;

public class BrowserController : MonoBehaviour
{
    public string test;
    PlayerController pc;
    public Queue<System.Action> functionQueue = new Queue<System.Action>();

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

    private void Awake()
    {
        pc = GameObject.Find("Drone_W").GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
            ReceiveData(test);
    }

    void ReceiveData(string value)
    {        
        Debug.Log("Received from web app: " + value);

        string[] commands = value.Split(';') ;

        for (int i = 0; i < commands.Length; i++)
        {
            string methodName = commands[i];
            Debug.Log("Adding method to queue: " + methodName);

            if (methodName.StartsWith("Attack"))
            {
                if (methodName.Length > 8)   // INCLUDES PARAMETERS
                {
                    int duration = int.Parse(Regex.Match(methodName, @"\d+").Value);    // EXTRACT NUMBERS FROM STRING
                    functionQueue.Enqueue(() => pc.Attack(duration)); 
                }else
                    functionQueue.Enqueue(() => pc.Attack());
            }

            if (methodName.StartsWith("Move")) 
            {
                int unit = 0;
                if (Regex.IsMatch(methodName, @"\d+"))
                    unit = int.Parse(Regex.Match(methodName, @"\d+").Value);    // EXTRACT NUMBERS FROM STRING

                if (methodName.Contains("forward"))
                    functionQueue.Enqueue(() => pc.Move("forward", unit)); 
                else if (methodName.Contains("back"))
                    functionQueue.Enqueue(() => pc.Move("back", unit));
                else if (methodName.Contains("left"))
                    functionQueue.Enqueue(() => pc.Move("left", unit));
                else if (methodName.Contains("right"))
                    functionQueue.Enqueue(() => pc.Move("right", unit));
                else
                {
                    string target = Regex.Match(methodName, @"[\(](.*?)[\)]").Groups[1].Value;
                    functionQueue.Enqueue(() => pc.Move(GameObject.Find(target))); 
                }
            }

            if (methodName.StartsWith("Restart"))
            {
                SceneManager.LoadScene("TrainingRoom");
                Time.timeScale = 1;
            }
        }
        NextAction();
             
        
    }
    public void NextAction()
    {
        
        if (functionQueue.Count > 0)
        {
            System.Action nextFunction = functionQueue.Dequeue();
            nextFunction.Invoke();
        }

    }
    
}
