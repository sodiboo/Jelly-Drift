using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeCheckpointUser : CheckpointUser
{
    CheckpointUser parent;
    private void Awake()
    {
        parent = ChaosController.Instance.car.GetComponent<CheckpointUser>();
        checkedPoints = parent.checkedPoints;
        player = parent.player;
    }
}
