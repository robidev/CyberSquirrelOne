using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TigerForge;
using System;

public class StateSerializer : MonoBehaviour
{
    void Start()
    {
        //Save();
        //Load();
    }

    public void LoadFromFile(string fileName)
    {
        if(fileName == "" || fileName == null)
        {
            Debug.Log("No file specified to load");
            return;
        }
        EasyFileSave saveFile = new EasyFileSave(fileName);
        Debug.Log("Filename:" + saveFile.GetFileName());
        try
        {
            if(!saveFile.Load())
            {
                Debug.Log("load issue");
                return;
            }

            object[] obj = Resources.FindObjectsOfTypeAll<SerializedObject>(); //this ensures disabled object are also considered
            //object[] obj = GameObject.FindObjectsOfType(typeof (SerializedObject)); //this ignores disabled objects
            foreach (object o in obj)
            {
                SerializedObject g = (SerializedObject) o;
                if(g.UUID == "")
                    continue;

                var objData = saveFile.GetBinary(g.UUID);
                if(objData != null)
                {
                    Debug.Log(g.UUID + " is loaded");
                    g.setLoadData(objData);
                }
            }
            Debug.Log("load done");  
   
        } catch(Exception e) {
            Debug.Log("Exception while saving file:" + e);
        } finally {
            saveFile.Dispose(); 
        }
    }

    public void SaveToFile(string fileName)
    {
        if(fileName == "" || fileName == null)
        {
            Debug.Log("No file specified to save");
            return;
        }
        EasyFileSave saveFile = new EasyFileSave(fileName);
        Debug.Log("Filename:" + saveFile.GetFileName());
        try
        {
            object[] obj = Resources.FindObjectsOfTypeAll<SerializedObject>(); //this ensures disabled object are also considered
            //object[] obj = Resources.FindObjectsOfType(typeof (SerializedObject)); //this ignores disabled objects
            foreach (object o in obj)
            {
                SerializedObject g = (SerializedObject) o;
                if(g.UUID == "")
                    continue;
                    
                saveFile.AddBinary(g.UUID, g.getSaveData());
                //Debug.Log(g.UUID + " is saved");
            }
            saveFile.Save();
            PlayerPrefs.SetString("LastCheckPoint",fileName);
            PlayerPrefs.Save();
            Debug.Log("saving done");

        } catch(Exception e) {
            Debug.Log("Exception while saving file:" + e);
        } finally {
            saveFile.Dispose(); 
        }
    }
}
