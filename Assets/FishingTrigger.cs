using UnityEngine;

public class FishingTrigger : MonoBehaviour {

    public bool triggered = false;

    protected void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("FishingTrigger entered by " + collider.gameObject.name);
        triggered |= collider.tag == "Player";
    }

    public bool GetTriggered()
    {
        return triggered;
    }

    public void SetTriggered(bool t) {
        triggered = t;
    }
}
