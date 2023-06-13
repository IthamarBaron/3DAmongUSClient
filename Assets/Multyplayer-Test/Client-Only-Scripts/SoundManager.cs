using UnityEngine;

/// <summary>
/// This class is used to play audio clips for the LocalPlayer
/// </summary>
public class SoundManager : MonoBehaviour
{
    private static AudioSource audioSource;
    //public AudioSource audioSourcePointer;
    private void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
    }

    /// <summary>
    /// plays a sound for the LocalPlayer to hear 
    /// </summary>
    /// <param name="_soundName">name of the audio file we want to play</param>
    public static void PlaySound(string _soundName)
    {
        string filePath = "sfx/SoundEffects/" + _soundName; //file path to the audio clip
        Debug.Log(filePath);
        AudioClip clip = Resources.Load<AudioClip>(filePath);
        try
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
        catch (System.Exception)
        {
            Debug.LogWarning("SOUND NOT FOUND");
            throw;
        }


    }
}