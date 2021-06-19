using System.Collections;
using UnityEngine;
using Mirror;
using Model;
using View.Renderer;

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


            if ((row == 0 && col1 > 11) || (row == 1 && col1 > 23))
            {
                return sCount;

            }


            Point point = points[col0];
            Point nextPoint = points[col1];


            if (point.transform.childCount == 0)
            {
                return CheckNext(0, row, col0 + 1, col1 + 1);
            }

            Tile tile = point.transform.GetChild(0).GetComponent<TileRenderer>().tile;

            if (nextPoint.transform.childCount == 0)
            {
                col0 = col1;


                if (!doubledPer && sCount < 2 || doubledPer && sCount != 1 || doubledPer && !tile.isJoker || (!sameColorPer && !sameNumberPer && !doubledPer))
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

            Tile nextTile = nextPoint.transform.GetChild(0).GetComponent<TileRenderer>().tile;

            if (tile.isJoker && nextTile.isJoker)
                return -1;

            if (tile.isJoker)
            {
                return CheckNext(sCount + 1, row, col0 + 1, col1 + 1);
            }

            if (nextTile.isJoker)
            {
                return CheckNext(sCount + 1, row, col0, col1 + 1);
            }



            if (tile.number == nextTile.number && tile.color == nextTile.color && !sameColorPer && !sameNumberPer)
            {
                col0 = col1;
                doubledPer = true;
                return CheckNext(1, row, col0, col1 + 1);

            }
            else if (((nextTile.number == 1 && tile.number == 13) || (nextTile.number == tile.number + 1)) && nextTile.color == tile.color)
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
            else if (nextTile.number == tile.number && nextTile.color != tile.color)
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