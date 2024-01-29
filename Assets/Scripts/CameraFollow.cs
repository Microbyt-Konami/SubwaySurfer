using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // fields
    [SerializeField] private float rayDistance;
    [SerializeField] private float speedOffset;

    // Components
    private Transform target;
    private Transform myTransform;
    private Vector3 cameraOffset;
    private Vector3 followPosition;

    //Variables
    private float y;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        myTransform = GetComponent<Transform>();
        cameraOffset = myTransform.position;
    }

    // Update is called once per frame
    //void Update()
    //{

    //}

    private void LateUpdate()
    {
        followPosition = target.position + cameraOffset;
        myTransform.position = followPosition;
        UpdateCameraOffset();
    }

    private void UpdateCameraOffset()
    {
        RaycastHit hitInfo;

        if (Physics.Raycast(target.position, Vector3.down, out hitInfo, rayDistance))
            y = Mathf.Lerp(y, hitInfo.point.y, Time.deltaTime * speedOffset);
        // Comentamos para que solo se user el lerp si la camara está a la distancia (rayDistance) del suelo o vagon
        //else
        //    y = Mathf.Lerp(y, target.position.y, Time.deltaTime * speedOffset);
        followPosition.y = cameraOffset.y + y;
        myTransform.position = followPosition;
    }
}
