using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour {

    protected bool triggered;
    protected bool engaged;
    protected string dialogue;

	// Use this for initialization
	protected virtual void Start () {
        triggered = false;
        engaged = false;
        dialogue = "";
	}
	
	// Update is called once per frame
	protected virtual void Update () {
		
	}

    public virtual string GetDialogue() {
        return dialogue;
    }

    public virtual bool IsTriggered() {
        return triggered;
    }

    public virtual bool IsEngaged() {
        return engaged;
    }

    public virtual void SetTriggered(bool triggered) {
        this.triggered = triggered;
    }

    public virtual void SetEngaged(bool engaged) {
        this.engaged = engaged;
    }

    protected void OnTriggerEnter2D(Collider2D collider)
    {
        triggered |= collider.tag == "Player";
    }

    protected void OnTriggerExit2D(Collider2D collider)
    {
        triggered = false;
    }
}
