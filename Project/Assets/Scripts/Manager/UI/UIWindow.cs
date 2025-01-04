using System;
using UnityEngine;
using UnityEngine.UI;
using Framework;

public abstract class UIWindow : MonoBehaviour
{
    public Canvas canvas = null;
    public CanvasGroup canvasGroup = null;
    private string mWindowName;

    // 是否播放默认对话框的声音
    public bool isPlayDefaultAudio = true;

    protected Animator _animator;

    public string WindowName
    {
        set { mWindowName = value; }
        get { return mWindowName; }
    }

    [HideInInspector] public UIWindowType m_WindowType;

    //[HideInInspector]
    public UIWindowLayer uiWindowLayer;

    [HideInInspector] public bool mIsOpen = false;

    public bool IsWindowOpened => mIsOpen;

    protected bool canClickMask = false;

    void Awake()
    {
        InitCanvas();

        _animator = transform.GetComponent<Animator>();

        gameObject.GetOrCreateComponent<GraphicRaycaster>();

        SetLanguage();

        PrivateAwake();
    }

    void SetLanguage()
    {
        Text[] labels = this.transform.GetComponentsInChildren<Text>(true);
        foreach (Text label in labels)
        {
            if (LocalizationManager.Instance.TryGetLocalizedString(label.text.Trim(), out var result))
            {
                label.text = result;
            }
        }
    }

    protected virtual void OnEnable()
    {
        StartCoroutine(CommonUtils.DelayWork(0.5f, () => { canClickMask = true; }));
    }

    public void InitCanvas()
    {
        if (canvas != null)
            return;

        canvas = gameObject.GetOrCreateComponent<Canvas>();
        canvas.overrideSorting = true;
    }

    public void OpenWindow(params object[] objs)
    {
        ShowUI();
        OnOpenWindow(objs);
        OpenWindowAudio();
    }

    protected virtual void OpenWindowAudio()
    {
        if (isPlayDefaultAudio)
        {
            if (mWindowName.Contains("Popup"))
            {
                AudioManager.Instance.PlaySound(SfxNameConst.sfx_pop_open);
            }
            else
            {
                AudioManager.Instance.PlaySound(SfxNameConst.panel_in);
            }
        }
    }

    void CloseWindowAudio()
    {
        if (isPlayDefaultAudio && mIsOpen)
        {
            // if (mWindowName.Contains("Popup"))
            // {
            //     AudioManager.Instance.PlaySound(SfxNameConst.sfx_pop_close);
            // }
            // else
            // {
            //     AudioManager.Instance.PlaySound(SfxNameConst.panel_out);
            // }
        }
    }

    public void CloseWindow(bool destroy = false)
    {
        OnCloseWindow(destroy);
        CloseWindowAudio();

        if (destroy)
        {
            DestroyUI();
        }
        else
        {
            HideUI();
        }
    }

    public virtual bool OnBack()
    {
        AudioManager.Instance.PlaySound(SfxNameConst.button_s);
        return UIManager.Instance.CloseWindow(mWindowName, true);
    }

    public void CloseWindowWithinUIMgr(bool destroy = false, Action callback = null)
    {
        callback?.Invoke();
        UIManager.Instance.CloseWindow(mWindowName, destroy);
    }

    public void ReloadWindow()
    {
        ShowUI();
        OnReloadWindow();
    }

    private void ShowUI()
    {
        mIsOpen = true;
        if (gameObject == null)
        {
            Debug.LogWarning($"UI已被销毁: {WindowName}");
            return;
        }

        gameObject.SetActive(true);
    }

    private void HideUI()
    {
        mIsOpen = false;
        gameObject.SetActive(false);
    }

    private void DestroyUI()
    {
        mIsOpen = false;
        Destroy(gameObject);
    }

    #region 子类继承重写

    /// <summary>
    /// 打开界面时调用
    /// </summary>
    /// <param name="objs"></param>
    protected virtual void OnOpenWindow(params object[] objs)
    {
    }

    /// <summary>
    /// 关闭界面时调用
    /// </summary>
    protected virtual void OnCloseWindow(bool destroy = false)
    {
    }

    /// <summary>
    /// 重新加载界面时调用
    /// </summary>
    protected virtual void OnReloadWindow()
    {
    }

    /// <summary>
    /// 私有Awake方法，会在基类Awake执行后调用
    /// </summary>
    public abstract void PrivateAwake();

    public GameObject BindEvent(string target, GameObject par = null, Action<GameObject> action = null, bool playAudio = true)
    {
        par ??= gameObject;

        GameObject obj = par.transform.Find(target)?.gameObject;
        if (obj == null)
        {
            Debug.LogError($"未找到 {gameObject.name}/{target}");
            return null;
        }

        Button button = obj.GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener
            (
                delegate()
                {
                    if (playAudio)
                    {
                        AudioManager.Instance.PlaySound(SfxNameConst.button_s);
                    }

                    action?.Invoke(obj);
                }
            );
        }

        return obj;
    }

    public GameObject FindObj(string path, GameObject par = null)
    {
        par ??= gameObject;
        return par.transform.Find(path)?.gameObject;
    }

    #endregion

    public GameObject BindClick(string target, Action<GameObject> action = null, GameObject par = null, bool playAudio = true)
    {
        par ??= gameObject;

        GameObject obj = par.transform.Find(target)?.gameObject;
        if (obj == null)
        {
            Debug.LogError($"未找到 {gameObject.name}/{target}");
            return null;
        }

        Button button = obj.GetComponent<Button>();
        if (button == null)
        {
            button = obj.AddComponent<Button>();
            obj.AddComponent<EmptyRaycast>();
        }

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() =>
        {
            if (playAudio)
                AudioManager.Instance.PlaySound(SfxNameConst.button_s);
            action?.Invoke(obj);
        });

        return obj;
    }

    public virtual void ClickUIMask()
    {
        if (!canClickMask)
            return;

        canClickMask = false;
        AnimCloseWindow();
    }

    public void SetCanClickMask(bool canClick)
    {
        canClickMask = canClick;
    }

    public virtual void AnimCloseWindow(Action closeCall = null, bool destroy = true)
    {
        if (_animator == null)
        {
            CloseWindowWithinUIMgr(destroy);
            closeCall?.Invoke();
            return;
        }

        StartCoroutine(CommonUtils.PlayAnimation(_animator, UIAnimationConst.DisAppear, null, () =>
        {
            CloseWindowWithinUIMgr(destroy);
            closeCall?.Invoke();
        }));
    }

    public virtual void UpdateCanvasSortOrder()
    {
    }
}