using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject carPrefab; // Reference to your car prefab
    public string SpawnPoint = "SpawnPoint";

    void Start()
    {
        // Find all objects with the "SpawnPoint" tag
        GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag(SpawnPoint);

        // Iterate through each spawn point and spawn a car
        foreach (GameObject spawnPoint in spawnPoints)
        {
            // Get the spawn position and rotation of the spawn point
            Vector3 spawnPosition = spawnPoint.transform.position;
            _Grid grid =spawnPoint.GetComponent<_Grid>();
            
            Quaternion spawnRotation = Quaternion.Euler(0f, Mathf.Round(spawnPoint.transform.rotation.eulerAngles.y / 90) * 90, 0f);

            // Spawn the car prefab at the spawn point
            Instantiate(carPrefab,spawnPosition, spawnRotation);

            
        }
    }
}
