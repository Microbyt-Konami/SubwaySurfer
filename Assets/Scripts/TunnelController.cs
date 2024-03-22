using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelController : MonoBehaviour
{
    [SerializeField] private bool hasTunnelRight;
    [SerializeField] private bool hasTunnelCenter;
    [SerializeField] private bool hasTunnelLeft;
    // Start is called before the first frame update
    // void Start()
    // {

    // }

    // Update is called once per frame
    // void Update()
    // {

    // }

    public bool CanMove(Side side, Side sideNew)
    {
        switch (sideNew)
        {
            case Side.Left:
                return hasTunnelLeft;
            case Side.Middle:
                return hasTunnelCenter;
            case Side.Right:
                return hasTunnelRight;
            default:
                return false;
        }
    }
}
