using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingMinigame : MonoBehaviour
{
    public bool fishing;
    public GameObject xObject, yObject, clawObject;
    private GameObject maybeCapsule;
    public int numItems = 5;
    public int zoneOneDepth = 3;
    public int yMax = 5;
    public float xMult = 3;
    public float yMult = 2;
    public float clawRadius = 1f;
    public float capsuleScale = 1f;
    public bool debug = true;
    public bool retracting;


    private float xScale, yScale;
    [ReadOnly] public bool xSet, ySet;
    [ReadOnly] public bool doneRetracting;
    [ReadOnly] public bool gotCapsule;
    private float startTime;
    private float xStart;
    private int xMax = 14;
    private int debounce = 2;

    // Use this for initialization
    void Start()
    {
        GenerateItems();

        startTime = 0;
        xSet = false;
        ySet = false;
        doneRetracting = false;
        gotCapsule = false;
        xScale = 1;
        yScale = 1;
        xStart = xObject.transform.position.x;
        xObject.transform.localScale = new Vector3(xScale, 0.2f, 1);
        yObject.transform.localScale = new Vector3(0.1f, yScale, 1);
    }

    // Update is called once per frame
    void Update()
    {
        // TODO "press R to start fishing" or walk away to cancel
        // (check player position/trigger on ground)

        if (fishing)
        {

            if (!xSet && Input.GetKeyDown("space"))
            {
                xSet = true;
                startTime = 0;
            }
            if (!ySet && Input.GetKeyDown("space") && debounce == 0)
            {
                ySet = true;
                startTime = 0;
            }

            // debounce so there is time between spacebar presses
            if (xSet && debounce > 0)
            {
                debounce -= 1;
            }

            // horizontal motion starts immediately
            if (!xSet)
            {
                CastHorizontal();
            }

            // vertical motion starts after horizontal is set
            if (xSet && !ySet)
            {
                CastVertical();
            }

            UpdateClaw();

            if (retracting)
            {
                // claw closes and rod retracts after vertical is set
                // claw may or may not have an item in it
                if (xSet && ySet && !doneRetracting)
                {
                    if (!gotCapsule) {
                        maybeCapsule = GetItemOrNull();
                    }
                    if (xScale > 1)
                    {
                        RetractRod(maybeCapsule);
                    }
                    else
                    {
                        doneRetracting = true;
                    }

                }
                // check to see if we got anything
                if (xSet && ySet && doneRetracting)
                {
                    if (maybeCapsule != null)
                    {
                        Debug.Log("Got item: " + maybeCapsule.name);
                    }
                    else
                    {
                        Debug.Log("Got nothing :(");
                    }
                }

            }

            startTime += Time.deltaTime;

            if (debug)
            {
               // Debug.Log("xScale: " + xScale + " yScale: " + yScale);
            }
        }
    }

    void GenerateItems()
    {
        RectTransform[] kids = gameObject.GetComponentsInChildren<RectTransform>();
        RectTransform[] capsules = new RectTransform[5];
        int c = 0;
        for (int i = 0; i < kids.Length; i++)
        {
            if (kids[i].gameObject.tag == "Capsule")
            {
                capsules[c] = kids[i];
                c++;
            }
        }

        // generate a number of items in random locations within fishing zone
        // make sure none of them overlap 
        List<float> xs = new List<float>();
        List<float> ys = new List<float>();
        for (int i = 0; i < numItems; i++)
        {
            float x = Random.Range(5, -7);

            while (xs.Contains(Mathf.Round(x))
            || xs.Contains(Mathf.Round(x - 1)))
            {
                x += 1;
                if (x >= 5)
                {
                    x = -7;
                }
            }
            xs.Add(x);

            float y = Random.Range(0, -1 * zoneOneDepth) + 2.5f;
            ys.Add(y);

            RectTransform myCap = capsules[i];
            myCap.position = new Vector3(x, y, 1);
            myCap.localScale = new Vector3(capsuleScale, capsuleScale, 1);
        }

        /*
        if (debugCapsules)
        {
            foreach (float x in xs)
            {
                Debug.Log("x: " + x);
            }
            foreach (float y in ys)
            {
                Debug.Log("y: " + y);
            }

        */
    }

    GameObject GetItemOrNull()
    {
        Vector3 vector = clawObject.transform.position;
        Collider[] c = Physics.OverlapSphere(vector, clawRadius);
        if (c.Length > 0)
        {
            gotCapsule = true;
            for (int i = 0; i < c.Length; i++) {
                if (c[i].gameObject.tag == "Capsule") {
                    return c[i].gameObject;
                }
            }
        }
        return null;
    }

    void RetractRod(GameObject item)
    {
        // retract fishing rod with item in it
        if (yScale > 0)
        {
            yScale -= Time.deltaTime * yMult;
            yObject.transform.localScale = new Vector3(0.1f, yScale, 1);
            if (item != null)
            {
                // move capsule to correct position each frame
                Vector3 capPos = item.transform.position;
                Vector3 capVec = new Vector3(capPos.x, capPos.y + Time.deltaTime * yMult, 1);
                item.transform.position = capVec;
            }

        }
        else
        {
            if (xScale > 1)
            {
                xScale -= Time.deltaTime * xMult;
                xObject.transform.localScale = new Vector3(xScale, 0.2f, 1);
                Vector3 yPos = new Vector3(-1 * xScale + xStart,
                                           yObject.transform.position.y,
                                                         1);
                yObject.transform.position = yPos;
                if (item != null)
                {
                    // move capsule to correct position each frame
                    Vector3 capPos = item.transform.position;
                    Vector3 capVec = new Vector3(capPos.x + Time.deltaTime * xMult, capPos.y, 1);
                    item.transform.position = capVec;
                }

            }
        }

    }

    void CastHorizontal() {
        xScale = Mathf.PingPong(startTime * xMult, xMax);
        xObject.transform.localScale = new Vector3(xScale, 0.2f, 1);
        Vector3 yPos = new Vector3(-1 * xScale + xStart,
                                   yObject.transform.position.y,
                                                 1);
        yObject.transform.position = yPos;
    }

    void CastVertical() {
        yScale = Mathf.PingPong(startTime * yMult, yMax) + 1;
        yObject.transform.localScale = new Vector3(0.1f, yScale, 1);
    }

    void UpdateClaw() {
        // move claw to correct position each frame
        Vector3 getY = yObject.transform.position;
        Vector3 clawPos = new Vector3(getY.x, getY.y - yScale - 0.5f, 1);
        clawObject.transform.position = clawPos;
    }
}
