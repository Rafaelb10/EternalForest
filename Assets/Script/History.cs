using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class History : MonoBehaviour
{
    [SerializeField] private List<TextMeshProUGUI> texts;
    [SerializeField] private float letterDelay = 0.1f;
    [SerializeField] private float shakeAngle = 0.01f;
    [SerializeField] private float shakeSpeed = 0.1f;   

    private void Start()
    {
        foreach (var text in texts)
        {
            text.gameObject.SetActive(false);
        }

        StartCoroutine(PlayTexts());
    }

    private IEnumerator PlayTexts()
    {
        foreach (var text in texts)
        {
            text.gameObject.SetActive(true);
            yield return StartCoroutine(RevealTextWithSoftShake(text));
            yield return new WaitForSeconds(0.5f);
        }

        yield return StartCoroutine(AfterAllTexts());
    }

    private IEnumerator RevealTextWithSoftShake(TextMeshProUGUI text)
    {
        string fullText = text.text;
        text.text = "";

        float elapsed = 0f;

        for (int i = 0; i < fullText.Length; i++)
        {
            text.text += fullText[i];

            float shakeTime = 0f;
            while (shakeTime < letterDelay)
            {
                shakeTime += Time.deltaTime;
                elapsed += Time.deltaTime;

                float angle = Mathf.Sin(elapsed * shakeSpeed) * shakeAngle;
                text.rectTransform.rotation = Quaternion.Euler(0f, 0f, angle);

                yield return null;
            }
        }

        text.rectTransform.rotation = Quaternion.identity;
    }

    private IEnumerator AfterAllTexts()
    {
        yield return new WaitForSeconds(4f);
        FindAnyObjectByType<SaveController>().ChangeScena("Game");
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }
}