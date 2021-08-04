using System;
using System.Collections.Generic;
using Controller;
using Mirror;
using UnityEngine;
using View.Renderer;

namespace Model
{
    public class PlayerField : NetworkBehaviour
    {
        public Player player;
        public TileRenderer tileRenderer;
        
        public SyncList<TileController> tileRenderers = new SyncList<TileController>();

        private Transform setParants;

        private void Start()
        {
            setParants = FindObjectOfType<SetParant>().transform;
        }

        [Command]
        public void CmdPlayerOnDrop(Tile tile,TileController tileController)
        {
            // TileRenderer den = Instantiate(tileRenderer);
            // NetworkServer.Spawn(den.gameObject);
            // tileRenderers.Add(den.GetComponent<TileController>());
            RpcPlayerOnDrop(tileController);
            Debug.Log("CmdPlayerOnDrop: ");
        }
        
        [ClientRpc]
        private void RpcPlayerOnDrop(TileController tile)
        {
            //Todo Algoritmayi tek fonskiyonla halletmek => Player Dizilim Array => Player ID => Player Opponent => TileField?
            
            //  Todo PlayerDrop(Player.PlayerType playerType, int[] dizilim) => Fonksiyon ornek;{
            //  TileRenderer den = Instantiate(tileRenderer, Player.gameManager.playerTileField.content, false);
            //  den.tile = tile;
            //  den.Render();
            //  }
            
            
            
            if (player.isLocalPlayer)
            {
                // TileRenderer den = Instantiate(tileRenderer, Player.gameManager.playerTileField.content, false);
                // tile.GetComponent<TileController>().parentToReturnTo = Player.gameManager.playerTileField.content;
                // den.GetComponent<TileController>().parentToReturnTo =
                //     Player.gameManager.playerTileField.content.transform;
                // den.tile = tile;
                // den.Render();
                // NetworkServer.Spawn(den.gameObject);
                tile.transform.SetParent(Player.gameManager.playerTileField.content, false);
                // tileRenderers.Add(den.GetComponent<TileController>());
            }
            
            else if (player.playerType == PlayerType.Player1)
            {
                // for (int i = 0; i < player.opponents.Count; i++)
                // {
                //     TileRenderer den;
                //     
                //     
                //     // if (player.opponents[i].playerType == PlayerType.Player2)
                //     // {
                //     //     TileRenderer den = Instantiate(tileRenderer, Player.gameManager.opponentTileField3.content, false);
                //     //     den.tile = tile;
                //     //     den.Render();
                //     // }
                //     //     
                //     // if (player.opponents[1].playerType == PlayerType.Player3)
                //     // {
                //     //     TileRenderer den = Instantiate(tileRenderer, Player.gameManager.opponentTileField2.content, false);
                //     //     den.tile = tile;
                //     //     den.Render();
                //     // }
                //     //     
                //     // if (player.opponents[2].playerType == PlayerType.Player4)
                //     // {
                //     //     TileRenderer den = Instantiate(tileRenderer, Player.gameManager.opponentTileField.content, false);
                //     //     den.tile = tile;
                //     //     den.Render();
                //     // }
                // }
                Player players = Player.localPlayer;
                TileRenderer den;
                TileRenderer tileRendererss;
                switch (players.playerType)
                {
                    case PlayerType.Player2:
                        // den = Instantiate(tileRenderer, Player.gameManager.opponentTileField3.content, false);
                        // den.GetComponent<TileController>().parentToReturnTo =
                        //     Player.gameManager.opponentTileField3.content.transform;
                        // den.tile = tile;
                        // den.Render();
                        // tileRenderers.Add(den.GetComponent<TileController>());
                        tile.transform.SetParent(Player.gameManager.opponentTileField3.content, false);
                        tile.parentToReturnTo = Player.gameManager.opponentTileField3.content;
                        tile.tileRenderer.Render();
                        // tile.GetComponent<TileRenderer>().Render();
                        // tileRendererss = tile.GetComponent<TileRenderer>();
                        // tile.transform.SetParent(Player.gameManager.opponentTileField3.content, false);
                        // tileRendererss.Render();
                        break;
                    case PlayerType.Player3:
                        // den = Instantiate(tileRenderer, Player.gameManager.opponentTileField2.content, false);
                        // den.GetComponent<TileController>().parentToReturnTo =
                        //     Player.gameManager.opponentTileField2.content.transform;
                        // den.tile = tile;
                        // den.Render();
                        // tileRenderers.Add(den.GetComponent<TileController>());
                        
                        tile.transform.SetParent(Player.gameManager.opponentTileField2.content, false);
                        tile.parentToReturnTo = Player.gameManager.opponentTileField2.content;
                        tile.tileRenderer.Render();
                        break;
                    case PlayerType.Player4:
                        // den = Instantiate(tileRenderer, Player.gameManager.opponentTileField.content, false);
                        // den.GetComponent<TileController>().parentToReturnTo =
                        //     Player.gameManager.opponentTileField.content.transform;
                        // den.tile = tile;
                        // den.Render();
                        // NetworkServer.Spawn(den.gameObject);
                        
                        tile.transform.SetParent(Player.gameManager.opponentTileField.content, false);
                        tile.parentToReturnTo = Player.gameManager.opponentTileField.content;
                        tile.tileRenderer.Render();
                        // tileRenderers.Add(den.GetComponent<TileController>());
                        break;
                }
            }
            
            
            else if (player.playerType == PlayerType.Player2)
            {
                // foreach (var opponent in player.opponents)
                // {
                //     
                //         if (opponent.playerType == PlayerType.Player1)
                //         {
                //             TileRenderer den = Instantiate(tileRenderer, Player.gameManager.opponentTileField.content, false);
                //             den.tile = tile;
                //             den.Render();
                //         }
                //         
                //         if (opponent.playerType == PlayerType.Player3)
                //         {
                //             TileRenderer den = Instantiate(tileRenderer, Player.gameManager.opponentTileField3.content, false);
                //             den.tile = tile;
                //             den.Render();
                //         }
                //         
                //         if (opponent.playerType == PlayerType.Player4)
                //         {
                //             TileRenderer den = Instantiate(tileRenderer, Player.gameManager.opponentTileField2.content, false);
                //             den.tile = tile;
                //             den.Render();
                //         }
                // }
                Player players = Player.localPlayer;
                TileRenderer den;
                switch (players.playerType)
                {
                    case PlayerType.Player1:
                        // den = Instantiate(tileRenderer, Player.gameManager.opponentTileField.content, false);
                        // den.GetComponent<TileController>().parentToReturnTo =
                        //     Player.gameManager.opponentTileField.content.transform;
                        // den.tile = tile;
                        // den.Render();
                        // NetworkServer.Spawn(den.gameObject);
                        tile.transform.SetParent(Player.gameManager.opponentTileField.content, false);
                        tile.parentToReturnTo = Player.gameManager.opponentTileField.content;
                        tile.tileRenderer.Render();
                        // tileRenderers.Add(den.GetComponent<TileController>());
                        break;
                    case PlayerType.Player3:
                        // den = Instantiate(tileRenderer, Player.gameManager.opponentTileField3.content, false);
                        // den.GetComponent<TileController>().parentToReturnTo =
                        //     Player.gameManager.opponentTileField3.content.transform;
                        // den.tile = tile;
                        // den.Render();
                        // NetworkServer.Spawn(den.gameObject);
                        tile.transform.SetParent(Player.gameManager.opponentTileField3.content, false);
                        tile.parentToReturnTo = Player.gameManager.opponentTileField3.content;
                        tile.tileRenderer.Render();
                        // tileRenderers.Add(den.GetComponent<TileController>());
                        break;
                    case PlayerType.Player4:
                        // den = Instantiate(tileRenderer, Player.gameManager.opponentTileField2.content, false);
                        // den.GetComponent<TileController>().parentToReturnTo =
                        //     Player.gameManager.opponentTileField2.content.transform;
                        // den.tile = tile;
                        // den.Render();
                        // NetworkServer.Spawn(den.gameObject);
                        tile.transform.SetParent(Player.gameManager.opponentTileField2.content, false);
                        tile.parentToReturnTo = Player.gameManager.opponentTileField2.content;
                        tile.tileRenderer.Render();
                        // tileRenderers.Add(den.GetComponent<TileController>());
                        break;
                }
            }
            
            else if (player.playerType == PlayerType.Player3)
            {
                // foreach (var opponent in player.opponents)
                // {
                //     
                //         if (opponent.playerType == PlayerType.Player1)
                //         {
                //             TileRenderer den = Instantiate(tileRenderer, Player.gameManager.opponentTileField2.content, false);
                //             den.tile = tile;
                //             den.Render();
                //         }
                //         if (opponent.playerType == PlayerType.Player2)
                //         {
                //             TileRenderer den = Instantiate(tileRenderer, Player.gameManager.opponentTileField.content, false);
                //             den.tile = tile;
                //             den.Render();
                //         }
                //         if (opponent.playerType == PlayerType.Player4)
                //         {
                //             TileRenderer den = Instantiate(tileRenderer, Player.gameManager.opponentTileField3.content, false);
                //             den.tile = tile;
                //             den.Render();
                //         }
                // }
                Player players = Player.localPlayer;
                TileRenderer den;
                switch (players.playerType)
                {
                    case PlayerType.Player1:
                        // den = Instantiate(tileRenderer, Player.gameManager.opponentTileField2.content, false);
                        // den.tile = tile;
                        // den.Render();
                        // NetworkServer.Spawn(den.gameObject);
                        tile.transform.SetParent(Player.gameManager.opponentTileField2.content, false);
                        tile.parentToReturnTo = Player.gameManager.opponentTileField2.content;
                        tile.tileRenderer.Render();
                        // tileRenderers.Add(den.GetComponent<TileController>());
                        break;
                    case PlayerType.Player2:
                        // den = Instantiate(tileRenderer, Player.gameManager.opponentTileField.content, false);
                        // den.tile = tile;
                        // den.Render();
                        // NetworkServer.Spawn(den.gameObject);
                        tile.transform.SetParent(Player.gameManager.opponentTileField.content, false);
                        tile.parentToReturnTo = Player.gameManager.opponentTileField.content;
                        tile.tileRenderer.Render();
                        // tileRenderers.Add(den.GetComponent<TileController>());
                        break;
                    case PlayerType.Player4:
                        // den = Instantiate(tileRenderer, Player.gameManager.opponentTileField3.content, false);
                        // den.tile = tile;
                        // den.Render();
                        // NetworkServer.Spawn(den.gameObject);
                        tile.transform.SetParent(Player.gameManager.opponentTileField3.content, false);
                        tile.parentToReturnTo = Player.gameManager.opponentTileField3.content;
                        tile.tileRenderer.Render();
                        // tileRenderers.Add(den.GetComponent<TileController>());
                        break;
                }
            }
            
            else if (player.playerType == PlayerType.Player4)
            {
                // foreach (var opponent in player.opponents)
                // {
                //     
                //         if (opponent.playerType == PlayerType.Player1)
                //         {
                //             TileRenderer den = Instantiate(tileRenderer, Player.gameManager.opponentTileField3.content, false);
                //             den.tile = tile;
                //             den.Render();
                //         }
                //         if (opponent.playerType == PlayerType.Player2)
                //         {
                //             TileRenderer den = Instantiate(tileRenderer, Player.gameManager.opponentTileField2.content, false);
                //             den.tile = tile;
                //             den.Render();
                //         }
                //         if (opponent.playerType == PlayerType.Player3)
                //         {
                //             TileRenderer den = Instantiate(tileRenderer, Player.gameManager.opponentTileField.content, false);
                //             den.tile = tile;
                //             den.Render();
                //         }
                // }
                Player players = Player.localPlayer;
                TileRenderer den;
                switch (players.playerType)
                {
                    case PlayerType.Player1:
                        // den = Instantiate(tileRenderer, Player.gameManager.opponentTileField3.content, false);
                        // den.GetComponent<TileController>().parentToReturnTo =
                        //     Player.gameManager.opponentTileField3.content.transform;
                        // den.tile = tile;
                        // den.Render();
                        // NetworkServer.Spawn(den.gameObject);
                        tile.transform.SetParent(Player.gameManager.opponentTileField3.content, false);
                        tile.parentToReturnTo = Player.gameManager.opponentTileField3.content;
                        tile.tileRenderer.Render();
                        // tileRenderers.Add(den.GetComponent<TileController>());
                        break;
                    case PlayerType.Player2:
                        // den = Instantiate(tileRenderer, Player.gameManager.opponentTileField2.content, false);
                        // den.GetComponent<TileController>().parentToReturnTo =
                        //     Player.gameManager.opponentTileField2.content.transform;
                        // den.tile = tile;
                        // den.Render();
                        // NetworkServer.Spawn(den.gameObject);
                        tile.transform.SetParent(Player.gameManager.opponentTileField2.content, false);
                        tile.parentToReturnTo = Player.gameManager.opponentTileField2.content;
                        tile.tileRenderer.Render();
                        // tileRenderers.Add(den.GetComponent<TileController>());
                        break;
                    case PlayerType.Player3:
                        // den = Instantiate(tileRenderer, Player.gameManager.opponentTileField.content, false);
                        // den.GetComponent<TileController>().parentToReturnTo =
                        //     Player.gameManager.opponentTileField.content.transform;
                        // den.tile = tile;
                        // den.Render();
                        // NetworkServer.Spawn(den.gameObject);
                        tile.transform.SetParent(Player.gameManager.opponentTileField.content, false);
                        tile.parentToReturnTo = Player.gameManager.opponentTileField.content;
                        tile.tileRenderer.Render();
                        // tileRenderers.Add(den.GetComponent<TileController>());
                        break;
                }
            }
            
            Debug.Log("RpcPlayerOnDrop");
        }
        
