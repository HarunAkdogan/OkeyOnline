using System.Collections.Generic;
using Controller;
using UnityEngine;
using UnityEngine.EventSystems;
using View.Renderer;

namespace Model
{
    public class PlayerTileField : MonoBehaviour,IDropHandler
    {
        public Transform content;

        public List<Tile> tiles = new List<Tile>();
        
        public void DropTile(Tile _tile)
        {
            tiles.Add(_tile);
        }
        
        public void OnDrop(PointerEventData eventData)
        {
            TileController tileController = eventData.pointerDrag.GetComponent<TileController>();
            Player player = Player.localPlayer;
            tileController.parentToReturnTo = content.transform;
            player.playerField.CmdPlayerOnDrop(tileController.tileRenderer.tile,tileController);
            // player.RemoveTile(tileController.tileRenderer.tile);
            Debug.Log("Ondrop" + player.tiles.Count);
        }
    }
}