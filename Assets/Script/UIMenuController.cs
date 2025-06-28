using System.IO;
using System.Security.Cryptography.X509Certificates;
using TMPro;
using UnityEngine;

public class UIMenuController : MonoBehaviour
{
    [SerializeField] private GameObject SplashScreen;
    [SerializeField] private GameObject Menu;
    [SerializeField] private GameObject Options;
    [SerializeField] private GameObject OptionsController;
    [SerializeField] private GameObject Save;
    [SerializeField] private GameObject SaveHave;
    [SerializeField] private GameObject SaveDontHave;
    [SerializeField] private TextMeshProUGUI Level;

    private float i;
    private float splashCondition;

    private string savePath;

    private void Start()
    {
        SplashScreen.SetActive(true);
        Menu.SetActive(false);
        Options.SetActive(false);
        OptionsController.SetActive(false);
        Save.SetActive(false);
        SaveHave.SetActive(false);
        SaveDontHave.SetActive(false);

        savePath = Path.Combine(Application.persistentDataPath, "SaveData.json");
    }

    private void Update()
    {
        if (splashCondition == 0)
        {
            if (Input.anyKeyDown)
            {
                OpenMenu();
                splashCondition = 1; 
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OpenMenu();
        }

        UpdateSaveUI();
        UpdateLevelUI();
    }

    private void UpdateSaveUI()
    {
        bool saveExists = File.Exists(savePath);

        if (saveExists && !SaveHave.activeSelf)
        {
            SaveHave.SetActive(true);
            SaveDontHave.SetActive(false);
        }
        else if (!saveExists && !SaveDontHave.activeSelf)
        {
            SaveHave.SetActive(false);
            SaveDontHave.SetActive(true);
        }
    }

    [System.Serializable]
    private class SaveData
    {
        public int _level;
    }

    private void UpdateLevelUI()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            SaveData saveData = JsonUtility.FromJson<SaveData>(json);
            Level.text = $"{saveData._level}";
        }
        else
        {
            Level.text = "00";
        }
    }

    public void OpenMenu()
    {
        SplashScreen.SetActive(false);
        Menu.SetActive(true);
        Options.SetActive(false);
        OptionsController.SetActive(false);
        Save.SetActive(false);
        SaveHave.SetActive(false);
        SaveDontHave.SetActive(false);
    }

    public void OpenOptions()
    {
        Menu.SetActive(false);
        Options.SetActive(true);

        Options.transform.localPosition = new Vector3(-1000, 0, 0);
        LeanTween.moveLocalX(Options, 0, 0.5f).setEaseOutExpo();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void CloseOptions()
    {
        Menu.SetActive(true);
        Options.SetActive(false);
        OptionsController.SetActive(false);
    }

    public void OpenOptionsControl()
    {
        if (i == 0)
        {
            Menu.SetActive(false);
            Options.SetActive(true);
            OptionsController.SetActive(true);

            OptionsController.transform.localPosition = new Vector3(0, -1000, 0);
            LeanTween.moveLocalY(OptionsController, 0, 0.5f).setEaseOutBack();

            i = 1;
        }
        else if (i == 1)
        {
            Menu.SetActive(false);
            Options.SetActive(true);
            OptionsController.SetActive(false);
            i = 0;
        }
    }

    public void OpenSave()
    {
        SplashScreen.SetActive(false);
        Menu.SetActive(false);
        Options.SetActive(false);
        OptionsController.SetActive(false);
        Save.SetActive(true);
    }
}