using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageWindow : MonoBehaviour
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
            "The Image window is to display images.";
    }
    public Image image;

    public void DisplayImage(Image _image)
    {
        image = _image;
        gameObject.SetActive(true);
    }

    public void DisplaySprite(Sprite _sprite)
    {
        image.color = Color.white;
        image.sprite = _sprite;
        image.type = Image.Type.Simple;
        image.preserveAspect = true;
        gameObject.SetActive(true);
    }

    public void Close()
    {
        image.color = Color.black;
        image.sprite = null;
        gameObject.SetActive(false);
    }
}
