using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Structure", menuName = "Structure Data")]
public class StructureData : ScriptableObject
{
    public GameObject structurePrefab;
    public string structureName;
    public Sprite previewImage;
    public int Id;

    public int buildingLimit;

}
