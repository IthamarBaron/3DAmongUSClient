using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// download task behavior
/// </summary>
public class Download : MonoBehaviour
{

    public GameObject button;
    public GameObject arrow;
    public GameObject ball;

    bool goUp = true;
    int count = 0;
    public void DownloadButton()
    {
        button.GetComponent<Button>().enabled = false;
        InvokeRepeating(nameof(Countdown), 0, 0.01f);
        
        button.transform.GetChild(0).GetComponent<Text>().text = "";
    }

    void Countdown()
    {
        if (button.transform.localScale.x < 2.5f)
        {
            button.transform.localScale = new Vector3(button.transform.localScale.x + 0.05f, 1, 1);
        }
        else
        {
            if (count == 0)
            {
                arrow.SetActive(true);
                arrow.transform.localPosition = new Vector3(-120, 32);
                ball.SetActive(true);
            }
            if (count % 100 == 0 && count <= 800)
            {
                ball.transform.localPosition = new Vector2(ball.transform.localPosition.x + 46.25f, ball.transform.localPosition.y);
            }
            if (count == 800)
            {
                goUp = false;
            }
            count++;
        }
        
        
    }

    private void Update()
    {
        arrow.transform.Translate(new Vector2(100 * Time.deltaTime, 0));
        if (arrow.transform.localPosition.x > 180)
        {
            arrow.transform.localPosition = new Vector3(-120, 32);
        }

        if (goUp)
        {
            if (transform.localPosition.y < 4)
                transform.Translate(new Vector2(0, 1000 * Time.deltaTime));
        }
        else
        {
            if (transform.position.y < -360)
            {
                Destroy(gameObject);
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
                transform.Translate(new Vector2(0, -1000 * Time.deltaTime));

        }
    }
    private void OnDestroy()
    {
        SoundManager.PlaySound("TaskEnd");
        ClientSend.UpdateTaskProgressServer(); // letting the server know to increse the task counter
        PlayerController.canMove = true;
        CameraController.canLook = true;
    }
}
