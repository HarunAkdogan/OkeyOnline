using Model;
using UnityEngine;
using UnityEngine.EventSystems;
using View.Renderer;

namespace Controller
{
    public class TileController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public Tile tile;
        public TileRenderer tileRenderer;
        
        
        public Transform parentToReturnTo = null;

        [HideInInspector]
        public DragController dragController;

        public void Start()
        {
            dragController = FindObjectOfType<DragController>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            Debug.Log("OnBeginDrag");
            parentToReturnTo = this.transform.parent;

            GetComponent<CanvasGroup>().blocksRaycasts = false;
            dragController.tileController = this;
            dragController.isDragging = true;
            //dragController.ResetPositions();


        }
        
        public void OnDrag(PointerEventData eventData)
        {
            
            transform.position = eventData.position;
            
        }
        
        public void OnEndDrag(PointerEventData eventData)
        {
            Debug.Log("OnEndDrag");
            this.transform.SetParent(parentToReturnTo, false);
            this.transform.localPosition = Vector3.zero;
            GetComponent<CanvasGroup>().blocksRaycasts = true;
            dragController.isDragging = false;
        }

    }
}