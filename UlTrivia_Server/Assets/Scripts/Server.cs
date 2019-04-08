using UnityEngine;
using UnityEngine.Networking;

public class Server : MonoBehaviour
{
	private const int MAX_USER = 4;
	private const int PORT = 26000;
	private const int WEB_PORT = 26001;
	private const int BYTE_SIZE = 1024;	
	
	private byte m_reliableChannel;
	private byte m_error;
	
	private int m_hostId;
	private int m_webHostId;

	private bool m_isStarted;

	private void Start()
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

		int recHostId;		// Web ? Or standalone
		int connectionId;	// Which user is sending ?
		int channelId;		// Which lane was it sent from
		
		byte[] recBuffer = new byte[BYTE_SIZE];
		int dataSize;

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
				Debug.Log("Data");
				break;
			
			case NetworkEventType.BroadcastEvent:
				Debug.Log("Unexpected network event type");
				break;
		}
	}			

	public void Init()
	{
		// Needs to be the same on the client !
		NetworkTransport.Init();

		ConnectionConfig cc = new ConnectionConfig();
		m_reliableChannel = cc.AddChannel(QosType.Reliable);

		HostTopology topo = new HostTopology(cc, MAX_USER);

		// Server only code
		m_hostId = NetworkTransport.AddHost(topo, PORT, null);
		m_webHostId = NetworkTransport.AddWebsocketHost(topo, WEB_PORT, null);

		Debug.Log(string.Format("Opening connection on port {0} and webport {1}", PORT, WEB_PORT));
		m_isStarted = true;
	}

	public void Shutdown()
	{
		m_isStarted = false;
		NetworkTransport.Shutdown();
	}

}