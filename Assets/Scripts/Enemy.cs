using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TreeEditor;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform playerTransform, eyeRayTransform, playerHead, playerFeet;
    public int frameCounter;
    public bool isActive, isInactive, isGuarding, playerVisible, playerSeen, slotFull;
    // Awake is called before the first frame update
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        playerTransform = GameObject.Find("Player").transform;
        playerHead = playerTransform.Find("HeadScanPoint");
        playerFeet = playerTransform.Find("FootScanPoint");
        frameCounter = 0;
    }

    void Active()
    {
        if (playerSeen)
        {
            agent.SetDestination(playerTransform.position);
        }
        if (playerVisible)
        {
            Shoot();
        }
        if (!slotFull)
        {
            GameObject closestGun = SearchForWeapons();
            if (closestGun != null)
            {
                float gunDist = CalculatePathLength(closestGun.transform.position, agent);
                float playerDist = CalculatePathLength(playerTransform.position, agent);
                if (gunDist > playerDist && playerSeen)
                {
                    agent.SetDestination(playerTransform.position);
                }
                else
                {
                    agent.SetDestination(closestGun.transform.position);
                }
            }
            else
            {
                if (playerSeen)
                {
                    agent.SetDestination(playerTransform.position);
                }
            }
        }
    }

    void Inactive()
    {

    }

    void Guarding()
    {
        if (playerVisible)
        {
            Shoot();
        }
        if (!slotFull)
        {
            
        }
    }

    GameObject SearchForWeapons()
    {
        ThrowableScript[] throwables = FindObjectsOfType<ThrowableScript>();
        List<GameObject> availableGuns = new();
        if (throwables.Length > 0) 
        {
            foreach (ThrowableScript throwable in throwables)
            {
                if (!throwable.isEquipped)
                {
                    if (throwable.tag == "Gun")
                    {
                        availableGuns.Add(throwable.gameObject);
                    }
                }
            }
        }
        GameObject closestGun = null;
        if (availableGuns.Count > 0) 
        {
            if (availableGuns.ElementAt(0) != null)
            {
                closestGun = availableGuns.ElementAt(0);
                float minDistance = CalculatePathLength(closestGun.transform.position, agent);
                foreach (GameObject gun in availableGuns)
                {
                    if (minDistance > CalculatePathLength(gun.transform.position, agent))
                    {
                        minDistance = CalculatePathLength(gun.transform.position, agent);
                        closestGun = gun;
                    }
                }
            }
        }
        return closestGun;
    }

    void EquipGun(GameObject gun)
    {

    }

    void Shoot()
    {

    }

    float CalculatePathLength(Vector3 targetPosition, NavMeshAgent nav)
    {
        // Create a path and set it based on a target position.
        NavMeshPath path = new NavMeshPath();
        if (nav.enabled)
            nav.CalculatePath(targetPosition, path);

        // Create an array of points which is the length of the number of corners in the path + 2.
        Vector3[] allWayPoints = new Vector3[path.corners.Length + 2];

        // The first point is the enemy's position.
        allWayPoints[0] = transform.position;

        // The last point is the target position.
        allWayPoints[allWayPoints.Length - 1] = targetPosition;

        // The points inbetween are the corners of the path.
        for (int i = 0; i < path.corners.Length; i++)
        {
            allWayPoints[i + 1] = path.corners[i];
        }

        // Create a float to store the path length that is by default 0.
        float pathLength = 0;

        // Increment the path length by an amount equal to the distance between each waypoint and the next.
        for (int i = 0; i < allWayPoints.Length - 1; i++)
        {
            pathLength += Vector3.Distance(allWayPoints[i], allWayPoints[i + 1]);
        }

        return pathLength;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(0, Quaternion.LookRotation(playerTransform.position).y, 0);
        frameCounter %= 3;
        Ray lineOfSight;
        RaycastHit lineOfSightInfo;
        if (frameCounter % 3 == 0)
        {
            lineOfSight = new Ray(eyeRayTransform.position, (playerHead.position - eyeRayTransform.position).normalized);
        }
        else if (frameCounter % 3 == 1)
        {
            lineOfSight = new Ray(eyeRayTransform.position, (playerTransform.position - eyeRayTransform.position).normalized);
        }
        else
        {
            lineOfSight = new Ray(eyeRayTransform.position, (playerFeet.position - eyeRayTransform.position).normalized);
        }
        frameCounter++;
        if (Physics.Raycast(lineOfSight, out lineOfSightInfo))
        {
            if (lineOfSightInfo.collider.gameObject == playerTransform.gameObject)
            {
                playerSeen = true;
                playerVisible = true;
            }
            else
            {
                playerVisible = false;
            }
        }        
        if (isActive)
        {
            Active();
        }
        if (isGuarding)
        {
            Guarding();
        }
    }
}
