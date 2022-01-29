using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tools
{
    public static GameObject GetClosest (GameObject needle, IEnumerable<GameObject> haystack)
    {
        GameObject bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = needle.transform.position;
        foreach(GameObject potentialTarget in haystack)
        {
            Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if(dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }
     
        return bestTarget;
    }

    public static void LookTowards(Transform source, Transform target)
    {
        Vector3 dir = target.transform.position - source.transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        source.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
