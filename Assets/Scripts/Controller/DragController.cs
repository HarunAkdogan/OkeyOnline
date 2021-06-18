using System.Collections;
using System.Collections.Generic;
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
        private List<Point> movingPoints = new List<Point>();

        private int countChild;
        private Point point;
        private Vector3 mousePos;

        private int emptyIndex, confirmedIndex;
        private int offset;

        public bool isDragging = false;

        private void Start()
        {
            handController = FindObjectOfType<HandController>();
            points = handController.hand.points;
        }

        /*
        public IEnumerator ResetPosSmooth()
        {
            isShifting = true;
            float elapsedTime = 0;
            float waitTime = 0.5f;

            Vector3 currentPos = confirmedIndex > point.id ? new Vector3(-20, 0, 0) : new Vector3(20, 0, 0);

            if (point.id - 1 > confirmedIndex)
            {
                while (elapsedTime < waitTime)
                {

                    for (int i = confirmedIndex; i < point.id - 1; i++)
                    {
                        if (point != null && point.transform.childCount > 0 && points[i] != tileController.transform.parent.GetComponent<Point>())
                        {
                            if (points[i].transform.childCount > 0)
                            {
                                points[i].transform.GetChild(0).localPosition = Vector3.Lerp(currentPos, Vector3.zero, (elapsedTime / waitTime));

                            }
                        }

                    }

                    elapsedTime += Time.deltaTime;
                    yield return null;

                }
            }
            else if (point.id - 1 < confirmedIndex)
            {
                while (elapsedTime < waitTime)
                {
                    for (int i = confirmedIndex; i > point.id - 1; i--)
                    {

                        if (point != null && point.transform.childCount > 0 && points[i] != tileController.transform.parent.GetComponent<Point>())
                        {
                            if (points[i].transform.childCount > 0)
                            {

                                points[i].transform.GetChild(0).localPosition = Vector3.Lerp(currentPos, Vector3.zero, (elapsedTime / waitTime));


                            }
                        }

                    }
                    elapsedTime += Time.deltaTime;
                    yield return null;

                }
            }

            /*
            while (elapsedTime < waitTime)
            {
                foreach (Point p in points)
                {
                    if (point != null && point.transform.childCount > 0 && p != tileController.transform.parent.GetComponent<Point>())
                    {
                        if (p.transform.childCount > 0)
                        {

                            p.transform.GetChild(0).localPosition = Vector3.Lerp(currentPos, Vector3.zero, (elapsedTime / waitTime));


                        }
                    }
                }

                elapsedTime += Time.deltaTime;
                yield return null;
            }
            */
            /*
        foreach (Point p in points)
        {
            if (point != null && point.transform.childCount > 0 && p != tileController.transform.parent.GetComponent<Point>())
            {
                if (p.transform.childCount > 0)
                {
                    p.transform.GetChild(0).localPosition = Vector3.zero;

                }
            }
        }
            */
            /*
            emptyIndex = -1;
            confirmedIndex = -1;
            ResetPositions();

            yield return new WaitForSeconds(0.1f);
            isShifting = false;
            yield return null;
        }
    */
        private void FixedUpdate()
        {
            
            if (!isDragging) {

               // Debug.Log("Not Dragging.");
                foreach (Point p in points)
                {
                        if (p.transform.childCount > 0)
                        {
                            p.transform.GetChild(0).localPosition = Vector3.Lerp(p.transform.GetChild(0).localPosition, Vector3.zero, Time.deltaTime * 3);
                        }
                    
                }
            }
            
        }

        public void ControlCase(Point _point, Vector3 _mousePos)
        {
            point = _point;
            mousePos = _mousePos;
            countChild = point.transform.childCount;


            if (tileController != null && countChild == 1 && isDragging)
            {
                if (point.transform.GetChild(0).GetComponent<TileController>() != tileController)
                {
                    if (mousePos.x >= point.transform.position.x)
                    {
                        ShiftLeft();
                    }
                    else if (mousePos.x < point.transform.position.x)
                    {
                        ShiftRight();
                    }

                }

            }

        }

        public void Swap() { }

        public void ShiftLeft()
        {
            movingPoints.Clear();
            emptyIndex = -1;
            offset = point.id < 13 ? 0 : 12;

            for (int i = point.id - 1; i >= offset; i--)
            {
                if (points[i].transform.childCount == 0 || points[i] == tileController.transform.parent.GetComponent<Point>())
                {
                    emptyIndex = i;
                    //Debug.Log("Solda boş yer..." + emptyIndex);
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
                            movingPoints.Add(points[i]);
                            points[i].transform.GetChild(0).localPosition = new Vector3(-20, 0, 0);
                        }
                    }
                }
            }


        }
        public void ShiftRight()
        {
            movingPoints.Clear();
            emptyIndex = -1;
            offset = point.id < 13 ? 0 : 12;

            for (int i = point.id - 1; i <= offset + 11; i++)
            {
                if (points[i].transform.childCount == 0 || points[i] == tileController.transform.parent.GetComponent<Point>())
                {
                    emptyIndex = i;
                    //Debug.Log("Sağda boş yer.." + emptyIndex);
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
                            movingPoints.Add(points[i]);
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
            ResetPositions();

            if (emptyIndex > -1)
            {
                if (emptyIndex < point.id - 1) //ShiftLeft
                {
                    for (int i = emptyIndex; i < point.id - 1; i++)
                    {
                        if (points[i + 1].transform.childCount > 0)
                        {
                            points[i + 1].transform.GetChild(0).SetParent(points[i].transform);
                            points[i].GetComponent<PointController>().DropTile(points[i + 1].tile);
                        }
                    }
                }
                else if (emptyIndex > point.id - 1) // ShiftRight
                {
                    for (int i = emptyIndex; i > point.id - 1; i--)
                    {
                        if (points[i - 1].transform.childCount > 0)
                        {
                            points[i - 1].transform.GetChild(0).SetParent(points[i].transform);
                            points[i].GetComponent<PointController>().DropTile(points[i - 1].tile);
                        }
                    }
                }

                point.GetComponent<PointController>().DropTile(tileController.tile);
                tileController.parentToReturnTo = point.transform;

                confirmedIndex = emptyIndex;

               
            }


            

        }


        public void ResetPositions()
        {
            
                foreach (Point p in movingPoints)
                {
                    if(p.transform.childCount > 0)
                    p.transform.GetChild(0).localPosition = Vector3.zero;
                }
                movingPoints.Clear();
            
        }


    }
}