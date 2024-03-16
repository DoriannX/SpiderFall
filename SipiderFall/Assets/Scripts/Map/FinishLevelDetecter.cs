using UnityEngine;

public class FinishLevelDetecter : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.parent.TryGetComponent<GatheredEnemyCounter>(out GatheredEnemyCounter gatheredEnemyCounter))
        {
            if (gatheredEnemyCounter.GetGatheredEnemyAmount() > 1)
                print("congrats you won !");
            else
                print("Sorry you lose you didn't bring any enemy");

            collision.transform.parent.gameObject.SetActive(false);
        }
    }
}
