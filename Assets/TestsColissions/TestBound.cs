using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBound : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        // Collider Bounds obtiene los valores del tama�o de collider BOUNDS == LIMITE
        Bounds collisionBounds = collision.collider.bounds;
        Bounds playerBounds = gameObject.GetComponent<Collider>().bounds;

        // Saber el valor XYZ de un collider
        Debug.Log($"Collider bounds X: {collisionBounds.size.x}");
        Debug.Log($"player bounds X: {playerBounds.size.x}");

        // SABER EL TAMA�O DE LA COLISION

        // obtener el punto m�ximo de los m�nimos de X
        float minX = Mathf.Max(collisionBounds.min.x, playerBounds.min.x);
        Debug.Log($"Punto m�nimo del collider del colision: {collisionBounds.min.x}");
        Debug.Log($"Punto m�nimo del collider del player: {playerBounds.min.x}");
        Debug.Log($"m�ximo de los m�nimos: {minX}");

        // obtener el punto m�ximo de los m�nimos de X
        float maxX = Mathf.Min(collisionBounds.max.x, playerBounds.max.x);
        Debug.Log($"Punto m�ximo del collider del colision: {collisionBounds.max.x}");
        Debug.Log($"Punto m�ximo del collider del player: {playerBounds.max.x}");
        Debug.Log($"m�nimo de los m�ximos: {maxX}");

        // SABER EL PUNTO EXACTO DONDE COLISIONA UN OBJETO

        // Al m�nimo sumarle el m�ximo y obtener el total
        // Lo divido por 2 as� estar� en la mitad de la colisi�n o pivote
        // Se lo resto al total de collider del que necesite obtener la posici� exacta

        float average = (minX + maxX) / 2 - collisionBounds.min.x;
        Debug.Log($"Average: {average}");
    }
}
