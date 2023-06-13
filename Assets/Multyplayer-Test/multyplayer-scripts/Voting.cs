using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
/// responsible for managing the players meeting interface and voting
/// </summary>
public class Voting : MonoBehaviour
{
    
    public static string[] names;
    public GameObject player_frame;
    int[] votes;
    public static bool voted = false;
    private Interact interactCamera;
    
    private void Start()
    {
        interactCamera = FindObjectOfType<Interact>();
        int playersInMeetingPlusOne = names.Length;
        GameObject[] alivePlayers = new GameObject[playersInMeetingPlusOne];
        votes = new int[playersInMeetingPlusOne];

        GameObject skipFrame = Instantiate(player_frame, transform.parent);
        skipFrame.transform.localPosition = new Vector2(-263.3f, -136.1f);
        skipFrame.transform.GetChild(0).GetComponent<Text>().text = "Skip Vote";
        skipFrame.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(delegate { Vote(0); }); // Use the local variable
        skipFrame.transform.GetChild(1).GetComponent<Image>().enabled = false;
        for (int i = 1, y = 82; i < playersInMeetingPlusOne; i++)
        {
            Debug.Log("name: "+names[i-1]);
            Debug.Log("Char: "+ names[i - 1][0]);
            int id = int.Parse(names[i - 1][0].ToString());
            Debug.Log("ID on top = "+id);

            if ((i - 1) % 2 == 0)
            {
                GameObject playerFrame = Instantiate(player_frame, transform.parent);
                playerFrame.transform.GetChild(0).GetComponent<Text>().text = names[i - 1];
                playerFrame.transform.localPosition = new Vector2(-160, y);
                playerFrame.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(delegate { Vote(id); }); // Use the local variable
                Debug.Log("left" + i);
            }
            else
            {
                GameObject playerFrame = Instantiate(player_frame, transform.parent);
                playerFrame.transform.GetChild(0).GetComponent<Text>().text = names[i - 1];
                playerFrame.transform.localPosition = new Vector2(160, y);
                playerFrame.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(delegate { Vote(id); }); // Use the local variable
                y -= 50;
                Debug.Log("right" + i);
            }
        }
    }

    /// <summary>
    /// Sends a vote to the server
    /// </summary>
    /// <param name="_id">The player id You voted for</param>
    public void Vote(int _id)
    {
        if (!voted && interactCamera.transform.parent.GetComponent<PlayerManager>() != null && interactCamera.transform.parent.GetComponent<PlayerManager>().isAlive)
        {
            Debug.Log("VOTED TO PLAYER WITH ID: " + _id);
            SoundManager.PlaySound("LockVote");
            string _infoString = (names.Length - 1).ToString() + "!+@_#)$(%*^&" + _id.ToString();
            ClientSend.CastVote(_infoString);
            Debug.Log("sent: " + _infoString);
        }
        voted = true;
    }

    
}
