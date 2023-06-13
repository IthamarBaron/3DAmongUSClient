using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
/// this code controlls all easter eggs related to the player's name (unrelated to project this was made for fun (: )
/// </summary>
public class InputFieldController : MonoBehaviour
{
    private InputField inputField;
    public AudioSource audioSource;
    public AudioClip bingChillingSFX;
    public Sprite bingChillingSprite;
    public Sprite rareTeletabie;
    public Image backgroundImage;
    void Start()
    {
        inputField = GetComponent<InputField>();
        inputField.onValueChanged.AddListener(OnInputChange);

        System.Random rnd = new System.Random();
        if (rnd.Next(1,300) == 1)
        {
            backgroundImage.sprite = rareTeletabie;
        }

    }

    public void OnInputChange(string _newText)
    {
        if (_newText.ToLower() == "bingchilling" || _newText == "bing chilling")
        {
            audioSource.clip = bingChillingSFX;
            audioSource.Play();
            backgroundImage.sprite = bingChillingSprite;
        }

    }
}