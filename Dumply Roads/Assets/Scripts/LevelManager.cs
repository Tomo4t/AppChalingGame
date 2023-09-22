using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public GameObject RedCar, BlueCar, PinkCar,BlackCar; 
    public string SpawnPointRed = "SpawnPointRed",SpownPointBlue = "SpownPointBlue", SpownPointPink = "SpownPointPink", SpownPointBlack = "SpownPointBlack";
    public Sprite WinStar;


    private int CarNededToWin; 
    public static int SafedCares;
    public static bool AllisSafed;
    public static bool gameispossed;
    public static bool GameStarted;
    public static bool GoatStar = false;
    private int x = 1;
    private void Awake()
    {
        GoatStar = false;
        AllisSafed = false;
        SafedCares = 0;
    }
    void Start()
    {
        gameispossed = false;
        // Find all objects with the "SpawnPoint" tag
        GameObject[] spawnPointsRed = GameObject.FindGameObjectsWithTag(SpawnPointRed);

        GameObject[] spawnPointsBlue = GameObject.FindGameObjectsWithTag(SpownPointBlue);

        GameObject[] spawnPointsPink = GameObject.FindGameObjectsWithTag(SpownPointPink);

        GameObject[] spawnPointsBlack = GameObject.FindGameObjectsWithTag(SpownPointBlack);

        // Iterate through each spawn point and spawn a car
        foreach (GameObject spawnPoint in spawnPointsRed)
        {
            // Get the spawn position and rotation of the spawn point
            Vector3 spawnPosition = spawnPoint.transform.position + new Vector3(0,0.17f,0);
            _Grid grid =spawnPoint.GetComponent<_Grid>();
            
            Quaternion spawnRotation = Quaternion.Euler(0f, Mathf.Round(spawnPoint.transform.rotation.eulerAngles.y / 90) * 90, 0f);

            // Spawn the car prefab at the spawn point
            Instantiate(RedCar,spawnPosition, spawnRotation);

            
        }
       
     
        foreach (GameObject spawnPoint in spawnPointsBlue)
        {
            // Get the spawn position and rotation of the spawn point
            Vector3 spawnPosition = spawnPoint.transform.position + new Vector3(0, 0.17f, 0);
            _Grid grid = spawnPoint.GetComponent<_Grid>();

            Quaternion spawnRotation = Quaternion.Euler(0f, Mathf.Round(spawnPoint.transform.rotation.eulerAngles.y / 90) * 90, 0f);

            // Spawn the car prefab at the spawn point
            Instantiate(BlueCar, spawnPosition, spawnRotation);


        }


        foreach (GameObject spawnPoint in spawnPointsPink)
        {
            // Get the spawn position and rotation of the spawn point
            Vector3 spawnPosition = spawnPoint.transform.position + new Vector3(0, 0.17f, 0);
            _Grid grid = spawnPoint.GetComponent<_Grid>();

            Quaternion spawnRotation = Quaternion.Euler(0f, Mathf.Round(spawnPoint.transform.rotation.eulerAngles.y / 90) * 90, 0f);

            // Spawn the car prefab at the spawn point
            Instantiate(PinkCar, spawnPosition, spawnRotation);


        }


        foreach (GameObject spawnPoint in spawnPointsBlack)
        {
            // Get the spawn position and rotation of the spawn point
            Vector3 spawnPosition = spawnPoint.transform.position + new Vector3(0, 0.17f, 0);
            _Grid grid = spawnPoint.GetComponent<_Grid>();

            Quaternion spawnRotation = Quaternion.Euler(0f, Mathf.Round(spawnPoint.transform.rotation.eulerAngles.y / 90) * 90, 0f);

            // Spawn the car prefab at the spawn point
            Instantiate(BlackCar, spawnPosition, spawnRotation);


        }


        CarNededToWin = spawnPointsRed.Length + spawnPointsBlue.Length + spawnPointsPink.Length + spawnPointsBlack.Length;
    }
    private void Update()
    {
        

        if (CarNededToWin == SafedCares)
        {
            int levelIndex = SceneManager.GetActiveScene().buildIndex;


            AllisSafed = true;
            if (x == 1)
            {
                x = 0;
                if (GoatStar == true)
                {
                    UIManager.Instance.star.sprite = WinStar;
                    PlayerPrefs.SetInt("Level" + levelIndex + "Star",2);
                }
                else
                {
                    PlayerPrefs.SetInt("Level" + levelIndex + "Star", 1);
                }
                
                UIManager.Instance.activeWinScrean();
                
                
                int eventOccurred = PlayerPrefs.GetInt("Level" + levelIndex + "_EventOccurred"); 
                if (eventOccurred == 0)
                {
                    // The event hasn't occurred in this level yet, so you can proceed with the event.
                    MenuManager.unlocedLevels ++;
                    PlayerPrefs.SetInt("levelRetched", MenuManager.unlocedLevels);
                    Debug.Log("it didnt Hapend");

                    PlayerPrefs.SetInt("Level" + levelIndex + "_EventOccurred", 1);
                    PlayerPrefs.Save();
                }
                else
                {
                    // The event has already occurred in this level, so you can skip it.
                    Debug.Log("it Oredy Hapend");
                }
               
               


            }
           
        }
    }
}
