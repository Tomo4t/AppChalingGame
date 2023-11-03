using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    private Camera m_Camera;

    public Sprite happeStar, SadStar;
    [SerializeField] private List<Sprite> locak;
    [SerializeField] public List<GameObject> Levels;
    [SerializeField] public static List<Vector3> levelPositions;
    [SerializeField] public float cameraMoveSpeed = 2.0f;
     
    [SerializeField] public LayerMask touchLayerMask;

    
    private static Vector3 orginalpos;
    private Vector2 touchStartPos;

    private bool isSwitshing = false;

    public static bool allowTouch = true;
    public static int currentLevelIndex = 0;
    public static int unlocedLevels = 1;
  


        private void Awake()
    {
        allowTouch = true;
        unlocedLevels = PlayerPrefs.GetInt("levelRetched");

       

        if (PlayerPrefs.GetInt("levelRetched") == 0 )
        {
            unlocedLevels =1;
        }
        m_Camera = Camera.main;
    }
    private void Start()
    {
        
       if (orginalpos != Vector3.zero)  { m_Camera.transform.position = orginalpos;}
        if (currentLevelIndex != 0)
        {
            SwitchToLevel(currentLevelIndex, false);
        }
        
        PlayerPrefs.SetInt("levelRetched", unlocedLevels);
        
    }

   
    private void Update()
    {
        
        orginalpos = m_Camera.transform.position;
        
        levelPositions = new List<Vector3>();

        //Chat GPT UNLockLevels
        foreach (GameObject level in Levels)
        {
            if (level != null)
            {
                levelPositions.Add(level.transform.position);


                Transform lockedTransform = level.transform.Find("Locked"), StarTransform = level.transform.Find("Star");

                if (StarTransform != null)
                {
                    SpriteRenderer StarSprit = StarTransform.GetComponent<SpriteRenderer>();

                    if (StarSprit != null)
                    {
                       int havestar = PlayerPrefs.GetInt("Level" + levelPositions.Count + "Star");
                        if (havestar == 1)
                        {
                            StarSprit.sprite = SadStar;
                        }
                        else if (havestar == 2)
                        {
                            StarSprit.sprite = happeStar;
                        }
                        else
                        {
                            StarSprit.gameObject.SetActive(false);
                        }




                    }

                }

                if (lockedTransform != null)
                {
                    // Get the Sprite component from the "Locked" child object
                    SpriteRenderer lockedSprite = lockedTransform.GetComponent<SpriteRenderer>();


                    if (lockedSprite != null)
                    {
                        if (Levels.Count > locak.Count)
                        {
                            locak.Add(lockedSprite.sprite);
                        }

                        if (locak.Count <= PlayerPrefs.GetInt("levelRetched"))
                        {
                            lockedSprite.sprite = null;
                            if (level.GetComponent<BoxCollider>() == null)
                            {
                                level.AddComponent<BoxCollider>();
                            }

                        }

                    }
                    else
                    {
                        Debug.LogWarning("No Sprite component found on the 'Locked' child object of " + level.name);
                    }
                }
                else
                {
                    Debug.LogWarning("No 'Locked' child object found on " + level.name);
                }

              

            }
        }

        
        //Switch Betwen Levels
        if (isSwitshing == false && allowTouch)
        {
            if (Input.touchCount > 0 && isSwitshing == false)
            {

                Touch touch = Input.GetTouch(0);


                if (touch.phase == TouchPhase.Began)
                {

                    RaycastHit hit;
                    Ray ray = Camera.main.ScreenPointToRay(touch.position);
                    if (Physics.Raycast(ray, out hit, Mathf.Infinity, touchLayerMask))
                    {
                        TransitionManger.instance.loadlevel(true,"Level" + (currentLevelIndex + 1));
                       
                    }
                }


                switch (touch.phase)
                {
                    case TouchPhase.Began:

                        touchStartPos = touch.position;
                        break;
                    case TouchPhase.Ended:
                        Vector2 touchEndPos = touch.position;
                        float swipeDistance = touchEndPos.x - touchStartPos.x;

                        if (Mathf.Abs(swipeDistance) >= 100f) // Adjust this value for your desired sensitivity
                        {

                            if (swipeDistance > 0)
                            {
                                // Swipe right, move to the previous level

                                SwitchToLevel(currentLevelIndex - 1, true);
                            }
                            else
                            {
                                // Swipe left, move to the next level
                                SwitchToLevel(currentLevelIndex + 1, true);
                            }
                        }
                        break;
                }
            }

        }

    }


    private void SwitchToLevel(int targetLevelIndex ,bool wait)
    {
        
        if (targetLevelIndex >= 0 && targetLevelIndex < levelPositions.Count && isSwitshing == false)
        {
            isSwitshing = true;
            currentLevelIndex = targetLevelIndex;
            Vector3 targetPosition = levelPositions[targetLevelIndex];

            StartCoroutine(MoveCamera(targetPosition, wait));
        }
    }

    private IEnumerator MoveCamera(Vector3 targetPosition, bool wait)
    {
        float startTime = Time.time;
        Vector3 initialPosition = m_Camera.transform.position;
        float journeyLength = Vector3.Distance(initialPosition, targetPosition);

        while (Time.time - startTime < 1.0f && wait)
        {
            float distanceCovered = (Time.time - startTime) * cameraMoveSpeed;
            float fractionOfJourney = distanceCovered / journeyLength;

            m_Camera.transform.position = Vector3.Lerp(initialPosition,new Vector3( targetPosition.x, m_Camera.transform.position.y, m_Camera.transform.position.z), fractionOfJourney);
            
           
            yield return null;
        }
         m_Camera.transform.position = new Vector3(targetPosition.x, m_Camera.transform.position.y, m_Camera.transform.position.z);
         isSwitshing = false;

    }
    

  
    
}
