using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public GameObject buttonPrefab;
    public Transform buttonParent;
    public Button distroyModeButten;
    public static UIManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        
        distroyModeButten.GetComponent<Button>().onClick.AddListener(() => dystroymode());
        AssinButtens();
    }


    public void AssinButtens()
    {
        for (int i = 0; i < StructureManager.Instance.structureDataList.Count; i++)
        {
            GameObject buttonInstance = Instantiate(buttonPrefab, buttonParent);
            
            TextMeshProUGUI buttonText = buttonInstance.GetComponentInChildren<TextMeshProUGUI>();

            TextMeshProUGUI NumText = buttonInstance.transform.Find("NumText")?.GetComponent<TextMeshProUGUI>();

            Image image = buttonInstance.GetComponent<Image>();

            StructureData structureData = StructureManager.Instance.structureDataList[i];
            
            int buildingLimit = StructureManager.Instance.buildingLimits[i];

            int structureId = structureData.Id;

            

            structureData.buildingLimit = buildingLimit ;
            

            if (structureData.previewImage != null)
            {
                buttonText.text = null;
                image.sprite = structureData.previewImage;
            }
            else if (structureData.structureName != null)
            {
                
               buttonText.text = structureData.structureName;
                
            }

            NumText.text = "You Have: " + structureData.buildingLimit.ToString();

            int structureIndex = i; // Store the current index for the lambda expression
            buttonInstance.GetComponent<Button>().onClick.AddListener(() => ButtonClicked(structureIndex, buildingLimit, NumText, structureId));
        }
    }
   
    private void ButtonClicked(int structureIndex,int builldLimt,TextMeshProUGUI NumText, int structureId)
    {
        StructureManager.currentStructureId = StructureManager.Instance.structureDataList[structureIndex].Id;
        StructureManager.currentStructureIndex = structureIndex;

        int structureCount = BuildingManager.Instance.GetAvailableCapacity(structureId,builldLimt);
        NumText.text = "You Have: " + structureCount.ToString();

        Debug.Log("Button with index " + structureIndex  +" Button with ID " + StructureManager.currentStructureId  + " you have "+ builldLimt +" buildes" + " was clicked." );
        _Grid.distroyMode = false;
    }
    public void UpdateNumText(int structureIndex, int structureId, int buildingLimit)
    {

        TextMeshProUGUI NumText = buttonParent.GetChild(structureIndex).Find("NumText")?.GetComponent<TextMeshProUGUI>();
        if (NumText != null)
        {
            int structureCount = BuildingManager.Instance.GetAvailableCapacity(structureId, buildingLimit);
            NumText.text = "You Have: " + structureCount.ToString();
        }
    }


    private void dystroymode()
    {

      _Grid.distroyMode = !_Grid.distroyMode;
        Debug.Log("is " + _Grid.distroyMode);

    }
}
