using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class CarMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float turnSpeed = 90f;
    public float deadEndRange = 1f;
    public LayerMask IgnorlayerMask,Roads;

    private bool isMoving = false;
    private bool isTurning = false;

    public static bool GameStarted;
    private bool isDead = false;
    private Rigidbody rb;
    private MeshCollider col;

    private int x = 0;
    private Vector3 lastPosition; // Store the car's last position

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<MeshCollider>();
    }
    private void Update()
    {
        
        
        if (GameStarted)
        {
            if (!isMoving && !isTurning && !isDead)
            {

                StartCoroutine(MoveAndRotate());

            }

            if (isDead)
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
        

        if (Physics.Raycast(transform.position, Vector3.down, out hit, deadEndRange, ~IgnorlayerMask))
        {
            Debug.DrawRay(transform.position, Vector3.down * deadEndRange, Color.green);

            ID idScript = hit.collider.GetComponent<ID>();

            if (idScript != null)
            {
                int structureID = idScript.structureID;

             // Adjust car behavior based on structure ID
               if (structureID == 1)
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

                    // ...

                    if (farthestChild != null)
                    {
                        // Calculate the direction to the farthest child
                        Vector3 directionToFarthestChild = farthestChild.position - transform.position;

                        // Calculate the target rotation based on the direction with 90-degree snapping
                        Quaternion targetRotation = Quaternion.LookRotation(directionToFarthestChild);
                        targetRotation.eulerAngles = new Vector3(0f, Mathf.Round(targetRotation.eulerAngles.y / 90) * 90, 0f);

                        // Calculate the angle between the current rotation and the target rotation
                        float angleDifference = Quaternion.Angle(transform.rotation, targetRotation);

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

                    // ...

                }
                else if (structureID == 0)
                {
                    Vector3 targetPosition = transform.position + transform.forward;
                    while (transform.position != targetPosition)
                    {
                        isMoving = true;
                        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                        yield return null;
                    }

                    isMoving = false;
                }
            }
        }
        
        
    }
   
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bordaer"))
        {
            isDead = true;
        }
        if (other.gameObject.CompareTag("Car"))
        {
            
            col.isTrigger = false;
           
            isDead = true;
            
        }
    }


}
