using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public abstract class Mountable : MonoBehaviour, Interactable
{
    public enum Mountables
    {
        STEERING_WHEEL,
        CANNON
    }

    public abstract Mountables MountableType
    {
        get;
    }

    public int id;
    public bool mounted = false;
    public Boat boat;
    public bool reverseRotation;
    public Transform seat = null;
    public Transform body = null;
    public Transform luancher = null;
    public Transform cameraPosition = null;
    public float health = 100;
    public float maxHealth = 100;

    void Start()
    {
        if (this.seat == null)
            this.seat = this.transform;

        if (this.body == null)
            this.body = this.transform;

        if (this.luancher == null)
            this.luancher = this.transform;
    }

    public void mount()
    {
        this.mounted = true;

        if (this.cameraPosition != null)
        {
            CameraController.main.positionTarget = this.cameraPosition;

        }

        this.OnMount();
    }

    public void dismount()
    {
        this.mounted = false;

        if (this.cameraPosition != null)
        {
            CameraController.main.positionTarget = PlayerManager.player.transform;
        }

        this.OnDismount();
    }

    public abstract void OnMount();
    public abstract void OnDismount();

    public InteractableType GetInteractableType()
    {
        return InteractableType.MOOUNTABLE;
    }

    public int GetId()
    {
        return this.id;
    }

    public InteractionType[] GetPossibleInteractionTypes()
    {
        return new InteractionType[]{
            InteractionType.USE,
            InteractionType.REPAIR,
        };
    }
}
