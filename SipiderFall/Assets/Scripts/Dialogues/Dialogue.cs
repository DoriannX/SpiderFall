using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Dialogue : MonoBehaviour
{
    TextMeshProUGUI _text;
    [SerializeField] string[] lines;
    [SerializeField] float _textSpeed;
    int index;
    public static Dialogue Instance;

    private void Awake()
    {
        _text = GetComponentInChildren<TextMeshProUGUI>();
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        _text.text = string.Empty;
        StartDialogue();
    }
    void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach(char  c in lines[index].ToCharArray())
        {
            _text.text += c;
            yield return new WaitForSecondsRealtime(_textSpeed);
        }
    }

    public void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            _text.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
            gameObject.SetActive(false);
    }

    public void ToggleDialogue(bool state)
    {
        gameObject.SetActive(state);
    }

    public IEnumerator FinishTuto()
    {
        print("tuto is about to finish");
        NextLine();
        yield return new WaitForSecondsRealtime(2);
        gameObject.SetActive(false);
    }
}
