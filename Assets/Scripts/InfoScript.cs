using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class InfoScript : NetworkBehaviour
{
  

  public void Test1()
  {
        RunTestServer();
  }

  [Command]
  public void RunTestServer()
  {
        RunTestClients();
  }

  [ClientRpc]
  public void RunTestClients()
  {
          Debug.Log("Testing server messages");

  }

}
