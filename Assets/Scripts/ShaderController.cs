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

    public void ActivateChangeValues()
    {
        curveXInitial = curveX = Random.value > .5f ? firstValue : -firstValue;
        curveYInitial = curveY = Random.value > .5f ? firstValue : -firstValue;
        curveXEnd = -curveX;
        curveYEnd = -curveY;
        transitionTime = 0f;
        changeValuesCoroutine = StartCoroutine(ChangeValuesCourotine());
    }

    public void DesActivateChangeValues() => StopCoroutine(changeValuesCoroutine);

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
        while (true)
        {
            if (transitionTime > waitForTransition)
            {
                // Los valores deben permanecer fijos durante 3 segundos y luego iniciar la transición hacia el valor opuesto o puede mantenerse fijo en el valor anterior para que no sea tan repetitivo.
                yield return new WaitForSeconds(waitForChangeValue);

                curveX = curveXInitial = curveXEnd;
                curveY = curveYInitial = curveYEnd;
                if (Random.value < .5f)
                    curveX *= -1;
                if (Random.value < .5f)
                    curveY *= -1;
                curveXEnd = -curveX;
                curveYEnd = -curveY;
                transitionTime = 0f;

                yield return null;

                continue;
            }
            // La transición de un valor a otro debe durar 2 segundos
            transitionTime += Time.deltaTime;
            curveX = Mathf.Lerp(curveXInitial, curveXEnd, transitionTime / waitForTransition);
            curveY = Mathf.Lerp(curveYInitial, curveYEnd, transitionTime / waitForTransition);

            yield return null;
        }
    }
}