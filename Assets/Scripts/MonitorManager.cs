using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonitorManager : MonoBehaviour
{
    public Monitor previous;
    public Monitor current;
    public Monitor next;

    public Transform rearSwapPos;
    private Vector3 previousPos;
    private Vector3 currentPos;
    private Vector3 nextPos;

    // Start is called before the first frame update
    void Start()
    {
        previousPos = previous.transform.position;
        currentPos = current.transform.position;
        nextPos = next.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            SwitchPrevious();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            SwitchNext();
        }
    }

    private void SwitchPrevious()
    {
        Monitor temp;
        previous.MoveTo(currentPos);
        current.MoveTo(nextPos);
        next.MoveTo(rearSwapPos.position, previousPos);
    }

    private void SwitchNext()
    {
        Monitor temp;
        previous.MoveTo(rearSwapPos.position, nextPos);
        current.MoveTo(previousPos);
        next.MoveTo(currentPos);
    }
}
