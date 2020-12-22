using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class MenuBar : MonoBehaviour
{
    [System.Serializable]
    public class SubMenuBarItemClass
    {
        public string name;
        [HideInInspector] public GameObject element;
        public UnityEvent onClick;
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
    //public string[] menubarItems;
    //List<GameObject> menuBarItems = new List<GameObject>();
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
                else
                {
                    SubMenuItem.element = Instantiate(MenuBarItemButton, MenuItem.subElement.transform) as GameObject;
                    SubMenuItem.element.GetComponent<Button>().onClick.AddListener(() => {Debug.Log("clicked:" + SubMenuItem.name);});
                    SubMenuItem.element.GetComponentInChildren<TextMeshProUGUI>().text = SubMenuItem.name;
                }

            }
        }      
    }
}
