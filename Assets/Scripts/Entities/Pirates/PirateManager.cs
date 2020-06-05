using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PirateManager : MonoBehaviour
{
    public static PirateManager instance;
    public static Dictionary<int, Pirate> Pirates = new Dictionary<int, Pirate>();

    private static int nextId = 1;

    public Pirate piratePrefab;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    public static int GetNextId()
    {
        return nextId++;
    }


    public static void SpawnPirates(Packet packet)
    {
        int pirateCount = packet.ReadInt();

        for (int i = 1; i <= pirateCount; i++)
        {
            int id = packet.ReadInt();
            int boatId = packet.ReadInt();
            Vector3 localPos = packet.ReadVector3();
            Quaternion localRot = packet.ReadQuaternion();

            Boat boat = BoatManager.Boats[boatId];
            Pirate pirate = Instantiate<Pirate>(instance.piratePrefab, boat.transform);

            pirate.id = id;
            pirate.boat = boat;
            pirate.transform.localPosition = localPos;
            pirate.transform.rotation = localRot;

            if (id == Client.instance.id)
            {
                CameraController.main.positionTarget = pirate.transform;
                CameraController.main.rotateTarget = pirate.transform;
                PlayerManager.player = pirate;
            }

            Pirates.Add(pirate.id, pirate);
        }
    }

    public static void UpdatePiratePositions(Packet packet)
    {
        int count = packet.ReadInt();

        for (int i = 1; i <= count; i++)
        {
            int id = packet.ReadInt();
            Vector3 pos = packet.ReadVector3();
            Quaternion rot = packet.ReadQuaternion();

            if (!Pirates.ContainsKey(id))
                continue;

            Pirate pirate = Pirates[id];
            pirate.TargetPosition = pos;
            pirate.transform.localRotation = rot;
        }
    }
}
