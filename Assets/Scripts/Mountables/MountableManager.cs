using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MountableManager : MonoBehaviour
{
    public static Dictionary<Mountable.Mountables, Mountable> MountablesByType = new Dictionary<Mountable.Mountables, Mountable>();
    public Mountable[] Mountables;

    private void Awake()
    {
        foreach (Mountable mountable in Mountables)
        {
            MountablesByType[mountable.MountableType] = mountable;
        }
    }

    public static void MountConfirm(Packet packet)
    {
        int id = packet.ReadInt();
        PlayerManager.player.boat.mountables[id].mount();
    }

    public static void DismountConfirm(Packet packet)
    {
        int id = packet.ReadInt();
        PlayerManager.player.boat.mountables[id].dismount();
    }
}
