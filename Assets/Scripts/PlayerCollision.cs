using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CollisionX { None, Left, Middle, Right }
public enum CollisionY { None, Up, Middle, Down, LowDown }
public enum CollisionZ { None, Forward, Middle, Backward }

public class PlayerCollision : MonoBehaviour
{
    // Fields
    private CollisionX _collisionX;
    private CollisionY _collisionY;
    private CollisionZ _collisionZ;

    // Componentes
    private PlayerController playerController;

    public CollisionX CollisionX { get => _collisionX; set => _collisionX = value; }
    public CollisionY CollisionY { get => _collisionY; set => _collisionY = value; }
    public CollisionZ CollisionZ { get => _collisionZ; set => _collisionZ = value; }

    public void OnCharacterColission(Collider collider)
    {
        _collisionX = CalcCollisionX(collider);
        _collisionY = CalcCollisionY(collider);
        _collisionZ = CalcCollisionZ(collider);
        SetAnimatorByColission(collider);
    }

    private void SetAnimatorByColission(Collider collider)
    {
        if (_collisionZ == CollisionZ.Backward && _collisionX == CollisionX.Middle)
        {
            if (_collisionY == CollisionY.LowDown)
            {
                collider.enabled = false;
                playerController.SetPlayerAnimator(playerController.IdStumbleLow, false);
            }
            else if (_collisionY == CollisionY.Down)
            {
                playerController.SetPlayerAnimator(playerController.IdDeathLower, false);
                print("Dead Lower");
                GameManager.Instance.GameOver();
            }
            else if (_collisionY == CollisionY.Middle)
            {
                if (collider.CompareTag("TrainOn"))
                {
                    playerController.SetPlayerAnimator(playerController.IdDeathMovingTrain, false);
                    print("Dead MovingTrain");
                    GameManager.Instance.GameOver();
                }
                else if (!collider.CompareTag("Ramp"))
                {
                    playerController.SetPlayerAnimator(playerController.IdDeathBounce, false);
                    print("Dead Bounce");
                    GameManager.Instance.GameOver();
                }
            }
            else if (_collisionY == CollisionY.Up && !playerController.IsRolling)
            {
                playerController.SetPlayerAnimator(playerController.IdDeathUpper, false);
                print("Dead Upper");
                GameManager.Instance.GameOver();
            }
        }
        else if (_collisionZ == CollisionZ.Middle)
        {
            if (_collisionX == CollisionX.Right)
            {
                playerController.SetPlayerAnimator(playerController.IdStumbleSideRight, false);
            }
            else if (_collisionX == CollisionX.Left)
            {
                playerController.SetPlayerAnimator(playerController.IdStumbleSideLeft, false);
            }
        }
        else
        {
            if (_collisionX == CollisionX.Right)
            {
                playerController.SetPlayerAnimatorWithLayer(playerController.IdStumbleCornerRight);
            }
            else if (_collisionX == CollisionX.Left)
            {
                playerController.SetPlayerAnimatorWithLayer(playerController.IdStumbleCornerLeft);
            }
        }
    }

    private void Awake()
    {
        playerController = gameObject.GetComponent<PlayerController>();
    }

    private CollisionX CalcCollisionX(Collider collider)
    {
        // Collider Bounds obtiene los valores del tamaño de collider BOUNDS == LIMITE
        Bounds characterControllerBounds = playerController.MyCharacterController.bounds;
        Bounds colliderBounds = collider.bounds;
        // obtener el punto máximo de los mínimos de X
        float minX = Mathf.Max(colliderBounds.min.x, characterControllerBounds.min.x);
        // obtener el punto máximo de los mínimos de X
        float maxX = Mathf.Min(colliderBounds.max.x, characterControllerBounds.max.x);
        // SABER EL PUNTO EXACTO DONDE COLISIONA UN OBJETO

        // Al mínimo sumarle el máximo y obtener el total
        // Lo divido por 2 así estaré en la mitad de la colisión o pivote
        // Se lo resto al total de collider del que necesite obtener la posició exacta

        float average = (minX + maxX) / 2 - colliderBounds.min.x;

        // Como lo divimos en tres partes 1/3=0.33
        const float fraccion = 1.0f / 3.0f;

        CollisionX colX =
            (average > colliderBounds.size.x - fraccion)
                ? CollisionX.Right
                : (average < fraccion)
                    ? CollisionX.Left
                    : CollisionX.Middle;

        return colX;
    }

    private CollisionY CalcCollisionY(Collider collider)
    {
        // Collider Bounds obtiene los valores del tamaño de collider BOUNDS == LIMITE
        Bounds characterControllerBounds = playerController.MyCharacterController.bounds;
        Bounds colliderBounds = collider.bounds;
        // obtener el punto máximo de los mínimos de Y
        float minY = Mathf.Max(colliderBounds.min.y, characterControllerBounds.min.y);
        // obtener el punto máximo de los mínimos de Y
        float maxY = Mathf.Min(colliderBounds.max.y, characterControllerBounds.max.y);
        // SABER EL PUNTO EXACTO DONDE COLISIONA UN OBJETO

        // Al mínimo sumarle el máximo y obtener el total
        // Lo divido por 2 así estaré en la mitad de la colisión o pivote
        // Se lo resto al total de collider del que necesite obtener la posició exacta

        float average = (minY + maxY) / 2 - colliderBounds.min.y;

        // Como lo divimos en tres partes 1/3=0.33
        const float fraccion = 1.0f / 3.0f;
        const float fraccionMidle = fraccion / 2.0f;

        CollisionY colY =
            (average > colliderBounds.size.y - fraccion)
                ? CollisionY.Up
                : (average < fraccionMidle)
                    ? CollisionY.LowDown
                    : (average < fraccion)
                    ? CollisionY.Down
                    : CollisionY.Middle;

        return colY;
    }

    private CollisionZ CalcCollisionZ(Collider collider)
    {
        // Collider Bounds obtiene los valores del tamaño de collider BOUNDS == LIMITE
        Bounds characterControllerBounds = playerController.MyCharacterController.bounds;
        Bounds colliderBounds = collider.bounds;
        // obtener el punto máximo de los mínimos de Z
        float minZ = Mathf.Max(colliderBounds.min.z, characterControllerBounds.min.z);
        // obtener el punto máximo de los mínimos de Z
        float maxZ = Mathf.Min(colliderBounds.max.z, characterControllerBounds.max.z);
        // SABER EL PUNTO EXACTO DONDE COLISIONA UN OBJETO

        // Al mínimo sumarle el máximo y obtener el total
        // Lo divido por 2 así estaré en la mitad de la colisión o pivote
        // Se lo resto al total de collider del que necesite obtener la posició exacta

        float average = (minZ + maxZ) / 2 - colliderBounds.min.z;

        // Como lo divimos en tres partes 1/3=0.33
        const float fraccion = 1.0f / 3.0f;

        CollisionZ colZ =
            (average > colliderBounds.size.z - fraccion)
                ? CollisionZ.Forward
                : (average < fraccion)
                    ? CollisionZ.Backward
                    : CollisionZ.Middle;

        return colZ;
    }
}
