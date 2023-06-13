using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class ClientHandle : MonoBehaviour
{
    public static bool HandledRoles = false;
    public static bool GotMeetingInfo = false;

    public static float timerDuration = 0.5f; // giving the client time to "spawn" the other cliets

    /// <summary>
    /// handles the welcome packet from the server & connects UDP
    /// </summary>
    /// <param name="_packet"></param>
    public static void Welcome(Packet _packet)
    {
        string _msg = _packet.ReadString();
        int _myId = _packet.ReadInt();

        Debug.Log($"Message from server: {_msg}");
        Client.instance.myId = _myId;
        ClientSend.WelcomeReceived();

        // Now that we have the client's id, connect UDP
        Client.instance.udp.Connect(((IPEndPoint)Client.instance.tcp.socket.Client.LocalEndPoint).Port);
        Client clientHandleInstance = FindObjectOfType<Client>();
        clientHandleInstance.Invoke("FlipPacketsRestriction", timerDuration);//NOT A TIME OUT... ok maybe :3
    }

    /// <summary>
    /// spawns a player 
    /// </summary>
    /// <param name="_packet">packet from the server containing name, position, and rotation of the player</param>
    public static void SpawnPlayer(Packet _packet)
    {
        int _id = _packet.ReadInt();
        string _username = _packet.ReadString();
        Vector3 _position = _packet.ReadVector3();
        Quaternion _rotation = _packet.ReadQuaternion();

        GameManager.instance.SpawnPlayer(_id, _username, _position, _rotation);
    }

    /// <summary>
    /// updates player's position (self and others)
    /// </summary>
    /// <param name="_packet">packet containing player to move and his desired position</param>
    public static void PlayerPosition(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();
        if (Client.iniciatedOtherClients)
        {
            GameManager.players[_id].transform.position = _position;
        }

    }

    /// <summary>
    /// updates player's rotation (self and others)
    /// </summary>
    /// <param name="_packet">packet containing player to rotate and his desired rotation</param>
    public static void PlayerRotation(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Quaternion _rotation = _packet.ReadQuaternion();
        if (Client.iniciatedOtherClients)
            GameManager.players[_id].transform.rotation = _rotation;
    }

    /// <summary>
    /// handleing disconnections  (removing from dictionery)
    /// </summary>
    /// <param name="_packet">packet containing id of disconnected player</param>
    public static void PlayerDisconnected(Packet _packet)
    {
        int _id = _packet.ReadInt();

        Destroy(GameManager.players[_id].gameObject);
        GameManager.players.Remove(_id);
    }

    /// <summary>
    /// Gives players their roles and allows the game to continue 
    /// </summary>
    /// <param name="_packet">packet containing impostors id / impostors id code</param>
    public static void SetRoles(Packet _packet)
    {
        int _impostorID1 = _packet.ReadInt();
        int _impostorID2 = 0;
        if (GameManager.IMPOSTORS == 2)
        {
            _impostorID2 = _impostorID1 % 10;
            _impostorID1 /= 10;
        }

        if (_impostorID1 == Client.instance.myId || _impostorID2 == Client.instance.myId)
        {
            Client.instance.isImpostor = true;
            Debug.Log("Set you Impostor");
        }
        else
        {
            Client.instance.isImpostor = false;
            Debug.Log("Set you Crew mate");
        }
        HandledRoles = true;
    }

    /// <summary>
    /// Eliminates a dead player and spawns a body
    /// </summary>
    /// <param name="_packet">cintains the ID of a dead plaeyr</param>
    public static void EliminatePlayer(Packet _packet)
    {
        int _targetId = _packet.ReadInt();
        GameManager.players[_targetId].Die("killed", Client.instance.myId == _targetId,_targetId);
        ClientSend.GetGameStatus();
    }

    /// <summary>
    /// Updates the Green Bar on the left side depending on task program
    /// </summary>
    /// <param name="_packet">contains task progress</param>
    public static void UpdateTaskProgressClient(Packet _packet)
    {
        float _taskProgress = _packet.ReadFloat();
        float _temp = _taskProgress * 100;
        Interact.taskProgress = (int)_temp;//round up the value (yes i know this is stupid, last sc fix)
        ClientSend.GetGameStatus();
    }

    /// <summary>
    /// letting the clients know somone started a madbay scan
    /// </summary>
    /// <param name="_packet">not intresting data</param>
    public static void ServerStartMadBayScan(Packet _packet)
    {
        //string _temp = _packet.ReadString();
        Instantiate(Interact.madBayParticales, new Vector3(242.442f, 10.22f, 260.324f), Quaternion.Euler(-90f, 0f, 0f));
    }

    /// <summary>
    /// starts an emergancyMeeting
    /// </summary>
    /// <param name="_packet">contains the identifier for the meeting type and the info string containing names</param>
    public static void StartEmergencyMeeting(Packet _packet)
    {
        int _identifier = _packet.ReadInt();// 0 if normal meeting else -> dead plaeyr id
        string _infoString = _packet.ReadString();
        DeadBodyManager.ClearDeadBodies();
        ClientSend.TeleportToMap();
        PlayerController.canMove = false;
        if (_identifier == 0)
        {
            Instantiate(Resources.Load<GameObject>("Tasks_n_UI/" + "StartMeeting_Screen"), Interact.playerUI.transform);
        }
        else
        {
            DeadBodyReported_screen.deadPlayerID = _identifier;
            Instantiate(Resources.Load<GameObject>("Tasks_n_UI/" + "DeadBodyReported_Screen"), Interact.playerUI.transform);
        }


        string[] _names = _infoString.Split("!+@_#)$(%*^&");//Note _names[] will have an extara empty space at the end
        Voting.names = _names;
        GotMeetingInfo = true;
    }

    /// <summary>
    /// Ends the meeting
    /// </summary>
    /// <param name="_packet">result of the meeting</param>
    public static void EndMeeting(Packet _packet)
    {
        int _playerToEject = _packet.ReadInt();

        if (_playerToEject != 0 )
        {
            GameManager.players[_playerToEject].Die("ejected", Client.instance.myId == _playerToEject,0);
            //TODO: Write to the screen that the player was ejected
        }
        else
        {
            //TODO: Write to the screen that the meeting was skiped or tied.
        }
        //free the restrictions we started and reset vars
        Destroy(Interact.meetingScreenObject);
        PlayerController.canMove = true;
        CameraController.canLook = true;
        Voting.voted = false;
        Cursor.lockState = CursorLockMode.Locked;

        Interact.meetingInProgress = false;
        Interact.killCoolDownTimer = 20;

        ClientSend.GetGameStatus();

    }

    /// <summary>
    /// updates the game status for all players
    /// </summary>
    /// <param name="_packet">containing the game status</param>
    public static void GetGameStatus(Packet _packet)
    {
        string _status = _packet.ReadString();
        Interact.gameStatus = _status;
    }
}