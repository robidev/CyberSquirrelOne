using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EditorWindow : MonoBehaviour
{
    public TMP_InputField TextField;

    public void DisplayText(string text)
    {
        TextField.text = text;
        gameObject.SetActive(true);
    }

    public void DisplayTextResourceByName(string resourceName)
    {
        TextAsset text = Resources.Load(resourceName) as TextAsset;
        TextField.text = text.text;
        gameObject.SetActive(true);
    }

        public void DisplayTextResourceObject(TextAsset TextResource)
    {
        TextField.text = TextResource.text;
        //TextField.verticalScrollbar.Rebuild()
        gameObject.SetActive(true);
    }
}
