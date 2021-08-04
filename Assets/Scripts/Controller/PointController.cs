using System;
using System.Linq;
using Mirror;
using Model;
using UnityEngine;
using UnityEngine.EventSystems;
using View.Renderer;

namespace Controller
{
    public class PointController : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerMoveHandler, IEndDragHandler
    {
        public Point point;
        
        public Table table;

        public Tile tile;
        public TileRenderer tileRenderer;
        public Transform parentToReturnTo = null;

        [HideInInspector]
        public DragController dragController;

        [HideInInspector]
        public SeriesController seriesController;

        int countChild;


        private void Start()
        {
            point = GetComponent<Point>();
            table = FindObjectOfType<Table>();
            dragController = FindObjectOfType<DragController>();

        }

        public void DropTile(Tile _tile)
        {
            point.tile = _tile;
        }
        
        public void OnDrop(PointerEventData eventData)
        {
            TileController tileController = eventData.pointerDrag.GetComponent<TileController>();
            Model.Player player = Model.Player.localPlayer;

            if (player.tiles.Count <= 14)
            {
                if (tileController != null)
                {
                    if (countChild == 0)
                    {
                        if (tileController.tileRenderer.tile.number == 0)
                        {
                            var item = table.tiles.First();
                            
                            // tileController.tileRenderer.tile = item;
                            // tileController.tileRenderer.Render();
                            // tileController.tile = item;
                            // tileController.tileRenderer.Render();
                            table.PullForTile();
                            tileController.tileRenderer.tile = item;
                            tileController.tileRenderer.Render();
                            player.playerField.CmdPlayerTableTileDrop(tileController,item);
                            if (player.isLocalPlayer)
                            {
                                player.AddTile(item);
                            }
                            table.RemoveTiles(item);
                            Debug.Log("Number 0");
                        }
                        else
                        {
                            if (tileController.parentToReturnTo == Model.Player.gameManager.opponentTileField3.content)
                            {
                                player.AddTile(tileController.tileRenderer.tile);
                                DropTile(tileController.tileRenderer.tile);
                                player.playerField.CmdPlayerFieldTileDrop(tileController);
                            }
                           
                        }
                        
                        tileController.parentToReturnTo = this.transform;
                        DropTile(tileController.tileRenderer.tile);
                    }
                    else if (tileController.transform.parent.GetComponent<PointController>() != this) //&& gameManager.players[playerId ??? ].tiles.Contains(tileController.tile)
                    {
                        dragController.ConfirmShift();
                    }


                }
            }
            dragController.isDragging = false;
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            countChild = transform.childCount;
        }

        public void OnPointerMove(PointerEventData eventData)
        {
            countChild = transform.childCount;
            dragController.ControlCase(point, eventData.position);
      
        }

        public void OnEndDrag(PointerEventData eventData)
        {

            dragController.isDragging = false;
        }

    }
}