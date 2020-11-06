using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAlongPath : MonoBehaviour
    {
    public GameObject[] PathNode;
    public GameObject MovedObject;
    public float MoveSpeed;
    float Timer;
    static Vector3 CurrentPositionHolder;
    int CurrentNode;
    private Vector2 startPosition;


    // Use this for initialization
    void Start () 
    {
        //PathNode = GetComponentInChildren<>();
        CheckNode ();
    }

    public Transform nodes;
     
    void Awake()
    {
        PathNode=new GameObject[nodes.childCount];
        for(int i=0; i<nodes.childCount; i++)
        {
            PathNode[i] = nodes.GetChild(i).gameObject;
        }
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
    }

    // Update is called once per frame
    void Update () {

        Timer += Time.deltaTime * MoveSpeed;

        if (MovedObject.transform.position != CurrentPositionHolder) {
            MovedObject.transform.position = Vector3.Lerp (startPosition, CurrentPositionHolder, Timer);
        }
        else{
            if(CurrentNode < PathNode.Length -1)
            {
                CurrentNode++;
                CheckNode ();
            }
        }
    }
}