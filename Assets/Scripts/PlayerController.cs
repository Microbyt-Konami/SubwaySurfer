using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerPosition { Left = -2, Middle = 0, Right = 2 }

public class PlayerController : MonoBehaviour
{
    private PlayerPosition playerPosition;
    private Transform playerTransform;
    private bool swipeLeft, swipeRight;
    private float newXPosition;

    // Start is called before the first frame update
    void Start()
    {
        playerPosition = PlayerPosition.Middle;
        playerTransform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        GetSwipe();
        SetPlayerPosition();

    }
    private void GetSwipe()
    {
        swipeLeft = Input.GetKeyDown(KeyCode.LeftArrow);
        swipeRight = Input.GetKeyDown(KeyCode.RightArrow);
    }

    private void SetPlayerPosition()
    {
        if (swipeLeft)
        {
            switch (playerPosition)
            {
                case PlayerPosition.Middle:
                    UpdatePlayerXPosition(PlayerPosition.Left);
                    break;
                case PlayerPosition.Right:
                    UpdatePlayerXPosition(PlayerPosition.Middle);
                    break;
            }
        }
        else if (swipeRight)
        {
            switch (playerPosition)
            {
                case PlayerPosition.Middle:
                    UpdatePlayerXPosition(PlayerPosition.Right);
                    break;
                case PlayerPosition.Left:
                    UpdatePlayerXPosition(PlayerPosition.Middle);
                    break;
            }
        }
        MovePlayer();
    }

    private void UpdatePlayerXPosition(PlayerPosition position)
    {
        newXPosition = (int)position;
        playerPosition = position;
    }

    private void MovePlayer()
    {
        playerTransform.position = new Vector3(newXPosition, 0, 0);
    }
}
