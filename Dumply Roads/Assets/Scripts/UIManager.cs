
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class UIManager : MonoBehaviour
{
    public GameObject buttonPrefab;
    public Transform buttonParent;
    public Button distroyModeButten, startButten, PussButten, resumButten, restretButten, HomeButten;
    public static UIManager Instance;
    public Sprite retryButten;
    public GameObject pussPnell, winPanel;

    public static string currentSceneName;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        AssinButtens();

        currentSceneName = SceneManager.GetActiveScene().name;

        distroyModeButten.GetComponent<Button>().onClick.AddListener(() => dystroymode());

        startButten.GetComponent<Button>().onClick.AddListener(() => startGame());

        PussButten.GetComponent<Button>().onClick.AddListener(() => stopGame());

        resumButten.GetComponent<Button>().onClick.AddListener(() => resumeLevel());

        restretButten.GetComponent<Button>().onClick.AddListener(() => ResetLevel());

        HomeButten.GetComponent<Button>().onClick.AddListener(() => goHome());
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

       // Debug.Log("Button with index " + structureIndex  +" Button with ID " + StructureManager.currentStructureId  + " you have "+ builldLimt +" buildes" + " was clicked." );
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
    private void startGame()
    {
        
        CarMovement.GameStarted = true;
        startButten.GetComponent<Button>().image.sprite = retryButten;

        startButten.GetComponent<Button>().onClick.AddListener(() => ResetLevel());
    }
    private void stopGame() 
    {
       
        Time.timeScale = 0;
        LevelManager.gameispossed = true;
        PussButten.gameObject.SetActive(false);
        distroyModeButten.gameObject.SetActive(false);
        startButten.gameObject.SetActive(false);
        pussPnell.SetActive(true);
    }
    public void resumeLevel() 
    { 
        Time.timeScale = 1;
        LevelManager.gameispossed = false;
        pussPnell.SetActive(false );
        PussButten.gameObject.SetActive(true);
        distroyModeButten.gameObject.SetActive(true);
        startButten.gameObject.SetActive(true);
    }

    public void ResetLevel()
    {
        CarMovement.GameStarted= false;
        Time.timeScale= 1;
        SceneManager.LoadScene(currentSceneName);
    }
    public void goHome()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }

    private void dystroymode()
    {
        if (CarMovement.GameStarted == false)
        {
            _Grid.distroyMode = !_Grid.distroyMode;
            

        }

    }
    public void activeWinScrean(int stars)
    {
        buttonParent.gameObject.SetActive(false);
        distroyModeButten.gameObject.SetActive(false);
        startButten.gameObject.SetActive(false);
        PussButten.gameObject.SetActive(false);
        winPanel.SetActive(true);
        Debug.Log(stars);
    }
}
