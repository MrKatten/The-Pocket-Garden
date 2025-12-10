using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MainMenu : MonoBehaviour
{
//    [SerializeField] Button[] btn;
//    [SerializeField] Sprite clicked;
//    [SerializeField] Sprite normal;
    Sprite btnSprite;
    [SerializeField] GameObject _mainmenu;
    [SerializeField] GameObject _achievement;
    [SerializeField] GameObject _settings;
    void Start()
    {
        
    }

    //public void Click(int id)
    //{
    //    btnSprite = btn[id].GetComponent<Image>().sprite;
    //    btnSprite = clicked;
    //    StartCoroutine(Wait());
        
    //}
    public void Play()
    {
        SceneManager.LoadScene("Game");
    }
    public void Settings()
    {
        _mainmenu.SetActive(false);
        _settings.SetActive(true);
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void Achievement()
    {
        _mainmenu.SetActive(false);
        _achievement.SetActive(true);
    }
    public void Back()
    {
        _mainmenu.SetActive(true);
        _achievement.SetActive(false);
        _settings.SetActive(false);
    }
    //IEnumerator Wait()
    //{
    //    yield return new WaitForSeconds(1); 
    //}
}
