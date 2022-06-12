using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class WaitForPlayersToLoadScene : MonoBehaviour
{
    [SerializeField] private float maxTime = 15f;

    private void Start()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        StartCoroutine(WaitForAllPlayers());
    }

    private IEnumerator WaitForAllPlayers()
    {
        while (maxTime >= 0)
        {
            maxTime -= Time.deltaTime;
            if (NetworkManager.Instance.PlayersInRoom.Count == PhotonNetwork.CurrentRoom.PlayerCount || maxTime <= 0)
            {
                StopAllCoroutines();
                DataPackets.FinishLoading();
            }
            yield return null;
        }
    }
}
