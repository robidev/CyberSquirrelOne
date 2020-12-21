using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class MenuBar : MonoBehaviour
{
    public Transform MenuBarPanel;
    public GameObject MenuBarItem;
    public GameObject MenuBarItemMenu;
    public GameObject MenuBarItemButton;

    public string[] menubarItems;
    List<GameObject> menuBarItems = new List<GameObject>();
    void Start()
    {
        foreach(string item in menubarItems)
        {
            if(item[0] == '+')
            {
                GameObject menuBarItem = Instantiate(MenuBarItem, MenuBarPanel) as GameObject;
                Transform name = menuBarItem.transform.Find("Name");
                if(name != null)
                {
                    name.GetComponent<TextMeshProUGUI>().text = item.Substring(1);
                }
                menuBarItems.Add(menuBarItem);
                LayoutRebuilder.ForceRebuildLayoutImmediate(MenuBarPanel.GetComponent<RectTransform>());
            }
        }
        //this is needed for the reposition of the buttons
        LayoutRebuilder.ForceRebuildLayoutImmediate(MenuBarPanel.GetComponent<RectTransform>());
    
        GameObject menuBarItemMenu = null;
        int i = 0;
        foreach(string item in menubarItems)
        {
            if(item[0] == '+')
            {
                menuBarItemMenu = Instantiate(MenuBarItemMenu, menuBarItems[i].GetComponent<RectTransform>()) as GameObject;//get location from button
                menuBarItems[i].GetComponent<MenuBarButtonHelper>().SubMenu = menuBarItemMenu;
                menuBarItemMenu.transform.SetParent(MenuBarPanel.parent.GetComponent<RectTransform>(),true);//change parent to prevent layout rebuild
                menuBarItemMenu.SetActive(false);
                i++;
            }
            else if(menuBarItemMenu != null)
            {
                GameObject menuBarItemButton = Instantiate(MenuBarItemButton, menuBarItemMenu.transform) as GameObject;
                menuBarItemButton.GetComponent<Button>().onClick.AddListener(() => {Debug.Log("clicked:" + item);});
                menuBarItemButton.GetComponentInChildren<TextMeshProUGUI>().text = item;
            }
        }      
    }
}
