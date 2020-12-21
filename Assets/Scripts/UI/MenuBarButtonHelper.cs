using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;// Required when using Event data.

public class MenuBarButtonHelper : MonoBehaviour, ISelectHandler, IDeselectHandler// required interface when using the OnSelect method.
{
    public GameObject SubMenu;
	//Do this when the selectable UI object is selected.
	public void OnSelect (BaseEventData eventData) 
	{
		//Debug.Log (this.gameObject.name + " was selected");
        SubMenu.SetActive(true);
	}

	public void OnDeselect (BaseEventData eventData) 
	{
		//Debug.Log (this.gameObject.name + " was deselected");
		StartCoroutine(DelayDisable());
	}    

	IEnumerator DelayDisable()
	{
		yield return new WaitForSeconds(0.1f);
		SubMenu.SetActive(false);
	}
}