using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressionBar : MonoBehaviour
{
    Slider progress;
    Transform _playerTransform;

    private void Awake()
    {
        progress = GetComponent<Slider>();
    }

    private void Start()
    {
        _playerTransform = Player.Instance.transform;
    }

    private void Update()
    {
        progress.value = -(_playerTransform.position.y / ProceduralGeneration.Instance.GetMapSize());
    }
}
