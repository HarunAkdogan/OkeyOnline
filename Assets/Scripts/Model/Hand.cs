using System;
using Mirror;
using Unity.VisualScripting;
using UnityEngine;

namespace Model
{
    public class Hand : NetworkBehaviour
    {
        public Point[] points;
        private void Awake()
        {
            points = new Point[25];
            
            for (int i = 0; i < GetComponentsInChildren<Point>().Length; i++)
            {
                GetComponentsInChildren<Point>()[i].id = i + 1;
                points[i] = GetComponentsInChildren<Point>()[i];
            }
        }
    }
}