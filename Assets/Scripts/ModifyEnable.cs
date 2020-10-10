using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifyEnable : MonoBehaviour
{
    public void ModifyToggle()
    {
        gameObject.SetActive(!gameObject.activeSelf);
        
        if(gameObject.activeSelf == true)//if we just enabled the dialog, disable siblings
        {
            foreach( ModifyEnable modifyEnable in transform.parent.GetComponentsInChildren<ModifyEnable>())
            {
                if(modifyEnable.gameObject != gameObject)
                {
                    modifyEnable.gameObject.SetActive(false);
                }
            }
        }
    }
}
