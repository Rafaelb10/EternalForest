using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _text;

    public void SetName(string name)
    {
        _name.text = name;
    }

    public void SetDialogue(string text)
    {
        _text.text = text;
    }
}
