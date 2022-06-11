using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;

    private void Awake() => PhotonNetwork.Instantiate(playerPrefab.name, transform.position, Quaternion.identity);
}