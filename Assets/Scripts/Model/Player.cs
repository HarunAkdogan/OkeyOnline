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
    public enum PlayerType { Player, Opponent }
    public class Player : NetworkBehaviour
    {
        public static Player localPlayer;
        
        [Header("Tiles")]
        public List<Tile> tiles = new List<Tile>();
        public static GameManager gameManager;

        [Header("PlayerHand")] 
        public Hand playerHand;

        [Header("Tile Renderer")] 
        public TileRenderer tileRenderer;
        public override void OnStartClient()
        {
            base.OnStartClient();
            localPlayer = this;
        }

        private void Start()
        {
            gameManager = FindObjectOfType<GameManager>();
            playerHand = FindObjectOfType<Hand>();
        }
        
        public void GiveTiles(List<Tile> playerTiles)
        {
            tiles.AddRange(playerTiles);
        }
        
        public void GetDeals()
        {
            var result = Enumerable.Range(0, 24).OrderBy(g => Guid.NewGuid()).Take(15).ToArray();
          

            int i = 0;
            foreach (var tile in tiles)
            {
                TileRenderer instantiate = Instantiate(tileRenderer);
                instantiate.transform.SetParent(playerHand.points[result[i]].transform,false);
                tileRenderer.tile = tile;
                playerHand.points[i].GetComponent<PointController>().DropTile(tile);
                tileRenderer.Render();
                i++;
            }

        }

    }
}