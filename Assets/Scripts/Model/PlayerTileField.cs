using UnityEngine;
using UnityEngine.EventSystems;
using View.Renderer;

namespace Model
{
    public class PlayerTileField : MonoBehaviour,IDropHandler
    {
        public Transform content;

        public void OnDrop(PointerEventData eventData)
        {
            TileRenderer tileRenderer = eventData.pointerDrag.GetComponent<TileRenderer>();
            Player player = Player.localPlayer;
            
        }
    }
}