using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;

public class PlantsUI : MonoBehaviour
{
    [SerializeField] GameObject _panel;
    [SerializeField] GameObject _imageHidden;
    [SerializeField] GameObject _imageUnhidden;
    bool _hidden = true;
    public void OnClick()
    {
        if (_hidden)
        {
            _panel.transform.DOMoveX(110, 0.5f).From(-150);
            _hidden = !_hidden;
            _imageHidden.SetActive(false);
            _imageUnhidden.SetActive(true);
        }
        else
        {
            _panel.transform.DOMoveX(-150, 0.5f).From(110);
            _hidden = !_hidden;
            _imageHidden.SetActive(true);
            _imageUnhidden.SetActive(false);
        }
        
    }
}
