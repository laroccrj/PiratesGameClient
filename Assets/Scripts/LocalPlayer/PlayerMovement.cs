using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public InteractionType interactionType = InteractionType.USE;

    void Update()
    {
        if(Input.GetKeyDown(PlayerInput.instance.keyBinds[PlayerInput.Inputs.SWITCH_INTERACTION_TYPE])) {
            if (this.interactionType == InteractionType.USE)
                this.interactionType = InteractionType.REPAIR;
            else
                this.interactionType = InteractionType.USE;
        }

        if (Input.GetMouseButtonDown(1))
        {
            Vector2 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePoint, Vector2.zero, 10, 1 << PlayerManager.player.boat.deck.gameObject.layer);
            if (hit.collider != null)
            {
                if(hit.collider == PlayerManager.player.boat.deck)
                {
                    SendMovement(PlayerManager.player.boat.deck.transform.InverseTransformPoint(hit.point));
                }
                else if (hit.collider.GetComponent<Interactable>() != null)
                {
                    Interactable interactable = hit.collider.GetComponent<Interactable>();
                    if (Array.Exists<InteractionType>(interactable.GetPossibleInteractionTypes(), el => el == this.interactionType))
                    {
                        this.RequestInteraction(interactable, this.interactionType);
                    }
                }
            }
        }
    }

    private void SendMovement(Vector3 position)
    {
        using (Packet packet = new Packet((int)ClientPackets.pirateMovement))
        {
            packet.Write(Client.instance.id);
            packet.Write(position);
            ClientSend.SendUDPData(packet);
        }
    }

    private void RequestInteraction(Interactable interactable, InteractionType interactionType)
    {
        using (Packet packet = new Packet((int)ClientPackets.interactionRequest))
        {
            packet.Write(Client.instance.id);
            packet.Write(interactable.GetId());
            packet.Write((int)interactable.GetInteractableType());
            packet.Write((int)interactionType);
            ClientSend.SendUDPData(packet);
        }
    }
}
