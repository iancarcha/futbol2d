using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalKeeper : MonoBehaviour
{
    public Action OnScore;
    private void OnCollisionEnter2D(Collision2D col)
    {
        IInteractable ball = col.gameObject.GetComponent<IInteractable>();
        if (ball != null)
        {
            ball.RemoveInteraction();
            OnScore?.Invoke();
        }
    }
}
