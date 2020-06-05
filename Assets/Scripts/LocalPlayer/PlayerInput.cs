using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public enum Inputs
    {
        Left,
        Right,
        Use,
        SWITCH_INTERACTION_TYPE,
    }

    public Dictionary<Inputs, KeyCode> keyBinds = new Dictionary<Inputs, KeyCode>()
    {
        { Inputs.Left, KeyCode.A },
        { Inputs.Right, KeyCode.D },
        { Inputs.Use, KeyCode.Space },
        { Inputs.SWITCH_INTERACTION_TYPE, KeyCode.R }
    };

    public static PlayerInput instance;

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

    private class SendableInput
    {
        public Inputs id;
        public bool down;

        public SendableInput(Inputs id, bool down)
        {
            this.id = id;
            this.down = down;
        }
    }

    public void Update()
    {
        List<SendableInput> inputs = new List<SendableInput>();

        foreach(KeyValuePair<Inputs, KeyCode> keyBind in keyBinds)
        {
            if (Input.GetKeyDown(keyBind.Value))
            {
                inputs.Add(new SendableInput(keyBind.Key, true));
            }
            else if (Input.GetKeyUp(keyBind.Value))
            {
                inputs.Add(new SendableInput(keyBind.Key, false));
            }
        }

        if (inputs.Count > 0)
        {
            using (Packet packet = new Packet((int)ClientPackets.playerInput))
            {
                packet.Write(Client.instance.id);
                packet.Write(inputs.Count);

                foreach (SendableInput input in inputs)
                {
                    packet.Write((int)input.id);
                    packet.Write(input.down);
                }

                ClientSend.SendUDPData(packet);
            }
        }
    }
}
