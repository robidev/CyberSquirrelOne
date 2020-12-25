using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageWindow : MonoBehaviour
{
    // Start is called before the first frame update
    public Image image;

    public void DisplayImage(Image _image)
    {
        image = _image;
        gameObject.SetActive(true);
    }

    public void DisplaySprite(Sprite _sprite)
    {
        image.sprite = _sprite;
        gameObject.SetActive(true);
    }
}
