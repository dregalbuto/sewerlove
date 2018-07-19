using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateUIPlaceholder : MonoBehaviour {

    public Text myText;
    public GameObject playerObject;
    private PlayerController playerController; // maybe give to parent?


	// Use this for initialization
	void Start () {
        playerController = playerObject.GetComponent<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () {
        myText.text = playerController.facingLeft.ToString();
	}
}
