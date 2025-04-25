using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NeedsUIController : MonoBehaviour
{
    [System.Serializable]
    public class NeedUI
    {
        public NeedData need;
        public TMP_Text needText;
        public Slider slider;
    }
    
    [SerializeField] private NeedUI[] _needUIs;

    private void Start()
    {
        foreach (var needUI in _needUIs)
        {
            if (needUI.slider != null)
            {
                needUI.slider.minValue = 0;
                needUI.slider.maxValue = needUI.need.maxValue;
            }
        }
    }
    
    private void Update()
    {
        foreach (var needUI in _needUIs)
        {
            if (needUI.need != null && needUI.slider != null)
            {
                needUI.slider.value = needUI.need.CurrentValue;
                
                needUI.needText.text = $" {needUI.need.name} : {Mathf.RoundToInt(needUI.need.CurrentValue)} / <color=#d2d2d2>{needUI.need.maxValue}</color>";
            }
        }
    }
}