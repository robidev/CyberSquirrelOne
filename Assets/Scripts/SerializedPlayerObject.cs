using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SerializedPlayerObject : SerializedObject
{
    public override object getSaveData()
    {
        //TODO: ensure player is saved to a safe position
        var obj = new PlayerObjectData { 
            x = gameObject.transform.position.x,
            y = gameObject.transform.position.y,
            z = gameObject.transform.position.z };
        return obj;
    }

    public override void setLoadData(object obj)
    {
        PlayerObjectData data = (PlayerObjectData) obj;
        gameObject.transform.position = new Vector3(data.x,data.y,data.z);
        Debug.Log(data.x);
    }
    [System.Serializable]
    private class PlayerObjectData
    {
        public float x;
        public float y;
        public float z;
    }
}
