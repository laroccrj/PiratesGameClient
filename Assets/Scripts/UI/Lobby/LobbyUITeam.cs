using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUITeam : MonoBehaviour
{
    public Button selectTeamButton;
    public Team team;
    public Text playerNamePrefab;
    public Text teamName;
    public float playerNameSpacing = 50;

    private void Start()
    {
        this.teamName.text = team.name;

        foreach (Player player in this.team.Players.Values)
        {
            Text playerName = GameObject.Instantiate<Text>(this.playerNamePrefab, this.transform);
            playerName.text = player.name;
            playerName.transform.Translate(Vector3.down * playerNameSpacing);

            playerNameSpacing += playerNameSpacing + playerName.transform.localScale.y;
        }

        selectTeamButton.onClick.AddListener(ChangeTeam);
    }

    private void ChangeTeam()
    {
        using (Packet packet = new Packet((int)ClientPackets.changeTeam))
        {
            packet.Write(Client.instance.id);
            packet.Write(team.id);
            ClientSend.SendTCPData(packet);
        }
    }
}
