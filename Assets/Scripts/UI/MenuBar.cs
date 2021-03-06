﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
//using UnityEngine.EventSystems;

public class MenuBar : MonoBehaviour
{
    [System.Serializable]
    public class SubMenuBarItemClass
    {
        public string name;
        [HideInInspector] public GameObject element;
        public Button.ButtonClickedEvent onClick;
    }

    [System.Serializable]
    public class MenuBarItemClass
    {
        public string name;
        [HideInInspector] public GameObject element;
        [HideInInspector] public GameObject subElement;
        public List<SubMenuBarItemClass> subMenuItems;
    }
    public Transform MenuBarPanel;
    public GameObject MenuBarItem;
    public GameObject MenuBarItemMenu;
    public GameObject MenuBarItemButton;
    public GameObject MenuBarItemDivider;
    public List<MenuBarItemClass> menuBarItems;

    void Start()
    {
        foreach(MenuBarItemClass MenuItem in menuBarItems)
        {
            MenuItem.element = Instantiate(MenuBarItem, MenuBarPanel) as GameObject;
            Transform name = MenuItem.element.transform.Find("Name");
            if(name != null)
            {
                name.GetComponent<TextMeshProUGUI>().text = MenuItem.name;
            }
            LayoutRebuilder.ForceRebuildLayoutImmediate(MenuBarPanel.GetComponent<RectTransform>());
        }
        //this is needed for the reposition of the buttons
        LayoutRebuilder.ForceRebuildLayoutImmediate(MenuBarPanel.GetComponent<RectTransform>());
    
        foreach(MenuBarItemClass MenuItem in menuBarItems)
        {
            //create a submenu panel under the button and hide it
            MenuItem.subElement = Instantiate(MenuBarItemMenu, MenuItem.element.GetComponent<RectTransform>()) as GameObject;//get location from button
            MenuItem.element.GetComponent<MenuBarButtonHelper>().SubMenu = MenuItem.subElement;
            MenuItem.subElement.transform.SetParent(MenuBarPanel.parent.GetComponent<RectTransform>(),true);//change parent to prevent layout rebuild
            MenuItem.subElement.SetActive(false);

            //create the submenu buttons
            foreach(SubMenuBarItemClass SubMenuItem in MenuItem.subMenuItems)
            {
                if(SubMenuItem.name == "---")
                {
                    SubMenuItem.element = Instantiate(MenuBarItemDivider, MenuItem.subElement.transform) as GameObject;

                }
                else if(SubMenuItem.name == "Minimise")
                {
                    SubMenuItem.element = Instantiate(MenuBarItemButton, MenuItem.subElement.transform) as GameObject;
                    SubMenuItem.element.GetComponent<Button>().onClick.AddListener(()=>{transform.parent.parent.GetComponent<MenuHelper>().ToMenu();});
                    SubMenuItem.element.GetComponentInChildren<TextMeshProUGUI>().text = SubMenuItem.name;
                }
                else if(SubMenuItem.name == "Copy")
                {
                    string[] editmenu = {
                        "Cut     <align=\"right\">Ctrl+X</align>",
                        "Copy    <align=\"right\">Ctrl+C</align>",
                        "Paste   <align=\"right\">Ctrl+V</align>"};
                    foreach(string item in editmenu)
                    {
                        SubMenuItem.element = Instantiate(MenuBarItemButton, MenuItem.subElement.transform) as GameObject;
                        SubMenuItem.element.GetComponentInChildren<TextMeshProUGUI>().text = item;
                        SubMenuItem.element.GetComponentInChildren<TextMeshProUGUI>().color = new Color(0.7f,0.7f,0.7f);
                        SubMenuItem.element.GetComponent<Button>().interactable = false;
                        
                        SpriteState st = SubMenuItem.element.GetComponent<Button>().spriteState;
                        st.disabledSprite = st.selectedSprite;
                        SubMenuItem.element.GetComponent<Button>().spriteState = st;
                    }
                }
                else if(SubMenuItem.name == "Exit" || SubMenuItem.name == "Close")
                {
                    SubMenuItem.element = Instantiate(MenuBarItemButton, MenuItem.subElement.transform) as GameObject;
                    SubMenuItem.element.GetComponentInChildren<TextMeshProUGUI>().text = "<u>C</u>lose   <align=\"right\">Alt+F4</align>";
                    SubMenuItem.element.GetComponent<Button>().onClick.AddListener(()=>{transform.parent.parent.gameObject.SetActive(false);});
                }
                else
                {
                    SubMenuItem.element = Instantiate(MenuBarItemButton, MenuItem.subElement.transform) as GameObject;
                    SubMenuItem.element.GetComponent<Button>().onClick = SubMenuItem.onClick;
                    //SubMenuItem.element.GetComponent<Button>().onClick.AddListener();
                    SubMenuItem.element.GetComponentInChildren<TextMeshProUGUI>().text = SubMenuItem.name;
                }

            }
        }      
    }

    void OnEnable()
    {
        foreach(MenuBarItemClass MenuItem in menuBarItems)
        {
            if(MenuItem.subElement)
                MenuItem.subElement.SetActive(false);
        }
    }
}
