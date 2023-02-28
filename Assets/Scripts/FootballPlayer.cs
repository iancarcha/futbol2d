using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class FootballPlayer : MonoBehaviour
{
    
    [SerializeField] protected float speed;
    [SerializeField] protected float turnRate = 10f;
    [SerializeField] protected Rigidbody2D rigidbody2D;
    [SerializeField] private KeyCode[] controls;
    [SerializeField] protected Transform otherGoal;
    [SerializeField] protected Transform ballPosition;
    public Transform OtherGoal => otherGoal;
    public bool HasBall => ballPosession != null && ballPosession.parent == transform;
    protected Vector2 movement;
    protected Transform ballPosession;
    protected bool _canPlay;

    //0: Up, 1: Left, 2: Down, 3: Right, 4: Shoot
    private void FixedUpdate()
    {
        
        if (_canPlay)
        {
            //Move up
            if (Input.GetKey(controls[0]))
            {
                movement = Vector2.up;
                transform.rotation = Quaternion.RotateTowards(transform.rotation,
                    Quaternion.Euler(new Vector3(0f, 0, 90)), turnRate);
            }
            //Move left
            else if (Input.GetKey(controls[1]))
            {
                movement = Vector2.left;
                transform.rotation = Quaternion.RotateTowards(transform.rotation,
                    Quaternion.Euler(new Vector3(0f, 0, 180)), turnRate);
            }

            //Move down
            else if (Input.GetKey(controls[2]))
            {
                movement = Vector2.down;
                transform.rotation = Quaternion.RotateTowards(transform.rotation,
                    Quaternion.Euler(new Vector3(0f, 0, -90)), turnRate);
            }
            //Move right
            else if (Input.GetKey(controls[3]))
            {
                movement = Vector2.right;
                transform.rotation = Quaternion.RotateTowards(transform.rotation,
                    Quaternion.Euler(new Vector3(0f, 0, 0)), turnRate);
            }
            else
            {
                movement = Vector2.zero;
            }

            rigidbody2D.velocity = new Vector2(movement.x, movement.y) * speed;
        }
    }

    protected void Update()
    {
        TranslateBall();
    }

    protected void TranslateBall()
    {
        if (ballPosession && ballPosession.parent == transform)
        {
            ballPosession.position = ballPosition.position;
            ballPosession.gameObject.GetComponent<Rigidbody2D>().angularVelocity = 0f;
        }
    }
    private void OnCollisionEnter2D(Collision2D col)
    {
        var interactable =  col.gameObject.GetComponent<IInteractable>();
        if (interactable != null)
        {
            col.transform.parent = null;
            ballPosession = col.transform;
            ballPosession.parent = transform;
        }
    }
    private void OnCollisionStay2D(Collision2D col)
    {
        var interactable =  col.gameObject.GetComponent<IInteractable>();
        if (interactable != null)
        {
            if (Input.GetKey(controls[4]))
            {
                if (ballPosession)
                {
                    ballPosession.parent = null;
                    ballPosession = null;
                }
                interactable.Interact(this);
            }
        }
    }

    public void DisablePlayer()
    {
        _canPlay = false;
        rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
    }
    
    public void EnablePlayer()
    {
        _canPlay = true;
        rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
    
}
