using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    private Score score;

    [SerializeField] private int scorePoint;
    private void Start()
    {
        score = new Score(scorePoint);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == GameManager.Instance.Player)
        {
            GameManager.Instance.RemoveFromOccupiedList(transform.parent.gameObject);
            //add score to player
            Destroy(transform.parent.gameObject);
        }
    }
}
