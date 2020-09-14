using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableEffect : MonoBehaviour
{
    GameObject child;

    void Start()
    {
        child = transform.GetChild(0).gameObject;
    }

    public void Break()
    {
        child.SetActive(false);
    }
}
