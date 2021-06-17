using System.Collections;
using UnityEngine;
using Mirror;
using Model;

namespace Controller
{
    public class SeriesController : NetworkBehaviour
    {
        [HideInInspector]
        public HandController handController;

        [HideInInspector]
        public Point[] points;

        private bool doubledPer = true;
        private bool sameColorPer = false;
        private bool sameNumberPer = false;


        private void Start()
        {
            handController = FindObjectOfType<HandController>();
            points = handController.hand.points;

        }
        public bool CheckSeries()
        {
            sameColorPer = false;
            sameNumberPer = false;
            doubledPer = true;

            int checkRow0 = CheckNext(0, 0, 0, 1);

            sameColorPer = false;
            sameNumberPer = false;

            int checkRow1 = CheckNext(0, 1, 12, 13);

            sameColorPer = false;
            sameNumberPer = false;
            doubledPer = true;

            if (checkRow0 > -1 && checkRow1 > -1)
                return true;

            return false;
        }

        public int CheckNext(int seriesCount, int row, int col0, int col1)
        {
            int sCount = seriesCount;

            Debug.Log("Girdim, satır " + row );

            if ((row == 0 && col1 > 11) || (row == 1 && col1 > 23))
            {
                Debug.Log("Taştı.");
                return sCount;

            }
            

            Point point = points[col0];
            Point nextPoint = points[col1];

            Debug.Log("Cocuk0 " + point.transform.childCount);

            Debug.Log("Cocuk1 " + nextPoint.transform.childCount);



            if (point.transform.childCount == 0)
            {
                        Debug.Log("Ben boşum");
                        return CheckNext(0, row, col0 + 1, col1 + 1);
            }

            if (nextPoint.transform.childCount == 0)
            {
                Debug.Log("Nextim boş");
                col0 = col1;


                if (!doubledPer && sCount < 2 || doubledPer && sCount != 1 || doubledPer && !point.tile.isJoker || (!sameColorPer && !sameNumberPer && !doubledPer))
                {
                    return -1;
                }
                else if (!sameNumberPer && !sameColorPer && sCount == 1)
                {

                    return CheckNext(1, row, col0, col1 + 1);
                }
                else
                {
                    sameNumberPer = false;
                    sameColorPer = false;
                    return CheckNext(0, row, col0, col1 + 1);
                }
            }


            if (point.tile.isJoker && nextPoint.tile.isJoker)
                return -1;

            if (point.tile.isJoker)
            {
                return CheckNext(sCount + 1, row, col0 + 1, col1 + 1);
            }

            if (nextPoint.tile.isJoker)
            {
                return CheckNext(sCount + 1, row, col0, col1 + 1);
            }



            if (point.tile.number == nextPoint.tile.number && point.tile.color == nextPoint.tile.color && !sameColorPer && !sameNumberPer)
            {
                col0 = col1;
                doubledPer = true;
                return CheckNext(1, row, col0, col1 + 1);

            }
            else if (((nextPoint.tile.number == 1 && point.tile.number == 13) || (nextPoint.tile.number == point.tile.number + 1)) && nextPoint.tile.color == point.tile.color)
            {
                doubledPer = false;
                col0 = col1;
                if (sameNumberPer && sCount > 0)
                    return -1;
                else
                {
                    sameColorPer = true;
                    return CheckNext(sCount + 1, row, col0, col1 + 1);
                }
            }
            else if (nextPoint.tile.number == point.tile.number && nextPoint.tile.color != point.tile.color)
            {
                doubledPer = false;
                col0 = col1;
                if (sameColorPer && sCount > 0)
                    return -1;
                else
                {
                    sameNumberPer = true;
                    return CheckNext(sCount + 1, row, col0, col1 + 1);
                }
            }

            return -1;
        }



    }
}