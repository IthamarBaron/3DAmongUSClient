using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// controlls the minimap on the top left
/// </summary>
public class MiniMapScript : MonoBehaviour
{

    private Transform player;
    float temp;
    void Start()
    {
        player = GameObject.Find("LocalPlayer(Clone)").transform;
        temp = transform.rotation.y;
    }

    private void LateUpdate()
    {
        player = GameObject.Find("LocalPlayer(Clone)").transform;
        if (player != null)
        {
            Vector3 newPosition = player.position;
            newPosition.y = transform.position.y;
            transform.position = newPosition;

            
            transform.rotation = Quaternion.Euler(90f, temp, 0f);
        }

    }
}
