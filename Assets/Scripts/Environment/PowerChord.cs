using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerChord : MonoBehaviour
{

    public Transform anchor;
    DistanceJoint2D joint;
    LineRenderer line;
    // Start is called before the first frame update
    void Start()
    {
        joint = GetComponent<DistanceJoint2D> ();
        line = GetComponent<LineRenderer>();
        line.SetPosition(0,anchor.position);
        line.startColor = Color.black;
        line.endColor = Color.black;
    }

    // Update is called once per frame
    void Update()
    {
		  line.SetPosition(1,transform.position);
    }
}
