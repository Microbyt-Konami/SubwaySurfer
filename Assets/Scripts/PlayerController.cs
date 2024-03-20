using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Side { Left = -2, Middle = 0, Right = 2 }

public class PlayerController : MonoBehaviour
{
    // Fields
    [Header("Player Controller")]
    [SerializeField] private float fowardSpeed;
    [SerializeField] private float jumpPower;
    [SerializeField] private float dodgeSpeed;

    // Fields - Debug
    [Header("Player States")]
    [SerializeField] private bool noMove;
    [SerializeField] private bool isRolling;
    [SerializeField] private bool isJumping;
    [SerializeField] private bool isGrounded;

    // Components
    [SerializeField] private Side position;
    [SerializeField] private Side oldPosition;
    [SerializeField] private Side savePosition;
    private bool hasChangeSaveXPosition = false;
    private Transform myTransform;
    private Animator myAnimation;
    private CharacterController _myCharacterController;
    private PlayerCollision playerCollision;
    private TunnelController tunnelController;

    // Variables
    private bool swipeLeft, swipeRight, swipeUp, swipeDown;
    [SerializeField] private float newXPosition;
    [SerializeField] private float xPosition;
    [SerializeField] private float saveXPosition;
    private float yPosition;
    private float? newZPosition;
    private float rollTimer;
    private Vector3 motionVector;

    // Ids
    private int idDodgeLeft = Animator.StringToHash("DodgeLeft");
    private int idDodgeRight = Animator.StringToHash("DodgeRight");
    private int idJump = Animator.StringToHash("Jump");
    private int idFall = Animator.StringToHash("Fall");
    private int idLanding = Animator.StringToHash("Landing");
    private int idRoll = Animator.StringToHash("Roll");
    private int idStumbleLow = Animator.StringToHash("StumbleLow");
    private int idStumbleCornerLeft = Animator.StringToHash("StumbleCornerLeft");
    private int idStumbleCornerRight = Animator.StringToHash("StumbleCornerRight");
    private int idStumbleFall = Animator.StringToHash("StumbleFall");
    private int idStumbleOffLeft = Animator.StringToHash("StumbleOffLeft");
    private int idStumbleOffRight = Animator.StringToHash("StumbleOffRight");
    private int idStumbleSideLeft = Animator.StringToHash("StumbleSideLeft");
    private int idStumbleSideRight = Animator.StringToHash("StumbleSideRight");
    private int idDeathBounce = Animator.StringToHash("DeathBounce");
    private int idDeathLower = Animator.StringToHash("DeathLower");
    private int idDeathMovingTrain = Animator.StringToHash("DeathMovingTrain");
    private int idDeathUpper = Animator.StringToHash("DeathUpper");
    private int idStumbleSideEnd = Animator.StringToHash("StumbleSideEnd");

    public CharacterController MyCharacterController { get => _myCharacterController; set => _myCharacterController = value; }
    public int IdStumbleLow { get => idStumbleLow; set => idStumbleLow = value; }
    public int IdDeathLower { get => idDeathLower; set => idDeathLower = value; }
    public int IdDeathMovingTrain { get => idDeathMovingTrain; set => idDeathMovingTrain = value; }
    public int IdDeathBounce { get => idDeathBounce; set => idDeathBounce = value; }
    public int IdDeathUpper { get => idDeathUpper; set => idDeathUpper = value; }
    public bool IsRolling { get => isRolling; set => isRolling = value; }
    public int IdStumbleCornerLeft { get => idStumbleCornerLeft; set => idStumbleCornerLeft = value; }
    public int IdStumbleCornerRight { get => idStumbleCornerRight; set => idStumbleCornerRight = value; }
    public int IdStumbleSideLeft { get => idStumbleSideLeft; set => idStumbleSideLeft = value; }
    public int IdStumbleSideRight { get => idStumbleSideRight; set => idStumbleSideRight = value; }
    public bool NoMove { get => noMove; set => noMove = value; }
    public Vector3 Position => myTransform.position;

    public void SetPlayerAnimator(int id, bool isCrossFade, float fadeTime = 0.1f, bool restoreXPosition = false)
    {
        // 0=> Base Layer
        myAnimation.SetLayerWeight(0, 1);
        if (restoreXPosition)
            StartCoroutine(RestoreXPositionCourotine());
        if (isCrossFade)
            myAnimation.CrossFadeInFixedTime(id, fadeTime);
        else
            myAnimation.Play(id);
        ResetCollision();
    }

    IEnumerator RestoreXPositionCourotine()
    {
        myAnimation.SetBool(idStumbleSideEnd, false);
        do
        {
            yield return null;
        } while (!myAnimation.GetBool(idStumbleSideEnd));
        RestoreXPostion();
    }

    public void SetPlayerAnimatorWithLayer(int id)
    {
        // 1=> Layer StumbleCorner
        myAnimation.SetLayerWeight(1, 1);
        myAnimation.Play(id);
        ResetCollision();
    }

    public void SetPlayerPositionZ(float positionZ)
    {
        newZPosition = positionZ;
    }

    public void SaveXPosition()
    {
        savePosition = oldPosition;
        saveXPosition = (float)oldPosition;
        hasChangeSaveXPosition = false;
    }

    public void RestoreXPostion() => hasChangeSaveXPosition = true; //UpdatePlayerXPosition(oldPosition);

