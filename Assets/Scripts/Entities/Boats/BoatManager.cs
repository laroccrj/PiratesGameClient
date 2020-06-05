using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatManager : MonoBehaviour
{
    public static BoatManager instance;
    public static Dictionary<int, Boat> Boats = new Dictionary<int, Boat>();
    public static Wall wallPrefab;
    public Boat boatPrefab;
    public Wall wallPrefabBad;

    public static Dictionary<BoatEntityType, BoatEntity> BoatEntityByType = new Dictionary<BoatEntityType, BoatEntity>();
    public BoatEntity[] BoatEntities;

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

        foreach (BoatEntity boatEntity in BoatEntities)
        {
            BoatEntityByType[boatEntity.BoatEntityType] = boatEntity;
        }
    }

    public static void RecieveBoatsFromServer(Packet packet)
    {
        Debug.Log("Recieving Boats");
        int boatCount = packet.ReadInt();

        for (int i = 1; i <= boatCount; i++)
        {
            int id = packet.ReadInt();
            Vector3 pos = packet.ReadVector3();
            Quaternion rot = packet.ReadQuaternion();

            Boat boat = GameObject.Instantiate<Boat>(instance.boatPrefab, pos, rot);
            boat.Init(id);

            int boatEntityTypeCount = packet.ReadInt();

            for (int ii = 1; ii <= boatEntityTypeCount; ii++)
            {
                BoatEntityType boatEntityType = (BoatEntityType)packet.ReadInt();
                int entityOfTypeCount = packet.ReadInt();

                for (int iii = 1; iii <= entityOfTypeCount; iii++)
                {
                    int entiyId = packet.ReadInt();
                    Vector3 localPosition = packet.ReadVector3();
                    Quaternion localRotation = packet.ReadQuaternion();
                    Vector3 localScale = packet.ReadVector3();

                    BoatEntity entity = Instantiate<BoatEntity>(BoatEntityByType[boatEntityType], boat.transform);
                    entity.id = entiyId;
                    entity.transform.localPosition = localPosition;
                    entity.transform.localRotation = localRotation;
                    entity.transform.localScale = localScale;
                    entity.ReadDataFromPacket(packet);

                    boat.boatEntitiesByType[boatEntityType][entity.id] = entity;
                }
            }

            Boats.Add(boat.id, boat);
        }
    }

    public static void HandleBoatTransformUpdate(Packet packet)
    {
        int id = packet.ReadInt();
        Vector3 pos = packet.ReadVector3();
        Quaternion rot = packet.ReadQuaternion();

        if (!BoatManager.Boats.ContainsKey(id))
            return;

        Boat boat = BoatManager.Boats[id];
        boat.TargetPosition = pos;
        boat.transform.rotation = rot;

        int boatEntityTypeCount = packet.ReadInt();

        for (int ii = 1; ii <= boatEntityTypeCount; ii++)
        {
            BoatEntityType boatEntityType = (BoatEntityType)packet.ReadInt();
            int entityOfTypeCount = packet.ReadInt();

            for (int iii = 1; iii <= entityOfTypeCount; iii++)
            {
                int entiyyId = packet.ReadInt();
                Vector3 localPosition = packet.ReadVector3();
                Quaternion localRotation = packet.ReadQuaternion();
                Vector3 localScale = packet.ReadVector3();

                BoatEntity entity = boat.boatEntitiesByType[boatEntityType][entiyyId];
                entity.transform.localPosition = localPosition;
                entity.transform.localRotation = localRotation;
                entity.transform.localScale = localScale;
                entity.ReadDataFromPacket(packet);
            }
        }
    }
}
