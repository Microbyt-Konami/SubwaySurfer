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
    private int idJump = Animator.StringToHash("Jump");
    private int idFall = Animator.StringToHash("Fall");
    private int idLanding = Animator.StringToHash("Landing");

    // Start is called before the first frame update
    void Start()
    {
        playerPosition = PlayerPosition.Middle;
        playerTransform = GetComponent<Transform>();
        playerAnimation = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        yPosition = -7;
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
                    SetPlayerAnimator(idDodgeLeft, false);
                    break;
                case PlayerPosition.Right:
                    UpdatePlayerXPosition(PlayerPosition.Middle);
                    SetPlayerAnimator(idDodgeLeft, false);
                    break;
            }
        }
        else if (swipeRight)
        {
            switch (playerPosition)
            {
                case PlayerPosition.Middle:
                    UpdatePlayerXPosition(PlayerPosition.Right);
                    SetPlayerAnimator(idDodgeRight, false);
                    break;
                case PlayerPosition.Left:
                    UpdatePlayerXPosition(PlayerPosition.Middle);
                    SetPlayerAnimator(idDodgeRight, false);
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

    private void SetPlayerAnimator(int id, bool isCrossFade, float fadeTime = 0.1f)
    {
        if (isCrossFade)
            playerAnimation.CrossFadeInFixedTime(id, fadeTime);
        else
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
            // 0 => corresponde al indice del layer por defecto es el 0
            if (playerAnimation.GetCurrentAnimatorStateInfo(0).IsName("Fall"))
                SetPlayerAnimator(idLanding, false);
            if (swipeUp)
            {
                yPosition = jumpPower;
                // Mezcla la animación actual con la de jump
                //playerAnimation.CrossFadeInFixedTime(idJump, 0.1f);
                SetPlayerAnimator(idJump, true, 1f);
            }
        }
        else
        {
            // Se baja mas rápido
            yPosition -= jumpPower * 2 * Time.deltaTime;
            SetPlayerAnimator(idFall, false);
        }
    }
}
