using TMPro;
using UnityEngine;


public class PlayerManager : MonoBehaviour
{

    public Material[] colors = new Material[10];//make sure colors are matched with Gamemanager's colors[]

    public int id;
    public string username;
    public MeshRenderer playerBody;
    public MeshRenderer playerBackpack;
    public TextMeshProUGUI playerName;

    public bool isAlive = true;
    public GameObject deadBody;


    /// <summary>
    /// Method responsable for killing the payer
    /// </summary>
    /// <param name="_isMe">if the player's we want to kill id matches my id</param>
    /// <param name="_wayOfDying">the way the player died [Ejected/Killed] to know if we need to create a dead body</param>
    /// <param name= "_targetID"> we give this to the id so we know who was reported
    public void Die(string _wayOfDying,bool _isMe, int _targetID)
    {
        isAlive = false;

        if (_wayOfDying.ToLower() == "killed")
        {
            Vector3 playerPos = this.transform.position;
            //getting players rotation
            Quaternion playerRotation = this.transform.rotation;
            Material bodyColor;
            if (_isMe)
            {
                SoundManager.PlaySound("CrewmateKill");
                Instantiate(Resources.Load<GameObject>("Tasks_n_UI/" + "Death_Screen"), Interact.playerUI.transform);
                //Getting player color
                bodyColor = colors[id - 1];
            }
            else
            {
                //Getting player color
                bodyColor = playerBackpack.material;
            }

            //applying color to the body
            deadBody.GetComponentInChildren<Renderer>().material = bodyColor;
            //spawning body
            
            GameObject instantiatedDeadBody = Instantiate(deadBody, playerPos, playerRotation); 
            instantiatedDeadBody.transform.GetComponentInChildren<DeadBodyManager>().id = _targetID;
        }

        playerBody.enabled = false;
        playerBackpack.enabled = false;
        playerName.enabled = false;

    }
}


