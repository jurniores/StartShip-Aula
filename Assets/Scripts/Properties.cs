using System.Collections;
using System.Collections.Generic;
using Omni.Core;
using Omni.Threading.Tasks;
using UnityEngine;

public partial class Properties : NetworkBehaviour
{
    private float hpTotal = 100;
    [NetworkVariable]
    [SerializeField]
    private float m_Hp = 100;
    [SerializeField]
    private BillboardCanvas canvasHp;
    const float dano = 10;
    private byte team = 0;
    Controller controller;
    Spawn spawn1, spawn2;
    public byte Team
    {
        get => team;
        set
        {
            team = value;
            if (IsClient)
            {
                controller.SetColorTeam(value);
                spawn1 = NetworkService.Get<Spawn>("Spawn0");
                spawn2 = NetworkService.Get<Spawn>("Spawn1");
                SpawnPosition();
            }

        }
    }

    protected override void OnStart()
    {
        if (IsClient)
        {
            controller = Identity.Get<Controller>();
        }
    }


    [Server(ConstantsGame.DEMAGE_IN_PLAYER)]
    void DemageRPCServer(DataBuffer buffer)
    {
        Hp -= dano;
        if (Hp <= 0)
        {
            Hp = hpTotal;
        }
    }

    [Server(ConstantsGame.HEAD_SHOT)]
    void HeadShotRPC(DataBuffer buffer)
    {
        Hp -= 100;
        if (Hp <= 0)
        {
            Hp = hpTotal;
        }
    }

    async partial void OnHpChanged(float prevHp, float nextHp, bool isWriting)
    {
        if (isWriting == false)
        {
            if (nextHp <= 0)
            {
                controller.enabled = false;

                SpawnPosition();
                await UniTask.WaitForSeconds(1);
                controller.enabled = true;
            }
            else
            {
                canvasHp.HPCanvas(nextHp, hpTotal);
            }
        };

    }

    void SpawnPosition()
    {
        if (team == 0)
        {
            spawn1.SetPlayerSpawn(transform);
        }
        else
        {
            spawn2.SetPlayerSpawn(transform);
        }
        controller.move = transform.position;
    }
}
