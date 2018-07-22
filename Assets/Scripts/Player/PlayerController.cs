using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1f;
    [ReadOnly] public bool facingLeft = true;
    [ReadOnly] public bool isWalking = false;
    [ReadOnly] public bool isClimbing = false;
    private bool interacting = false;
    private Rigidbody2D body;
    private Animator animator;
    private GameObject npcController;

    // Use this for initialization
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        npcController = GameObject.FindGameObjectWithTag("NPCParent");
    }

    // Update is called once per physics tick
    void FixedUpdate()
    {
        float xInput = Input.GetAxisRaw("Horizontal");
        float yInput = Input.GetAxisRaw("Vertical");
        bool spaceInput = Input.GetKeyDown("space");
        TransformPlayer(xInput, yInput);
        HandleInteractions(spaceInput);
    }

    void TransformPlayer(float xInput, float yInput)
    {
        if (!interacting)
        {
            if (IsMoving(xInput))
            {
                isWalking = true;
                FlipPlayer(xInput);
                MovePlayer(xInput, 0);
            }
            else
            {
                isWalking = false;
            }
            if (IsMoving(yInput) && OnLadder())
            {
                isClimbing = true;
                MovePlayer(0, yInput);
            }
            isClimbing &= OnLadder();

            animator.SetBool("Walking", isWalking);
            animator.SetBool("Climbing", isClimbing);
        }
    }

    bool IsMoving(float x)
    {
        return Mathf.Abs(x - 0) > float.Epsilon;
    }

    void FlipPlayer(float xInput)
    {
        if (xInput > 0 && facingLeft || xInput < 0 && !facingLeft)
        {
            facingLeft = !facingLeft;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }

    void MovePlayer(float xInput, float yInput)
    {
        body.MovePosition(new Vector2(NewX(xInput), NewY(yInput)));
    }

    float NewX(float x)
    {
        return transform.position.x + x * moveSpeed * Time.deltaTime;
    }

    float NewY(float y)
    {
        return transform.position.y + y * moveSpeed * Time.deltaTime;
    }

    bool OnLadder()
    {
        return false;
    }

    void HandleInteractions(bool spaceBar)
    {
        if (spaceBar && SceneManager.GetActiveScene().name == "Town")
        {
            interacting = !interacting;

            NPC[] npcs = npcController.GetComponentsInChildren<NPC>();
            foreach (NPC n in npcs)
            {
                if (interacting)
                {
                    if (n.IsTriggered())
                    {
                        n.SetEngaged(true);
                    }
                }
                else
                {
                    n.SetEngaged(false);
                }

            }
        }
    }
}
