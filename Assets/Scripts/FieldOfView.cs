using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FieldOfView : MonoBehaviour
{
    [SerializeField]
    private float viewRadius = 4f;
    [Range(0, 360)]
    [SerializeField]
    private float viewAngle = 90f;
    [SerializeField]
    private Transform fovCheck;
    [SerializeField]
    private float aggroRange = 2f;

    public LayerMask targetMask;
    public LayerMask obstacleMask;

    public List<Transform> visibleTargets = new List<Transform>();

    public bool playerInFOV;
    public bool playerInAggroRange;

    void Start()
    {
        StartCoroutine("FindTargetWithDelay", 0.2f);
    }

    IEnumerator FindTargetWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }

    //Finds targets inside field of view not blocked by walls
    void FindVisibleTargets()
    {
        visibleTargets.Clear();
        playerInFOV = false;
        playerInAggroRange = false;
        //Adds targets in view radius to an array
        Collider2D[] targetsInViewRadius = Physics2D.OverlapCircleAll(fovCheck.position, viewRadius, targetMask);

        //For every targetsInViewRadius it checks if they are inside the field of view
        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - fovCheck.position).normalized;
            if (Vector3.Angle(fovCheck.transform.forward, dirToTarget) < viewAngle / 2)
            {
                float dstToTarget = Vector3.Distance(fovCheck.position, target.position);
                //If line draw from object to target is not interrupted by wall, add target to list of visible targets
                if (!Physics2D.Raycast(fovCheck.position, dirToTarget, dstToTarget, obstacleMask))
                {
                    visibleTargets.Add(target);
                    playerInFOV = true;
                    if (dstToTarget <= aggroRange)
                    {
                        playerInAggroRange = true;
                    }
                }
            }
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(fovCheck.position, viewRadius);

        //Draws cone of view
        Gizmos.color = Color.green;
        Vector3 newDirA = Quaternion.Euler( 0, 0, viewAngle/2) * fovCheck.transform.forward;
        Vector3 newDirB = Quaternion.Euler( 0, 0, -viewAngle/2) * fovCheck.transform.forward;
        Gizmos.DrawRay(fovCheck.position, newDirA * viewRadius);
        Gizmos.DrawRay(fovCheck.position, newDirB * viewRadius);

        foreach(Transform visibleTarget in visibleTargets)
        {
            Gizmos.DrawLine(fovCheck.position, visibleTarget.position);
        }
    }
}

