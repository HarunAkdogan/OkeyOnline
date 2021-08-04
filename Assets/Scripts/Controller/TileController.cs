using System;
using Mirror;
using Model;
using UnityEngine;
using UnityEngine.EventSystems;
using View.Renderer;

namespace Controller
{
    [Serializable]
    public class TileController : NetworkBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public Tile tile;
        public TileRenderer tileRenderer;
        public Transform parentToReturnTo = null;

        [HideInInspector]
        public DragController dragController;

        [HideInInspector]
        public SeriesController seriesController;

        public void Start()
        {
            dragController = FindObjectOfType<DragController>();
            seriesController = FindObjectOfType<SeriesController>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            Debug.Log("OnBeginDrag");

            parentToReturnTo = this.transform.parent;
            transform.SetParent(dragController.points[24].transform, false);
            //this.transform.SetParent(this.transform.parent.parent,true);
            GetComponent<CanvasGroup>().blocksRaycasts = false;

            dragController.tileController = this;
            dragController.isDragging = true;

        }
        
        public void OnDrag(PointerEventData eventData)
        {
            transform.position = eventData.position;
            transform.localScale = Vector3.one * 1.1f;

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