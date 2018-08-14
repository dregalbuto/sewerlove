using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingMinigame : MonoBehaviour
{
    public GameObject xObject, yObject, clawObject;
    public FishingTrigger fishingTrigger;
    public int numItems = 5;
    public int zoneOneDepth = 3;
    public int yMax = 5;
    public float xMult = 3;
    public float yMult = 2;
    public float clawRadius = 1f;
    public float capsuleScale = 1f;
    public bool debug = true;

    [ReadOnly] public bool allowMovement = true;
    [ReadOnly] public bool startedFishing;
    [ReadOnly] public bool xSet, ySet;
    [ReadOnly] public bool doneRetracting;
    [ReadOnly] public bool gotCapsule;
    [ReadOnly] public bool exit;
    [ReadOnly] public bool restart;

    private GameObject inventoryParent;
    private GameObject maybeCapsule;
    private float xScale, yScale;
    private float startTime;
    private float xStart;
    private int xMax = 14;
    private int debounce = 2;

    private void Reset()
    {
        startTime = 0;
        startedFishing = false;
        xSet = false;
        ySet = false;
        doneRetracting = false;
        gotCapsule = false;
        exit = false;
        restart = false;
        xScale = 1;
        yScale = 1;
        xStart = xObject.transform.position.x;
        xObject.transform.localScale = new Vector3(xScale, 0.2f, 1);
        yObject.transform.localScale = new Vector3(0.1f, yScale, 1);
    }

    // Use this for initialization
    void Start()
    {
        GenerateItems();
        Reset();

    }

    // Update is called once per frame
    void Update()
    {
        // debounce so there is time between spacebar presses
        if (debounce > 0)
        {
            debounce -= 1;
        }

        if (!fishingTrigger.triggered)
        {
            allowMovement = true;
        }

        if (fishingTrigger.triggered)
        {
            if (!exit)
            {
                allowMovement = false;
            }
            else
            {
                allowMovement = true;
            }

            if (!startedFishing && Input.GetKeyDown("space") && fishingTrigger.triggered)
            {
                startedFishing = true;
                debounce = 2;
                restart = false;
            }

            if (startedFishing)
            {

                if (!xSet && Input.GetKeyDown("space") && debounce == 0)
                {
                    xSet = true;
                    startTime = 0;
                    debounce = 2;
                }
                if (!ySet && Input.GetKeyDown("space") && debounce == 0)
                {
                    ySet = true;
                    startTime = 0;
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

                // claw closes and rod retracts after vertical is set
                // claw may or may not have an item in it
                if (xSet && ySet && !doneRetracting)
                {
                    if (!gotCapsule)
                    {
                        maybeCapsule = GetItemOrNull();
                    }
                    if (xScale > 1)
                    {
                        RetractRod(maybeCapsule);
                    }
                    else
                    {
                        doneRetracting = true;
                        if (maybeCapsule != null)
                        {
                            maybeCapsule.GetComponent<Renderer>().enabled = false;
                            inventoryParent = GameObject.FindWithTag("InventoryParent");
                            InventoryController ic = inventoryParent.GetComponent<InventoryController>();
                            ic.AddItem(maybeCapsule);
                        }

                    }

                }
                // do another round, or let the player leave
                if (xSet && ySet && doneRetracting && Input.GetKeyDown("space"))
                {
                    Reset();
                    restart = true;
                }
                else if (xSet && ySet && doneRetracting && Input.GetKeyDown(KeyCode.Escape))
                {
                    exit = true;
                }

                // update the clock
                startTime += Time.deltaTime;

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
    }

    GameObject GetItemOrNull()
    {
        Vector3 vector = clawObject.transform.position;
        Collider[] c = Physics.OverlapSphere(vector, clawRadius);
        if (c.Length > 0)
        {
            gotCapsule = true;
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i].gameObject.tag == "Capsule")
                {
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

    void CastHorizontal()
    {
        xScale = Mathf.PingPong(startTime * xMult, xMax);
        xObject.transform.localScale = new Vector3(xScale, 0.2f, 1);
        Vector3 yPos = new Vector3(-1 * xScale + xStart,
                                   yObject.transform.position.y,
                                                 1);
        yObject.transform.position = yPos;
    }

    void CastVertical()
    {
        yScale = Mathf.PingPong(startTime * yMult, yMax) + 1;
        yObject.transform.localScale = new Vector3(0.1f, yScale, 1);
    }

    void UpdateClaw()
    {
        // move claw to correct position each frame
        Vector3 getY = yObject.transform.position;
        Vector3 clawPos = new Vector3(getY.x, getY.y - yScale - 0.5f, 1);
        clawObject.transform.position = clawPos;
    }

    public bool GetIsFishing()
    {
        return startedFishing;
    }

    public List<string> GetPrompt()
    {
        return new List<string>
        {
            "Press SPACE to set your cast distance, " +
            "then press SPACE again to reach for an item!",
            "Press SPACE to start."
        };
    }

    public List<string> GetFailure()
    {
        return new List<string>
        {
            "Aww beans... :( " ,
            "Press SPACE to try again or press ESC to stop fishing."
        };
    }

    public List<string> GetSuccess()
    {
        return new List<string>
        {
            "Sweet! I got a " + maybeCapsule.name + "!\n" ,
            "Press SPACE to try again or press ESC to stop fishing."
        };
    }

    public bool GetIsDoneFishing()
    {
        return doneRetracting;
    }

    public bool GetExit()
    {
        return exit;
    }

    public GameObject GetCapsule()
    {
        return maybeCapsule;
    }

    public bool GetStart()
    {
        return fishingTrigger.triggered;
    }

    public bool GetAllowMovement()
    {
        return allowMovement;
    }

    public bool GetRestart()
    {
        return restart;
    }

}
