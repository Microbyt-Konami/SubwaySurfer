using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CollisionX { Left, Middle, Right, None }

public class PlayerCollision : MonoBehaviour
{
    // Fields
    [SerializeField] private CollisionX collisionX = CollisionX.None;

    // Componentes
    private PlayerController playerController;

    public void OnCharacterColission(Collider collider) => CalcCollisionX(collider);

    private void Awake()
    {
        playerController = gameObject.GetComponent<PlayerController>();
    }

    private void CalcCollisionX(Collider collider)
    {
        // Collider Bounds obtiene los valores del tamaño de collider BOUNDS == LIMITE
        Bounds characterControllerBounds = playerController.MyCharacterController.bounds;
        Bounds colliderBounds = collider.bounds;
        // obtener el punto máximo de los mínimos de X
        float minX = Mathf.Max(characterControllerBounds.min.x, colliderBounds.min.x);
        // obtener el punto máximo de los mínimos de X
        float maxX = Mathf.Min(characterControllerBounds.max.x, colliderBounds.max.x);
        // SABER EL PUNTO EXACTO DONDE COLISIONA UN OBJETO

        // Al mínimo sumarle el máximo y obtener el total
        // Lo divido por 2 así estaré en la mitad de la colisión o pivote
        // Se lo resto al total de collider del que necesite obtener la posició exacta

        float average = (minX + maxX) / 2 - characterControllerBounds.min.x;

        // Como lo divimos en tres partes 1/3=0.33
        const float fraccion = 1.0f / 3.0f;

        if (average > colliderBounds.size.x - fraccion)
            collisionX = CollisionX.Right;
        else if (average < fraccion)
            collisionX = CollisionX.Left;
        else
            collisionX = CollisionX.Middle;
    }
}
