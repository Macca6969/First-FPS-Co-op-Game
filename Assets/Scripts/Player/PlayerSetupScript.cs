using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using Mirror;

[RequireComponent(typeof(Player))]
public class PlayerSetupScript : NetworkBehaviour
{



public InputController inputController;
public PlayerController playerController;
public PlayerLook playerLook;
public Camera cam;
public GameObject playerUI;
public AudioListener playerListener;


private void Start() 
{
   /* if (!isLocalPlayer)
    {
      cam.gameObject.SetActive(false);
      playerUI.gameObject.SetActive(false);
    }
    else
    {
          GetComponent<Player>().PlayerSetup();
    }
*/


GetComponent<Player>().SetupPlayer();


}


public override void OnStartClient()
{
    base.OnStartClient();

    string _netID = GetComponent<NetworkIdentity>().netId.ToString();
    Player _player = GetComponent<Player>();

    GameManager.RegisterPlayer(_netID, _player);
}


public override void OnStartLocalPlayer()
{

//disable by default on player prefab
  
  inputController.enabled = true;
  playerController.enabled = true;
  playerLook.enabled = true;
  playerListener.enabled = true;
}

private void OnDisable()
{
    GameManager.UnRegisterPlayer (transform.name);
    Destroy(playerUI);

}


}

