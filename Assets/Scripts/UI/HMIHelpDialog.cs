using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HMIHelpDialog : MonoBehaviour
{
    public TextMeshProUGUI textField;

    public void Close()
    {
        Destroy(gameObject);
    }
}
