
using UnityEngine;

public class _Grid : MonoBehaviour
{
    [SerializeField] private LayerMask touchLayerMask;
    [SerializeField] private Color hitColor;

    public bool canBeEdited = true;

    
    private GameObject currentStructurePrefab;
    private int currentStructureId = -1;
    private int destroyedStructureIndex = -1;
    private StructureData destroyedStructureData;
    private GameObject currentStructureInstance;

    private Color originalColor;
    private SpriteRenderer spriteRenderer;
    private StructureManager structureManager;

    

    public static bool distroyMode;
    private void Start()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.up, out hit, 1))
        {
            if (hit.collider.CompareTag("Bordaer"))
            {
                canBeEdited = false;
            }
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;

        // Initialize the reference to the StructureManager
        
        structureManager = StructureManager.Instance;
    }

    void Update()
    {
        if (LevelManager.GameStarted == false && canBeEdited && LevelManager.gameispossed == false)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved)
                {
                    Ray ray = Camera.main.ScreenPointToRay(touch.position);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit, Mathf.Infinity, touchLayerMask))
                    {
                        if (hit.collider.gameObject == gameObject)
                        {
                            StructureData currentStructureData = structureManager.GetCurrentStructureData();

                            if (currentStructureData != null)
                            {
                                int buildingLimit = currentStructureData.buildingLimit;

                                if (distroyMode)
                                {

                                    DestroyCurrentStructure(destroyedStructureData, destroyedStructureIndex);

                                }
                                else if (CanBuildStructure(currentStructureData.Id, buildingLimit))
                                {
                                    spriteRenderer.color = hitColor;

                                    if (currentStructureInstance == null)
                                    {
                                        currentStructurePrefab = currentStructureData.structurePrefab;
                                        currentStructureInstance = SpawnStructure(transform.position);
                                    }
                                    else if (touch.phase != TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
                                    {

                                        RotateCurrentStructure();

                                    }

                                }
                                else if (currentStructureInstance != null && (touch.phase != TouchPhase.Moved || touch.phase == TouchPhase.Stationary))
                                {
                                    
                                       RotateCurrentStructure();
                                    
                                }
                            }
                            else
                            {
                                Debug.Log("No structure selected.");
                            }
                        }
                    }
                }
                else if (touch.phase == TouchPhase.Ended)
                {
                    spriteRenderer.color = originalColor;
                }
            }
        }
    }

    private bool CanBuildStructure(int structureId, int buildingLimit)
    {
        if (buildingLimit <= 0)
        {
            return true; // No building limit specified
        }

        // Use the BuildingManager to check if the building limit for this structureId is not reached
        return BuildingManager.Instance.CanBuildStructure(structureId, buildingLimit);
    }

    private void DestroyCurrentStructure(StructureData data, int structureIndex)
    {
        if (currentStructureInstance != null)
        {
            Destroy(currentStructureInstance, 0.1f);
            BuildingManager.Instance.DecrementStructureCount(currentStructureId);
            currentStructureId = -1;


            UIManager.Instance.UpdateNumText(structureIndex, data.Id, data.buildingLimit);


            destroyedStructureIndex = -1;

        }
    }

    private void RotateCurrentStructure()
    {
        if (currentStructureInstance != null)
        {
            Quaternion currentRotation = currentStructureInstance.transform.rotation;
            Quaternion rotationToAdd = Quaternion.Euler(0f, 90f, 0f);
            Quaternion newRotation = currentRotation * rotationToAdd;

            currentStructureInstance.transform.rotation = newRotation;
        }
    }
   
    private GameObject SpawnStructure(Vector3 spawnPosition)
    {
        if (currentStructurePrefab != null)
        {
            GameObject newStructure = Instantiate(currentStructurePrefab, spawnPosition, Quaternion.identity);
            int currentStructureId = structureManager.GetCurrentStructureData().Id;



            destroyedStructureData = structureManager.GetCurrentStructureData();
            destroyedStructureIndex = StructureManager.currentStructureIndex;


           
           

            

            GameObject childObject = newStructure.transform.GetChild(0).gameObject;
            ID idScript = childObject.AddComponent<ID>();
            idScript.structureID = currentStructureId;


            BuildingManager.Instance.IncrementStructureCount(currentStructureId);
            
            this.currentStructureId = currentStructureId;
            UIManager.Instance.UpdateNumText(StructureManager.currentStructureIndex, currentStructureId, structureManager.GetCurrentStructureData().buildingLimit);

            return newStructure;
        }
        return null;



    }


}

