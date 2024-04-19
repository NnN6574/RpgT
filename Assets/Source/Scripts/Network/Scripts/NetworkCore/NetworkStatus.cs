using Mirror;

namespace Network.Scripts.NetworkCore
{
    public class NetworkStatus
    {
        public TypeConnection TypeConnection { get; private set; }

        public bool IsReady { get; private set; }

        public void SetHost() => TypeConnection = TypeConnection.Host;
        public void SetClient() => TypeConnection = TypeConnection.Client;
        public bool IsCaseServer => TypeConnection == TypeConnection.Host;
        public static bool IsServer =>NetworkClient.localPlayer != null && NetworkClient.localPlayer.isServer;  

        
        public void Ready()
        {
            if (NetworkClient.ready)
            {
                if (!IsReady)
                {
                    IsReady = true;
                }
                
                return;
            }

            IsReady = NetworkClient.Ready();
        }
        
        public virtual void Dispose()
        {
            IsReady = false;
        }

    }
}