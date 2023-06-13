using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Threading;
using TMPro;


[Serializable]
public class Interact : MonoBehaviour
{
    #region vars
    //Vector3[] mapSpwanLocations = {new Vector3(221.13f, y, 244.75f),new Vector3(221.31f, y, 235.15f),new Vector3(215.37f,y,240.23f),new Vector3(225.05f, y, 240.01f),new Vector3(216.51f, y, 242.99f),new Vector3(224.17f, y, 236.71f),new Vector3(223.54f, y, 243.26f),new Vector3(215.79f, y, 237.4f),new Vector3(218.7f, y, 244.6f),new Vector3(218.08f, y, 235.44f) };
    private int killCooldown = 20;
    public float interactRange = 3f;
    public GameObject playerManager;
    private int playerBuffer = GameManager.IMPOSTORS*2 + 2; // how many players are needed to start the game
    public GameObject roleScreen;
    public static GameObject menue;
    public GameObject instantiateRoleScreenHere;
    public static float taskProgress = 0f;

    private GameObject resultScreen;
    public static string gameStatus = "gaming";
    public Sprite crewmateVictoryScreen;
    public Sprite impostorVictoryScreen;

    //layer masks
    public LayerMask startGameMask;
    public LayerMask playerMask;
    public LayerMask madBayScan;
    public LayerMask deadBodyMask;
    public LayerMask butonMask;
    public LayerMask taskMask;


    //conditons
    private bool inLobby = true;
    private bool oneTimeScreenFlag = true;
    private bool distributedRolesAndTasks = false;
    private bool gameEnded = false;
    public static bool meetingInProgress = false;



    private bool canKill = true;
    public static int killCoolDownTimer = 0;
    

    public static GameObject UIMenu;
    public static Dictionary<string, bool> myTasks;
    public static GameObject meetingScreenObject;
    private TextMeshProUGUI killCooldownDisplay;

    //pointers to give a refrence to static object from the editor
    public GameObject meetingScreenPointer;
    public static GameObject meetingScreen;
    public GameObject playerUiPointer;
    public static GameObject playerUI;
    public ParticleSystem madBayParticalesPointer;
    public static ParticleSystem madBayParticales;
    public TextMeshProUGUI taskLogUIPointer;
    public static TextMeshProUGUI taskLogUI;

#endregion
    private void Start()
    {
        UIMenu = GameObject.Find("Menu");
        playerUI = playerUiPointer;
        madBayParticales = madBayParticalesPointer;
        taskLogUI = taskLogUIPointer;
        meetingScreen = meetingScreenPointer;
    }
    void Update()
    {
        //still in lobby
        if (inLobby && playerManager.GetComponent<PlayerManager>().id == 1 && Input.GetKeyDown(KeyCode.E) && Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit startGameHit, interactRange, startGameMask) /*&& GameManager.players.Count >= playerBuffer*/)
        {
            ClientSend.GetRoles(GameManager.IMPOSTORS);
            inLobby = false;
        }

