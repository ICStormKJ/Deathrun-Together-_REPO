using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialWallMove: MonoBehaviour
{
    [SerializeField] private Transform target;
    private bool started;

    private void Start()
    {
    }
    private void Update()
    {
        if (started)
        {
            Vector3.MoveTowards(transform.position, target.position, 0.5f);
            if (Vector3.Distance(transform.position, target.position) <= 0.5f)
            {
                started = false;
                TakeToMain();
            }
        }
    }

    public void StartTrap()
    {
        started = true;
    }

    private IEnumerator TakeToMain()
    {
        yield return new WaitForSeconds(5);
        GetComponent<EnterLevel>().OpenLevel();
    }

}
