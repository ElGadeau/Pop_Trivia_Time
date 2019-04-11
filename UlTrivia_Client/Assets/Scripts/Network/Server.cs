using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Networking;

public class Server : MonoBehaviour
{
    private const int MAX_USER  = 4;
    private const int PORT      = 26000;
    private const int WEB_PORT  = 26001;
    private const int BYTE_SIZE = 1024;

    private byte m_reliableChannel;
    private int channelId; // Which lane was it sent from
    private byte m_error;

    private int m_hostId;

    private bool m_isStarted;

    //test to save answers
    public class answers
    {
        public string answersText = "default";
        public int idUser;
    }
    public static List<answers> m_answerList;
    //end test to save answers
    
    //test to save vote
    public class vote
    {
        public int voteValue;
        public int idUser;
    }
    public static List<vote> m_voteList;
    //end test to save vote

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        
        m_voteList = new List<vote>();
        m_answerList = new List<answers>();
        for (int i = 0; i < 5; i++)
        {
            m_voteList.Add(new vote());
            m_answerList.Add(new answers());
        }
        
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

        int recHostId;    // Web ? Or standalone
        int connectionId; // Which user is sending ?

        byte[] recBuffer = new byte[BYTE_SIZE];
        int    dataSize;

        NetworkEventType type = NetworkTransport.Receive(out recHostId, out connectionId, out channelId, recBuffer,
                BYTE_SIZE, out dataSize, out m_error);

        switch (type)
        {
            case NetworkEventType.Nothing:
                break;

            case NetworkEventType.ConnectEvent:
                Debug.Log(string.Format("User {0} has connected!", connectionId));
                break;

            case NetworkEventType.DisconnectEvent:
                Debug.Log(string.Format("User {0} has disconnected.", connectionId));
                break;

            case NetworkEventType.DataEvent:
                BinaryFormatter formatter = new BinaryFormatter();
                MemoryStream    ms        = new MemoryStream(recBuffer);
                NetMsg          msg       = (NetMsg) formatter.Deserialize(ms);

                OnData(connectionId, channelId, recHostId, msg);
                break;

            case NetworkEventType.BroadcastEvent:
                Debug.Log("Unexpected network event type");
                break;
        }
    }

#region OnData
    private void OnData(int connectionId, int channelId, int recHostId, NetMsg msg)
    {
        //Debug.Log("Received a message of type"  + msg.OP);
        switch (msg.OP)
        {
            case NetOP.None:
                Debug.Log("Unhandled NetOP request");
                break;

            case NetOP.CreateAccount:
                CreateAccount(connectionId, channelId, recHostId, (Net_CreateAccount) msg);
                break;

            case NetOP.SelectChara:
                SelectCharacter(connectionId, channelId, recHostId, (Net_CharacterSelection) msg);
                break;
            
            case NetOP.SendText:
                SendText(connectionId, channelId, recHostId, (Net_SendText) msg);
                break;
            
            case NetOP.SendVote:
                SendVote(connectionId, channelId, recHostId, (Net_SendVote) msg);
                break;
        }
    }

    private void CreateAccount(int connectionId, int channelId, int recHostId, Net_CreateAccount ca)
    {
        Debug.Log(string.Format("{0},{1}, {2}", ca.Username, ca.Password, ca.Email));
    }

    private void SelectCharacter(int connectionId, int channelId, int recHostId, Net_CharacterSelection cs)
    {
        Debug.Log(string.Format("{0}, is selected by {1}", cs.Name, connectionId));
        SendClients(cs);
    }

    private void SendText(int connectionId, int channelId, int recHostId, Net_SendText st)
    {
        m_answerList[connectionId].answersText = st.Text;
        m_answerList[connectionId].idUser = connectionId;
    }

    private void SendVote(int connectionId, int channelId, int recHostId, Net_SendVote sv)
    {
        m_voteList[connectionId].voteValue = sv.Vote;
        m_voteList[connectionId].idUser = connectionId;
    }
#endregion

    public void Init()
    {
        // Needs to be the same on the client !
        NetworkTransport.Init();

        ConnectionConfig cc = new ConnectionConfig();
        m_reliableChannel = cc.AddChannel(QosType.Reliable);

        HostTopology topo = new HostTopology(cc, MAX_USER);

        // Server only code
        m_hostId    = NetworkTransport.AddHost(topo, PORT, null);

        Debug.Log(string.Format("Opening connection on port {0} and webport {1}", PORT, WEB_PORT));
        m_isStarted = true;
        
        
    }

    public void Shutdown()
    {
        m_isStarted = false;
        NetworkTransport.Shutdown();
    }

#region Send
    //every class depending on NetMsg will work here as parameter
    public void SendClient(int p_connectionId, NetMsg msg)
    {
        // Sending buffer and receive buffer need to hold same size !
        byte[] buffer = new byte[BYTE_SIZE];

        BinaryFormatter formatter = new BinaryFormatter();
        MemoryStream    ms        = new MemoryStream(buffer);
        formatter.Serialize(ms, msg);

        NetworkTransport.Send(m_hostId, p_connectionId, m_reliableChannel, buffer, BYTE_SIZE, out m_error);
    }

    public void SendClients(NetMsg msg)
    {
        byte[] buffer = new byte[BYTE_SIZE];

        BinaryFormatter formatter = new BinaryFormatter();
        MemoryStream    ms        = new MemoryStream(buffer);
        formatter.Serialize(ms, msg);
        
        NetworkTransport.StartSendMulticast(m_hostId, channelId, buffer, BYTE_SIZE, out m_error);
    }
#endregion
}