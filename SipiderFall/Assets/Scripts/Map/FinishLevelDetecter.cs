using UnityEngine;
using UnityEngine.Events;

public class FinishLevelDetecter : MonoBehaviour
{
    //Instance
    public static FinishLevelDetecter Instance;

    //Unity event
    [HideInInspector] public UnityEvent LevelFinished;

    //If the player won
    public bool Won = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.parent.TryGetComponent<GatheredEnemyCounter>(out GatheredEnemyCounter gatheredEnemyCounter))
        {
            if (gatheredEnemyCounter.GetGatheredEnemyAmount() >= (int)(EnemyManager.Instance.EnemyAmount * .2f)){
                print("congrats you won !");
                SFXManager.Instance.PlayWin();
                Won = true;
            }
            else{
                print("Sorry you lose you didn't bring any enemy");
                SFXManager.Instance.PlayLose();
            }

            collision.transform.parent.gameObject.SetActive(false);
            LevelFinished?.Invoke();
        }
    }

    public bool HasWon()
    {
        return Won;
    }
}
