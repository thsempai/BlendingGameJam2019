using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gauge : MonoBehaviour
{
    public RectTransform gauge;
    public int value;
    private float initialScale;
    public int maxValue=20;

    void Start()
    {
        initialScale = gauge.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        float scale = initialScale * Mathf.Max((float)value,Mathf.Epsilon) / (float)maxValue;
        Vector3 gaugeScale = gauge.localScale;
        gaugeScale.x = scale;
        gauge.localScale = gaugeScale;
    }
}
