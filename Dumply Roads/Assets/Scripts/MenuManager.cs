using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    private Camera m_Camera;
    public List<GameObject> Levels;
    private List<Vector3> levelPositions;
    private int currentLevelIndex = 0;
    public float cameraMoveSpeed = 2.0f;
    private Vector2 touchStartPos;
    public LayerMask touchLayerMask;
    private bool isSwitshing = false;

   
        private void Awake()
    {
        m_Camera = Camera.main;
    }
   
    
    private void Update()
    {
       
            levelPositions = new List<Vector3>();

            foreach (GameObject level in Levels)
            {
                if (level != null)
                {
                    levelPositions.Add(level.transform.position);
                    level.AddComponent<BoxCollider>();
                }
            }
        if (isSwitshing == false)
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
                        LoadLevel("Level" + (currentLevelIndex + 1));
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

                                SwitchToLevel(currentLevelIndex - 1);
                            }
                            else
                            {
                                // Swipe left, move to the next level
                                SwitchToLevel(currentLevelIndex + 1);
                            }
                        }
                        break;
                }
            }

        }

    }


    private void SwitchToLevel(int targetLevelIndex)
    {
        
        if (targetLevelIndex >= 0 && targetLevelIndex < levelPositions.Count && isSwitshing == false)
        {
            isSwitshing = true;
            currentLevelIndex = targetLevelIndex;
            Vector3 targetPosition = levelPositions[targetLevelIndex];

            StartCoroutine(MoveCamera(targetPosition));
        }
    }

    private IEnumerator MoveCamera(Vector3 targetPosition)
    {
        float startTime = Time.time;
        Vector3 initialPosition = m_Camera.transform.position;
        float journeyLength = Vector3.Distance(initialPosition, targetPosition);

        while (Time.time - startTime < 1.0f)
        {
            float distanceCovered = (Time.time - startTime) * cameraMoveSpeed;
            float fractionOfJourney = distanceCovered / journeyLength;

            m_Camera.transform.position = Vector3.Lerp(initialPosition,new Vector3( targetPosition.x, m_Camera.transform.position.y, m_Camera.transform.position.z), fractionOfJourney);
            
           
            yield return null;
        }
         m_Camera.transform.position = new Vector3(targetPosition.x, m_Camera.transform.position.y, m_Camera.transform.position.z);
         isSwitshing = false;

    }
    

    private void LoadLevel(string levelName)
    {
        isSwitshing=true;
        SceneManager.LoadScene(levelName);
    }

   
}
