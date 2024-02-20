using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    private Collectible score;

    [SerializeField] private int scorePoint;
    [SerializeField] private string collectibleName;
    [SerializeField] private Color colour;

    public Collectible Score
    {
        get { return score; }
        set { score = value; }
    }
    private void Start()
    {
        if (GameManager.Instance.Level >= 3)
        {
            CheckIfCapsuleOrSphere(22, 20);
        }
        else if (GameManager.Instance.Level == 2)
        {
            CheckIfCapsuleOrSphere(12, 10);
        }
        else
        {
            CheckIfCapsuleOrSphere(2, 1);
        }
        score = new Collectible(scorePoint, collectibleName, colour);
    }

    private void CheckIfCapsuleOrSphere(int capsuleScore, int sphereScore)
    {
        if (collectibleName == "Capsule")
        {
            scorePoint = capsuleScore;
        }
        else
        {
            scorePoint = sphereScore;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == GameManager.Instance.PlayerInstance)
        {
            bool removedFromList = GameManager.Instance.RemoveFromOccupiedList(transform.parent.gameObject);
            //add score and spawn collectible
            GameManager.Instance.CurrentTouchedCollectible = score.CollectibleName;
            GameManager.Instance.UpdateTouchedCollectible(score.Colour);
            GameManager.Instance.AddOrReduceScore(score.Reward);
            GameManager.Instance.SpawnCollectible();
            GameManager.Instance.NumOfPushedItems++;

            if (!removedFromList)
            {
                return;
            }
            Destroy(transform.parent.gameObject);
        }
    }
}
