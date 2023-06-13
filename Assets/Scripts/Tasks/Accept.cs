using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Accept power task behaior
/// </summary>
public class Accept : MonoBehaviour
{
    public GameObject r_switch;
    int rotat = 0;
    bool goUp = true;

    public void pressed()
    {
        r_switch.GetComponent<Button>().enabled = false;
        
        rotat++;
    }

    private void Update()
    {
        if (rotat < 40 && rotat != 0)
        {
            r_switch.transform.Rotate(new Vector3(0, 0, Time.deltaTime * -1000));
            rotat++;
        }
        if (rotat >= 40)
        {
            if (rotat > 300)
            {
                goUp = false;
            }
            rotat++;
        }

        if (goUp)
        {
            if (transform.localPosition.y < 0)
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
