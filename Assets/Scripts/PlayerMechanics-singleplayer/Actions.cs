using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//in case of detection problems make raycast before keypress
public class Actions : MonoBehaviour
{
    public float interactRange = 1f;
    public bool isImposter = true; // default value
    public Transform groundCheck;

    public LayerMask crewMateMask;
    public LayerMask groundMask;
    public LayerMask ventMask;
    public LayerMask deadBodyMask;
    public LayerMask butonMask;
    public LayerMask taskMask;

    public GameObject UI;

    private readonly string[] tasksNames = { "Empty_Garbage", "Clean_O2_Filter", "Swipe_Card", "Download_Data(x)", "Calibrate_Distributor", "Fix_Wiring", "Align_Engine_Output", "Chart_Course", "Stabilize_Steering", "Unlock_Manifolds", "Start_Reactor","Prime_Shields","Fuel_Engines","Clear_Astroids", "Divert_Power", "Accept_Power", "Submit_Scan"};
    //dont remove this array  ^^

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (isImposter == true)
            {
                isImposter = false;
            }
            else
            {
                isImposter = true;
            }
        }
        #region inpostor
        if (isImposter)
        {
            RaycastHit hit;// this var is used to save the raycast hit INFO
            //(Input.GetMouseButton(0) || Input.GetKeyDown(KeyCode.Q)) && Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, interactRange, crewMateMask))
            //<kill>
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, interactRange, crewMateMask)&& Input.GetKeyDown(KeyCode.Q))
            {
                HealthStatus target = hit.transform.GetComponent<HealthStatus>();
                if (target != null)
                {
                    target.Kill();
                }
            }

            //<vent>

            // projecting a sphere from the player's legs to see if he is standing on a vent
            if (Input.GetKeyDown(KeyCode.V) && Physics.CheckSphere(groundCheck.position, (interactRange / 2), groundMask))
            {
                // ADD VENT
                Debug.Log("Vented");
            }
        }
        #endregion
        #region Crew mate
        //Crew mate interactions
        if (!isImposter)
        {
            RaycastHit hit;// this var is used to save the raycast hit INFO
            //<tasks>
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, interactRange, taskMask) && Input.GetKeyDown(KeyCode.E) )
            {
                string interactedTask = hit.transform.name;
                print(interactedTask);
                Instantiate(Resources.Load<GameObject>("Tasks_n_UI/" + interactedTask), UI.transform);

                Cursor.lockState = CursorLockMode.None;
                GetComponent<playerMovment>().enabled = false;
                transform.Find("Main Camera").GetComponent<CameraLook>().enabled = false;

                if (interactedTask == "Submit Scan")
                {
                    //scan
                }
            }
        }

        #endregion

        //Both can ineract with
        //<Dead-Bodys>
        if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.R)) && Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit hitReport, interactRange, deadBodyMask))
        {
            //hit works normaly it is declared in the if statment (named hitReport).

            /* TODO:
             * Remove dead body
             * Start emergency meeting
             * show player's name with an X on it (signifying deth)
             * show an icon next to the reporter's name
             */
            Debug.Log("DEAD BODY REPORTED");
        }

        //<Meetings>
        if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.E)) && Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit hitMeeting, interactRange, butonMask))
        {
            //hit is na,e hitMeeting
            /*
             * TODO:
             * Clear all dead bodys in the map
             * Teleport everyone to middle
             * start meeting
             * AFTER METING ENDS
             * Restart cooldowns
             */
            Debug.Log("STARTED MEETING");
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        if (Cursor.lockState == CursorLockMode.Locked)
        {
            GetComponent<playerMovment>().enabled = true;
            transform.Find("Main Camera").GetComponent<CameraLook>().enabled = true;
        }

        
    }

   

}
