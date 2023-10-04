using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class TextManager : MonoBehaviour
{
    public TextMeshProUGUI text;
    public TextMeshPro text2;
    [HideInInspector]public float intValue;

    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        text2 = GetComponent<TextMeshPro>();
    }

    public void SetTextInt(float value)
    {
        try
        {
            if (text != null)
            {
                text.text = value.ToString();
            }else
            {
                text2.text = value.ToString();
            }
        }catch
        {

        }
        
        intValue = value;
    }

    public void SetTextString(string value)
    {
        if (text != null)
        {
            text.text = value;
        }else
        {
            text2.text = value;
        }
    }
}
