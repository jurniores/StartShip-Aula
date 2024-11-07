using System.Collections;
using System.Collections.Generic;
using Omni.Core;
using Omni.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetManagerClient : ClientBehaviour
{

    // Update is called once per frame
    void Update()
    {

    }

    public void Login()
    {
        Local.Invoke(ConstantsGame.LOGIN_GAME);
        print("Enviei login");
    }

    [Client(ConstantsGame.LOGIN_GAME)]
    async void RPCCLientLogin()
    {
        await NetworkManager.LoadSceneAsync(1);
    }

}
