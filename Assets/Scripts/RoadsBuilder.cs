using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR

[CustomEditor(typeof(RoadsBuilder))]
public class RoadsBuilderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (target is RoadsBuilder roadsBuilder)
        {
            if (GUILayout.Button("Build"))
                roadsBuilder.Build();
        }
    }
}

#endif

[ExecuteInEditMode]
public class RoadsBuilder : MonoBehaviour
{
    [SerializeField] private GameObject target;
    // Start is called before the first frame update
    //void Start()
    //{

    //}

    // Update is called once per frame
    //void Update()
    //{

    //}

#if UNITY_EDITOR

    public void Build()
    {
        Vector3 position = new Vector3(0, 0, -18);
        int roadNumber = 0;

        do
        {
            var road = Instantiate(target, position, Quaternion.identity, transform);

            road.name = roadNumber == 0 ? "road" : $"road ({roadNumber})";
            roadNumber++;
            position.z += 18;
        } while (position.z <= 2100);
    }
#endif
}