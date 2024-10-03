using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonitorManager : MonoBehaviour
{
    public List<Artifact> artifacts { get; set; }
    public Transform rearSwapPos;

    private Monitor temp;
    public Monitor left;
    public Monitor middle;
    public Monitor right;

    private Vector3 leftPos;
    private Vector3 middlePos;
    private Vector3 rightPos;

    // Start is called before the first frame update
    void Start()
    {
        UpdatePositions();
    }

    void UpdatePositions()
    {
        leftPos = left.transform.position;
        middlePos = middle.transform.position;
        rightPos = right.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (left.Busy() || middle.Busy() || right.Busy()) return;

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Switchleft();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Switchright();
        }
    }

    public void Switchleft()
    {
        if (left.index == 0) return;

        AudioManager.instance.PlayMonitorButtonClip();
        UpdatePositions();
        
        right.MoveTo(rearSwapPos.position, leftPos);
        middle.MoveTo(rightPos);
        left.MoveTo(middlePos);

        // L M R
        // N L M

        temp = right;
        right = middle;
        middle = left;
        left = temp;
        left.setImage(artifacts[middle.index-1].imageURL, middle.index-1);
    }

    public void Switchright()
    {
        if (right.index == artifacts.Count - 1) return;

        AudioManager.instance.PlayMonitorButtonClip();
        UpdatePositions();

        left.MoveTo(rearSwapPos.position, rightPos);
        middle.MoveTo(leftPos);
        right.MoveTo(middlePos);

        // L M R
        // M R N

        temp = left;
        left = middle;
        middle = right;
        right = temp;

        right.setImage(artifacts[middle.index+1].imageURL, middle.index+1);
    }
}
