using Controller;
using Mirror;
using Unity.VisualScripting;

namespace Core.Serializer
{
    public static class TileControllerSerializer
    {
        public static void WriteItem(this NetworkWriter writer, TileController tile)
        {
            NetworkIdentity networkIdentity = tile.GetComponent<NetworkIdentity>();
            writer.WriteNetworkIdentity(networkIdentity);

        }

        public static TileController ReadeItem(this NetworkReader reader)
        {
            NetworkIdentity networkIdentity = reader.ReadNetworkIdentity();
            TileController tileController = networkIdentity != null
                ? networkIdentity.GetComponent<TileController>()
                : null;
            return tileController;

        }
    }
}