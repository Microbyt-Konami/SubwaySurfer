using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerPosition { Left = -2, Middle = 0, Right = 2 }

public class PlayerController : MonoBehaviour
{
    // Fields
    [SerializeField] private float dodgeSpeed;
    [SerializeField] private float jumpPower = 7;

    // Components
    private PlayerPosition playerPosition;
    private Transform playerTransform;
    private Animator playerAnimation;
    private CharacterController characterController;

    // Variables
    private bool swipeLeft, swipeRight, swipeUp, swipeDown;
    private float newXPosition, xPosition, yPosition;
    private Vector3 motionVector;

    // Ids
    private int idDodgeLeft = Animator.StringToHash("DodgeLeft");
    private int idDodgeRight = Animator.StringToHash("DodgeRight");

    // Start is called before the first frame update
    void Start()
    {
        playerPosition = PlayerPosition.Middle;
        playerTransform = GetComponent<Transform>();
        playerAnimation = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
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
        swipeUp = Input.GetKeyDown(KeyCode.UpArrow);
    }

    private void SetPlayerPosition()
    {
        if (swipeLeft)
        {
            switch (playerPosition)
            {
                case PlayerPosition.Middle:
                    UpdatePlayerXPosition(PlayerPosition.Left);
                    SetPlayerAnimator(idDodgeLeft);
                    break;
                case PlayerPosition.Right:
                    UpdatePlayerXPosition(PlayerPosition.Middle);
                    SetPlayerAnimator(idDodgeLeft);
                    break;
            }
        }
        else if (swipeRight)
        {
            switch (playerPosition)
            {
                case PlayerPosition.Middle:
                    UpdatePlayerXPosition(PlayerPosition.Right);
                    SetPlayerAnimator(idDodgeRight);
                    break;
                case PlayerPosition.Left:
                    UpdatePlayerXPosition(PlayerPosition.Middle);
                    SetPlayerAnimator(idDodgeRight);
                    break;
            }
        }
        MovePlayer();
        Jump();
    }

    private void UpdatePlayerXPosition(PlayerPosition position)
    {
        newXPosition = (int)position;
        playerPosition = position;
    }

    private void SetPlayerAnimator(int id)
    {
        playerAnimation.Play(id);
    }

    private void MovePlayer()
    {
        motionVector = new Vector3(xPosition - playerTransform.position.x, yPosition * Time.deltaTime, 0);
        xPosition = Mathf.Lerp(xPosition, newXPosition, Time.deltaTime * dodgeSpeed);
        //playerTransform.position = new Vector3(xPosition, 0, 0);
        // cuidado es relativo no absoluto
        characterController.Move(motionVector);
    }

    private void Jump()
    {
        if (characterController.isGrounded)
        {
            if (swipeUp)
                yPosition = jumpPower;
        }
        else
        {
            // Se baja mas r�pido
            yPosition -= jumpPower * 2 * Time.deltaTime;
        }
    }
}
