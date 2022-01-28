using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpCamera : MonoBehaviour
{
    public Vector2 positionToMoveTo;
    [SerializeField] public float duration;
    Vector2 backupPosition;
    void Start()
    {
        backupPosition = this.transform.position;
        StartCoroutine(LerpPosition(positionToMoveTo));
    }

    private float nextUpdate = 2;
    public void Update()
    {
        if (Time.timeSinceLevelLoad > nextUpdate)
        {
            Vector2 swp = positionToMoveTo;
            positionToMoveTo = backupPosition;
            backupPosition = swp;
            nextUpdate += 2;
            StartCoroutine(LerpPosition(positionToMoveTo));
        }
    }

    IEnumerator LerpPosition(Vector3 targetPosition)
    {
        float time = 0;
        Vector2 startPosition = transform.position;

        while (time < duration)
        {
            Vector3 target = Vector2.Lerp(startPosition, targetPosition, time / duration);
            transform.position = target + Vector3.back;
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition + Vector3.back;
    }
}