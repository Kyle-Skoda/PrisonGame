using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class NetworkManager : MonoBehaviourPunCallbacks, IOnEventCallback
{
    public bool startGame = false;

    public static NetworkManager Instance;

    public List<Player> PlayersInRoom = new List<Player>();
    public GameState CurrentGameState = GameState.Lobby;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            PhotonNetwork.AutomaticallySyncScene = true;
        }
        else
            Destroy(this);
    }

    public override void OnEnable() => PhotonNetwork.AddCallbackTarget(this);
    public override void OnDisable() => PhotonNetwork.RemoveCallbackTarget(this);

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        PlayersInRoom.Add(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        PlayersInRoom.Add(otherPlayer);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        base.OnMasterClientSwitched(newMasterClient);
        if (PhotonNetwork.IsMasterClient)
        {
            //TODO: Display start button
        }
    }

    public override void OnJoinedRoom()
    {
        foreach (var p in PhotonNetwork.PlayerList) PlayersInRoom.Add(p);
    }

    public override void OnLeftRoom()
    {
        PlayersInRoom.Clear();
        PlayersInRoom.Add(PhotonNetwork.LocalPlayer);
    }

    public Player GetPlayerByID(int playerID)
    {
        for (int i = 0; i < PlayersInRoom.Count; i++)
            if (PlayersInRoom[i].ActorNumber == playerID)
                return PlayersInRoom[i];

        Debug.LogError($"PLAYER WITH VIEW ID {playerID} NOT FOUND");
        return null;
    }

    private void Update()
    {
        if (startGame)
        {
            startGame = false;
            StartGame();
        }
    }

    public void StartGame()
    {
        if (!PhotonNetwork.IsMasterClient || CurrentGameState != GameState.Lobby) return;

        // if (PlayersInRoom.Count <= 1)
        // {
        //     Debug.LogWarning($"Not enough players");
        //     return;
        // }
        PhotonNetwork.LoadLevel("Game");
        TeamManager.Instance.CreateTeams();
    }

    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == (byte)Event.StartGame)
        {
            CurrentGameState = GameState.Loading;
        }
        else if (photonEvent.Code == (byte)Event.SetGameLoop)
        {
            CurrentGameState = GameState.GameLoop;
        }
    }
}

public enum GameState
{
    Lobby,
    Loading,
    GameLoop,
    DisplayScoreboard,
}
