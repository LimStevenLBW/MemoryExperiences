using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonitorManager : MonoBehaviour
{
    public List<Monitor> monitorList;
    public Transform rearSwapPos;

    private Monitor previous;
    private Monitor current;
    private Monitor next;

    private Vector3 previousPos;
    private Vector3 currentPos;
    private Vector3 nextPos;

    // Start is called before the first frame update
    void Start()
    {
        previous = monitorList[0];
        current = monitorList[1];
        next = monitorList[2];

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

    public void SwitchPrevious()
    {
        AudioManager.instance.PlayMonitorButtonClip();
        monitorList[3] = monitorList[0];
        previous.MoveTo(currentPos);

        monitorList[0] = monitorList[1];


        current.MoveTo(nextPos);
        monitorList[1] = monitorList[2];

        next.MoveTo(rearSwapPos.position, previousPos);
        monitorList[2] = monitorList[3];
        

    }

    public void SwitchNext()
    {
        Monitor temp;
        next.MoveTo(currentPos);
        next = current;

        previous.MoveTo(rearSwapPos.position, nextPos);
        temp = previous;

        previous = next;
        current.MoveTo(previousPos);
        current = temp;

    }
}
