using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenu : MonoBehaviour
{
    //[SerializeField] public UIDocument _uiDoc;
    //private VisualElement _root;
    void Start()
    {
        //_root = _uiDoc.rootVisualElement;

        //VisualElement playButton = _root.Q<VisualElement>("PlayButton");
        //VisualElement settingsButton = _root.Q<VisualElement>("SettingsButton");
        //VisualElement quitButton = _root.Q<VisualElement>("QuitButton");

        //playButton.RegisterCallback<ClickEvent>(Play);
        //settingsButton.RegisterCallback<ClickEvent>(Settings);
        //quitButton.RegisterCallback<ClickEvent>(Quit);
    }

    public void Play()
    {
        SceneManager.LoadScene("Game");
    }
    public void Settings()
    {
        Debug.Log("Settings");
    }
    public void Quit()
    {
        Application.Quit();
    }
}
