using System;
using Core.Manager;
using Mirror;
using UnityEngine;

namespace Network
{
    public class NetworkManagerOkey : NetworkManager
    {
        public static NetworkManagerOkey instance;

        public override void Awake()
        {
            base.Awake();
            instance = this;
        }

        public GameManager gameManager;
        public override void OnServerAddPlayer(NetworkConnection conn)
        {
            Transform startPos = GetStartPosition();
            GameObject player = Instantiate(playerPrefab);

            NetworkServer.AddPlayerForConnection(conn, player);
            gameManager.PlayersAdd(player.GetComponent<Model.Player>());
        }

        public void AddPlayer(Model.Player player)
        {
            gameManager.PlayersAdd(player);
        }
        
    }
}