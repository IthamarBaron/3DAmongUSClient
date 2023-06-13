using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// a place to store data such as id on a dead body 
/// </summary>
public class DeadBodyManager : MonoBehaviour
{
    public int id;

    public static void ClearDeadBodies()
    {
        // Find all instances of the Deadbody prefab
        GameObject[] deadbodyInstances = GameObject.FindGameObjectsWithTag("deadBody");

        // Destroy each instance
        foreach (GameObject deadbodyInstance in deadbodyInstances)
        {
            Destroy(deadbodyInstance);
        }
    }
}
