using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModifyEnable : MonoBehaviour
{
    public Button button;
    public void ModifyToggle()
    {
        gameObject.SetActive(!gameObject.activeSelf);

        ColorBlock colors = button.colors;
        if(gameObject.activeSelf == true)
        {
            colors.normalColor = Color.grey;
            colors.highlightedColor= Color.grey;
            colors.pressedColor = Color.grey;
            colors.selectedColor = Color.grey;
        }
        else
        {
            colors.normalColor = Color.black;
            colors.highlightedColor= Color.black;
            colors.pressedColor = Color.black;
            colors.selectedColor = Color.black;
        }
        button.colors = colors;

        if(gameObject.activeSelf == true)//if we just enabled the dialog, disable siblings
        {
            foreach( ModifyEnable modifyEnable in transform.parent.GetComponentsInChildren<ModifyEnable>())
            {
                if(modifyEnable.gameObject != gameObject)
                {
                    ColorBlock _colors = modifyEnable.button.colors;
                    _colors.normalColor = Color.black;
                    _colors.highlightedColor= Color.black;
                    _colors.pressedColor = Color.black;
                    _colors.selectedColor = Color.black;
                    modifyEnable.button.colors = _colors;

                    modifyEnable.gameObject.SetActive(false);
                }
            }
        }
    }
}
