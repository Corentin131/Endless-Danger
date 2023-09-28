using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class TextManager : MonoBehaviour
{
    public TextMeshProUGUI text;
    [HideInInspector]public float intValue;

    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    public void SetTextInt(float value)
    {
        try
        {
            text.text = value.ToString();
        }catch
        {

        }
        
        intValue = value;
    }

    public void SetTextString(string value)
    {
        text.text = value;
    }
}