        // game started
        if (ClientHandle.HandledRoles)
        {
            // display role and give tasks
            if (oneTimeScreenFlag)
            {
                GameObject _roleScreen = Instantiate(roleScreen,instantiateRoleScreenHere.transform); // displaying the role the user got
                _roleScreen.transform.parent = menue.transform;
                _roleScreen.transform.localPosition = new Vector2(0, 0);
                oneTimeScreenFlag = false;

                ClientSend.TeleportToMap();// teleporting players to map

                if (!Client.instance.isImpostor)
                {
                    Destroy(playerUI.transform.GetChild(5).gameObject);
                    myTasks = GetTaskSequence(Client.instance.myId); // setting player's taks
                    UpdateTaksLog();//printing tasks to the screen
                }
                else
                {
                    killCooldownDisplay = playerUI.transform.GetChild(5).gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                }
                resultScreen = playerUI.transform.GetChild(9).gameObject;


                distributedRolesAndTasks = true;
                Destroy(instantiateRoleScreenHere);
            }

            //game is still going
            if (distributedRolesAndTasks && !gameEnded)
            {
                //updating game status
                if (gameStatus == "crewmatewin")
                {
                    gameEnded = true;
                    SoundManager.PlaySound("CrewmateVictory");
                    resultScreen.SetActive(true);
                    resultScreen.GetComponent<Image>().sprite = crewmateVictoryScreen;
                }
                else if (gameStatus == "impostorwin")
                {
                    gameEnded = true;
                    SoundManager.PlaySound("ImpostorVictory");
                    resultScreen.SetActive(true);
                    resultScreen.GetComponent<Image>().sprite = impostorVictoryScreen;
                }


                #region Everyone
                if (transform.parent.GetComponent<PlayerManager>().isAlive)
                {
                    #region Report
                    if (Input.GetKeyDown(KeyCode.R) && Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit reportInfo, interactRange, deadBodyMask))
                    {
                        //sending this incremented value so the server knows this is a report and not a button meeting
                        Debug.Log("deadBody belonged to plaeyr: "+reportInfo.transform.GetComponent<DeadBodyManager>().id);
                        ClientSend.CallEmergencyMeeting((reportInfo.transform.GetComponent<DeadBodyManager>().id*10)+10);
                    }
                    #endregion

                    #region EmergencyMeeting
                    if (!meetingInProgress && Input.GetKeyDown(KeyCode.E) && Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit meetingInfo, interactRange, butonMask))
                    {
                        ClientSend.CallEmergencyMeeting(Client.instance.myId); // calling a meeting

                    }
                    if (ClientHandle.GotMeetingInfo) // a meeting has started
                    {
                        meetingInProgress = true;
                        ClientHandle.GotMeetingInfo = false;
                    }
                    #endregion

                }
                #endregion

                #region Impostors

