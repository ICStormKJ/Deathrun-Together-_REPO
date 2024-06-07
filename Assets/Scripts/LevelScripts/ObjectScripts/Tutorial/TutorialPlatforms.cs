using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPlatforms : MonoBehaviour
{
    [SerializeField] private GameObject endpos;
    private Vector3 start;
    private Vector3 end;
    private float t = 0f;
    [SerializeField] private float speed;
    // Start is called before the first frame update
    void Start()
    {
        start = transform.position;
        end = endpos.transform.position;
    }

    public void MovePlatforms()
    {
        t += speed * Time.deltaTime;
        transform.position = Vector3.Lerp(start, end, t);
        if (t >= 1.0f)
        {
            Vector3 temp = end;
            end = start;
            start = temp;
            t = 0f;
        }
    }
}
