using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    public static Dictionary<ProjectileType, Projectile> ProjectileByType = new Dictionary<ProjectileType, Projectile>();
    public static Dictionary<int, Projectile> Projectiles = new Dictionary<int, Projectile>();
    public Projectile[] projectilePrefabs;

    private void Awake()
    {
        foreach (Projectile projectile in projectilePrefabs)
        {
            ProjectileByType[projectile.ProjectileType] = projectile;
        }
    }

    public static void SpawnProjectile (Packet packet)
    {
        int id = packet.ReadInt();
        ProjectileType type = (ProjectileType)packet.ReadInt();
        int boatId = packet.ReadInt();
        int boatEntityId = packet.ReadInt();
        BoatEntityType boatEntityType = (BoatEntityType) packet.ReadInt();


        Boat boat = BoatManager.Boats[boatId];
        BoatEntity boatEntity = boat.boatEntitiesByType[boatEntityType][boatEntityId];
        ProjectileLauncher projectileLauncher = boatEntity as ProjectileLauncher;
        Transform launcher = projectileLauncher != null ? projectileLauncher.GetLauncher() : boatEntity.transform;

        Projectile projectile = GameObject.Instantiate<Projectile>(ProjectileByType[(ProjectileType)type], launcher.position, launcher.rotation);
        projectile.TargetPosition = launcher.position;
        Projectiles.Add(id, projectile);
    }

    public static void HandleProjectileHit(Packet packet)
    {
        int id = packet.ReadInt();
        Vector3 position = packet.ReadVector3();

        if (!Projectiles.ContainsKey(id))
            return;

        Projectile projectile = Projectiles[id];
        Projectiles.Remove(id);
        projectile.OnProjectileHit(position);
    }

    public static void HandleProjectileTransformUpdate(Packet packet)
    {
        int count = packet.ReadInt();

        for (int i = 1; i <= count; i++)
        {
            int id = packet.ReadInt();
            Vector3 pos = packet.ReadVector3();
            Quaternion rot = packet.ReadQuaternion();

            if (Projectiles.ContainsKey(id))
            {
                Projectile projectile = Projectiles[id];
                projectile.TargetPosition = pos;
                projectile.transform.rotation = rot;
            }
        }
    }
}
