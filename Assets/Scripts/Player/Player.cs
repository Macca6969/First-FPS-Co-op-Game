using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using TMPro;

public class Player : NetworkBehaviour
{

   #region Variables

  [SyncVar]
  private bool _isDead = false;
  public bool isDead 
  {
    get { return _isDead; }
    protected set { _isDead = value; }
  }

  [SerializeField] private bool firstSetup = true;

  [SerializeField] private int playerMaxHealth = 100;

  [SyncVar] private int playerCurrentHealth;

  [SerializeField] private Behaviour[] disableOnDeath;
  [SerializeField] private bool [] wasEnabled;

  [SerializeField] private CharacterController characterController;
  [SerializeField] private Camera cam;
  [SerializeField] public GameObject playerUI;
  [SerializeField] private PlayerController playerController;
  [SerializeField] private Pistol pistol;
  [SerializeField] private GameObject rightHand;
  [SerializeField] private AudioScript audioScript;

  [Header("UI")]
  [SerializeField] Image healthBarImage;
  [SerializeField] private GameObject healthTextUI;

  #endregion



  #region Player Setup
  public void SetupPlayer()
  {
      if(isLocalPlayer)
      {
      cam.gameObject.SetActive(true);
      playerUI.gameObject.SetActive(true);
      }

       CmdBroadCastNewPlayerSetup();
  }

  [Command(requiresAuthority = false)]
  private void CmdBroadCastNewPlayerSetup()
  {
      RpcSetupPlayerOnAllClients();
  }

[ClientRpc]
private void RpcSetupPlayerOnAllClients()
{
  if (firstSetup)
  {
   wasEnabled = new bool[disableOnDeath.Length];
      for (int i = 0; i < wasEnabled.Length; i++)
      {
           wasEnabled[i] = disableOnDeath[i].enabled;
      }
      firstSetup = false;
  }

       SetDefaults();
}

public void SetDefaults ()
{
  isDead = false;

  if (isDead == false) 
      {
        playerController.enabled = true;
        pistol.enabled = true;
      }

    Cursor.lockState = CursorLockMode.Locked;

    playerCurrentHealth = playerMaxHealth;
    healthTextUI.GetComponent<TMP_Text>().text = playerCurrentHealth + "/" + playerMaxHealth;
    healthBarImage.fillAmount = (playerCurrentHealth + 0.0f) / (playerMaxHealth + 0.0f);

    characterController = GetComponent<CharacterController>();
    characterController.enabled = true;
    
    Debug.Log(transform.name + "character controller enabled.");
   
    //Enable the components
    for( int i = 0; i < disableOnDeath.Length; i++)
    {
      disableOnDeath[i].enabled = wasEnabled[i];
    }

   //Enable the collider
   Collider _col = GetComponent<Collider>();
   _col.enabled = true;
        Debug.Log("Collider enabled.");   

}

private void Start()
 {
    playerCurrentHealth = playerMaxHealth;
 }


#endregion

#region Player Damage, death, respawn

[Command(requiresAuthority = false)]
public void CmdTakeDamage (int _amount)
{
  if (isDead)
      return;


    playerCurrentHealth -= _amount;
    healthBarImage.fillAmount = (playerCurrentHealth + 0.0f) / (playerMaxHealth + 0.0f);
    healthTextUI.GetComponent<TMP_Text>().text = playerCurrentHealth + "/" + playerMaxHealth;

    StartCoroutine(PlayerHitSound());
             
              IEnumerator PlayerHitSound()
              {
              yield return new WaitForSeconds(0.3f);
              Debug.Log("Playing hit marker sound.");
              audioScript.playerHitSound.Play();
              }

    Debug.Log(transform.name + "now has " + playerCurrentHealth + "health.");

    if (playerCurrentHealth <= 0)
    {
        Die();
    }
}


private void Die()
  {
    isDead = true;

    for (int i = 0; i < disableOnDeath.Length; i++)
    {
      disableOnDeath[i].enabled = false;
    }

     Collider _col = GetComponent<Collider>();
    if (_col != false)
        _col.enabled = false;
        Debug.Log("Collider disabled.");

    Debug.Log (transform.name + " is DEAD!");

    rightHand.gameObject.SetActive(false);



    if (isDead == true) 
      {
        playerController.enabled = false;
        pistol.enabled = false;
      }
    

    
    StartCoroutine(Respawn());
  }

  private IEnumerator Respawn ()
  {
     yield return new WaitForSeconds (GameManager.instance.matchSettings.respawnTime);
     
     Debug.Log("starting respawn.");

     transform.position = NetworkManager.singleton.GetStartPosition().position;
     transform.rotation = Quaternion.Euler(0, 0, 0);
     rightHand.gameObject.SetActive(true);

     yield return new WaitForSeconds(0.1f);

     SetupPlayer();
 

     Debug.Log(transform.name + " respawned.");
  }

#endregion

}
