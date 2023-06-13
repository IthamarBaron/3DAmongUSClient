using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// display emergency m,eeting call
/// </summary>
public class EmergencyMeetingCalled : MonoBehaviour
{
    public GameObject blast;
    public Text text;
    bool appearing = true;
    byte a = 0;
    int time = 0;
    void Update()
    {
        if (appearing)
        {
            a += 5;
            blast.transform.GetChild(0).GetComponent<Image>().color = new Color32(255, 255, 255, a); 
            blast.GetComponent<Image>().color = new Color32(255, 255, 255, a);
            text.color = new Color32(0, 0, 0, a);
            if (a == 255)
            {
                appearing = false;
            }
        }
        else if (time == 2)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SoundManager.PlaySound("EmergencyMeeting");
        InvokeRepeating(nameof(Timer), 0, 1);
    }

    public void Timer()
    {
        time++;
    }

    private void OnDestroy()
    {
        Interact.meetingScreenObject = Instantiate(Interact.meetingScreen, Interact.playerUI.transform);
        CameraController.canLook = false;
        Cursor.lockState = CursorLockMode.None;
    }
}
