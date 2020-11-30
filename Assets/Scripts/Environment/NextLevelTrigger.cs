using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelTrigger : MonoBehaviour
{
    public string filter = "";
    public LayerMask layer;
    public List<string> requiredConditions;
    public bool[] requiredConditionsState;
    private bool AllConditionsMet = false;

    void Start()
    {
        requiredConditionsState = new bool[requiredConditions.Count];
    }
    public void SetCondition(string condition)
    {
        if(requiredConditions.Contains(condition))
        {
            requiredConditionsState[requiredConditions.IndexOf(condition)] = true;
            Debug.Log("condition " + condition + " set");
            //check all conditions if they now are true
            foreach(bool state in requiredConditionsState)
            {
                if(state == false)
                    return;
            }
            //all states are true
            AllConditionsMet = true;
        }
        else
        {
            Debug.Log("condition " + condition + " does not exit in requiredConditions");
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("hasbeenshown:" + other.name);
        if((filter == "" || other.name == filter) && (layer.value == 0 || layer.value == (1 << other.gameObject.layer)) && AllConditionsMet == true)
        {
            Debug.Log("next level");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
