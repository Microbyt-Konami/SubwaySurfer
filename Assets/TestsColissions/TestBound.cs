using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBound : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        // Collider Bounds obtiene los valores del tamaño de collider BOUNDS == LIMITE
        Bounds collisionBounds = collision.collider.bounds;
        Bounds playerBounds = gameObject.GetComponent<Collider>().bounds;

        // Saber el valor XYZ de un collider
        Debug.Log($"Collider bounds X: {collisionBounds.size.x}");
        Debug.Log($"player bounds X: {playerBounds.size.x}");

        // SABER EL TAMAÑO DE LA COLISION

        // obtener el punto máximo de los mínimos de X
        float minX = Mathf.Max(collisionBounds.min.x, playerBounds.min.x);
        Debug.Log($"Punto mínimo del collider del colision: {collisionBounds.min.x}");
        Debug.Log($"Punto mínimo del collider del player: {playerBounds.min.x}");
        Debug.Log($"máximo de los mínimos: {minX}");

        // obtener el punto máximo de los mínimos de X
        float maxX = Mathf.Min(collisionBounds.max.x, playerBounds.max.x);
        Debug.Log($"Punto máximo del collider del colision: {collisionBounds.max.x}");
        Debug.Log($"Punto máximo del collider del player: {playerBounds.max.x}");
        Debug.Log($"mínimo de los máximos: {maxX}");

        // SABER EL PUNTO EXACTO DONDE COLISIONA UN OBJETO

        // Al mínimo sumarle el máximo y obtener el total
        // Lo divido por 2 así estaré en la mitad de la colisión o pivote
        // Se lo resto al total de collider del que necesite obtener la posició exacta

        float average = (minX + maxX) / 2 - collisionBounds.min.x;
        Debug.Log($"Average: {average}");
    }
}
