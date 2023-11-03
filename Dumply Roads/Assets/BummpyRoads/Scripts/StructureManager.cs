using System.Collections.Generic;
using UnityEngine;

public class StructureManager : MonoBehaviour
{
    public static StructureManager Instance;

    
    public List<StructureData> structureDataList;
    public List<int> buildingLimits;

    public static int currentStructureIndex = 0;
    public static int currentStructureId = 0;



    private void Awake()
    {
        Instance = this;
    }

    public StructureData GetCurrentStructureData()
    {
        if (currentStructureIndex >= 0 && currentStructureIndex < structureDataList.Count)
        {
            return structureDataList[currentStructureIndex];
        }
        return null;
    }


}
