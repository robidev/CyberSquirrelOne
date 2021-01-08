using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SerializedPhysicsObject : SerializedObject
{
    private PhysicsObjectData safeLocation;

    void Awake()
    {
        safeLocation = new PhysicsObjectData { 
            x  = gameObject.transform.localPosition.x,
            y  = gameObject.transform.localPosition.y,
            z  = gameObject.transform.localPosition.z,
            rx = gameObject.transform.localRotation.x,
            ry = gameObject.transform.localRotation.y,
            rz = gameObject.transform.localRotation.z,
            rw = gameObject.transform.localRotation.w, 
            enabled = gameObject.activeSelf };
    }
    public override object getSaveData()
    {
        if(safeLocation == null)
            Awake();
        safeLocation.x  = gameObject.transform.localPosition.x;
        safeLocation.y  = gameObject.transform.localPosition.y;
        safeLocation.z  = gameObject.transform.localPosition.z;
        safeLocation.rx = gameObject.transform.localRotation.x;
        safeLocation.ry = gameObject.transform.localRotation.y;
        safeLocation.rz = gameObject.transform.localRotation.z;
        safeLocation.rw = gameObject.transform.localRotation.w; 
        safeLocation.enabled = gameObject.activeSelf;

        Debug.Log("save:" + gameObject.transform.localPosition.ToString() + " " + gameObject.transform.localRotation.ToString());
        return safeLocation;
    }

    public override void setLoadData(object obj)
    {
        PhysicsObjectData data = (PhysicsObjectData) obj;
        gameObject.transform.localPosition = new Vector3(data.x,data.y,data.z);
        gameObject.transform.localRotation = new Quaternion(data.rx,data.ry,data.rz,data.rw);
        gameObject.SetActive(data.enabled);
        Debug.Log("load:" + gameObject.transform.localPosition.ToString() + " " + gameObject.transform.localRotation.ToString());
    }

    [System.Serializable]
    private class PhysicsObjectData
    {
        public float x;
        public float y;
        public float z;
        public float rx;
        public float ry;
        public float rz;
        public float rw;
        public bool enabled;
    }
}
