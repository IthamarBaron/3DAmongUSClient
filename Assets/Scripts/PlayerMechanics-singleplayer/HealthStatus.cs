using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthStatus : MonoBehaviour
{
    public float tempAddToY = 0.2f; // according to model
    public Transform playerTransform;
    public GameObject playerBody;
    public GameObject deadBody;
    public GameObject backpack;

    public void Kill()
    {
        //if the player was killed we want him to die
        Die();
    }
    void Die()
    {
        //Getting player pos
        Vector3 playerPos = playerTransform.transform.position;
        playerPos.y += tempAddToY; //CHANGE THIS ACCORDING TO THE MODEL
        //getting players rotation
        Quaternion playerRotation = playerTransform.rotation;
  

        //Getting player color
        Material bodyColor = backpack.GetComponent<Renderer>().material;
        //applying color to the body
        deadBody.GetComponentInChildren<Renderer>().material = bodyColor;
        //spawning body
        Instantiate(deadBody, playerPos, playerRotation);

        //destroying player model
        Destroy(playerBody);

    }
}