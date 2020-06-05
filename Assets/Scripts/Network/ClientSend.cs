using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientSend : MonoBehaviour
{
    public static void SendTCPData(Packet packet)
    {
        packet.WriteLength();
        Client.instance.tcp.SendData(packet);
    }
    
    public static void SendUDPData(Packet packet)
    {
        packet.WriteLength();
        Client.instance.udp.SendData(packet);
    }

    #region Packets
    public static void WelcomeRecieved()
    {
        using (Packet packet = new Packet((int)ClientPackets.welcomeReceived))
        {
            packet.Write(Client.instance.id);
            packet.Write(PlayerManager.name);

            SendTCPData(packet);
        }
    }
    #endregion
}
