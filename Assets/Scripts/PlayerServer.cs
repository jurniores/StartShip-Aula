using System.Collections;
using System.Collections.Generic;
using Omni.Core;
using UnityEngine;

public class PlayerServer : NetworkBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    

    [Server(ConstantsGame.MOVE_PLAYER)]
    void MoveServerRPC(DataBuffer buffer)
    {
        Remote.Invoke(ConstantsGame.MOVE_PLAYER, buffer, target: Target.GroupMembersExceptSelf);
    }
    [Server(ConstantsGame.SHOT_PLAYER)]
    void TiroRPc(DataBuffer buffer)
    {
        Remote.Invoke(ConstantsGame.SHOT_PLAYER, buffer, target: Target.GroupMembersExceptSelf);
    }
    [Server(ConstantsGame.STOP_SHOT_PLAYER)]
    void StopTiroRPc(DataBuffer buffer)
    {
        Remote.Invoke(ConstantsGame.STOP_SHOT_PLAYER, buffer, target: Target.GroupMembersExceptSelf);
    }
}
