using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    // Fields
    [SerializeField] private float positionXEnd;
    // Components
    private PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        playerController = FindFirstObjectByType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        // Cuando el Player llegue a la posición 2000 de Z deberá volver a la posición 0 de Z para poder seguir recorriendo el nivel desde el inicio. El cambio de posición del Player con respecto al nivel no debe tener ningún salto visual.
        if (playerController.Position.z >= positionXEnd)
            playerController.ResetPositionZ();
    }
}
