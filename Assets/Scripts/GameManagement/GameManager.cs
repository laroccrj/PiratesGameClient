using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Dictionary<int, Player> Players = new Dictionary<int, Player>();
    public Dictionary<int, Team> Teams = new Dictionary<int, Team>();

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

    public static void HandleUpdateGameData(Packet packet)
    {
        instance.Players = new Dictionary<int, Player>();
        instance.Teams = new Dictionary<int, Team>();

        int teamCount = packet.ReadInt();

        for (int team_i = 1; team_i <= teamCount; team_i++)
        {
            int teamId = packet.ReadInt();
            string teamName = packet.ReadString();
            Team team = new Team(teamId, teamName);
            instance.Teams.Add(team.id, team);
        }

        int playerCount = packet.ReadInt();

        for (int player_i = 1; player_i <= playerCount; player_i++)
        {
            int playerId = packet.ReadInt();
            string playerName = packet.ReadString();
            int teamId = packet.ReadInt();
            Player player = new Player(playerId, playerName);
            player.team = instance.Teams[teamId];
            instance.Teams[teamId].Players.Add(player.id, player);

            instance.Players.Add(player.id, player);
        }

        LobbyUi.instance.UpdateUI();
    }

    public void RequestStartGame()
    {
        using (Packet packet = new Packet((int)ClientPackets.startGame))
        {
            packet.Write(Client.instance.id);
            ClientSend.SendTCPData(packet);
        }
    }

    public static void HandleGameStart(Packet packet)
    {
        UiManager.instance.lobby.SetActive(false);
    }
}
