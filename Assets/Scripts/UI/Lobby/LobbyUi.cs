using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUi : MonoBehaviour
{
    public static LobbyUi instance;

    public LobbyUITeam teamPrefab;
    public Button startGameBtn;

    public GameObject teams;

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

        this.startGameBtn.onClick.AddListener(GameManager.instance.RequestStartGame);
    }

    public void UpdateUI()
    {
        int posX = 0;

        foreach (Transform child in this.teams.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Team team in GameManager.instance.Teams.Values)
        {
            LobbyUITeam uiTeam = GameObject.Instantiate<LobbyUITeam>(this.teamPrefab, this.teams.transform);
            uiTeam.team = team;
            uiTeam.transform.Translate(Vector3.right * posX);
            posX += 150;
        }
    }
}
