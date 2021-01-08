using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SerializedPhysicsObject : SerializedObject
{
    private PhysicsObjectData safeLocation;
    public override object getSaveData()
    {
        safeLocation = new PhysicsObjectData { 
            x  = gameObject.transform.localPosition.x,
            y  = gameObject.transform.localPosition.y,
            z  = gameObject.transform.localPosition.z,
            rx = gameObject.transform.localRotation.x,
            ry = gameObject.transform.localRotation.y,
            rz = gameObject.transform.localRotation.z,
            rw = gameObject.transform.localRotation.w };

        safeLocation.enabled = gameObject.activeSelf;

        return safeLocation;
    }

    public override void setLoadData(object obj)
    {
        PhysicsObjectData data = (PhysicsObjectData) obj;
        gameObject.transform.localPosition = new Vector3(data.x,data.y,data.z);
        gameObject.transform.localRotation = new Quaternion(data.rx,data.ry,data.rz,data.rw);
        gameObject.SetActive(data.enabled);
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
