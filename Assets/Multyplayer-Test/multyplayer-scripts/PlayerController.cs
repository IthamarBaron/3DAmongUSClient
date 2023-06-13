using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// controlls player movment
/// </summary>
public class PlayerController : MonoBehaviour
{

    public static bool canMove = true;
    private void FixedUpdate()
    {
        if (canMove)
            SendInputToServer();
        else
            SendEmptyInputsToServer();
    }

    /// <summary>Sends player input to the server.</summary>
    private void SendInputToServer()
    {
        bool[] _inputs = new bool[]
        {
            Input.GetKey(KeyCode.W),
            Input.GetKey(KeyCode.S),
            Input.GetKey(KeyCode.A),
            Input.GetKey(KeyCode.D),
            false//Input.GetKey(KeyCode.Space)
        };

        ClientSend.PlayerMovement(_inputs);
    }
    /// <summary>
    /// when we dont want to move the player we cant just NOT send inputs, this is because
    /// when the server does not receive the udp packets it will close the connection so we will
    /// just send a "demo" inputs to maintain connecteion while not closing the connection ("keep alive").
    /// </summary>
    private void SendEmptyInputsToServer()
    {
        bool[] _inputs = new bool[5];
        ClientSend.PlayerMovement(_inputs);
    }
}