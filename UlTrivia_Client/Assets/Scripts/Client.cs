using UnityEngine;
using UnityEngine.Networking;

public class Client : MonoBehaviour
{
    private const int MAX_USER = 4;
    private const int PORT = 26000;
    private const int WEB_PORT = 26001;
    private const string SERVER_IP = "127.0.0.1";

    private int m_hostId;

    private bool m_isStarted;

    private byte m_reliableChannel;
    private byte m_error;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        Init();
    }

    public void Init()
    {
        // Needs to be the same on the server !
        NetworkTransport.Init();

        ConnectionConfig cc = new ConnectionConfig();
        m_reliableChannel = cc.AddChannel(QosType.Reliable);

        HostTopology topo = new HostTopology(cc, MAX_USER);

        // Client only code
        m_hostId = NetworkTransport.AddHost(topo, 0);

#if UNITY_WEBGL && !UNITY_EDITOR
		// Web Client
		NetworkTransport.Connect(m_hostId, SERVER_IP, WEB_PORT, 0, out m_error);
        Debug.Log("Connection from Web");
#else
        // Standalone Client
        NetworkTransport.Connect(m_hostId, SERVER_IP, PORT, 0, out m_error);
        Debug.Log("Connection from standalone");
#endif

        Debug.Log(string.Format("Attempting to connect on {0}...", SERVER_IP));
        m_isStarted = true;
    }

    public void Shutdown()
    {
        m_isStarted = false;
        NetworkTransport.Shutdown();
    }
}