using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// make sures the cameras in the room "security" only work when the player is close
/// </summary>
public class CamerasRender : MonoBehaviour
{
    public Camera[] cameras = new Camera[3];
    public float maxX;
    public float minX;
    public float maxZ;
    public float minZ;

    // Update is called once per frame
    void Update()
    {
        GameObject localPlayer = GameObject.Find("LocalPlayer(Clone)");
        if (localPlayer != null && localPlayer.transform.position.x > minX && localPlayer.transform.position.x < maxX && localPlayer.transform.position.z > minZ && localPlayer.transform.position.z < maxZ)
        {
            foreach (var cam in cameras)
                cam.enabled = true;
        }
        else
        {
            foreach (var cam in cameras)
                cam.enabled = false;
        }
    }
}
