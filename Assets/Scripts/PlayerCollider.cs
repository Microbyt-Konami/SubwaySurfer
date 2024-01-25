using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
    // Componentes
    [SerializeField] private PlayerCollision playerCollision;

    // Start is called before the first frame update
    void Start()
    {
        playerCollision = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCollision>();

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            return;

        playerCollision.OnCharacterColission(collision.collider);
    }
}
