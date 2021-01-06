using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SerializedObjectiveArrows : SerializedObject
{
    ObjectiveArrowsData data;
    ObjectiveArrow objectiveArrow;
    // Start is called before the first frame update
    void Start()
    {
        objectiveArrow = GetComponent<ObjectiveArrow>();
        data = new ObjectiveArrowsData{ 
            objectiveEnabled = objectiveArrow.objectiveEnabled,
            objectiveKeepDisabled = objectiveArrow.objectiveKeepDisabled 
        }; //get/set
    }

    public override object getSaveData()
    {
        if(objectiveArrow == null || data == null)
            Start();
        data.objectiveEnabled = objectiveArrow.objectiveEnabled;
        data.objectiveKeepDisabled = objectiveArrow.objectiveKeepDisabled;
        return data;
    }

    public override void setLoadData(object obj)
    {
        if(objectiveArrow == null)
            objectiveArrow = GetComponent<ObjectiveArrow>();
            
        data = (ObjectiveArrowsData) obj;
        objectiveArrow.objectiveEnabled = data.objectiveEnabled;
        objectiveArrow.objectiveKeepDisabled = data.objectiveKeepDisabled;
    }

    [System.Serializable]
    private class ObjectiveArrowsData
    {
        public bool[] objectiveEnabled;
        public bool[] objectiveKeepDisabled;
    }
}
