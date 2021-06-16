using Model;
using UnityEngine;

namespace View.Helpers
{
    public static class TileColorTypeToColor
    {
        
        public static Color TileColorToColor(Tile.TileColor tileColor)
        {
            switch (tileColor)
            {
                case Tile.TileColor.Black: return Color.black;
                case Tile.TileColor.Blue: return Color.blue;
                case Tile.TileColor.Green: return Color.green;
                case Tile.TileColor.Red: return Color.red;
                default:
                    Debug.LogError("Error in Tile.IntToTileColor(int) input has to be 0-3, but was " + tileColor);
                    return Color.black;
            }
        }
    }
}