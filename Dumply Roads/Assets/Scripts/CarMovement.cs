using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AdaptivePerformance;

public class CarMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float turnSpeed = 90f;
    public float deadEndRange = 1f;
    public LayerMask IgnorlayerMask;
    public float WaiteBeforeMoving = 1f;
    public GameObject frount;
    public bool isRightFavored, dontWaitForAll = false, isBlue,isRed,isPink,isBlack;

    private bool isMoving = false;
    private bool isTurning = false;

    
    private bool isDead = false;

    private bool seenCar;
    private Rigidbody rb;
    private MeshCollider col;


    private bool carIsSafe = false, CarAtWrongExit;
    private int x = 0, y = 0, z = 0;
    private Vector3 lastPosition; 

    private void Start()
    {
        
        rb = GetComponent<Rigidbody>();
        col = GetComponent<MeshCollider>();
    }
    private void FixedUpdate()
    {
        RaycastHit hit;
       
        if (Physics.Raycast(transform.position, (frount.transform.position - transform.position).normalized, out hit, 0.8f, ~LayerMask.NameToLayer("Car")))
        {
            seenCar = true;
            StartCoroutine(CarAhead());
        }

        if (LevelManager.GameStarted)

        { 
            if (!isMoving && !isTurning && !isDead && seenCar == false && CarAtWrongExit == false)
            {

                StartCoroutine(MoveAndRotate());

            }
            if (LevelManager.AllisSafed)
            {
                if (z == 0)
                {
                    z = 1;
                    isMoving = true;
                }
            }
            if (carIsSafe)
            {
                if (y == 0)
                {
                    y = 1;
                    LevelManager.SafedCares += 1;
                }
            }

                if ((isDead && carIsSafe == false) || CarAtWrongExit == true)
            {
                if (x == 0)
                {
                    x = 1;
                    Debug.Log("You lost");
                }

            }
        }
       
    }

    private IEnumerator MoveAndRotate()
    {
        // Raycast to detect the structure beneath the car
        RaycastHit hit;
        

        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, ~IgnorlayerMask))
        {
           

            ID idScript = hit.collider.GetComponent<ID>();

            if (idScript != null)
            {
                int structureID = idScript.structureID;

             // Adjust car behavior based on structure ID
               if (structureID == 1 )
                     {
                        // Find all child objects of the road
                        Transform[] children = hit.collider.GetComponentsInChildren<Transform>();
                        List<Transform> roadChildren = new List<Transform>();

                        foreach (Transform child in children)
                        {
                            if (child != hit.collider.transform)
                            {
                                roadChildren.Add(child);
                            }
                        }

                        // Calculate the farthest child from the car
                        Transform farthestChild = null;
                        float maxDistance = 0f;

                        foreach (Transform child in roadChildren)
                        {
                            float distance = Vector3.Distance(transform.position, child.position);
                            if (distance > maxDistance)
                            {
                                maxDistance = distance;
                                farthestChild = child;
                            }
                        }

                    

                    if (farthestChild != null)
                    {
                        // Calculate the direction to the farthest child
                        Vector3 directionToFarthestChild = farthestChild.position - transform.position;

                        // Calculate the target rotation based on the direction with 90-degree snapping
                        Quaternion targetRotation = Quaternion.LookRotation(directionToFarthestChild);
                        targetRotation.eulerAngles = new Vector3(0f, Mathf.Round(targetRotation.eulerAngles.y / 90) * 90, 0f);

                     
                        // Find the next nearest child to turn towards
                        Transform nextNearestChild = null;
                        float minDistance = float.MaxValue;

                        foreach (Transform child in roadChildren)
                        {
                            if (child != farthestChild)
                            {
                                float distance = Vector3.Distance(transform.position, child.position);
                                if (distance < minDistance)
                                {
                                    minDistance = distance;
                                    nextNearestChild = child;
                                }
                            }
                        }

                        if (nextNearestChild != null)
                        {
                            // Calculate the direction to the next nearest child
                            Vector3 directionToNextNearestChild = nextNearestChild.position - transform.position;

                            // Calculate the target rotation to the next nearest child with 90-degree snapping
                            Quaternion nextTargetRotation = Quaternion.LookRotation(directionToNextNearestChild);
                            nextTargetRotation.eulerAngles = new Vector3(0f, Mathf.Round(nextTargetRotation.eulerAngles.y / 90) * 90, 0f);

                            // If the angle difference to the next nearest child is less than 170 degrees, turn the car
                            float angleToNextNearestChild = Quaternion.Angle(transform.rotation, nextTargetRotation);
                            if (angleToNextNearestChild < 170f)
                            {
                                targetRotation = nextTargetRotation;
                            }
                        }

                        // Rotate the car to the target rotation
                        while (transform.rotation != targetRotation)
                        {
                            isTurning = true;
                            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
                            yield return null;
                        }

                        isTurning = false;

                        // Move the car forward
                        Vector3 targetPosition = transform.position + transform.forward;
                        while (transform.position != targetPosition)
                        {
                            isMoving = true;
                            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                            yield return null;
                        }

                        isMoving = false;

                        // Check if the car has moved off the structure before allowing it to turn again
                        if (lastPosition != transform.position)
                        {
                            isTurning = false;
                            lastPosition = transform.position;

                            // Clear the roadChildren list when the car leaves
                            roadChildren.Clear();
                        }
                    }

                    

                }

                else if (structureID == 2)
                {
                    Transform[] children = hit.collider.GetComponentsInChildren<Transform>();
                    List<Transform> roadChildren = new List<Transform>();

                    foreach (Transform child in children)
                    {
                        if (child != hit.collider.transform)
                        {
                            roadChildren.Add(child);
                        }
                    }

                    Transform entrance = null;
                    Transform rightWay = null;
                    Transform leftWay = null;

                    entrance = roadChildren[0];
                    rightWay = roadChildren[1];
                    leftWay = roadChildren[2];

                    Transform TargetChiled = isRightFavored ? rightWay : leftWay;

                    
                    float angleDifference = Quaternion.Angle(transform.rotation, TargetChiled.rotation);
                   

                    
                    
                    if (angleDifference > 89)
                    {

                       float objectRotation = transform.rotation.eulerAngles.y;

                       


                        if (TargetChiled.position.z < entrance.position.z && objectRotation < 1 && objectRotation > -1) 
                            {
                            if (TargetChiled == rightWay) { TargetChiled = leftWay; } else { TargetChiled = rightWay; }
                             
                            }
                              
                              
                            else if (TargetChiled.position.x < entrance.position.x && objectRotation < 91 && objectRotation > 89)
                            {
                            if (TargetChiled == rightWay) { TargetChiled = leftWay; } else { TargetChiled = rightWay; }

                            }
                                
                            else if (TargetChiled.position.z > entrance.position.z && objectRotation  < 181 && objectRotation > 179)
                            {
                            if (TargetChiled == rightWay) { TargetChiled = leftWay; } else { TargetChiled = rightWay; }
                           
                            }
                          
                            else if (TargetChiled.position.x > entrance.position.x && objectRotation < 271 && objectRotation > 269)
                            {
                            if (TargetChiled == rightWay) { TargetChiled = leftWay; } else { TargetChiled = rightWay; }

                            }



                        else
                        {
                                TargetChiled = entrance;
                            }

                        
                        
                    }
                    
                    Vector3 directionToFarthestChild = TargetChiled.position - transform.position;

                    Quaternion targetRotation = Quaternion.LookRotation(directionToFarthestChild);
                    targetRotation.eulerAngles = new Vector3(0f, Mathf.Round(targetRotation.eulerAngles.y / 90) * 90, 0f);


                    while (transform.rotation != targetRotation)
                    {
                        isTurning = true;
                        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
                        yield return null;
                    }

                    isTurning = false;

                    // Move the car forward
                    Vector3 targetPosition = transform.position + transform.forward;
                    while (transform.position != targetPosition)
                    {
                        isMoving = true;
                        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                        yield return null;
                    }

                    isMoving = false;

                    // Check if the car has moved off the structure before allowing it to turn again
                    if (lastPosition != transform.position)
                    {
                        isTurning = false;
                        lastPosition = transform.position;

                        // Clear the roadChildren list when the car leaves
                        roadChildren.Clear();
                    }

                }

                else if (structureID == 0)
                {
                    
                    if (isMoving == false )
                    {
                        
                        Vector3 targetPosition = transform.position + transform.forward;
                        lastPosition = targetPosition;
                        
                        while (transform.position != targetPosition)
                        {
                            isMoving = true;
                            if (isDead)
                            {
                                targetPosition = transform.position;
                            }
                            if (carIsSafe == false || LevelManager.AllisSafed || dontWaitForAll)
                            {
                                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                            }
                            
                            yield return null;
                        }
                        isMoving = true;
                    }
                   
                    if ((!hit.collider.CompareTag("DontWait") && !hit.collider.CompareTag("SpawnPointRed") && !hit.collider.CompareTag("SpownPointBlue") && !hit.collider.CompareTag("SpownPointPink") && !hit.collider.CompareTag("SpownPointBlack") && !hit.collider.CompareTag("UniversoalWin") && !hit.collider.CompareTag("RedWin") && !hit.collider.CompareTag("BlueWin") && !hit.collider.CompareTag("PinkWin") && !hit.collider.CompareTag("BlackWin")) || CarAtWrongExit == true)
                    {
                        yield return new WaitForSeconds(WaiteBeforeMoving);
                    }



                    if (hit.collider.CompareTag("Entrince"))
                    {
                        CarAtWrongExit = true;
                    }


                    if (hit.collider.CompareTag("UniversoalWin"))
                    {
                        carIsSafe = true;

                    }

                    if (isRed && hit.collider.CompareTag("RedWin"))
                    {
                        carIsSafe = true;
                    }
                    else if (isRed &&( hit.collider.CompareTag("BlueWin") || hit.collider.CompareTag("PinkWin") || hit.collider.CompareTag("BlackWin")))
                    {
                        CarAtWrongExit = true;
                    }

                    if (isBlue && hit.collider.CompareTag("BlueWin"))
                    {
                        carIsSafe = true;
                    }
                    else if (isBlue && (hit.collider.CompareTag("RedWin") || hit.collider.CompareTag("PinkWin") || hit.collider.CompareTag("BlackWin")))
                    {
                        CarAtWrongExit = true;
                    }

                    if (isPink && hit.collider.CompareTag("PinkWin"))
                    {
                        carIsSafe = true;
                    }
                    else if (isPink && (hit.collider.CompareTag("BlueWin") || hit.collider.CompareTag("RedWin") || hit.collider.CompareTag("BlackWin")))
                    {
                        CarAtWrongExit = true;
                    }

                    if (isBlack && hit.collider.CompareTag("BlackWin"))
                    {
                        carIsSafe = true;
                    }
                    else if (isBlack && (hit.collider.CompareTag("BlueWin") || hit.collider.CompareTag("PinkWin") || hit.collider.CompareTag("RedWin")))
                    {
                        CarAtWrongExit = true;
                    }



                    if (lastPosition == transform.position)
                    {
                        isMoving = false;
                    }
                    
                   
                }
            }



        }
        else
        {
          
           isDead = true;
            
        }


    }

    private IEnumerator CarAhead()
    {
        yield return new WaitForSeconds(1);
        seenCar = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Star"))
        {
            other.GetComponent<MeshCollider>().enabled = false;
            LevelManager.GoatStar = true;
            Destroy(other.gameObject);

        }
        if (other.gameObject.CompareTag("Bordaer"))
        {
            
            col.isTrigger = false;
            rb.velocity = other.transform.position - transform.forward;
            isDead = true;
            
           


        }
        if (other.gameObject.CompareTag("Car"))
        {
            
            col.isTrigger = false;
            rb.velocity = other.transform.position - transform.forward;
            isDead = true;
            
        }
    }


}
