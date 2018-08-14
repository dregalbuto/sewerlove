using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIScript : MonoBehaviour
{
    private InventoryController inventory;
    private GameObject inventoryUI;
    private GameObject npcParent;
    private GameObject dialogueUI;
    private GameObject fishingParent;
    private Animator arrowAnimator;
    private Text dialogueTextBox;
    private Image portrait;
    private List<string> dialogue;
    private bool logged;
    public bool debug;
    public float bounceMult = 0.1f;
    public int fontSize = 200;
    private readonly bool[] scene = { false, false };
    //    private bool textDone = false;

    private bool showedPrompt = false;
    private bool showedResult = false;

    // Use this for initialization
    void Start()
    {
        // Scene-specific variables
        scene[0] = (SceneManager.GetActiveScene().name == "Town");
        scene[1] = (SceneManager.GetActiveScene().name == "Fishing");
        if (scene[0])
        {
            npcParent = GameObject.FindWithTag("NPCParent");
        }
        if (scene[1])
        {
            fishingParent = GameObject.FindWithTag("FishingParent");
        }

        // Variables shared by every scene 
        dialogueUI = GameObject.FindWithTag("DialogueUI");
        arrowAnimator = GameObject.FindWithTag("Arrow").GetComponent<Animator>();

        dialogueTextBox = dialogueUI.GetComponentInChildren<Text>();
        dialogueTextBox.fontSize = fontSize;

        dialogueUI.SetActive(false);
        logged = false;

        inventoryUI = GameObject.FindWithTag("InventoryUI");
        GameObject inventoryParent = GameObject.FindWithTag("InventoryParent");
        inventory = inventoryParent.GetComponent<InventoryController>();
    }

    // Update is called once per frame
    void Update()
    {
        //arrowAnimator.Play("ArrowNew");
        if (scene[0])
        {
            // Display dialogue for any engaged NPCs
            NPC[] npcs = npcParent.GetComponentsInChildren<NPC>();
            foreach (NPC n in npcs)
            {
                if (n.tag != "NPCParent")
                {
                    if (n.isActiveAndEnabled && n.IsEngaged())
                    {
                        if (!logged)
                        {
                            Debug.Log("Interacting with " + n.name);
                            logged = true;
                        }
                        // dialogueTextBox.text = n.GetDialogue()[0];
                        //   ShowDialogue(n.GetDialogue());
                        portrait = dialogueUI.GetComponentsInChildren<Image>()[1];
                        portrait.sprite = n.GetComponent<Image>().sprite;
                        dialogueUI.SetActive(true);

                    }
                    else
                    {
                        dialogueUI.SetActive(false);
                        logged = false;
                    }
                }
            }
        }
        else if (scene[1])
        {
            // Displays fishing instructions until fishing starts
            FishingMinigame fg = fishingParent.GetComponent<FishingMinigame>();

            if (fg.GetStart() && !fg.GetIsFishing() && !showedPrompt)
            {
                StartDialogue(fg.GetPrompt());
                showedPrompt = true;
                showedResult = false;
            }

            else if (showedPrompt && fg.GetIsFishing() && !fg.GetIsDoneFishing())
            {
                dialogueUI.SetActive(false);
            }

            else if (fg.GetIsDoneFishing() && !showedResult)
            {
                GameObject capsule = fg.GetCapsule();
                if (capsule == null)
                {
                    StartDialogue(fg.GetFailure());
                }
                else
                {
                    StartDialogue(fg.GetSuccess());
                }
                showedResult = true;
            }

            else if (showedResult && !fg.GetExit())
            {
                showedPrompt = false;
            }
            else if (showedResult && fg.GetExit())
            {
                dialogueUI.SetActive(false);
            }
        }

        DrawInventory();

    }


    void ResetFishing()
    {
        showedPrompt = false;
        showedResult = false;
    }

    void DrawInventory()
    {
        List<InventoryItem> invItems = inventory.GetInventoryItems();
        if (invItems.Count > 0)
        {
            InventoryItem gO = invItems[0];
            Image inventoryPic = inventoryUI.GetComponentsInChildren<Image>()[0];
            inventoryPic.sprite = gO.image.sprite;
            inventoryPic.color = gO.image.color;
        }
        inventoryUI.SetActive(true);
    }

    void StartDialogue(List<string> strings)
    {
        FeedDialogue(strings);
    }

    void FeedDialogue(List<string> strings)
    {
        dialogueUI.SetActive(true);
        dialogue = strings;
        StartCoroutine("DialogueStart");
    }

    IEnumerator DialogueStart()
    {
        foreach (string text in dialogue)
        {
            yield return StartCoroutine(Dialogue(text));

        }
    }


    IEnumerator Dialogue(string text)
    {
        dialogueTextBox.text = text;
        Debug.Log(text);
        yield return StartCoroutine(WaitForKeyDown(KeyCode.DownArrow));
    }

    IEnumerator WaitForKeyDown(KeyCode keyCode)
    {
        while (!Input.GetKeyDown(keyCode))
            yield return null;
    }
}
