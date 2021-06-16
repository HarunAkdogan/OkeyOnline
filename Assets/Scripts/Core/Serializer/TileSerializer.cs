using Mirror;
using Model;

namespace Core.Serializer
{
    public static class TileSerializer
    {
        public static byte TILE = 1;
        
        public static void WriteItem(this NetworkWriter writer, Tile tile)
        {
            writer.WriteInt32(tile.id);
            writer.WriteInt32(tile.number);
            writer.Write(tile.color);
            writer.WriteBoolean(tile.isJoker);
            writer.WriteBoolean(tile.isFalseJoker);
        }

        public static Tile ReadeItem(this NetworkReader reader)
        {
            return new Tile(reader.ReadInt32(), reader.ReadInt32(), reader.ReadeItem().color, reader.ReadBoolean(),
                reader.ReadBoolean());

        }

    }
}