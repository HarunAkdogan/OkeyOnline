using Controller;
using Mirror;
using Model;
using View.Renderer;

namespace Core.Serializer
{
    public static class TileRendererSerializer
    {
        public static void WriteItem(this NetworkWriter writer, TileRenderer tile)
        {
            NetworkIdentity networkIdentity = tile.GetComponent<NetworkIdentity>();
            writer.WriteNetworkIdentity(networkIdentity);

        }

        public static TileRenderer ReadeItem(this NetworkReader reader)
        {
            NetworkIdentity networkIdentity = reader.ReadNetworkIdentity();
            
            TileRenderer tileRenderer = networkIdentity != null
                ? networkIdentity.GetComponent<TileRenderer>()
                : null;
            return tileRenderer;

        }
    }
}