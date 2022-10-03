using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using Mirror;

[RequireComponent(typeof(Animator))]
public class Pistol : NetworkBehaviour
{
#region Variables ~
    [Header("PlayerFire")]
    [SerializeField] public GameObject pistolObj;
    [SerializeField] public bool isFiring = false;
    [SerializeField] public bool isReloading = false;
    [SerializeField] public GameObject muzzleFlash;
    [SerializeField] public AudioSource pistolShot;

    [SerializeField] static float distantFromTarget;
    [SerializeField] float toTarget;
    [SerializeField] public Camera cam;
    public bool isDead;

     [Header("AmmoDisplay")]
    [SerializeField] public int pistolCurrentAmmo = 10;
    [SerializeField] public int pistolMaxAmmo = 50;

    [SerializeField] public int pistolMagSize = 7;
    public GameObject ammoTextUI;
    public float pistolReloadSpeed = 0.8f;
    [SerializeField] public bool needReload;

    [Header("Pistol")]

    [SerializeField] public int pistolDamage = 10;
    public string _ID;

    [Header("Shooting")]

    private Animator Animator;

    [Header("Health")]
    public Player player;
    public GameObject healthTextUI;
   public AudioScript audioScript;

   #endregion



    private void Awake()
     {
        Animator = GetComponent<Animator>();
     }

    
    #region Player Shoots

      public void FirePistol()
   {

        //Run checks, and shoot the pistol.
        if (!isFiring && pistolCurrentAmmo >= 1 && !player.isDead) 
         {
            StartCoroutine(FirePistol());
             string playerShooting = transform.name;
             Debug.Log(playerShooting + "is shooting.");
             Vector3 playerLocation = transform.position;
             Player _playerID = GameManager.GetPlayer(_ID);
             //CmdPlayerSounds(playerLocation, _playerID);
         }

         //Out of ammo.
       if(!isFiring && pistolCurrentAmmo >=0 && !player.isDead)
        {
            audioScript.pistolEmpty.Play();
        }


        IEnumerator FirePistol()
         {

         RaycastHit _hit;
            if(Physics.Raycast(cam.transform.position, cam.transform.forward, out _hit))
            {
               if(_hit.collider.tag == "Player")
              {
                  Debug.Log("We hit " + _hit.collider.gameObject.name);
                  CmdPlayerHit(_hit.collider.name, pistolDamage);
              }
            }
         
        isFiring = true;
        pistolObj.GetComponent<Animator>().Play("FirePistol");
        //audioScript.pistolShot.Play();
        muzzleFlash.SetActive(true);
        yield return new WaitForSeconds(0.05f);
        muzzleFlash.SetActive(false);
        yield return new WaitForSeconds(0.15f);
        pistolObj.GetComponent<Animator>().Play("IdlePistol");
        pistolCurrentAmmo = pistolCurrentAmmo - 1;
        ammoTextUI.GetComponent<TMP_Text>().text =  pistolCurrentAmmo + "/" + pistolMaxAmmo;
        isFiring = false;
         }

   }

         
         [Command(requiresAuthority = false)]
         void CmdPlayerHit(string _playerID, int _damage)
         {
              Debug.Log(_playerID + " has been shot.");

              Player _player = GameManager.GetPlayer (_playerID);
              _player.CmdTakeDamage(_damage);

         }

         #endregion

    #region Player Reloading

         
        public void PistolReload()
        {
            //Tell the server we want to reload
            CmdPlayerReloading();
        }

        [Command(requiresAuthority = false)]
        public void CmdPlayerReloading()
        {
              //Let the player reload and notify everyone else they are, play sounds etc..
              RpcPlayerReloading();
        }

        [ClientRpc]
        public void RpcPlayerReloading()
         {
            if (isLocalPlayer && !isReloading)
           StartCoroutine(Reload());
           IEnumerator Reload()
           {
            Debug.Log ("Reload");
            isReloading = true;
            pistolObj.GetComponent<Animator>().Play("Reload");
            audioScript.pistolReload.Play();
            yield return new WaitForSeconds (pistolReloadSpeed);
            audioScript.pistolReload.Stop();
            pistolObj.GetComponent<Animator>().Play("IdlePistol");
            pistolCurrentAmmo = pistolMagSize;
            ammoTextUI.GetComponent<TMP_Text>().text =  pistolCurrentAmmo + "/" + pistolMaxAmmo;
            needReload = false;
            isReloading = false;
            
           }
         }

        #endregion


}
   

