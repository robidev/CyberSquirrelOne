using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EasyFileSaveExtension
{

    // ----------------------------------------
    // Add new extension inside this function.
    // ----------------------------------------

    public void Start()
    {

        // AddExtension method required parameters:
        // - Name of this extension
        // - CallBack function to execute when this extension save data
        // - An array of unique keys, where each key corresponds to each saved value
        AddExtension(
            "BoxCollider", 
            BoxColliderExtension, 
            new string[] { "centerX", "centerY", "centerZ", "sizeX", "sizeY", "sizeZ", "enabled", "isTrigger", "contactOffset" }
            );

    }

    // ----------------------------------------
    // Add callBack functions here.
    // ----------------------------------------

    // This extension allow Easy File Save to easily save BoxCollider data.
    void BoxColliderExtension()
    {
        // The boxCollider object data received by the AddCustom() method.
        var data = GetData("BoxCollider");

        // Casting of the object data to BoxCollider.
        BoxCollider bc = (BoxCollider)data;

        SetParameters(
            "BoxCollider",
            bc.center.x,
            bc.center.y,
            bc.center.z,
            bc.size.x,
            bc.size.y,
            bc.size.z,
            bc.enabled,
            bc.isTrigger,
            bc.contactOffset
            );

    }














    // ******************************************************
    //       DON'T MODIFY ANYTHING UNDER THIS COMMENT:
    // ******************************************************

    #region " EXTENSIONS ENGINE "

    public Dictionary<string, UnityAction> extensions = new Dictionary<string, UnityAction>();
    public Dictionary<string, object> data = new Dictionary<string, object>();
    public Dictionary<string, List<object>> pars = new Dictionary<string, List<object>>();
    public Dictionary<string, List<string>> mapping = new Dictionary<string, List<string>>();

    /// <summary>
    /// Add a new extension to the Easy File Save system
    /// </summary>
    private void AddExtension(string name, UnityAction callBack, string[] map)
    {
        if (!extensions.ContainsKey(name))
        {
            extensions.Add(name, callBack);
            data.Add(name, null);
            pars.Add(name, null);
            mapping.Add(name, new List<string>(map));
        }
        else
        {
            Debug.LogWarning("An extension with name '" + name + "' already exists.");
        }
    }

    /// <summary>
    /// Get the object data sent to this callback.
    /// </summary>
    private object GetData(string extensionName)
    {
        if (data.ContainsKey(extensionName)) return data[extensionName]; else return null;
    }

    /// <summary>
    /// Collect the object data to save.
    /// </summary>
    private void SetParameters(string extensionName, params object[] parameters)
    {
        pars[extensionName] = new List<object>(parameters);
    }

    #endregion

}
