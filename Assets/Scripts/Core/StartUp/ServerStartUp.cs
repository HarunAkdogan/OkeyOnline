using System;
using System.Collections;
using System.Collections.Generic;
using Core.Manager;
using Network;
using PlayFab;
using PlayFab.MultiplayerAgent.Model;
using UnityEngine;

namespace Core.StartUp
{
    public class ServerStartUp : MonoBehaviour
    {
        public Config config;

        public NetworkManagerOkey networkManagerOkey;
        
        
        private List<ConnectedPlayer> _connectedPlayers;

        
        private List<PlayerConnection> playerConnections = new List<PlayerConnection>();
        private List<ConnectedPlayer> connectedPlayers = new List<ConnectedPlayer>();
        
        
        void Start()
        {
            if (config.buildType == BuildType.REMOTE_SERVER)
            {
                StartPlayfabApi();
            }else if(config.buildType == BuildType.LOCAL_SERVER)
            {
                networkManagerOkey.networkAddress = "localhost";
                networkManagerOkey.StartHost();
            }
        }
        
        void StartPlayfabApi()
        {
            Debug.Log("[ServerStartUp].StartRemoteServer");
            _connectedPlayers = new List<ConnectedPlayer>();
            PlayFabMultiplayerAgentAPI.Start();
            PlayFabMultiplayerAgentAPI.IsDebugging = config.playFabDebugging;
            PlayFabMultiplayerAgentAPI.OnServerActiveCallback += OnServerActive;
            PlayFabMultiplayerAgentAPI.OnMaintenanceCallback += OnMaintenance;
            PlayFabMultiplayerAgentAPI.OnShutDownCallback += OnShutdown;
            PlayFabMultiplayerAgentAPI.OnAgentErrorCallback += OnAgentError;
            
            networkManagerOkey.OnPlayerAdded.AddListener(OnPlayerAdded);
            networkManagerOkey.OnPlayerRemoved.AddListener(OnPlayerRemoved);

            StartCoroutine(ReadyForPlayers());
            StartCoroutine(ShutdownServerInXTime());
        }
        
        IEnumerator ShutdownServerInXTime()
        {
            yield return new WaitForSeconds(300f);
            OnShutdown();
        }
        private void OnShutdown()
        {
            Debug.Log("Server is shutting down");
            foreach(var conn in networkManagerOkey.Connections)
            {
                conn.Connection.Send<ShutdownMessage>(new ShutdownMessage());
                
            }
            StartCoroutine(Shutdown());
        }
        
        IEnumerator Shutdown()
        {
            yield return new WaitForSeconds(5f);
            Application.Quit();
        }
        
        private void OnServerActive()
        {
            networkManagerOkey.StartListen();
            Debug.Log("Server Started From Agent Activation");
        }
        
        private void OnPlayerRemoved(string playfabId)
        {
            ConnectedPlayer player = _connectedPlayers.Find(x => x.PlayerId.Equals(playfabId, StringComparison.OrdinalIgnoreCase));
            _connectedPlayers.Remove(player);
            PlayFabMultiplayerAgentAPI.UpdateConnectedPlayers(_connectedPlayers);
            CheckPlayerCountToShutdown();
        }
        
        private void CheckPlayerCountToShutdown()
        {
            if (_connectedPlayers.Count <= 0)
            {
                OnShutdown();
            }
        }
        
        private void OnPlayerAdded(string playfabId)
        {
            _connectedPlayers.Add(new ConnectedPlayer(playfabId));
            PlayFabMultiplayerAgentAPI.UpdateConnectedPlayers(_connectedPlayers);
            GameManager.instance.StartCoroutine("InitGame");
        }
        
        private void OnMaintenance(DateTime? NextScheduledMaintenanceUtc)
        {
            
            Debug.LogFormat("Maintenance scheduled for: {0}", NextScheduledMaintenanceUtc.Value.ToLongDateString());
            foreach (var conn in networkManagerOkey.Connections)
            {
                conn.Connection.Send<MaintenanceMessage>(new MaintenanceMessage() {
                    ScheduledMaintenanceUTC = (DateTime)NextScheduledMaintenanceUtc
                });
            }
        }
        
        private void OnAgentError(string error)
        {
            Debug.Log(error);
        }
        
        IEnumerator ReadyForPlayers()
        {
            yield return new WaitForSeconds(.5f);
            PlayFabMultiplayerAgentAPI.ReadyForPlayers();
        }
    }
    
    
    public class PlayerConnection
    {
        public ConnectedPlayer ConnectedPlayer;
        public int ConnectionId;
    }
}