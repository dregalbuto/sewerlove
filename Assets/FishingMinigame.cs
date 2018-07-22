using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingMinigame : MonoBehaviour
{

    public GameObject xObject, yObject, clawObject;
    private Sprite xSprite, ySprite, clawSprite;
    private float xScale, yScale;
    [ReadOnly] public bool xSet, ySet;
    private float startTime;
    private float xStart;
    public int xMax = 14;
    public float xMult;
    public int yMax = 5;
    public float yMult;
    public int debounce = 2;

    // Use this for initialization
    void Start()
    {
        startTime = 0;
        xSet = false;
        ySet = false;
        xScale = 1;
        yScale = 1;
        xStart = xObject.transform.position.x;
        xObject.transform.localScale = new Vector3(xScale, 0.2f, 1);
        yObject.transform.localScale = new Vector3(0.1f, yScale, 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (!xSet && Input.GetKeyDown("space"))
        {
            xSet = true;
            startTime = 0;
        }
        if (!ySet && Input.GetKeyDown("space") && debounce == 0)
        {
            ySet = true;
        }

        // horizontal motion starts immediately
        if (!xSet)
        {
            xScale = Mathf.PingPong(startTime * xMult, xMax);
            xObject.transform.localScale = new Vector3(xScale, 0.2f, 1);
            Vector3 yPos = new Vector3(-1 * xScale + xStart,
                                       yObject.transform.position.y,
                                                     1);
            yObject.transform.position = yPos;
        }

        // vertical motion starts after horizontal is set
        if (xSet && !ySet)
        {
            yScale = Mathf.PingPong(startTime * yMult, yMax) + 1;
            yObject.transform.localScale = new Vector3(0.1f, yScale, 1);
        }

        // move claw to correct position each frame
        Vector3 getY = yObject.transform.position;
        Vector3 clawPos = new Vector3(getY.x, getY.y - yScale - 0.5f, 1);
        clawObject.transform.position = clawPos;


        if (xSet && debounce > 0) {
            debounce -= 1;
        }

        startTime += Time.deltaTime;

        Debug.Log("xScale: " + xScale + " yScale: " + yScale);
    }
}
