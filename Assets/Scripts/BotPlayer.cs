using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class BotPlayer : FootballPlayer
{
    [SerializeField] private Transform myGoal;
    [SerializeField] private Transform ball;
    [SerializeField] private FootballPlayer player;
    
    private float speedX;
    private float speedY;
    private State currentState;

    private bool _moveUp;
    private bool _moveLeft;
    private bool _moveRight;
    private bool _moveDown;
    private bool _goalKeeping;

    private bool _hasShot;
    private enum State
    {
        CATCH = 0,
        DEFEND = 1,
        ATTACK = 2,
    }
    private void Start()
    {
        speedX = speed;
        speedY = speed;
        currentState = State.CATCH;
    }

    private void FixedUpdate()
    {
        if (_canPlay)
        {
            //Move left
            if (_moveLeft)
            {
                Debug.Log("MOVING LEFT");
                movement = Vector2.left;
                transform.rotation = Quaternion.RotateTowards(transform.rotation,
                    Quaternion.Euler(new Vector3(0f, 0, 180)), turnRate);
            }
            //Move right
            else if (_moveRight)
            {
                Debug.Log("MOVING RIGHT");
                movement = Vector2.right;
                transform.rotation = Quaternion.RotateTowards(transform.rotation,
                    Quaternion.Euler(new Vector3(0f, 0, 0)), turnRate);
            }
            
            //Move up
            if (_moveUp)
            {
                Debug.Log("MOVING UP");
                movement = Vector2.up;
                transform.rotation = Quaternion.RotateTowards(transform.rotation,
                    Quaternion.Euler(new Vector3(0f, 0, 90)), turnRate);
            }
            //Move down
            else if (_moveDown)
            {
                Debug.Log("MOVING DOWN");
                movement = Vector2.down;
                transform.rotation = Quaternion.RotateTowards(transform.rotation,
                    Quaternion.Euler(new Vector3(0f, 0, -90)), turnRate);
            }

            rigidbody2D.velocity = new Vector2(movement.x * speedX, movement.y * speedY);
        }
    }
    
    private void OnCollisionStay2D(Collision2D col)
    {
        var interactable =  col.gameObject.GetComponent<IInteractable>();
        if (interactable != null)
        {
            if (Vector2.Distance(transform.position,otherGoal.position) < 2.5f)
            {
                if (ballPosession)
                {
                    ballPosession.parent = null;
                    ballPosession = null;
                    transform.rotation = Quaternion.RotateTowards(transform.rotation,
                        Quaternion.Euler(new Vector3(0f, 0, 180)), turnRate);
                }
                interactable.Interact(this);
                StartCoroutine(ShootAndDefend());
            }
        }
    }

    IEnumerator ShootAndDefend()
    {
        _hasShot = true;
        yield return new WaitForSeconds(Random.Range(0.5f, 0.75f));
        _hasShot = false;
    }

    private void OnCollisionExit(Collision other)
    {
        ballPosession = null;
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

    private void Update()
    {
        if (!HasBall)
        {
            if (!player.HasBall && !_hasShot) currentState = State.CATCH;
            else if(player.HasBall && Vector2.Distance(player.transform.position, myGoal.position) < 3) currentState = State.DEFEND;
            else if (!player.HasBall && _hasShot) currentState = State.DEFEND;
            else if (!HasBall && Vector2.Distance(player.transform.position, transform.position) < 1f)
                currentState = State.DEFEND;
        }
        else if (HasBall) currentState = State.ATTACK;
        
        switch (currentState)
        {
            case State.CATCH:
                Debug.Log("Catching");
                MoveTowards(ball);
                break;
            case State.DEFEND:
                Debug.Log("Defending");
                MoveTowards(myGoal, 0.25f);
                break;
            case State.ATTACK:
                Debug.Log("Attacking");
                MoveTowards(otherGoal, Random.Range(1,2));
                TranslateBall();
                break;
        }
    }

    private void MoveTowards(Transform target, float offsetX = 0, float offsetY = 0)
    {
        if (!_goalKeeping)
        {
            var distance = transform.position.x - target.position.x;
            Debug.Log("X:"+distance);
            if (distance > 0.05f + offsetX)
            {
                _moveUp = false;
                _moveDown = false;
                _moveLeft = true;
                _moveRight = false;
                speedX = speed;
            }
            else if(distance < -0.75f + offsetX)
            {
                _moveUp = false;
                _moveDown = false;
                _moveRight = true;
                _moveLeft = false;
                speedX = speed;
            }
            else if (distance >= -0.75 + offsetX && distance <= 0.05f + offsetX)
            {
                _moveLeft = false;
                _moveRight = false;
                speedX = 0;
                var distanceY = transform.position.y - target.position.y;
                Debug.Log("Y:"+distanceY);
                if (distanceY > 0.05f  + offsetY)
                {
                    _moveUp = false;
                    _moveDown = true;
                    speedY = speed;
                }
                else if (distanceY < -0.05  + offsetY)
                {
                    _moveUp = true;
                    _moveDown = false;
                    speedY = speed;
                }
                else if (distanceY >= -0.05 + offsetY && distanceY <= 0.05f + offsetY)
                {
                    _moveUp = false;
                    _moveDown = false;
                    speedY = 0;
                }
            }
        }
        else
        {
            _moveDown = false;
            _moveUp = false;
            _moveLeft = false;
            _moveRight = false;
        }


    }
}
