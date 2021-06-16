using System;
using Model;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Controller
{
    public class PointController : MonoBehaviour, IDropHandler, IPointerEnterHandler,IPointerMoveHandler, IPointerExitHandler, IEndDragHandler
    {
        
        public Point point;
        
        [HideInInspector]
        public TableController tableController;

        [HideInInspector]
        public DragController dragController;


        int countChild;
        private void Start()
        {
            point = GetComponent<Point>();
            tableController = FindObjectOfType<TableController>();
            dragController = FindObjectOfType<DragController>();
        }

        public void DropTile(Tile _tile)
        {
            point.tile = _tile;
        }

        public void OnDrop(PointerEventData eventData)
        {
            TileController tileController = eventData.pointerDrag.GetComponent<TileController>();

            if (tileController != null)
            {
                if (countChild ==0)
                {
                    if (tileController.tileRenderer.tile.number == 0)
                    {
                        tileController.tileRenderer.tile = tableController.PullForTile();
                        tileController.tileRenderer.Render();
                    } 
                    
                    tileController.parentToReturnTo = this.transform;
                    DropTile(tileController.tileRenderer.tile);
                }
                else
                {
                    dragController.ConfirmShift();
                }


            }

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

        public void OnPointerExit(PointerEventData eventData)
        {
            dragController.ResetPositions();
        }

        
        public void OnEndDrag(PointerEventData eventData)
        {
            dragController.ResetPositions();
        }
    }
}