using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class TeamManager : MonoBehaviourPunCallbacks, IOnEventCallback
{
    public static TeamManager Instance;

    public Teams LocalPlayerTeam = Teams.None;
    public List<Player> guards = new List<Player>();
    public List<Player> prisoners = new List<Player>();

    public Action OnTeamRecieved;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(this);
    }

    public override void OnEnable() => PhotonNetwork.AddCallbackTarget(this);
    public override void OnDisable() => PhotonNetwork.RemoveCallbackTarget(this);

    public void CreateTeams()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;
        List<Player> players = new List<Player>();
        foreach (var plyr in NetworkManager.Instance.PlayersInRoom) players.Add(plyr);

        int prisonerCount = 0;
        int gaurdCount = 0;
        int index = 0;
        List<Player> g = new List<Player>();
        List<Player> p = new List<Player>();
        for (int i = 0; i < NetworkManager.Instance.PlayersInRoom.Count; i++) index = (prisonerCount - gaurdCount > gaurdCount) ? gaurdCount++ : prisonerCount++;

        for (int i = 0; i < gaurdCount; i++)
        {
            index = UnityEngine.Random.Range(0, players.Count);
            g.Add(players[index]);
            players.Remove(players[index]);
        }

        for (int i = 0; i < players.Count; i++) p.Add(players[i]);

        for (int i = 0; i < g.Count; i++) DataPackets.CreateTeams(g[i].ActorNumber, Teams.Guard);
        for (int i = 0; i < p.Count; i++) DataPackets.CreateTeams(p[i].ActorNumber, Teams.Prisoner);
    }

    private void ClearTeams()
    {
        guards.Clear();
        prisoners.Clear();

        //TODO: SEND TEAM DATA OVER
    }

    public void ReceiveTeamData(int playerID, Teams team)
    {
        Player player = NetworkManager.Instance.GetPlayerByID(playerID);

        if (player == PhotonNetwork.LocalPlayer)
        {
            OnTeamRecieved?.Invoke();
            LocalPlayerTeam = team;
        }

        if (team == Teams.Prisoner)
            prisoners.Add(player);
        else if (team == Teams.Guard)
            guards.Add(player);
    }

    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == (byte)Event.CreateTeams)
        {
            object[] data = (object[])photonEvent.CustomData;

            ReceiveTeamData((int)data[0], (Teams)data[1]);
        }
    }
}

public enum Teams
{
    None = 0,
    Prisoner = 1,
    Guard = 2,
    Spectator = 3
}
