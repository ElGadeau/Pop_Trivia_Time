using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Networking;

public class Client : MonoBehaviour
{
    private const int    MAX_USER  = 4;
    private const int    PORT      = 26000;
    private const int    WEB_PORT  = 26001;
    private const string SERVER_IP = "10.5.4.53";
    private const int    BYTE_SIZE = 1024;

    private int m_hostId;
    int         m_connectionId; // Which user is sending ?

    private bool m_isStarted;

    private byte m_reliableChannel;
    private byte m_error;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        Init();
    }

    private void Update()
    {
        UpdateMessagePump();
    }

    public void UpdateMessagePump()
    {
        if (!m_isStarted)
            return;

        int recHostId; // Web ? Or standalone    (we'll use standalone only)
        int channelId; // Which lane was it sent from

        byte[] recBuffer = new byte[BYTE_SIZE];
        int    dataSize;

        NetworkEventType type = NetworkTransport.Receive(out recHostId, out m_connectionId, out channelId, recBuffer,
                BYTE_SIZE, out dataSize, out m_error);

        switch (type)
        {
            case NetworkEventType.Nothing:
                break;

            case NetworkEventType.ConnectEvent:
                Debug.Log("Successful connection to the server");
                break;

            case NetworkEventType.DisconnectEvent:
                Debug.Log("Connection Lost");
                break;

            case NetworkEventType.DataEvent:
                Debug.Log("Data");
                break;

            case NetworkEventType.BroadcastEvent:
                Debug.Log("Unexpected network event type");
                break;
        }
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

        NetworkTransport.Connect(m_hostId, SERVER_IP, PORT, 0, out m_error);
        Debug.Log("Connection from standalone");

        Debug.Log(string.Format("Attempting to connect on {0}...", SERVER_IP));
        m_isStarted = true;
    }

    public void Shutdown()
    {
        m_isStarted = false;
        NetworkTransport.Shutdown();
    }

#region Send
    //every class depending on NetMsg will work here as parameter
    public void SendServer(NetMsg msg)
    {
        // Sending buffer and receive buffer need to hold same size !
        byte[] buffer = new byte[BYTE_SIZE];

        buffer[0] = 255;
        BinaryFormatter formatter = new BinaryFormatter();
        MemoryStream ms = new MemoryStream(buffer);
        formatter.Serialize(ms, msg);
        
        NetworkTransport.Send(m_hostId, m_connectionId, m_reliableChannel, buffer, BYTE_SIZE, out m_error);
    }
#endregion
}