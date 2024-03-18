using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    TextMeshProUGUI _scoreText;
    int _score = 0;
    int _maxEnemy;

    private void Awake()
    {
        _scoreText = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        EnemyManager.Instance.EnemySpawned.AddListener(GetMaxEnemy);
    }

    void GetMaxEnemy()
    {
        _maxEnemy = EnemyManager.Instance.EnemyAmount;
    }

    private void Update()
    {
        _score = GatheredEnemyCounter.Instance.GetGatheredEnemyAmount();
        _scoreText.text = _score.ToString() + "/" + _maxEnemy.ToString();
    }

}
