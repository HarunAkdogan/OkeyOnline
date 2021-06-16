using System;
using Mirror;
using Model;

namespace Model
{
    [Serializable]
    public class Tile
    {
        public int id;
        public int number;
        public TileColor color;
        public bool isJoker;
        public bool isFalseJoker;
        
        public Tile(int id, int Number, TileColor Color,bool IsJoker, bool IsFalseJoker)
        {
            this.id = id;
            this.number = Number;
            this.color = Color;
            this.isJoker = IsJoker;
            this.isFalseJoker = IsFalseJoker;
        }

        

        public enum TileColor: byte
        {
            Red,
            Blue,
            Green,
            Black
        }

        public static TileColor IntToTileColor(int i)
        {
            switch (i)
            {
                case 0: return TileColor.Black;
                case 1: return TileColor.Blue;
                case 2: return TileColor.Green;
                case 3: return TileColor.Red;
                default:
                    return TileColor.Black;
            }
        }
    }
}

public class SyncListTile : SyncList<Tile>
{
   
};

