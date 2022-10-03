using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class AudioScript : NetworkBehaviour
{
  
    [SerializeField] public AudioSource playerHitSound;
    [SerializeField] public AudioSource pistolReload;
    [SerializeField] public AudioSource pistolEmpty;
    [SerializeField] public AudioClip pistolShot2; 
    [SerializeField] public AudioSource pistolShot;

    [Command(requiresAuthority = false)]
    public void CmdPlayerSounds(Vector3 playerPosition, string _playerID)
    {
         RpcPlayerSounds(playerPosition, _playerID);
    }

    [ClientRpc]
    public void RpcPlayerSounds(Vector3 playerPosition, string _playerID)
    {

        AudioSource.PlayClipAtPoint(pistolShot2, playerPosition, 1);
        Debug.Log("Playing pistol shot audio to everyone.");
        Player _player = GameManager.GetPlayer(_playerID);
        //AudioSource.Play(pistolShot);
        
    }

}
