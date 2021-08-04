using System;
using System.Collections.Generic;
using System.Linq;
using Controller;
using Core.Manager;
using Mirror;
using Network;
using UnityEngine;
using View.Renderer;

namespace Model
{
    public enum PlayerType { Player1, Player2, Player3, Player4 }
    public class Player : NetworkBehaviour
    {
        public static Player localPlayer;
        
        [Header("Tiles")]
        public List<Tile> tiles = new List<Tile>();
        public static GameManager gameManager;

        public PlayerType playerType = PlayerType.Player1;

        public List<Player> opponents = new List<Player>();
        
        public List<TileRenderer> tileRenderers = new List<TileRenderer>();
        
        [Header("Player ID")]
        public int playerId = 0;
        
        [Header("PlayerHand")] 
        public Hand playerHand;

        [Header("Tile Renderer")] 
        public TileRenderer tileRenderer;

        [Header("Player Field Drop")] 
        public PlayerField playerField;

        [HideInInspector]
        public DragController dragController;

        [HideInInspector]
        public SeriesController seriesController;

        public override void OnStartClient()
        {
            base.OnStartClient();
            if (!isServer)
            {
                NetworkManagerOkey.instance.AddPlayer(this);
            }
        }

        public override void OnStartLocalPlayer()
        {
            base.OnStartLocalPlayer();
            localPlayer = this;
        }

        private void Start()
        {
            gameManager = FindObjectOfType<GameManager>();
            playerHand = FindObjectOfType<Hand>();

            dragController = FindObjectOfType<DragController>();
            seriesController = FindObjectOfType<SeriesController>();
        }
        
        public void GiveTiles(List<Tile> playerTiles)
        {
            tiles.AddRange(playerTiles);
        }

        public void GiveSyncListTile(List<TileRenderer> tileRendererss)
        {
            tileRenderers.AddRange(tileRendererss);
        }

        [Command]
        public void AddTile(Tile tile)
        {
            RpcAddTile(tile);
        }

        [ClientRpc]
        private void RpcAddTile(Tile tile)
        {
            tiles.Add(tile);
        }
        
        [Command]
        public void RemoveTile(Tile tile)
        {
            RpcRemoveTile(tile);
        }

        [ClientRpc]
        private void RpcRemoveTile(Tile tile)
        {
            var item = tiles.Find(t => t.id == tile.id);
            tiles.Remove(item);
        }

        public void AddOpponents(List<Player> opponents)
        {
            foreach (var opponent in opponents)
            {
                if (opponent != this)
                {
                    this.opponents.Add(opponent);
                }
            }
        } 

        public void GetDeals()
        {
            if (!isLocalPlayer)
            {
                return;
            }
            var result = Enumerable.Range(0, 24).OrderBy(g => Guid.NewGuid()).Take(15).ToArray();
            
            int i = 0;
            foreach (var tile in tileRenderers)
            {
                playerHand.points[i].GetComponent<PointController>().DropTile(tile.tile);
                tile.transform.SetParent(playerHand.points[result[i]].transform,false);
                // tileRenderer.tile = tile.tile;
                tile.Render();
                // tileRenderer.Render();
                // NetworkServer.Spawn(instantiate.gameObject);
                i++;
            }

            dragController.points = playerHand.points;
            seriesController.points = playerHand.points;
        }

       
    }
}