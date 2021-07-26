using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GoalBell : MonoBehaviour
{
    [SerializeField]
    UnityEvent OnGoal;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ball"))
        {
            OnGoal.Invoke();
        }
    }
}
