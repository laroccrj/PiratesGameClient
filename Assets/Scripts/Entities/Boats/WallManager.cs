using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallManager : MonoBehaviour
{
    public static WallManager instance;

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

    public static void HandleWallHealthUpdate(Packet packet)
    {
        int wallId = packet.ReadInt();
        int boatId = packet.ReadInt();
        float maxHealth = packet.ReadFloat();
        float health = packet.ReadFloat();

        Boat boat = BoatManager.Boats[boatId];
        Wall wall = boat.walls[wallId];
        wall.maxHealth = maxHealth;
        wall.health = health;
    }
}
