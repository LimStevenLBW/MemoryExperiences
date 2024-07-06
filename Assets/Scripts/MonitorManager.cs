using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonitorManager : MonoBehaviour
{
    public Transform rearSwapPos;

    private Monitor temp;
    public Monitor previous;
    public Monitor current;
    public Monitor next;

    private Vector3 previousPos;
    private Vector3 currentPos;
    private Vector3 nextPos;

    // Start is called before the first frame update
    void Start()
    {
        UpdatePositions();
    }

    void UpdatePositions()
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

    public void SwitchPrevious()
    {
        AudioManager.instance.PlayMonitorButtonClip();
        UpdatePositions();
        previous.MoveTo(currentPos);
        next.MoveTo(rearSwapPos.position, previousPos);
        current.MoveTo(nextPos);

        temp = current;
        current = previous;
        previous = next;
        next = temp;



        /*
        temp = previous;
        previous = current;

        current.MoveTo(nextPos);
        current = next;
 

        next.MoveTo(rearSwapPos.position, previousPos);
        next = temp;
        */


    }

    public void SwitchNext()
    {
        AudioManager.instance.PlayMonitorButtonClip();
        UpdatePositions();
        next.MoveTo(currentPos);
        temp = next;
        next = current;

        current.MoveTo(previousPos);
        current = previous;

        previous.MoveTo(rearSwapPos.position, nextPos);
        previous = temp;





    }
}
