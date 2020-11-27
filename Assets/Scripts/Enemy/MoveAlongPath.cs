using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAlongPath : MonoBehaviour
{
    public GameObject[] PathNode;
    public GameObject MovedObject;
    public float MoveSpeed;
    float Timer;
    Vector3 CurrentPositionHolder;
    int CurrentNode;
    private Vector2 startPosition;
    public Transform nodes;
    float distance;
    public bool lineairSpeed = true;
    public bool EnableMovement = true;

    void Awake()
    {
        //Debug.Log(name + " Awake");
        if(nodes != null)
        {
            PathNode=new GameObject[nodes.childCount];
            for(int i=0; i<nodes.childCount; i++)
            {
                PathNode[i] = nodes.GetChild(i).gameObject;
            }
        }
    }
    // Use this for initialization
    void Start () 
    {
        //Debug.Log(name + " Start");
        CheckNode ();
    }
    void CheckNode()
    {
        Timer = 0;
        startPosition = MovedObject.transform.position;
        CurrentPositionHolder = PathNode[CurrentNode].transform.position;
        var properties = PathNode[CurrentNode].GetComponent<MoveAlongModifier>();
        if(properties != null)
        {
            properties.OnEnter();
        }
        distance = Vector3.Distance(startPosition, CurrentPositionHolder);
    }
    // Update is called once per frame
    void Update () {
        if(EnableMovement)
        {
            if(lineairSpeed)
                Timer += Time.deltaTime * (MoveSpeed / distance);
            else
                Timer += Time.deltaTime * MoveSpeed;

            if (MovedObject.transform.position != CurrentPositionHolder) {
                MovedObject.transform.position = Vector3.Lerp (startPosition, CurrentPositionHolder, Timer);
            }
            else{
                //Debug.Log(name + " is going:" + PathNode[CurrentNode].name);
                var properties = PathNode[CurrentNode].GetComponent<MoveAlongModifier>();
                if(properties != null)
                {
                    properties.OnReached();
                }
                if(CurrentNode < PathNode.Length -1)
                {
                    CurrentNode++;
                    CheckNode ();
                }
            }
        }
    }

    public void EnableMovementPath()
    {
        EnableMovement = true;
    }
}