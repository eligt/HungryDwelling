using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstructionsStepBulldozer : InstructionsStep
{
    public override bool meetsCondition()
    {
        BulldozerSpawner spawner = BulldozerSpawner.Instance;
        GameObject container = spawner.spawnRoot;
        BulldozerControl[] bulldozers = container.GetComponentsInChildren<BulldozerControl>();
        
        return bulldozers.Length > 0;
    }
}
