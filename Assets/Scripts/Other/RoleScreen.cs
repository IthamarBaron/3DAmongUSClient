using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// displays roles screen
/// </summary>
public class RoleScreen : MonoBehaviour
{
    public bool impostor;
    public GameObject text;
    byte colorAlpha = 0;
    byte textAlpha = 0;
    byte color1 = 130;
    byte color2 = 255;
    bool appearing = true;
    int counter = 0;

    private void Start()
    {
        impostor = Client.instance.isImpostor;
        if (impostor)
        {
            text.GetComponent<Text>().text = "Impostor";
            color1 = 255;
            color2 = 0;
        }
        else
        {            
            text.GetComponent<Text>().text = "Crewmate";
        }
    }
    void FixedUpdate()
    {
        if (appearing)
        {
            if (colorAlpha < 255)
            {
                colorAlpha += 15;
                GetComponent<Image>().color = new Color32(0, 0, 0, colorAlpha);
            }
        }
        else
        {
            if (colorAlpha > 0)
            {
                colorAlpha -= 15;
                textAlpha -= 15;
                GetComponent<Image>().color = new Color32(0, 0, 0, colorAlpha);
                text.GetComponent<Text>().color = new Color32(color1, color2, color2, textAlpha);
            }
            if (colorAlpha == 0)
                Debug.LogWarning("Interact.playerUI"+ Interact.playerUI);
                Interact.playerUI.SetActive(true);
                Debug.LogWarning("Interact.playerUI" + Interact.playerUI);
            Destroy(this.gameObject);
        }

        if (colorAlpha == 255)
        {   
            if (textAlpha < 255)
            {
                text.GetComponent<Text>().color = new Color32(color1, color2, color2, textAlpha);
                textAlpha += 5;
            }

            if (text.transform.localPosition.y < 0)
            {
                text.transform.Translate(new Vector2(0, 4));
            }
            
        }

        counter++;
        if (counter == 100)
        {
            appearing = false;
        }
        
        
    }
}
