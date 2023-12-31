using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public static Dictionary<int, PlayerManager> players = new Dictionary<int, PlayerManager>();
    public Material[] colors = new Material[10];
    public GameObject localPlayerPrefab;
    public GameObject playerPrefab;
    public const int IMPOSTORS = 1; //amount of impostors in the game (between 1-2)
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


    /// <summary>Spawns a player.</summary>
    /// <param name="_id">The player's ID.</param>
    /// <param name="_name">The player's name.</param>
    /// <param name="_position">The player's starting position.</param>
    /// <param name="_rotation">The player's starting rotation.</param>
    public void SpawnPlayer(int _id, string _username, Vector3 _position, Quaternion _rotation)
    {
        GameObject _player;
        if (_id == Client.instance.myId)
        {
            _player = Instantiate(localPlayerPrefab, _position, _rotation);
        }
        else
        {
            _player = Instantiate(playerPrefab, _position, _rotation);
            _player.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = _username;
            _player.transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().materials[0].color = colors[_id - 1].color;//change body color
            _player.transform.GetChild(0).GetChild(1).GetComponent<MeshRenderer>().materials[0].color = colors[_id - 1].color;//change Backpack Color

        }

        _player.GetComponent<PlayerManager>().id = _id;
        _player.GetComponent<PlayerManager>().username = _username;
        players.Add(_id, _player.GetComponent<PlayerManager>());
    }
}