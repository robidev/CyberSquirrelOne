using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SerializedDialogObject : SerializedObject
{
    DialogObjectData data;
    DialogueTrigger dialog;
    // Start is called before the first frame update
    void Start()
    {
        dialog = GetComponent<DialogueTrigger>();
        data = new DialogObjectData{ hasBeenShown = dialog.DialogHasBeenShown }; //get/set
    }

    public override object getSaveData()
    {
        if(dialog == null || data == null)
            Start();
        data.hasBeenShown = dialog.DialogHasBeenShown;
        return data;
    }

    public override void setLoadData(object obj)
    {
        if(dialog == null)
            dialog = GetComponent<DialogueTrigger>();
            
        data = (DialogObjectData) obj;
        dialog.DialogHasBeenShown = data.hasBeenShown;
        Debug.Log("data.hasBeenShown:" + data.hasBeenShown);
    }

    [System.Serializable]
    private class DialogObjectData
    {
        public bool hasBeenShown;
    }
}
