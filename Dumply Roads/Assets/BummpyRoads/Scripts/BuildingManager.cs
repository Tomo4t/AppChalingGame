using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    private Dictionary<int, int> structureCounts = new Dictionary<int, int>();

    public static BuildingManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void IncrementStructureCount(int structureId)
    {
        if (structureCounts.ContainsKey(structureId))
        {
            structureCounts[structureId]++;
        }
        else
        {
            structureCounts.Add(structureId, 1);
        }
    }

    public void DecrementStructureCount(int structureId)
    {
        if (structureCounts.ContainsKey(structureId) && structureCounts[structureId] > 0)
        {
            structureCounts[structureId]--;
        }
    }

    public int GetAvailableCapacity(int structureId, int buildingLimit)
    {
        if (structureCounts.ContainsKey(structureId))
        {
            int currentCount = structureCounts[structureId];
            return buildingLimit - currentCount; // Calculate and return available capacity
        }
        return buildingLimit; // Default available capacity if not found (full limit)
    }


    public bool CanBuildStructure(int structureId, int buildingLimit)
    {
        if (!structureCounts.ContainsKey(structureId))
        {
            return true; // No structures of this type have been built yet
        }

        return structureCounts[structureId] < buildingLimit;
    }
}
