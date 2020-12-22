using UnityEngine;


public class MenuBarUI : MonoBehaviour
{
	[SerializeField] public GameObject buttonPrefab;

	public virtual void AddMenu(MenuHelper helper)
	{
		var obj = Instantiate (buttonPrefab) as GameObject;
		obj.transform.SetParent(transform);

		var wrapper = obj.GetComponent<MenuButtonWrapper> ();
		if (wrapper == null)
			Debug.Log (string.Format("there's no MenuButtonWrapper in {0}.",buttonPrefab.name));
		else
		{
			wrapper.icon.sprite = helper.icon;
			wrapper.image.color = helper.imageColor;
			wrapper.text.text = helper.title;
			wrapper.text.color = helper.labelColor;
			wrapper.window = helper.gameObject;
			wrapper.button.onClick.AddListener(()=>{
				helper.window.SetActive (true);
				Destroy( wrapper.gameObject );
				//Debug.Log("button destroyed on click");
			});
			helper.window.SetActive (false);
		}
	}

	public void RemoveButton(GameObject window)
	{
		foreach(MenuButtonWrapper button in transform.GetComponentsInChildren<MenuButtonWrapper>())
		{
			if(button.window == window)
			{
				//Debug.Log("button destroyed on enable");
				Destroy( button.gameObject );
				return;
			}
		}
		//Debug.Log("NO button destroyed on enable");
	}
}
