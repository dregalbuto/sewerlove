using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    
    public float moveSpeed = 1f;
    private Rigidbody2D body;


	// Use this for initialization
	void Start () {
        body = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per physics tick
	void FixedUpdate () {
        float xInput = Input.GetAxisRaw("Horizontal");
        float yInput = Input.GetAxisRaw("Vertical");

        if (IsMoving(xInput) || IsMoving(yInput)) {
           // var moveVector = new Vector3(xInput, yInput, 0);
            body.MovePosition(new Vector2(NewX(xInput), NewY(yInput)));
        }
	}

    bool IsMoving(float x) {
        return x - 0 < float.Epsilon;
    }

    float NewX(float x){
        return transform.position.x + x * moveSpeed * Time.deltaTime;
    }

    float NewY(float y) {
        return transform.position.y + y * moveSpeed * Time.deltaTime;
    }
}
