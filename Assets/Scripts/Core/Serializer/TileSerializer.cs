using Mirror;
using Model;
using UnityEngine;

namespace Core.Serializer
{
    public static class TileSerializer
    {
        public static byte TILE = 1;
        
        public static void WriteItem(this NetworkWriter writer, Tile tile)
        {
            writer.WriteInt32(tile.id);
            writer.WriteInt32(tile.number);
            writer.WriteByte((byte)tile.color);
            writer.WriteBoolean(tile.isJoker);
            writer.WriteBoolean(tile.isFalseJoker);
        }

        public static Tile ReadeItem(this NetworkReader reader)
        {
            
            return new Tile(reader.ReadInt32(), reader.ReadInt32(), (Tile.TileColor)reader.ReadByte(), reader.ReadBoolean(),
                reader.ReadBoolean());

        }
        
    }
}