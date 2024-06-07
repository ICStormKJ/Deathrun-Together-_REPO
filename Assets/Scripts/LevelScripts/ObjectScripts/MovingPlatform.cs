using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MovingPlatform : Trap
{
    [SerializeField] private float speed; //speed of the platform

    //----------Vector3s to track the patrol points----------
    [SerializeField] private Transform endpos;
    private Vector3 start;
    private Vector3 end;
    private float t; //manage lerp interval
    // Start is called before the first frame update
    void Start()
    {
        start = transform.position;
        end = endpos.position;
        t = 0f;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (!isActive.Value)
        {
            return;
        }
        //----------Platform going back and forth----------
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
