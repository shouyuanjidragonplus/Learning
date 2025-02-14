using UnityEngine;
using GameFramework;
using UnityGameFramework.Runtime;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 内置的UI界面(热更之前)
/// </summary>
public class StartComponent : GameFrameworkComponent
{
    [Header("Loading Progress:")] [SerializeField]
    GameObject loadingProgressNode = null;

    [SerializeField] private Text loadSliderText;
    [SerializeField] private Slider loadSlider;

    private void Start()
    {
        ShowLoadingProgress();
    }

    public void ShowLoadingProgress(float defaultProgress = 0)
    {
        loadingProgressNode.SetActive(true);
        SetLoadingProgress(defaultProgress);
    }

    public void SetLoadingProgress(float progress, string txt = "")
    {
        loadSlider.value = progress;
        loadSliderText.text = Utility.Text.Format("{0} {1:N0}%", txt, loadSlider.value * 100);
    }

    public void HideLoadingProgress()
    {
        loadingProgressNode.SetActive(false);
    }
}