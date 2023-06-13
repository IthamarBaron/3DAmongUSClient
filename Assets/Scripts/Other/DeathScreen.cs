using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// displays death dcreen
/// </summary>
public class DeathScreen : MonoBehaviour
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
            blast.GetComponent<Image>().color = new Color32(255, 255, 255, a);
            text.color = new Color32(0, 0, 0, a);
            if (a == 255)
            {
                appearing = false;
            }
        }
        else if (time == 5)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SoundManager.PlaySound("CrewmateKill");
        InvokeRepeating(nameof(Timer), 0, 1);
    }

    public void Timer()
    {
        time++;
    }
}
