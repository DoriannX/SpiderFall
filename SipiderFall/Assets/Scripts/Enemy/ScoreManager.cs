using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    TextMeshProUGUI _scoreText;
    int _score = 0;

    private void Awake()
    {
        _scoreText = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        _score = GatheredEnemyCounter.Instance.GetGatheredEnemyAmount();
        _scoreText.text = _score.ToString() + "/" + EnemyManager.Instance.EnemyAmount.ToString();
    }

}
