using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LabelWrapper : MonoBehaviour
{
    public TMPro.TMP_Text label;

    public void UpdateLabel(string newText)
    {
        label.text = newText;
    }
}
