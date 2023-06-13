using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;


/// <summary>
/// controlls the TaskMap(Press M in game as crewmate)
/// </summary>
public class TaskMapManager : MonoBehaviour
{


    public GameObject taskMap;
    public Image taskMarker;
    private bool isEnabled;

    private static Dictionary<string, Vector2> taskMarkersDict = new Dictionary<string, Vector2>();
    private static List<string> keyList = new List<string>();
    private static Image[] taskMarkersArray = new Image[0];
    void Start()
    {
        if (!Client.instance.isImpostor)//ONLUY CREWMATES HAVE TASKS!
        {
            taskMarkersDict = GetTaskmarkersDict(Client.instance.myId);
            keyList = new List<string>(taskMarkersDict.Keys);// a list containing all the strings (keys) - used to get id
            taskMarkersArray = new Image[keyList.Count];

            InitializeMarkers();
        }

    }
    void Update()
    {
        if (!Client.instance.isImpostor)
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                taskMap.SetActive(!isEnabled);
                isEnabled = !isEnabled;
            }
            if (isEnabled)
            {
                UpdateMarkers();
            }
        }


    }

    /// <summary>
    /// sinitializes markers for the first time
    /// </summary>
    void InitializeMarkers()
    {
        int i = 0;
        foreach (var kvp in taskMarkersDict)
        {
            taskMarkersArray[i] = Instantiate(taskMarker, taskMap.transform);
            taskMarkersArray[i].transform.localPosition = kvp.Value;
            if (kvp.Key.ToLower().Contains("upload")|| kvp.Key.ToLower().Contains("accept"))
            {
                taskMarkersArray[i].transform.localRotation = Quaternion.Euler(0f,0f,180f);
            }
            i++;
        }
    }


    /// <summary>
    /// updates markers on the task map
    /// </summary>
    void UpdateMarkers()
    {
        foreach (var kvp in taskMarkersDict)
        {
            if (Interact.myTasks[kvp.Key])
            {
                taskMarkersArray[keyList.IndexOf(kvp.Key)].enabled = false;
            }
        }
    }

    /// <summary>
    /// gives the locations of the task markers and their related task name
    /// </summary>
    /// <param name="_myId">used to determant what are my task</param>
    /// <returns>Dictionery containing what are my tasks and the markers locations</returns>
    public static Dictionary<string, Vector2> GetTaskmarkersDict(int _myId)
    {
        int _taskSequence = _myId % 3;
        switch (_taskSequence)
        {
            case 0:
                Dictionary<string, Vector2> sequence0 = new Dictionary<string, Vector2>()
                {
                    {"DivertPower_Electrical", new Vector2(-167.8f,-44.8f)},
                    {"AcceptPower_Navigation", new Vector2(546.8f,73.4f)},
                    {"DownloadData_Cafeteria", new Vector2(176f,232.5f)},
                    {"UploadData_Admin", new Vector2(158.5f,-14.9f)},
                    {"MadBayScan",new Vector2(-136f,6f) }
                };
                return sequence0;
            case 1:
                Dictionary<string, Vector2> sequence1 = new Dictionary<string, Vector2>()
                {
                    {"DivertPower_Comunications", new Vector2(240.7f,-198.8f)},
                    {"AcceptPower_Weapons", new Vector2(390.1f,175.8f)},
                    {"DownloadData_Electrical", new Vector2(-200.7f,-38.9f)},
                    {"UploadData_Navigation", new Vector2(551.2f,73.7f)},
                };
                return sequence1;
            case 2:
                Dictionary<string, Vector2> sequence2 = new Dictionary<string, Vector2>()
                {
                    {"DivertPower_Shields", new Vector2(381.5f,-102.6f)},
                    {"AcceptPower_Security", new Vector2(-286.3f,61f)},
                    {"DownloadData_Comunications", new Vector2(164.5f,-191.8f)},
                    {"UploadData_Weapons", new Vector2(309.5f,219.3f)},
                };
                return sequence2;
            default:
                Debug.Log("Error getting tasks sequence!");
                return null;
        }
    }


}