    // Start is called before the first frame update
    void Start()
    {
        myTransform = GetComponent<Transform>();
        myAnimation = GetComponent<Animator>();
        _myCharacterController = GetComponent<CharacterController>();
        playerCollision = GetComponent<PlayerCollision>();

        tunnelController = null;
        newXPosition = xPosition = myTransform.position.x;
        position = (Side)(int)xPosition;
        newZPosition = myTransform.position.z;
        oldPosition = position;
        yPosition = -7;
    }

    // Update is called once per frame
    void Update()
    {
        if (!NoMove)
        {
            GetSwipe();
            SetPlayerPosition();
            MovePlayer();
            Jump();
            Roll();
        }
        isGrounded = _myCharacterController.isGrounded;
    }

    void LateUpdate()
    {
        if (newZPosition.HasValue)
        {
            myTransform.position = new Vector3(myTransform.position.x, myTransform.position.y, newZPosition.Value);
            newZPosition = null;
        }
        if (hasChangeSaveXPosition)
        {
            position = (Side)saveXPosition;
            newXPosition = xPosition = saveXPosition;
            myTransform.position = new Vector3(xPosition, myTransform.position.y, myTransform.position.z);
            hasChangeSaveXPosition = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Tunnel"))
        {
            print($"Enter Tunnel {other.gameObject.name}");
            tunnelController = other.gameObject.GetComponent<TunnelController>();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Tunnel"))
        {
            print($"Exit Tunnel {other.gameObject.name}");
            tunnelController = null;
        }
    }

    private void GetSwipe()
    {
        swipeLeft = Input.GetKeyDown(KeyCode.LeftArrow);
        swipeRight = Input.GetKeyDown(KeyCode.RightArrow);
        swipeUp = Input.GetKeyDown(KeyCode.UpArrow);
        swipeDown = Input.GetKeyDown(KeyCode.DownArrow);
    }

    private void ResetCollision()
    {
        print($"{playerCollision.CollisionX} {playerCollision.CollisionY} {playerCollision.CollisionZ}");
        playerCollision.CollisionX = CollisionX.None;
        playerCollision.CollisionY = CollisionY.None;
        playerCollision.CollisionZ = CollisionZ.None;
    }

    private bool CanMove(Side side, Side sideNew)
    {
        if (tunnelController == null)
            return true;

        return tunnelController.CanMove(side, sideNew);
    }

    private void SetPlayerPosition()
    {
        if (swipeLeft && !isRolling)
        {
            switch (position)
            {
                case Side.Middle:
                    if (CanMove(position, Side.Left))
                    {
                        UpdatePlayerXPosition(Side.Left);
                        SetPlayerAnimator(idDodgeLeft, false);
                    }
                    break;
                case Side.Right:
                    if (CanMove(position, Side.Middle))
                    {
                        UpdatePlayerXPosition(Side.Middle);
                        SetPlayerAnimator(idDodgeLeft, false);
                    }
                    break;
            }
        }
        else if (swipeRight && !isRolling)
        {
            switch (position)
            {
                case Side.Middle:
                    if (CanMove(position, Side.Right))
                    {
                        UpdatePlayerXPosition(Side.Right);
                        SetPlayerAnimator(idDodgeRight, false);
                    }
                    break;
                case Side.Left:
                    if (CanMove(position, Side.Middle))
                    {
                        UpdatePlayerXPosition(Side.Middle);
                        SetPlayerAnimator(idDodgeRight, false);
                    }
                    break;
            }
        }
    }

    private void UpdatePlayerXPosition(Side position)
    {
        newXPosition = (int)position;
        oldPosition = this.position;
        this.position = position;
    }

    private void MovePlayer()
    {
        motionVector = new Vector3(xPosition - myTransform.position.x, yPosition * Time.deltaTime, fowardSpeed * Time.deltaTime);
        xPosition = Mathf.Lerp(xPosition, newXPosition, Time.deltaTime * dodgeSpeed);
        //playerTransform.position = new Vector3(xPosition, 0, 0);
        // cuidado es relativo no absoluto
        _myCharacterController.Move(motionVector);
        if (newZPosition.HasValue)
            newZPosition += motionVector.z;
    }

    private void Jump()
    {
        if (_myCharacterController.isGrounded)
        {
            isJumping = false;
            // 0 => corresponde al indice del layer por defecto es el 0
            if (myAnimation.GetCurrentAnimatorStateInfo(0).IsName("Fall"))
                SetPlayerAnimator(idLanding, false);
            if (swipeUp && !isRolling)
            {
                isJumping = true;
                yPosition = jumpPower;
                // Mezcla la animaci�n actual con la de jump
                //playerAnimation.CrossFadeInFixedTime(idJump, 0.1f);
                SetPlayerAnimator(idJump, true, 1f);
            }
        }
        else
        {
            // Se baja mas r�pido
            yPosition -= jumpPower * 2 * Time.deltaTime;
            // Si la velocidad en y <=0 es que esta cayendo
            if (_myCharacterController.velocity.y <= 0)
                SetPlayerAnimator(idFall, false);
        }
    }

    private void Roll()
    {
        rollTimer -= Time.deltaTime;
        if (rollTimer <= 0)
        {
            isRolling = false;
            rollTimer = 0;
            // Poner ch a tama�o normal
            _myCharacterController.center = new Vector3(0, .45f, 0);
            _myCharacterController.height = .9f;
        }
        if (swipeDown && !isJumping)
        {
            isRolling = true;
            rollTimer = .5f;
            SetPlayerAnimator(idRoll, true);
            // Poner ch a tama�o chico
            _myCharacterController.center = new Vector3(0, .2f, 0);
            _myCharacterController.height = .4f;
        }
    }
}
