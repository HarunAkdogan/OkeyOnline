using System;
using System.Collections.Generic;
using System.Linq;
using Controller;
using Mirror;
using UnityEngine;
using View.Renderer;
using Random = UnityEngine.Random;

namespace Model
{
    public class Table : NetworkBehaviour
    {
        
        public SyncList<Tile> tiles = new SyncList<Tile>();
        public List<Tile> tiless = new List<Tile>();
        
        public Transform spawnTile;

        public TileRenderer tileRenderer;
        
        public void GetNTile(int N,Player player)
        { //TODO: This
            var sl = new List<TileRenderer>();
            for (var i = 0; i < N; i++)
            {
                // sl.Add(tiles.First());
                var item = tiles.First(); 
                // item = tiles.Single(t => t.id == item.id);
                Debug.Log("Item: " + item.number.ToString());
                GameObject instantiate = Instantiate(tileRenderer.gameObject);
                TileRenderer tileRenderers = instantiate.GetComponent<TileRenderer>();
                tileRenderers.tile = item;
                NetworkServer.Spawn(instantiate);
                sl.Add(instantiate.GetComponent<TileRenderer>());
                tiles.Remove(item);
            }

            // player.GiveSyncListTile(sl);
            // GetDeals(player);
            RpcPlayerTile(sl,player);
        }
        
        public void GetDeals(Player player)
        {
            // var result = Enumerable.Range(0, 24).OrderBy(g => Guid.NewGuid()).Take(15).ToArray();
            
            int i = 0;
            foreach (var tile in player.tiles)
            {
                // player.playerHand.points[i].GetComponent<PointController>().DropTile(tile);
                TileRenderer instantiate = Instantiate(tileRenderer);
                // instantiate.transform.SetParent(player.playerHand.points[result[i]].transform,false);
                tileRenderer.tile = tile;
                tileRenderer.Render();
                NetworkServer.Spawn(instantiate.gameObject);
                i++;
            }

        }

        [ClientRpc]
        private void RpcPlayerTile(List<TileRenderer> tileRenderers, Player player)
        {
            player.GiveSyncListTile(tileRenderers);
            player.GetDeals();
        }

        [Command(requiresAuthority = false)]
        public void RemoveTiles(Tile removeTile)
        {
            RemovesTiles(removeTile);
        }

        
        [ServerCallback]
        private void RemovesTiles(Tile removeTile)
        {
            var item = tiles.Find(t => t.id == removeTile.id);
            tiles.Remove(item);
        }

        public void GiveTile(SyncList<Tile> syncListTile)
        {
            foreach (var item in syncListTile)
            {
                tiles.Add(item);
                tiless.Add(item);
            }
        }

        [Command(requiresAuthority = false)]
        public void PullForTile()
        {
            GameObject instantiate = Instantiate(tileRenderer.gameObject);
            TileRenderer tileRenderers = instantiate.GetComponent<TileRenderer>();
            // var item = tiles.First();
            tileRenderers.tile = new Tile(Random.Range(0,2543),0,Tile.TileColor.Black,false,false);
            // tileRenderers.Render();
            NetworkServer.Spawn(instantiate);
            RpcPullForTile(instantiate.GetComponent<TileRenderer>());
            // TileRenderer tr = Instantiate(tileRenderer, spawnTile.transform, false);
            // // NetworkServer.Spawn(tr);
            // // sr.stone = new Stone(0, Stone.StoneColor.Red, false);
            // tr.tile = new Tile(0,0,Tile.TileColor.Black,false,false);
            // tr.Render();
            
            // player.tiles.Add(item);
            // tr.transform.SetParent(spawnTile,false);
            
            Debug.Log(tiles.Count.ToString() +" - "+ tiles.ToString());
            // RemovesTiles(item);
        }

        [ClientRpc]
        void RpcPullForTile(TileRenderer tileRenderer)
        {
            tileRenderer.transform.SetParent(spawnTile,false);
            tileRenderer.Render();
        }
        
    }
}