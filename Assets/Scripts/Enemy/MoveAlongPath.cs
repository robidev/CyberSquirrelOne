using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoveAlongPath : MonoBehaviour
{
    public GameObject[] PathNode;
    public GameObject MovedObject;
    public float MoveSpeed;
    float _Timer;
    Vector3 CurrentPositionHolder;
    int CurrentNode;
    private Vector3 startPosition;
    public Transform nodes;
    float distance;
    public bool lineairSpeed = true;
    public bool EnableMovement = true;
    public float MinimalDistance = 0.5f;
    public TextMeshProUGUI selectedText;

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
        _Timer = 0;
        startPosition = MovedObject.transform.position;
        CurrentPositionHolder = PathNode[CurrentNode].transform.position;
        var properties = PathNode[CurrentNode].GetComponent<MoveAlongModifier>();
        if(properties != null)
        {
            properties.OnEnter();
            //Debug.Log("onenter returned");
        }
        distance = Vector3.Distance(startPosition, CurrentPositionHolder);
    }
    // Update is called once per frame
    void Update () {
        if(EnableMovement)
        {
            if(lineairSpeed)
                _Timer += Time.deltaTime * (MoveSpeed / distance);
            else
                _Timer += Time.deltaTime * MoveSpeed;
            float _distance = Vector3.Distance(MovedObject.transform.position,CurrentPositionHolder);
            if(selectedText != null)
                selectedText.text = "dist:" + _distance.ToString();
                
            if (_distance > MinimalDistance) {
                MovedObject.transform.position = Vector3.Lerp (startPosition, CurrentPositionHolder, _Timer);
                //Debug.Log(_distance);
            }
            else{
                //Debug.Log(name + " is going:" + PathNode[CurrentNode].name);
                var properties = PathNode[CurrentNode].GetComponent<MoveAlongModifier>();
                if(properties != null)
                {
                    properties.OnReached();
                    //Debug.Log("onreached returned");
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