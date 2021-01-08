using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SerializedDialogObject : SerializedObject
{
    DialogObjectData data;
    DialogueTrigger dialog;
    // Start is called before the first frame update
    void Awake()
    {
        dialog = GetComponent<DialogueTrigger>();
        data = new DialogObjectData{ 
            hasBeenShown = dialog.DialogHasBeenShown,
            isActive = gameObject.activeSelf 
        }; //get/set
    }

    public override object getSaveData()
    {
        if(dialog == null || data == null)
            Awake();
        data.hasBeenShown = dialog.DialogHasBeenShown;
        data.isActive = gameObject.activeSelf;
        return data;
    }

    public override void setLoadData(object obj)
    {
        if(dialog == null)
            dialog = GetComponent<DialogueTrigger>();
            
        data = (DialogObjectData) obj;
        dialog.DialogHasBeenShown = data.hasBeenShown;
        gameObject.SetActive(data.isActive);
        Debug.Log("data.hasBeenShown:" + data.hasBeenShown);
        Debug.Log("data.isActive:" + data.isActive);
    }

    [System.Serializable]
    private class DialogObjectData
    {
        public bool hasBeenShown;
        public bool isActive;
    }
}
