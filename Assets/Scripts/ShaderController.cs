using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderController : MonoBehaviour
{
    [SerializeField, Range(-1, 1)] private float curveX;
    [SerializeField, Range(-1, 1)] private float curveY;
    [SerializeField] private Material[] materials;
    [SerializeField] private float firstValue;
    [SerializeField] private float waitForChangeValue;
    [SerializeField] private float waitForTransition;

    private Coroutine changeValuesCoroutine;
    private float transitionTime;
    private float curveXInitial, curveYInitial, curveXEnd, curveYEnd;

    public void ActivateChangeValues() => changeValuesCoroutine = StartCoroutine(ChangeValuesCourotine());

    public void DesActivateChangeValues() => StopCoroutine(changeValuesCoroutine);

    void Start()
    {
        curveX = curveY = curveXInitial = curveYInitial = 0;
        transitionTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var m in materials)
        {
            m.SetFloat(Shader.PropertyToID("_Curve_X"), curveX);
            m.SetFloat(Shader.PropertyToID("_Curve_Y"), curveY);
        }
    }

    void OnApplicationQuit()
    {
        foreach (var m in materials)
        {
            m.SetFloat(Shader.PropertyToID("_Curve_X"), 0);
            m.SetFloat(Shader.PropertyToID("_Curve_Y"), 0);
        }
    }

    IEnumerator ChangeValuesCourotine()
    {
        curveX = curveXInitial;
        curveY = curveYInitial;
        while (true)
        {
            // Los valores deben permanecer fijos durante 3 segundos y luego iniciar la transición hacia el valor opuesto
            // o puede mantenerse fijo en el valor anterior para que no sea tan repetitivo.
            yield return new WaitForSeconds(waitForChangeValue);

            transitionTime = 0f;
            // Al inicio la curvatura deberá empezar en 0 de X y 0 de Y, pudiendo ser el primer valor 1 o -1. 
            // Los valores deben permanecer fijos durante 3 segundos y luego iniciar la transición hacia el valor opuesto 
            // o puede mantenerse fijo en el valor anterior para que no sea tan repetitivo
            curveXEnd =
                 (curveXInitial == 0f)
                    ? Random.value > .5f ? firstValue : -firstValue
                    : (Random.value < .5f) ? curveXInitial : -curveXInitial;
            curveYEnd =
                (curveYInitial == 0f)
                   ? Random.value > .5f ? firstValue : -firstValue
                   : (Random.value < .5f) ? curveYInitial : -curveYInitial;
            transitionTime = 0f;

            yield return null;

            // La transición de un valor a otro debe durar 2 segundos
            do
            {
                transitionTime += Time.deltaTime;
                curveX = Mathf.Lerp(curveXInitial, curveXEnd, transitionTime / waitForTransition);
                curveY = Mathf.Lerp(curveYInitial, curveYEnd, transitionTime / waitForTransition);

                yield return null;
            } while (transitionTime < waitForTransition);
        }
    }
}