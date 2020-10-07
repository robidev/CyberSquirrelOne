using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifyEnable : MonoBehaviour
{
    public void ModifyToggle()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
