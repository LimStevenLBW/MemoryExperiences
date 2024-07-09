using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonitorManager : MonoBehaviour
{
    public List<Artifact> artifacts { get; set; }
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
        int previousIndex = previous.index;
        if (previousIndex == 0) return;
        Debug.Log("previous:" + previousIndex);
        

        AudioManager.instance.PlayMonitorButtonClip();
        UpdatePositions();
        previous.MoveTo(currentPos);
        next.MoveTo(rearSwapPos.position, previousPos);
        current.MoveTo(nextPos);

        temp = current;
        current = previous;
        previous = next;
        next = temp;

        previous.setImage(artifacts[previousIndex - 1].imageURL, previousIndex - 1);

    }

    public void SwitchNext()
    {
        int nextIndex = next.index;
        Debug.Log("next:" + nextIndex);
        if (nextIndex == artifacts.Count){
            return;
        }
        AudioManager.instance.PlayMonitorButtonClip();
        UpdatePositions();
        next.MoveTo(currentPos);
        current.MoveTo(previousPos);
        previous.MoveTo(rearSwapPos.position, nextPos);

        temp = current;
        current = next;
        next = previous;
        previous = temp;

        next.setImage(artifacts[nextIndex+1].imageURL, nextIndex+1);

    }
}
