using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueScript : MonoBehaviour {
    
    private int count = 0;
    public List<string> dialogue;
    public Text textBox;

	// Use this for initialization
	void Start () {
        StartCoroutine(ProgressDialogue());
	}
	
    IEnumerator ProgressDialogue()
    {
        Debug.Log("Waiting for prince/princess to rescue me...");
        yield return new WaitWhile(() => count < dialogue.Count);
        Debug.Log("Finally I have been rescued!");
    }

    void Update()
    {
        if (count <= dialogue.Count)
        {
            Debug.Log("Frame: " + count);
            textBox.text = dialogue[count];
            count++;
        }
    }
}
