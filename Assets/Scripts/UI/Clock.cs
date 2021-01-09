using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Clock : MonoBehaviour
{
    public GameObject HelpDialogPrefab;
    GameObject HelpInstance;
    public void NewHelpDialog()
    {
        if(HelpInstance != null)
        {
            Destroy(HelpInstance);
            HelpInstance = null;
        }  

        HelpInstance = Instantiate(HelpDialogPrefab,transform.parent);
        HelpInstance.GetComponent<RectTransform>().SetAsLastSibling();
        HelpInstance.GetComponent<HMIHelpDialog>().textField.text = 
            "The Clock is only to display the current time. Currently it servers no other purpose.";
    }
    public Text TimeObj;
    public Text DateObj;

    // Update is called once per frame
    void Update()
    {
        TimeObj.text = System.DateTime.Now.ToString("HH:mm:ss");
        DateObj.text = System.DateTime.Now.ToString("ddd. MMM dd, yyyy");
    }
}
