using System.Collections.Generic;
using Mirror;
using UnityEngine;

namespace Model
{
    public class Table : MonoBehaviour
    {
        
        public Stack<Tile> tiles = new Stack<Tile>(); // Ilk giren Son Cikar!
        
        public List<Tile> tiless = new List<Tile>();
    }
}