using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System;

public class Client : MonoBehaviour
{
    public static Client instance = null;
    public static int dataBufferSize = 4096;

    public string hostname = "ec2-3-20-233-235.us-east-2.compute.amazonaws.com";
    public string ip = "127.0.0.1";
    public int port = 9999;
    public int id = 0;
    public TCP tcp;
    public UDP udp;

    private delegate void PacketHandler(Packet packet);
    private static Dictionary<int, PacketHandler> packetHandlers;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Client instance already exists, destroying object");
            Destroy(this);
        }
    }

    private void Start()
    {
        this.tcp = new TCP();
        this.udp = new UDP();
    }

    public void ConnectToServer()
    {
        InitializeClientData();
        this.tcp.Connect();
    }

    private void InitializeClientData()
    {
        packetHandlers = new Dictionary<int, PacketHandler>()
        {
            { (int)ServerPackets.welcome,  ClientHandle.Welcome },
            { (int)ServerPackets.udpateGameData,  GameManager.HandleUpdateGameData },
            { (int)ServerPackets.sendBoats,  BoatManager.RecieveBoatsFromServer },
            { (int)ServerPackets.gameStart,  GameManager.HandleGameStart },
            { (int)ServerPackets.pirateSpawn,  PirateManager.SpawnPirates },
            { (int)ServerPackets.pirateMove,  PirateManager.UpdatePiratePositions },
            { (int)ServerPackets.boatTransformUpdate,  BoatManager.HandleBoatTransformUpdate },
            { (int)ServerPackets.projectileSpawn,  ProjectileManager.SpawnProjectile },
            { (int)ServerPackets.projectileUpdate,  ProjectileManager.HandleProjectileTransformUpdate },
            { (int)ServerPackets.projectileHit,  ProjectileManager.HandleProjectileHit },
        };

        Debug.Log("Initialized packets.");
    }

    public class UDP
    {
        public UdpClient socket;
        public IPEndPoint endPoint;

        public UDP()
        {
            endPoint = new IPEndPoint(IPAddress.Parse(instance.ip), instance.port);
            //endPoint = new IPEndPoint(IPAddress.Any, instance.port);
        }

        public void Connect(int localPort)
        {
            socket = new UdpClient(localPort);

            socket.Connect(endPoint);
            //socket.Connect(instance.hostname, instance.port);
            socket.BeginReceive(ReceiveCallback, null);

            using (Packet packet = new Packet())
            {
                SendData(packet);
            }
        }

        public void SendData(Packet packet)
        {
            try
            {
                packet.InsertInt(instance.id);
                if (socket != null)
                {
                    socket.BeginSend(packet.ToArray(), packet.Length(), null, null);
                }
            }
            catch (Exception e)
            {
                Debug.Log($"Erro sending data to server via UDP: {e}");
            }
        }

        private void ReceiveCallback(IAsyncResult result)
        {
            try
            {
                byte[] data = socket.EndReceive(result, ref endPoint);
                socket.BeginReceive(ReceiveCallback, null);

                if (data.Length < 4)
                {
                    Debug.Log("UDP Receive Callback size less than 4");
                    return;
                }

                HandleData(data);
            }
            catch
            {
                // todo
            }
        }

        private void HandleData(byte[] data)
        {
            using (Packet packet = new Packet(data))
            {
                int packetLength = packet.ReadInt();
                data = packet.ReadBytes(packetLength);
            }

            ThreadManager.ExecuteOnMainThread(() =>
            {
                using (Packet packet = new Packet(data))
                {
                    int packetId = packet.ReadInt();
                    packetHandlers[packetId](packet);
                }
            });
        }
    }

    public class TCP
    {
        public TcpClient socket;

        private NetworkStream stream;
        private Packet recieveData;
        private byte[] receiveBuffer;

        public void Connect()
        {
            socket = new TcpClient
            {
                ReceiveBufferSize = dataBufferSize,
                SendBufferSize = dataBufferSize
            };

            receiveBuffer = new byte[dataBufferSize];
            socket.BeginConnect(instance.ip, instance.port, ConnectCallback, socket);
            //socket.BeginConnect(instance.hostname, instance.port, ConnectCallback, socket);
        }

        private void ConnectCallback(IAsyncResult result)
        {
            socket.EndConnect(result);

            if (!socket.Connected)
            {
                return;
            }

            stream = socket.GetStream();

            recieveData = new Packet();

            stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
        }

        private void ReceiveCallback(IAsyncResult _result)
        {
            try
            {
                int _byteLength = stream.EndRead(_result);
                if (_byteLength <= 0)
                {
                    // TODO: disconnect
                    return;
                }

                byte[] _data = new byte[_byteLength];
                Array.Copy(receiveBuffer, _data, _byteLength);

                recieveData.Reset(HandleData(_data));
                stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
            }
            catch
            {
                // TODO: disconnect
            }
        }

        public void SendData(Packet packet)
        {
            try
            {
                if (this.socket != null)
                {
                    stream.BeginWrite(packet.ToArray(), 0, packet.Length(), null, null);
                }
            }
            catch (Exception e)
            {
                Debug.Log($"Erro sending data to server via TCP: {e}");
            }
        }

        private bool HandleData(byte[] data)
        {
            int packetLength = 0;
            recieveData.SetBytes(data);

            if (recieveData.UnreadLength() >= 4)
            {
                packetLength = recieveData.ReadInt();
                if (packetLength <= 0)
                {
                    return true;
                }
            }

            while (packetLength > 0 && packetLength <= recieveData.UnreadLength())
            {
                byte[] packetBytes = recieveData.ReadBytes(packetLength);
                ThreadManager.ExecuteOnMainThread(() =>
                {
                    using (Packet packet = new Packet(packetBytes))
                    {
                        int packetId = packet.ReadInt();
                        packetHandlers[packetId](packet);
                    }
                });

                packetLength = 0;

                if (recieveData.UnreadLength() >= 4)
                {
                    packetLength = recieveData.ReadInt();
                    if (packetLength <= 0)
                    {
                        return true;
                    }
                }
            }

            return (packetLength <= 1);
        }
    }
}
