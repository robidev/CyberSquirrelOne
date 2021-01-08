using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SerializedNextLevelObject : SerializedObject
{
    NextLevelTriggerObjectData data;
    NextLevelTrigger nextLevel;
    // Start is called before the first frame update
    void Awake()
    {
        nextLevel = GetComponent<NextLevelTrigger>();
        data = new NextLevelTriggerObjectData{ triggers = nextLevel.requiredConditionsState }; //get/set
    }

    public override object getSaveData()
    {
        if(nextLevel == null || data == null)
            Awake();

        data.triggers = nextLevel.requiredConditionsState;
        return data;
    }

    public override void setLoadData(object obj)
    {
        if(nextLevel == null)
            nextLevel = GetComponent<NextLevelTrigger>();
            
        data = (NextLevelTriggerObjectData) obj;
        nextLevel.requiredConditionsState = data.triggers;
        Debug.Log("data.triggers:" + data.triggers);
    }

    [System.Serializable]
    private class NextLevelTriggerObjectData
    {
        public bool[] triggers;
    }
}
