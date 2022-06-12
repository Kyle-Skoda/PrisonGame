using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class DataPackets : MonoBehaviour
{
    public static void CreateTeams(int playerID, Teams teamID)
    {
        object[] data = { playerID, teamID };
        SendPacket(Event.CreateTeams, data);
    }

    public static void StartGame()
    {
        object[] data = { };
        SendPacket(Event.StartGame, data);
    }

    public static void FinishLoading()
    {
        object[] data = { };
        SendPacket(Event.SetGameLoop, data);
    }

    private static void SendPacket(Event eventCode, object[] data)
    {
        RaiseEventOptions eventOptions = new RaiseEventOptions() { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent((byte)eventCode, data, eventOptions, SendOptions.SendReliable);
    }
}

public enum Event
{
    CreateTeams,
    ClearTeams,
    SetGameLoop,
    StartGame,
    EndGame,
}
