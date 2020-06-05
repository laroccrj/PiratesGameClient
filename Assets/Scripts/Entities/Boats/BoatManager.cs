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

        BoatManager.wallPrefab = wallPrefabBad;
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
            boat.id = id;

            int mountableCount = packet.ReadInt();
            for (int mountable_i = 1; mountable_i <= mountableCount; mountable_i++)
            {
                int mountableId = packet.ReadInt();
                Mountable.Mountables type = (Mountable.Mountables) packet.ReadInt();
                float maxHealth = packet.ReadFloat();
                float health = packet.ReadFloat();
                bool reverseRotation = packet.ReadBool();
                Vector3 mountablePos = packet.ReadVector3();
                Quaternion mountableRot = packet.ReadQuaternion();

                Mountable mountablePrefab = MountableManager.MountablesByType[type];
                Mountable mountable = GameObject.Instantiate<Mountable>(mountablePrefab, boat.transform);
                mountable.id = mountableId;
                mountable.boat = boat;
                mountable.maxHealth = maxHealth;
                mountable.health = health;
                mountable.reverseRotation = reverseRotation;
                mountable.transform.localPosition = mountablePos;
                mountable.transform.localRotation = mountableRot;
                boat.mountables.Add(mountable.id, mountable);
            }
            
            int wallCount = packet.ReadInt();
            for (int wall_i = 1; wall_i <= wallCount; wall_i++)
            {
                int wallId = packet.ReadInt();
                float wallMaxHealth = packet.ReadFloat();
                float wallHealth = packet.ReadFloat();
                Vector3 wallPos = packet.ReadVector3();
                Quaternion wallRot = packet.ReadQuaternion();
                Vector3 wallScale = packet.ReadVector3();

                Wall wall = GameObject.Instantiate<Wall>(wallPrefab, boat.transform);
                wall.id = wallId;
                wall.boat = boat;
                wall.health = wallHealth;
                wall.maxHealth = wallMaxHealth;
                wall.health = wallHealth;
                wall.transform.localPosition = wallPos;
                wall.transform.localRotation = wallRot;
                wall.transform.localScale = wallScale;
                boat.walls.Add(wallId, wall);
            }

            Boats.Add(boat.id, boat);
        }
    }

    public static void HandleBoatTransformUpdate(Packet packet)
    {
        int boatCount = packet.ReadInt();

        for (int i = 1; i <= boatCount; i++)
        {
            int id = packet.ReadInt();
            Vector3 pos = packet.ReadVector3();
            Quaternion rot = packet.ReadQuaternion();

            if (!BoatManager.Boats.ContainsKey(id))
                continue;

            Boat boat = BoatManager.Boats[id];
            boat.TargetPosition = pos;
            boat.transform.rotation = rot;

            int mountableCount = packet.ReadInt();
            for (int mountable_i = 1; mountable_i <= mountableCount; mountable_i++)
            {
                int mountableId = packet.ReadInt();
                float maxHealth = packet.ReadFloat();
                float health = packet.ReadFloat();
                Vector3 mountablePos = packet.ReadVector3();
                Quaternion mountableRot = packet.ReadQuaternion();

                Mountable mountable = boat.mountables[mountableId];
                mountable.maxHealth = maxHealth;
                mountable.health = health;
                mountable.body.transform.localPosition = mountablePos;
                mountable.body.transform.localRotation = mountableRot;
            }
        }
    }
}
