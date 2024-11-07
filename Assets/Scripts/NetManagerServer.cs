using System.Collections;
using System.Collections.Generic;
using Omni.Core;
using UnityEngine;

public class NetManagerServer : ServerBehaviour
{
    [SerializeField]
    private Dictionary<int, PlayerConfigs> entityList = new();
    private Dictionary<int, NetworkIdentity> identityList = new();
    NetworkGroup group;
    protected override void OnStart()
    {
        group = NetworkManager.Matchmaking.Server.AddGroup("newGame");
    }

    protected override void OnServerPeerDisconnected(NetworkPeer peer, Phase phase)
    {
        if (entityList.ContainsKey(peer.Id))
        {
            entityList.Remove(peer.Id);
            identityList[peer.Id].Destroy();
            identityList.Remove(peer.Id);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    [Server(ConstantsGame.LOGIN_GAME)]
    void RpcServer(DataBuffer buffer, NetworkPeer peer)
    {
        Remote.Invoke(ConstantsGame.LOGIN_GAME, peer, target: Target.Self);
    }

    void SetPlayer(NetworkPeer peer)
    {

        NetworkIdentity identity = NetworkManager.GetPrefab(0).SpawnOnServer(peer);
        Properties properties = identity.Get<Properties>();
        
        int team = entityList.Count % 2;
        properties.Team = (byte)team;

        var configPlayer = new PlayerConfigs()
        {
            peerId = peer.Id,
            IdentityId = identity.IdentityId,
            team = (byte)team
        };

        NetworkManager.Matchmaking.Server.JoinGroup(group, peer);
        using var playerBuffer = NetworkManager.Pool.Rent();
        playerBuffer.Write(configPlayer);
        Remote.Invoke(ConstantsGame.PLAYER_LOGGER, peer, playerBuffer, Target.GroupMembers);

        //

        using var buffer = NetworkManager.Pool.Rent();
        buffer.WriteAsJson(entityList);
        Remote.Invoke(ConstantsGame.PLAYER_LOGGER_ALL, peer, buffer, Target.Self);

        entityList.Add(peer.Id, configPlayer);
        identityList.Add(peer.Id, identity);
    }

    [Server(ConstantsGame.PLAYER_LOGGER)]
    void SetPlayerLoggeR(DataBuffer buffer, NetworkPeer peer)
    {
        SetPlayer(peer);
    }



}
public struct PlayerConfigs
{
    public int peerId;
    public int IdentityId;
    public byte team;
}