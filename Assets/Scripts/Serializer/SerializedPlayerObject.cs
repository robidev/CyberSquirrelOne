using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SerializedPlayerObject : SerializedObject
{
    private PlayerObjectData safeLocation;
    void Start()
    {
        safeLocation = new PlayerObjectData { 
            x = gameObject.transform.position.x,
            y = gameObject.transform.position.y,
            z = gameObject.transform.position.z };
        safeLocation.spriteRenderer = GetComponent<SpriteRenderer>().enabled;
    }
    public override object getSaveData()
    {
        if(safeLocation == null)
            Start();
            
        safeLocation.spriteRenderer = GetComponent<SpriteRenderer>().enabled;
        return safeLocation;
    }

    public override void setLoadData(object obj)
    {
        PlayerObjectData data = (PlayerObjectData) obj;
        gameObject.transform.position = new Vector3(data.x,data.y,data.z);
        GetComponent<SpriteRenderer>().enabled = data.spriteRenderer;
        //Debug.Log(data.x);
    }

    public void setSafeLocation()
    {
        safeLocation = new PlayerObjectData { 
            x = gameObject.transform.position.x,
            y = -3.9f, //gameObject.transform.position.y, clamp to floor
            z = gameObject.transform.position.z };
    }

    [System.Serializable]
    private class PlayerObjectData
    {
        public float x;
        public float y;
        public float z;
        public bool spriteRenderer;
    }
}
