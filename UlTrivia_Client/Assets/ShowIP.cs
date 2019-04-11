using System.Net.NetworkInformation;
using System.Net.Sockets;
using TMPro;
using UnityEngine;

public class ShowIP : MonoBehaviour
{
    public TextMeshProUGUI m_text;
    // Use this for initialization
    void Start()
    {
        m_text.text = string.Format("Server IP is : {0}", IPManager.GetIP(ADDRESSFAM.IPv4));
    }
}

//taken from stack Overflow user " Programmer "
public class IPManager
{
    public static string GetIP(ADDRESSFAM p_addfam)
    {
        //Return null if ADDRESSFAM is Ipv6 but Os does not support it
        if (p_addfam == ADDRESSFAM.IPv6 && !Socket.OSSupportsIPv6)
        {
            return null;
        }

        string output = "";

        foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
        {
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
            NetworkInterfaceType _type1 = NetworkInterfaceType.Wireless80211;
            NetworkInterfaceType _type2 = NetworkInterfaceType.Ethernet;

            if ((item.NetworkInterfaceType == _type1 || item.NetworkInterfaceType == _type2) &&
                item.OperationalStatus == OperationalStatus.Up)
#endif
            {
                foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses)
                {
                    //IPv4
                    if (p_addfam == ADDRESSFAM.IPv4)
                    {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            output = ip.Address.ToString();
                        }
                    }

                    //IPv6
                    else if (p_addfam == ADDRESSFAM.IPv6)
                    {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetworkV6)
                        {
                            output = ip.Address.ToString();
                        }
                    }
                }
            }
        }

        return output;
    }
}

public enum ADDRESSFAM
{
    IPv4,
    IPv6
}