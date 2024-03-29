﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SerializedPlayerObject : SerializedObject
{
    private PlayerObjectData safeLocation;
    void Awake()
    {
        safeLocation = new PlayerObjectData { 
            x = gameObject.transform.position.x,
            y = gameObject.transform.position.y,
            z = gameObject.transform.position.z };
        safeLocation.enabled = gameObject.activeSelf;
        //GetComponent<SpriteRenderer>().enabled;
        Debug.Log("start_" + safeLocation.x);
    }
    public override object getSaveData()
    {
        if(safeLocation == null)
            Awake();
            
        safeLocation.enabled = gameObject.activeSelf; //GetComponent<SpriteRenderer>().enabled;
        Debug.Log("safe_" + safeLocation.x);
        return safeLocation;
    }

    public override void setLoadData(object obj)
    {
        PlayerObjectData data = (PlayerObjectData) obj;
        gameObject.transform.position = new Vector3(data.x,data.y,data.z);
        gameObject.SetActive(data.enabled);
        //GetComponent<SpriteRenderer>().enabled = data.spriteRenderer;
        //Debug.Log("data.spriteRenderer:" + data.spriteRenderer);
        Debug.Log("load_" + data.x);
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
        public bool enabled;
    }
}
