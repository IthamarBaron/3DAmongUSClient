using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientSend : MonoBehaviour
{
    /// <summary>Sends a packet to the server via TCP.</summary>
    /// <param name="_packet">The packet to send to the sever.</param>
    private static void SendTCPData(Packet _packet)
    {
        _packet.WriteLength();
        Client.instance.tcp.SendData(_packet);
    }

    /// <summary>Sends a packet to the server via UDP.</summary>
    /// <param name="_packet">The packet to send to the sever.</param>
    private static void SendUDPData(Packet _packet)
    {
        _packet.WriteLength();
        Client.instance.udp.SendData(_packet);
    }

    #region Packets
    /// <summary>Lets the server know that the welcome message was received.</summary>
    public static void WelcomeReceived()
    {
        using (Packet _packet = new Packet((int)ClientPackets.welcomeReceived))
        {
            _packet.Write(Client.instance.myId);
            _packet.Write(UIManager.instance.usernameField.text);

            SendTCPData(_packet);
        }
    }

    /// <summary>Sends player input to the server.</summary>
    /// <param name="_inputs"></param>
    public static void PlayerMovement(bool[] _inputs)
    {
        if (Client.iniciatedOtherClients)
        {
            using (Packet _packet = new Packet((int)ClientPackets.playerMovement))
            {
                _packet.Write(_inputs.Length);
                foreach (bool _input in _inputs)
                {
                    _packet.Write(_input);
                }
                _packet.Write(GameManager.players[Client.instance.myId].transform.rotation);

                SendUDPData(_packet);
            }
        }

    }
    /// <summary>
    /// sends player reqest to teleport all players to the map when the game starts
    /// </summary>
    /// <param name="_position">the positon the players will be teleported to</param>
    public static void TeleportToMap()
    {
        using (Packet _packet = new Packet((int)ClientPackets.teleportToMap))
        {
            _packet.Write("x");//just not to send an empty packet

            SendTCPData(_packet);
        }
    }
    /// <summary>
    /// sends reqest for the server to generate roles for each player
    /// </summary>
    /// <param name="_impostors">amount of impostors that will be generated</param>
    public static void GetRoles(int _impostors)
    {
        using (Packet _packet = new Packet((int)ClientPackets.getRoles))
        {
            _packet.Write(_impostors);
            SendTCPData(_packet);
        }
    }
    /// <summary>
    /// sends a reqest for the server to kill a player
    /// </summary>
    /// <param name="_targetId"> player we want to kill ID</param>
    public static void AttemptKill(int _targetId)
    {
        using (Packet _packet = new Packet((int)ClientPackets.attemptKill))
        {
            _packet.Write(_targetId);
            SendTCPData(_packet);
        }
    }

    /// <summary>
    /// When somone finished a task this method is called to increment everyones taskbar
    /// </summary>
    public static void UpdateTaskProgressServer()
    {
        using (Packet _packet  = new Packet((int)ClientPackets.updateTaskProgressServer))
        {
            _packet.Write(1);// how much to increment  
            SendTCPData(_packet);
        }
    }

    /// <summary>
    /// tells te server to call an emergency meeting
    /// </summary>
    /// <param name="_callerORBodyIdCode">if > 11 than its a report if smalelr then caller</param>
    public static void CallEmergencyMeeting(int _callerORBodyIdCode)
    {
        using (Packet _packet = new Packet((int)ClientPackets.callEmergencyMeeting))
        {
            _packet.Write(_callerORBodyIdCode);
            SendTCPData(_packet);
        }
    }

    /// <summary>
    /// when a player wants to do a madbay scan we need to share to everyone that he is doing it
    /// </summary>
    public static void PlayerStartMadBayScan()
    {
        Debug.Log("SendingMadBayPacket");
        using (Packet _packet = new Packet((int)ClientPackets.playerStartMadBayScan))
        {
            _packet.Write("x"); // not sending an empty packet
            SendTCPData(_packet);
        }
    }


    /// <summary>
    /// Sending to the server the ID of the player you voted for
    /// </summary>
    /// <param name="_votingInfo">The amount of players in the meting and the id the client voted for</param>
    public static void CastVote(string _votingInfo)
    {
        using (Packet _packet = new Packet((int)ClientPackets.castVote))
        {
            _packet.Write(_votingInfo);
            SendTCPData(_packet);
        }
    }

    public static void GetGameStatus()
    {
        using (Packet _packet = new Packet((int)ClientPackets.getGameStatus))
        {
            _packet.Write("x");
            SendTCPData(_packet);
        }
    }
    #endregion
}