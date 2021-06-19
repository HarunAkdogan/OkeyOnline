using Model;
using UnityEngine;
using UnityEngine.EventSystems;
using View.Renderer;
using UnityEngine.UI;

namespace Controller
{
    public class TileController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
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

            parentToReturnTo = this.transform.parent;
            transform.SetParent(dragController.points[24].transform, false); 

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
            transform.localScale = Vector3.one;
            this.transform.SetParent(parentToReturnTo, false);
            this.transform.localPosition = Vector3.zero;
            GetComponent<CanvasGroup>().blocksRaycasts = true;
            dragController.isDragging = false;

            if (seriesController.CheckSeries())
                Debug.Log("Congratulations!");
        }

    }
}