using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MainMenu : MonoBehaviour
{
//    [SerializeField] Button[] btn;
//    [SerializeField] Sprite clicked;
//    [SerializeField] Sprite normal;
    Sprite btnSprite;
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
        Debug.Log("Settings");
    }
    public void Quit()
    {
        Application.Quit();
    }
    //IEnumerator Wait()
    //{
    //    yield return new WaitForSeconds(1); 
    //}
}
