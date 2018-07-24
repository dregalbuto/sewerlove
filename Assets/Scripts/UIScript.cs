using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIScript : MonoBehaviour
{

    private GameObject npcObject;
    private GameObject dialogueUI;
    private Animator dialogueAnimator;
    private Text text;
    private Image portrait;
    private bool logged;
    public bool debug;
    public float bounceMult = 1.0f;

    // Use this for initialization
    void Start()
    {
        npcObject = GameObject.FindWithTag("NPCParent");
        dialogueUI = GameObject.FindWithTag("DialogueUI");
        dialogueAnimator = GameObject.FindWithTag("Arrow").GetComponent<Animator>();
        dialogueAnimator.SetBool("Bouncing", true);
        dialogueAnimator.SetFloat("BounceMult", bounceMult);
        dialogueUI.SetActive(false);
        logged = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Town")
        {
            // Display dialogue for any engaged NPCs
            NPC[] npcs = npcObject.GetComponentsInChildren<NPC>();
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
                        dialogueUI.SetActive(true);
                        text = dialogueUI.GetComponentInChildren<Text>();
                        text.text = n.GetDialogue();
                        portrait = dialogueUI.GetComponentsInChildren<Image>()[1];
                        portrait.sprite = n.GetComponent<Image>().sprite;

                    }
                    else
                    {
                        dialogueUI.SetActive(false);
                        logged = false;
                    }
                }
            }
        }

    }
}
