using System.Collections;
using UnityEngine;
using Model;
using Mirror;

namespace Controller
{
    public class DragController : NetworkBehaviour
    {

        [HideInInspector]
        public HandController handController;

        [HideInInspector]
        public TileController tileController;

        [HideInInspector]
        public Point[] points;

        private int countChild;
        private Point point;
        private Vector3 mousePos;

        private int emptyIndex;
        private int offset;

        public void Start()
        {
            handController = FindObjectOfType<HandController>();
            points = handController.hand.points;
        }


        public void ControlCase(Point _point, Vector3 _mousePos)
        {
            point = _point;
            mousePos = _mousePos;
            countChild = point.transform.childCount;


            if (tileController != null && countChild == 1)
            {
                if (point.transform.GetChild(0).GetComponent<TileController>() != tileController)
                {
                    if (mousePos.x > point.transform.position.x + 3)
                    {
                        ShiftLeft();
                    }
                    else if (mousePos.x < point.transform.position.x - 3)
                    {
                        ShiftRight();
                    }
                    else
                    {
                        ResetPositions();
                    }

                }

            }

        }

        public void Swap() { }

        public void ShiftLeft()
        {
            emptyIndex = -1;
            offset = point.id < 13 ? 0 : 12;

            for (int i = point.id-1; i >= offset; i--)
            {
                if (points[i].transform.childCount == 0  || points[i] == tileController.transform.parent.GetComponent<Point>())
                {
                    emptyIndex = i;
                    Debug.Log("Solda boş yer..." + emptyIndex);
                    break;
                }
            }

            if (emptyIndex > -1)
            {
                for (int i = emptyIndex; i < point.id; i++)
                {
                    if (point.transform.GetChild(0).GetComponent<TileController>() != tileController)
                    {
                        if (points[i].transform.childCount > 0)
                        {
                            points[i].transform.GetChild(0).localPosition = new Vector3(-20, 0, 0);
                        }
                    }
                }
            }


        }
        public void ShiftRight()
        {
            emptyIndex = -1;
            offset = point.id < 13 ? 0 : 12;

            for (int i = point.id- 1; i <= offset + 11; i++)
            {
                if (points[i].transform.childCount == 0 || points[i] == tileController.transform.parent.GetComponent<Point>())
                {
                    emptyIndex = i;
                    Debug.Log("Sağda boş yer.." + emptyIndex);
                    break;
                }
            }

            if (emptyIndex > -1)
            {
                for (int i = emptyIndex; i > point.id - 2; i--)
                {
                    if (point.transform.GetChild(0).GetComponent<TileController>() != tileController)
                    {
                        if (points[i].transform.childCount > 0)
                        {
                            points[i].transform.GetChild(0).localPosition = new Vector3(20, 0, 0);
                        }
                    }
                }
            }

            /*
            foreach (Point p in points)
            {
                if (point.transform.GetChild(0).GetComponent<TileController>() != tileController)
                {
                    if (p.transform.childCount > 0)
                    {
                        if ((((p.id >= 1 && p.id <= 12) && (point.id >= 1 && point.id <= 12)) || ((p.id >= 13 && p.id <= 24) && (point.id >= 13 && point.id <= 24))) && p.id >= point.id)
                            p.transform.GetChild(0).localPosition = new Vector3(20, 0, 0);
                    }
                }
            }
            */
        }

        public void ConfirmShift()
        {
            if (emptyIndex > -1)
            {
                if (emptyIndex < point.id - 1)
                {
                    for (int i= emptyIndex; i<point.id - 1; i++)
                    {
                        if(points[i + 1].transform.childCount > 0)
                         points[i + 1].transform.GetChild(0).SetParent(points[i].transform);
                    }
                }
                else if(emptyIndex > point.id - 1)
                {
                    for (int i = emptyIndex; i > point.id - 1; i--)
                    {
                        if (points[i - 1].transform.childCount > 0)
                            points[i - 1].transform.GetChild(0).SetParent(points[i].transform);
                    }
                }
               
                tileController.parentToReturnTo = point.transform;
                
            }

            ResetPositions();

        }


        public void ResetPositions()
        {
            emptyIndex = -1;
            foreach (Point p in points)
            {
                if (point.transform.childCount > 0 && point.transform.GetChild(0).GetComponent<TileController>() != tileController)
                {
                    if (p.transform.childCount > 0)
                    {
                        p.transform.GetChild(0).localPosition = Vector3.zero;
                    }
                }
            }

        }


    }
}