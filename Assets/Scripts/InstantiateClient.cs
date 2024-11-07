using System.Collections;
using System.Collections.Generic;
using Omni.Core;
using Omni.Threading.Tasks;
using UnityEngine;

public class InstantiateClient : ClientBehaviour
{
    [SerializeField]
    private NetworkIdentity prefabPlayer;
    protected override void OnStart()
    {
        Local.Invoke(ConstantsGame.PLAYER_LOGGER);
    }

    // Update is called once per frame
    void Update()
    {

    }

    [Client(ConstantsGame.PLAYER_LOGGER)]
    void RPCLoggerClient(DataBuffer buffer)
    {
        var entityPlayer = buffer.Read<PlayerConfigs>();
        Properties propPlayer = prefabPlayer.SpawnOnClient(entityPlayer.peerId, entityPlayer.IdentityId).Get<Properties>();
        print("Entity Client: "+entityPlayer.team);
        propPlayer.Team = entityPlayer.team;

    }
    [Client(ConstantsGame.PLAYER_LOGGER_ALL)]
    void RPCLoggerClientAll(DataBuffer buffer)
    {
        var listPlayer = buffer.ReadAsJson<Dictionary<int, PlayerConfigs>>();

        foreach (var player in listPlayer)
        {
            Properties propPlayer = prefabPlayer.SpawnOnClient(player.Key, player.Value.IdentityId).Get<Properties>();

            propPlayer.Team = player.Value.team;
        }
    }
}
