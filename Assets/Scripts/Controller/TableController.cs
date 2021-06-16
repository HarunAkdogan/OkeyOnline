using System.Collections.Generic;
using Mirror;
using Model;
using UnityEngine;
using View.Renderer;

namespace Controller
{
    public class TableController : NetworkBehaviour
    {
        public Table table;

        public Transform spawnTile;

        public TileRenderer tileRenderer;
        
        public List<Tile> GetNTile(int N)
        { //TODO: This
            var sl = new List<Tile>();
            for (var i = 0; i < N; i++) sl.Add(table.tiles.Pop());
            return sl;
        }
        
        public void GiveTile(List<Tile> tiles)
        {
            foreach (var item in tiles)
            {
                table.tiles.Push(item);
                table.tiless.Add(item);
            }
        }

        public Tile PullForTile()
        {
            print("Pull");

            TileRenderer tr = Instantiate(tileRenderer, spawnTile.transform, false);
            // NetworkServer.Spawn(tr);
            // sr.stone = new Stone(0, Stone.StoneColor.Red, false);
            tr.tile = new Tile(0,0,Tile.TileColor.Black,false,false);
            tr.Render();
            // tr.transform.SetParent(spawnTile,false);
            return table.tiles.Pop();
        }
    }
}