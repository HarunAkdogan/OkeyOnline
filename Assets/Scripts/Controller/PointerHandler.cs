using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using View.Renderer;
using System.Linq;
using Model;

namespace Controller
{
    public class PointerHandler : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerMoveHandler, IEndDragHandler , IBeginDragHandler, IDragHandler
{

    public Point point;

    public Table table;

    public Tile tile;
    public TileRenderer tileRenderer;
    public Transform parentToReturnTo = null;

        int countChild;

    [HideInInspector]
    public DragController dragController;

    [HideInInspector]
    public SeriesController seriesController;

    private void Start()
    {
        point = GetComponent<Point>();
        table = FindObjectOfType<Table>();
        dragController = FindObjectOfType<DragController>();
        seriesController = FindObjectOfType<SeriesController>();
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
                        player.playerField.CmdPlayerTableTileDrop(tileController, item);
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



        public void OnBeginDrag(PointerEventData eventData)
        {
            Debug.Log("OnBeginDrag");

            parentToReturnTo = this.transform.parent;
            transform.SetParent(dragController.points[24].transform, false);
            //this.transform.SetParent(this.transform.parent.parent,true);
            GetComponent<CanvasGroup>().blocksRaycasts = false;

            //dragController.tileController = this;
            dragController.isDragging = true;

        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = eventData.position;
            transform.localScale = Vector3.one * 1.1f;
            dragController.isDragging = false;

        }

        public void OnEndDrag(PointerEventData eventData)
        {
            Model.Player player = Model.Player.localPlayer;
            Debug.Log("OnEndDrag");
            this.transform.SetParent(parentToReturnTo, false);
            transform.localScale = Vector3.one;
            this.transform.localPosition = Vector3.zero;

            GetComponent<CanvasGroup>().blocksRaycasts = true;

            if (seriesController.CheckSeries())
                Debug.Log("Congratulations!");
        }

    }
}
