using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    // Fields
    [SerializeField] private float positionZEnd;
    [SerializeField] private float positionZToChange;
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
        if (playerController.Position.z >= positionZEnd)
            playerController.SetPlayerPositionZ(playerController.Position.z - positionZEnd + positionZToChange);
    }
}