                if (Client.instance.isImpostor && transform.parent.GetComponent<PlayerManager>().isAlive)
                {
                    // update kill cooldown
                    killCooldownDisplay.text = killCoolDownTimer.ToString();
                    if (killCoolDownTimer <= 0)
                    {
                        canKill = true;
                        killCooldownDisplay.text = "";
                    }

                    //killing
                    if (Input.GetKeyDown(KeyCode.Q) && Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit impostorHitInfo, interactRange, playerMask)&&canKill)
                    {
                        GameObject _playerhit = impostorHitInfo.collider.gameObject;
                        if (_playerhit.transform.parent.transform.parent.GetComponent<PlayerManager>().isAlive)
                        {
                            SoundManager.PlaySound("ImpostorKill");
                            ClientSend.AttemptKill(impostorHitInfo.transform.parent.parent.GetComponent<PlayerManager>().id);
                            canKill = false;
                            killCoolDownTimer = killCooldown;
                            InvokeRepeating("DecrementTimer", 0, 1);
                        }

                    }

                }
                #endregion

                #region CrewMates
                if (!Client.instance.isImpostor)
                {
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        //tring to start madbay?
                        if (CanStartMadBay())
                        {
                            Debug.Log("Accepted MadBay Attempt");
                            PlayerController.canMove = false;
                            ClientSend.PlayerStartMadBayScan();
                            myTasks["MadBayScan"] = true;
                            Invoke("FinishMadbayScan", 10f);//10 sc timer
                        }
                        //doing a task
                        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit taskHitInfo, interactRange, taskMask))//normal tasks
                        {
                            string interactedTask = taskHitInfo.transform.name;//getting the name of the interacted task
                            string[] nameAndLocation = interactedTask.Split('_');
                            string taskName = nameAndLocation[0];
                            string taskLocation = nameAndLocation[1];
                            Debug.Log("Attempt to start task detected: "+interactedTask);


                            bool _divertedPower = HasDivertedPower();
                            bool _downloadedData = HasDownloadedData();
                      
                            if (myTasks.ContainsKey(interactedTask) && !myTasks[interactedTask]) //0
                            {
                                //some tasks require more contidions we check those here
                                if ((taskName == "AcceptPower" && _divertedPower) || (taskName == "UploadData" && _downloadedData))//1
                                {
                                    StartTask(interactedTask, taskName);
                                }
                                else if (taskName != "AcceptPower" && taskName != "UploadData")//2
                                {
                                    StartTask(interactedTask, taskName);
                                }
                            }
                        }

                    }
                }
                #endregion
               
            }

        }



    }

    /// <summary>
    /// Gives the player tasks based on his ID
    /// </summary>
    /// <param name="_myId">player's ID</param>
    /// <returns>Dictionary containing tasks names and locations as keys and the task status (T/F) as value</returns>
    public static Dictionary<string, bool> GetTaskSequence(int _myId)
    {
        int _taskSequence = _myId % 3;
        Debug.Log("SEQUENCE: "+_taskSequence);
        switch (_taskSequence)
        {
            case 0:
                Dictionary<string, bool> sequence0 = new Dictionary<string, bool>()
                {
                    {"DivertPower_Electrical", false},
                    {"AcceptPower_Navigation", false},
                    {"DownloadData_Cafeteria", false},
                    {"UploadData_Admin", false},
                    {"MadBayScan",false }
                };
                return sequence0;
            case 1:
                Dictionary<string, bool> sequence1 = new Dictionary<string, bool>()
                {
                    {"DivertPower_Comunications", false},
                    {"AcceptPower_Weapons", false},
                    {"DownloadData_Electrical", false},
                    {"UploadData_Navigation", false},
                };
                return sequence1;
            case 2:
                Dictionary<string, bool> sequence2 = new Dictionary<string, bool>()
                {
                    {"DivertPower_Shields", false},
                    {"AcceptPower_Security", false},
                    {"DownloadData_Comunications", false},
                    {"UploadData_Weapons", false},
                };
                return sequence2;
            default:
                Debug.Log("Error getting tasks sequence!");
                return null;
        }
    }

    /// <summary>
    /// Starts a task the player requested to start
    /// </summary>
    /// <param name="_interactedTask">task name and location</param>
    /// <param name="_taskName">name of the task</param>
    public static void StartTask(string _interactedTask,string _taskName)
    {
        Debug.Log("StartTask() was called on task: " + _taskName);
        PlayerController.canMove = false; // disabling the player from moving
        CameraController.canLook = false; // disabling the player from moving his camera
        Cursor.lockState = CursorLockMode.None; // freeing the mouse
        Instantiate(Resources.Load<GameObject>("Tasks_n_UI/" + _taskName), UIMenu.transform); // showing the task
        myTasks[_interactedTask] = true;// updating task status
        UpdateTaksLog();
    }

    /// <summary>
    /// check if the players can start madbay -> standing on madbay and having that task
    /// </summary>
    /// <returns>T/F if he can start</returns>
    bool CanStartMadBay()
    {
        // Perform a raycast downwards from the player's position
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 20f, madBayScan) && myTasks.ContainsKey("MadBayScan")&& !myTasks["MadBayScan"])
            return true;
        return false;
    }

    /// <summary>
    /// updating task progress and and letting the player move after he finishes the medbay scam 
    /// </summary>
    void FinishMadbayScan()
    {
        PlayerController.canMove = true;
        SoundManager.PlaySound("TaskEnd");
        myTasks["MadBayScan"] = true;
        ClientSend.UpdateTaskProgressServer(); // letting the server know to increse the task counter
    }
    /// <summary>
    /// checks if the player did the task of diverting the power
    /// </summary>
    /// <returns>true or false if he did</returns>
    bool HasDivertedPower()
    {
        foreach (var kvp in myTasks)
        {
            if (kvp.Key.Contains("Divert"))
            {
                return kvp.Value;
            }
        }
        return false;
    }
    /// <summary>
    /// checks if the player did the task of downloading data
    /// </summary>
    /// <returns>true or false if he did</returns>
    bool HasDownloadedData()
    {
        foreach (var kvp in myTasks)
        {
            if (kvp.Key.Contains("Download"))
            {
                return kvp.Value;
            }
        }
        return false;
    }

    /// <summary>
    /// updates the task log
    /// </summary>
    static void UpdateTaksLog()
    {
        string taskLog = "Tasks: \n";
        foreach (var kvp in myTasks)
        {
            if (!kvp.Value)
            {
                taskLog = taskLog + kvp.Key + "\n";
            }
        }
        Interact.taskLogUI.text = taskLog;
    }

    /// <summary>
    /// decrements the kill cooldown timer
    /// </summary>
    public void DecrementTimer()
    {
        if (killCoolDownTimer>0)
        {
            killCoolDownTimer--;
        }
    }

}
