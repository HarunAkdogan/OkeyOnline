using System;
using System.Collections.Generic;
using Core.Manager;
using Mirror;
using Network;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.MultiplayerModels;
using UnityEngine;
using UnityEngine.UI;

namespace Core.StartUp
{
    public class ClientStartUp : MonoBehaviour
    {
        public Config config;
        public NetworkManagerOkey networkManagerOkey;
        
        [SerializeField]
        public Text text;
        
        public void OnLoginUserButtonClick()
        {
            if (config.buildType == BuildType.REMOTE_CLIENT)
            {
                if (config.buildId == "")
                {
                    throw new Exception("A remote client build must have a buildId. Add it to the Configuration. Get this from your Multiplayer Game Manager in the PlayFab web console.");
                }
                else
                {
                    LoginRemoteUser();
                }
            }
            else if (config.buildType == BuildType.LOCAL_CLIENT)
            {
                networkManagerOkey.networkAddress = "localhost";
                networkManagerOkey.StartClient();
            }
        }
        
        public void OnCancelButtonClick()
        {
            if (config.buildType == BuildType.REMOTE_CLIENT)
            {
                Debug.Log("[ClientStartUp].OnCancelButtonClick");
                NetworkClient.Disconnect();
                networkManagerOkey.StopClient();
            }
        }
        
        private void LoginRemoteUser()
        {
            Debug.Log("[ClientStartUp].LoginRemoteUser");
            text.text = "[ClientStartUp].LoginRemoteUser";
            //We need to login a user to get at PlayFab API's. 
            LoginWithCustomIDRequest request = new LoginWithCustomIDRequest()
            {
                TitleId = PlayFabSettings.TitleId,
                CreateAccount = true,
                CustomId = SystemInfo.deviceUniqueIdentifier,
            };

            PlayFabClientAPI.LoginWithCustomID(request, OnPlayFabLoginSuccess, OnLoginError);
        }
        
        private void OnPlayFabLoginSuccess(LoginResult response)
        {
            Debug.Log("[OnPlayFabLoginSuccess]"+response.ToString());
            text.text = "[OnPlayFabLoginSuccess]" +response.ToString();
            if (config.ipAddress == "")
            {   //We need to grab an IP and Port from a server based on the buildId. Copy this and add it to your Configuration.
                RequestMultiplayerServer(); 
            }
            else
            {
                ConnectRemoteClient();
            }
        }
        
        private void OnLoginError(PlayFabError response)
        {
            text.text = "[OnLoginError: ]" +response.ToString() + " - " + response.Error.ToString();
            Debug.Log(response.ToString());
        }
        
        private void RequestMultiplayerServer()
        {
            Debug.Log("[ClientStartUp].RequestMultiplayerServer");
            text.text = "[ClientStartUp].RequestMultiplayerServer";
            RequestMultiplayerServerRequest requestData = new RequestMultiplayerServerRequest();
            requestData.BuildId = config.buildId;
            requestData.SessionId = System.Guid.NewGuid().ToString();
            requestData.PreferredRegions = new List<String>() {AzureRegion.EastUs.ToString()};
            PlayFabMultiplayerAPI.RequestMultiplayerServer(requestData, OnRequestMultiplayerServer, OnRequestMultiplayerServerError);
        }
        
        private void OnRequestMultiplayerServer(RequestMultiplayerServerResponse response)
        {
            Debug.Log(response.ToString());
            text.text = response.ToString();
            ConnectRemoteClient(response);
        }
        
        
        private void ConnectRemoteClient(RequestMultiplayerServerResponse response = null)
        {
            
            if(response == null) 
            {
                networkManagerOkey.networkAddress = config.ipAddress;
                networkManagerOkey.GetComponent<TelepathyTransport>().port = config.port;
            }
            else
            {
                Debug.Log("**** ADD THIS TO YOUR CONFIGURATION **** -- IP: " + response.IPV4Address + " Port: " + (ushort)response.Ports[0].Num);
                networkManagerOkey.networkAddress = response.IPV4Address;
                networkManagerOkey.GetComponent<TelepathyTransport>().port = (ushort)response.Ports[0].Num;
                
            }

            networkManagerOkey.StartClient();
        }
        
        private void OnRequestMultiplayerServerError(PlayFabError error)
        {
            Debug.Log(error.HttpCode + error.ErrorMessage);
            text.text = error.HttpCode + error.ErrorMessage;
        }
        
    }
}