        [Command]
        public void CmdPlayerFieldTileDrop(TileController removeTile)
        {
            RpcCmdPlayerFieldTileDrop(removeTile);
        }
        
        [ClientRpc]
        private void RpcCmdPlayerFieldTileDrop(TileController tileController)
        {
            if (isLocalPlayer)
            {
                Debug.Log(tileController.tileRenderer.tile.number.ToString());    
            }else
            {
                Debug.Log(tileController.tileRenderer.tile.number.ToString());    
                tileController.transform.SetParent(setParants,false);
            }
        }
        
        [Command]
        public void CmdPlayerTableTileDrop(TileController removeTile,Tile tile)
        {
            removeTile.tileRenderer.tile = tile;
            RpcPlayerTableTileDrop(removeTile);
        }
        
        [ClientRpc]
        private void RpcPlayerTableTileDrop(TileController tileController)
        {
            // if (isLocalPlayer)
            // {
            //     Debug.Log(tileController.tileRenderer.tile.number.ToString());    
            // }else
            // {
            //     Destroy(tileController.gameObject);    
            // }
            tileController.tileRenderer.Render();
            // tileController.parentToReturnTo = Player.gameManager.opponentTileField2.content;
            // tileController.transform.SetParent(Player.gameManager.opponentTileField2.content,false);
        }
    }
}