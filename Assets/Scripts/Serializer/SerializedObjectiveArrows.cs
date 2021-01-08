using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SerializedObjectiveArrows : SerializedObject
{
    ObjectiveArrowsData data;
    ObjectiveArrow objectiveArrow;
    // Start is called before the first frame update
    void Awake()
    {
        objectiveArrow = GetComponent<ObjectiveArrow>();
        data = new ObjectiveArrowsData{ 
            objectiveEnabled = objectiveArrow.objectiveEnabled,
            objectiveKeepDisabled = objectiveArrow.objectiveKeepDisabled,
            enableArrows = objectiveArrow.enableArrows,
            objectiveList = objectiveArrow.objectiveList
        }; //get/set
    }

    public override object getSaveData()
    {
        if(objectiveArrow == null || data == null)
            Awake();
            
        data.objectiveEnabled = objectiveArrow.objectiveEnabled;
        data.objectiveKeepDisabled = objectiveArrow.objectiveKeepDisabled;
        data.enableArrows = objectiveArrow.enableArrows;
        data.objectiveList = objectiveArrow.objectiveList;
        return data;
    }

    public override void setLoadData(object obj)
    {
        if(objectiveArrow == null)
            objectiveArrow = GetComponent<ObjectiveArrow>();
            
        data = (ObjectiveArrowsData) obj;
        objectiveArrow.objectiveEnabled = data.objectiveEnabled;
        objectiveArrow.objectiveKeepDisabled = data.objectiveKeepDisabled;
        objectiveArrow.enableArrows = data.enableArrows;
        objectiveArrow.objectiveList = data.objectiveList;
    }

    [System.Serializable]
    private class ObjectiveArrowsData
    {
        public bool[] objectiveEnabled;
        public bool[] objectiveKeepDisabled;
        public List<int> objectiveList;
        public bool enableArrows;
    }
}
