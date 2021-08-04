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

        private int emptyIndex;
        private int offset;

        public bool isDragging = false;

        private void Start()
        {
            //handController = FindObjectOfType<HandController>();
            //points = handController.hand.points;
            //points = Model.Player.localPlayer.playerHand.points;
        }


        private void Update()
        {
            //Taş sürükleme durumu yokken pozisyonları yumuşakça sıfırla.
            if (!isDragging)
            {
                foreach (Point p in points)
                {
                    if (p.transform.childCount > 0 && (p.transform.GetChild(0).localPosition.x > 0.01f || p.transform.GetChild(0).localPosition.x < -0.01f))
                    {
                        p.transform.GetChild(0).localPosition = Vector3.Lerp(p.transform.GetChild(0).localPosition, Vector3.zero, Time.deltaTime * 3);
                    }
                }
            }

        }

        //Kaydırmayı kontrol et.
        public void ControlCase(Point _point, Vector3 _mousePos)
        {
            point = _point;
            mousePos = _mousePos;
            countChild = point.transform.childCount;


            if (tileController != null && countChild == 1 && isDragging)
            {
                if (point.transform.GetChild(0).GetComponent<TileController>() != tileController)
                {

                    if (mousePos.x >= point.transform.position.x) //İmleç, taşın sağındaysa sola kaydır.
                    {
                        ShiftLeft();
                    }
                    else if (mousePos.x < point.transform.position.x) //İmleç, taşın solundaysa sağa kaydır.
                    {
                        ShiftRight();
                    }

                }

            }

        }

        //Sola kaydır.
        public void ShiftLeft()
        {
            if (isDragging)
                ResetPositions();

            movingPoints.Clear();
            emptyIndex = -1;
            offset = point.id < 13 ? 0 : 12; //Istakanın üst satırı mı alt satırı mı?

            for (int i = point.id - 1; i >= offset; i--)
            {
                if (points[i].transform.childCount == 0 || points[i] == tileController.transform.parent.GetComponent<Point>())
                {
                    emptyIndex = i;
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

        //Sağa kaydır.
        public void ShiftRight()
        {
            if (isDragging)
                ResetPositions();

            movingPoints.Clear();
            emptyIndex = -1;
            offset = point.id < 13 ? 0 : 12; //Istakanın üst satırı mı alt satırı mı?

            for (int i = point.id - 1; i <= offset + 11; i++)
            {
                if (points[i].transform.childCount == 0 || points[i] == tileController.transform.parent.GetComponent<Point>())
                {
                    emptyIndex = i;
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

        }

        //Kaydırmayı uygula.
        public void ConfirmShift()
        {

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

            }


        }


        public void ResetPositions()
        {

            foreach (Point p in movingPoints)
            {
                if (p.transform.childCount > 0)
                    p.transform.GetChild(0).localPosition = Vector3.zero;
            }
            movingPoints.Clear();

        }
    }
}