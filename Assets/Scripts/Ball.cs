using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Ball : MonoBehaviour, IInteractable
{
    [SerializeField] private Rigidbody2D rigidbody2D;
    [SerializeField] private Collider2D collider2D;
    [SerializeField] private AudioSource source;

    [SerializeField] private bool isIAGame;
    private PhysicsMaterial2D _material;

    private void Awake()
    {
        _material = collider2D.sharedMaterial;
    }

    public void Interact(FootballPlayer player)
    {
        source.Play();
        var randY = Random.Range(-0.01f, 0.011f);
        var randShootForce = Random.Range(10, 50);
        var rand = new Vector2(1, randY);
        if (IsFacingOther(player.transform,player.OtherGoal))
        {
            Vector2 direction = (player.OtherGoal.position - transform.position).normalized;
            rigidbody2D.AddForce((rand*direction)/randShootForce,ForceMode2D.Impulse);
            return;
        }
        rigidbody2D.AddForce((rand*Vector2.right)/randShootForce,ForceMode2D.Impulse);
    }
    
    private bool IsFacingOther(Transform player, Transform other){
        // Check if the gaze is looking at the front side of the object
        Vector2 toOther = (other.position - player.position).normalized;
        var result = Vector2.Dot(toOther, player.right);
        if( result > 0.75f)
        {
            return true;
        }
        return false;
    }

    public void RemoveInteraction()
    {
        collider2D.sharedMaterial = null;
        rigidbody2D.velocity = Vector2.zero;
        rigidbody2D.angularVelocity = 0f;

        if (transform.parent)
        {
            transform.parent.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

    public void EnableInteraction()
    {
        collider2D.sharedMaterial = _material;
    }
}