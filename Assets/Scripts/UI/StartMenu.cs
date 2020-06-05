using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    public InputField usernameField;
    public Text status;

    public void ConnectToServer()
    {
        this.status.text = "Conecting...";
        usernameField.interactable = false;
        PlayerManager.name = usernameField.text;
        Client.instance.ConnectToServer();
    }
}
