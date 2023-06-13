using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// responsible for the start menu
/// </summary>
public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject startMenu;
    public GameObject clientManager;
    public InputField usernameField;
    public InputField IPField;

    private void Start()
    {
        Interact.menue = this.gameObject;
    }
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

    /// <summary>Attempts to connect to the server.</summary>
    public void ConnectToServer()
    {
        if (usernameField.text.Length>15)
        {
            usernameField.text = "fool me once";
        }
        startMenu.SetActive(false);
        usernameField.interactable = false;
        Client.instance.ip = IPField.text;
        //Client.instance.name = usernameField.text;
        try
        {
            Client.instance.ConnectToServer();
        }
        catch
        {
            Debug.Log("Invalid IP");
        }
        
    }
}
