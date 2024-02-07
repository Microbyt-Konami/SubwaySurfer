using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderController : MonoBehaviour
{
    [SerializeField, Range(-1, 1)] private float curveX;
    [SerializeField, Range(-1, 1)] private float curveY;
    [SerializeField] private Material[] materials;

    // Update is called once per frame
    void Update()
    {
        foreach (var m in materials)
        {
            m.SetFloat(Shader.PropertyToID("_Curve_X"), curveX);
            m.SetFloat(Shader.PropertyToID("_Curve_Y"), curveY);
        }
    }
}
