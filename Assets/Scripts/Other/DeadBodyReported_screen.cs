using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// displays dead body reported screen acording to id (color)
/// </summary>
public class DeadBodyReported_screen : MonoBehaviour
{
    public GameObject blast;
    public Text text;
    bool appearing = true;
    static byte a = 0;
    int time = 0;
    public static int deadPlayerID=1;
    Color32[] colors = {new Color32(229,40,47,0),new Color32(162, 48, 197,0),new Color32(30,188,5,0),new Color32(38,107,171,0),new Color32(255,135,33,0),new Color32(50,50,50,0),new Color32(255,255,255,0),new Color32(0,255,181,0),new Color32(255,255,0,0), new Color32(255,113,255, 0) };

    void Update()
    {
        if (appearing)
        {
            a += 5;
            colors[deadPlayerID - 1].a = a;
            blast.transform.GetChild(0).GetComponent<Image>().color = colors[deadPlayerID - 1];
            blast.transform.GetChild(1).GetComponent<Image>().color = new Color32(255,255,255,a);
            blast.GetComponent<Image>().color = new Color32(255, 255, 255, a);
            text.color = new Color32(0, 0, 0, a);
            if (a == 255)
            {
                appearing = false;
            }
        }
        else if (time == 4)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SoundManager.PlaySound("ReportBobdyFound");
        blast.transform.GetChild(0).GetComponent<Image>().color = colors[deadPlayerID - 1];
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
