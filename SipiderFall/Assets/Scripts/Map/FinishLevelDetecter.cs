using UnityEngine;
using UnityEngine.Events;

public class FinishLevelDetecter : MonoBehaviour
{
    //Instance
    public static FinishLevelDetecter Instance;

    //Unity event
    [HideInInspector] public UnityEvent LevelFinished;

    //If the player won
    private bool _won = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.parent.TryGetComponent<GatheredEnemyCounter>(out GatheredEnemyCounter gatheredEnemyCounter))
        {
            if (gatheredEnemyCounter.GetGatheredEnemyAmount() >= 1){
                print("congrats you won !");
                _won = true;
            }
            else
                print("Sorry you lose you didn't bring any enemy");

            LevelFinished?.Invoke();
            collision.transform.parent.gameObject.SetActive(false);
        }
    }

    public bool HasWon()
    {
        return _won;
    }
}
