using UnityEngine;

[CreateAssetMenu(fileName = "DialogueData", menuName = "Scriptable Objects/DialogueData")]
public class DialogueData : ScriptableObject
{
    [SerializeField] private string _nameCharacther;
    [SerializeField] [TextArea(3, 10)] private string[] _word;
}
