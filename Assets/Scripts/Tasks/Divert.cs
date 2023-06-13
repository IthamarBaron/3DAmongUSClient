using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
/// <summary>
/// Divert power behavior
/// </summary>
public class Divert : MonoBehaviour, IDragHandler
{
    public GameObject places;
    GameObject relevantSwitch;
    bool mouseHeld = false;
    bool goUp = true;
    private void Start()
    {
        //temporary random
        relevantSwitch = places.transform.GetChild(0).GetChild(2).gameObject;
        relevantSwitch.GetComponent<Image>().color = new Color32(255, 102, 0, 255);

    }

    
    public void OnDrag(PointerEventData eventData)
    {
        if ((eventData.pointerEnter == relevantSwitch || mouseHeld) && goUp)
        {
            RectTransform a = (RectTransform)relevantSwitch.transform;
            //not a very efficient method to make the object stay on the y axis but nothing else work
            float axisXPos = a.anchoredPosition.x;
            a.anchoredPosition += eventData.delta / transform.parent.GetComponent<Canvas>().scaleFactor;
            float axisYPos = a.anchoredPosition.y >= -135 ? a.anchoredPosition.y : -135;
            a.anchoredPosition = new Vector2(axisXPos, axisYPos);   
            mouseHeld = true;
            if (a.anchoredPosition.y > -30)
            {
                goUp = false;
                relevantSwitch.GetComponent<Image>().color = new Color32(178, 71, 0, 255);
            }
        }
        

    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            mouseHeld = false;
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
