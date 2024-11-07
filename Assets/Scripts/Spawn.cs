using System.Collections;
using System.Collections.Generic;
using Omni.Core;
using UnityEngine;

public class Spawn : ServiceBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private List<Transform> listSpawn;

    public void SetPlayerSpawn(Transform transform)
    {
        var range = Random.Range(0, listSpawn.Count);
        transform.position = listSpawn[range].position;
    }
}
