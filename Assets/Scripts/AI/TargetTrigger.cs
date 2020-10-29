using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetTrigger : MonoBehaviour
{
    public GameObject receiver;
    public LayerMask layer;
    public string _tag;
    private bool triggered = false;

    void Start()
    {
        InvokeRepeating("unTrigger",3f,3f);//to ensure we re-trigger if we missed the onenter event for some reason
    }

    void unTrigger()
    {
        triggered = false;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        //Debug.Log(name + ":" + col.name);
        if( ((1<<col.gameObject.layer) & layer.value) > 0 && receiver != null)
        {
            //Debug.Log(name + ":" + col.name + " + layer");
            if(_tag.Equals("") || _tag.Equals(col.transform.tag))
            {
                //Debug.Log(name + ":" + col.name + " + layer + tag");
                var ai = receiver.GetComponent<EnemyAI>();
                //Debug.Log("receiver:" + receiver.name);
                if(ai != null)
                    ai.Enemytarget = col.transform; 
                else
                    Debug.Log("no enemy AI");
            }
            triggered = true;
        }   
    }

    void OnTriggerStay2D(Collider2D col)
    {
        //Debug.Log(name + ":" + col.name);
        if(triggered == false && ((1<<col.gameObject.layer) & layer.value) > 0 && receiver != null)
        {
            //Debug.Log(name + ":" + col.name + " + layer");
            if(_tag.Equals("") || _tag.Equals(col.transform.tag))
            {
                //Debug.Log(name + ":" + col.name + " + layer + tag");
                var ai = receiver.GetComponent<EnemyAI>();
                Debug.Log("receiver:" + receiver.name);
                if(ai != null)
                    ai.Enemytarget = col.transform; 
                else
                    Debug.Log("no enemy AI");
                triggered = true;
            }
        }   
    }

    void OnTriggerExit2D(Collider2D col)
    {
        //Debug.Log(name + ":" + col.name);
        if( ((1<<col.gameObject.layer) & layer.value) > 0 && receiver != null)
        {
            //Debug.Log(name + ":" + col.name + " + layer");
            if(_tag.Equals("") || _tag.Equals(col.transform.tag))
            {
                //Debug.Log(name + ":" + col.name + " + layer + tag");
                var ai = receiver.GetComponent<EnemyAI>();
                //Debug.Log("receiver:" + receiver.name);
                if(ai != null)
                    ai.UnsetEnemyTarget(col.transform);
                else
                    Debug.Log("no enemy AI");
                triggered = false;
            }
        }   
    }
}
