using UnityEditor;
using UnityEngine;

public class UIComplementary : MonoBehaviour
{
    [SerializeField] private GameObject complementary;

    private void Start()
    {
        AnimationUpAndDown();
    }

    public void GoToMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    public void AnimationUpAndDown()
    {
        complementary.transform.localPosition = new Vector3(0, -250, 0);
        LeanTween.moveLocalY(complementary, 0, 2f).setEaseOutBack();
    }
}
