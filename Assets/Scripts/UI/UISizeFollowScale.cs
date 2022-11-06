using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISizeFollowScale : MonoBehaviour
{
    public Slider scaleSlider;
    public Text scaleTxt;
    [Space(5)]
    public int maxWidth;
    public int maxHeight;

    private void Start()
    {
        ScaleShow(scaleSlider.value);
        scaleSlider.onValueChanged.AddListener((value) =>
        {
            ScaleShow(value);
        }); 
    }

    private void ScaleShow(float scale)
    {
        RectTransform rt=GetComponent<RectTransform>();
        if(rt!=null)
        {
            Vector2 currentSize = rt.sizeDelta;
            if (currentSize.x * scale > maxWidth || currentSize.y * scale > maxHeight)
                return;
        }
        GetComponent<Transform>().localScale = new Vector3(scale, scale, scale);
        double show = Math.Round(scale, 1);
        scaleTxt.text = $"x{show}";
    }
}
