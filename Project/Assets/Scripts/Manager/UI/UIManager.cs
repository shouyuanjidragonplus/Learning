using System;
using System.Collections;
using System.Collections.Generic;
using Framework.Asset;
using UnityEngine;
using UnityEngine.UI;

public partial class UIManager : Manager<UIManager>
{
    public Dictionary<string, UIWindow> mMemoryWindows = new Dictionary<string, UIWindow>();
    public Stack<UIWindow> mStackWindows = new Stack<UIWindow>();

    private Dictionary<int, LinkedList<UIWindow>> _layerList = new Dictionary<int, LinkedList<UIWindow>>(16);
    private ScreenOrientation _currentOrientation;
    private bool m_HavePopUp = false;

    public bool HavePopUpWindow => m_HavePopUp;

    public void Init()
    {
        allWindowMeta();
    }

    /// <summary>
    /// 打开一个UI
    /// UIManager.Instance.OpenHomeWindow("Common/GameUI")
    /// </summary>
    public UIWindow OpenView(string uipath, params object[] objs)
    {
        try
        {
            if (!_windowsInfo.ContainsKey(uipath))
            {
                Debug.LogError($"Create ui fail: {uipath}, no windows info inited");
                return null;
            }

            var wInfo = _windowsInfo[uipath];
            var windowType = wInfo.windowType;
            var uiWindowLayer = wInfo.windowLayer;
            if (windowType == UIWindowType.PopupTip && !m_HavePopUp)
            {
                m_HavePopUp = true;
            }

            if (!mMemoryWindows.TryGetValue(uipath, out var window))
            {
                window = UIRoot.Instance.CreateWindow(uipath, windowType, componentType: wInfo.componentType);

                if (window == null)
                {
                    Debug.LogError("Create ui fail: " + uipath);
                    return null;
                }

                mMemoryWindows[uipath] = window;
                window.WindowName = uipath;
                window.m_WindowType = windowType;
                window.uiWindowLayer = uiWindowLayer;

                InitUIMask(window, wInfo);
            }

            if (windowType != UIWindowType.Fixed) //不是常显UI
            {
                HandleWindowStack(window, windowType);
            }

            window.gameObject.SetActive(true);
            LayerListInsertWindow(window);
            window.OpenWindow(objs);
            return window;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error opening window {uipath}: {e.Message}");
            return null;
        }
    }

    private void HandleWindowStack(UIWindow window, UIWindowType windowType)
    {
        if (!mStackWindows.Contains(window))
        {
            if (windowType == UIWindowType.Normal && mStackWindows.Count > 0)
            {
                var stackWindow = mStackWindows.Peek();
                stackWindow?.CloseWindowWithinUIMgr(true);
            }

            mStackWindows.Push(window);
        }
        else
        {
            while (mStackWindows.Count > 0 && !mStackWindows.Peek().Equals(window))
            {
                mStackWindows.Peek().CloseWindowWithinUIMgr(true);
                mStackWindows.Pop();
            }
        }
    }

    private GameObject uiMaskObj = null;

    private void InitUIMask(UIWindow uiWindow, WindowInfo info)
    {
        if (uiWindow == null || !info.addUIMask)
            return;
        try
        {
            uiMaskObj ??= ResourcesManager.Instance.LoadResource<GameObject>("Prefabs/Common/UIMask");

            if (uiMaskObj == null)
                return;

            GameObject maskObj = Instantiate(uiMaskObj, uiWindow.transform, false);
            if (maskObj == null)
                return;
            maskObj.transform.SetAsFirstSibling();

            Button emptyBut = maskObj.GetComponent<Button>();
            if (emptyBut == null)
                return;

            emptyBut.onClick.AddListener(uiWindow.ClickUIMask);
        }
        catch (Exception e)
        {
            Debug.LogError($"Error initializing UI mask: {e.Message}");
        }
    }

    public bool Back()
    {
        if (mMemoryWindows == null || mMemoryWindows.Count <= 0)
        {
            return false;
        }

        foreach (var window in mMemoryWindows.Values)
        {
            if (window != null && window.m_WindowType != UIWindowType.Fixed)
            {
                return window.OnBack();
            }
        }

        return false;
    }

    public bool CloseWindow(string wName, bool destroy = false)
    {
        if (!mMemoryWindows.TryGetValue(wName, out var window))
            return false;

        window.CloseWindow(destroy);
        if (destroy)
        {
            LayerListRemoveWindow(window);
            mMemoryWindows.Remove(wName);
        }

        if (mStackWindows.Count > 0 && mStackWindows.Peek().Equals(window))
        {
            mStackWindows.Pop();
            while (mStackWindows.Count > 0 && !mMemoryWindows.ContainsKey(mStackWindows.Peek().WindowName))
            {
                mStackWindows.Pop();
            }

            if (mStackWindows.Count > 0)
            {
                mStackWindows.Peek().ReloadWindow();
            }
        }

        CheckPopUpWidow();
        return true;
    }

    private void CheckPopUpWidow()
    {
        foreach (UIWindow baseUI in mMemoryWindows.Values)
        {
            if (baseUI.m_WindowType == UIWindowType.PopupTip)
            {
                return;
            }
        }

        foreach (UIWindow staUI in mStackWindows)
        {
            if (staUI.m_WindowType == UIWindowType.PopupTip)
            {
                return;
            }
        }

        m_HavePopUp = false;
    }

    public T GetOpenedWindowByPath<T>(string uipath) where T : UIWindow
    {
        if (mMemoryWindows.TryGetValue(uipath, out var window) && window.gameObject.activeSelf)
            return window as T;

        return default;
    }

    public T GetUIWindow<T>() where T : UIWindow
    {
        foreach (var window in mMemoryWindows.Values)
        {
            if (window is T)
                return window as T;
        }

        return null;
    }

    public UIWindow GetOpenedWindowByPath(string uipath)
    {
        return GetOpenedWindowByPath<UIWindow>(uipath);
    }

    public UIWindow GetWindowByPath(string uipath)
    {
        mMemoryWindows.TryGetValue(uipath, out var window);
        return window;
    }

    public void UpdateAliveWindowSiblingIndex()
    {
        int siblingIndex = 5 + extraSiblingIndex; // 让出loading其他想挂在canvas下的奇怪东西
        int siblingStep = 10;
        for (int i = 0; i <= (int)UIWindowLayer.Max; i++)
        {
            if (_layerList.TryGetValue(i, out var layerWindows) && layerWindows != null)
            {
                if (layerWindows.Count > 0)
                {
                    var e = layerWindows.GetEnumerator();
                    while (e.MoveNext())
                    {
                        if (e.Current.canvas == null)
                            e.Current.InitCanvas();

                        if (e.Current.canvas != null && e.Current.gameObject.activeSelf && e.Current.canvas.sortingOrder < 1000)
                        {
                            e.Current.canvas.sortingOrder = siblingIndex;
                            e.Current.UpdateCanvasSortOrder();
                        }

                        siblingIndex += siblingStep;
                    }
                }
            }
        }
    }

    public int GetMaxSortingOrder()
    {
        int maxOrder = -1;
        for (int i = 0; i <= (int)UIWindowLayer.Max; i++)
        {
            if (_layerList.TryGetValue(i, out var layerWindows) && layerWindows != null)
            {
                if (layerWindows.Count > 0)
                {
                    var e = layerWindows.GetEnumerator();
                    while (e.MoveNext())
                    {
                        if (e.Current.canvas == null)
                            e.Current.InitCanvas();

                        if (e.Current.canvas == null)
                            continue;

                        if (!e.Current.gameObject.activeSelf)
                            continue;

                        maxOrder = Math.Max(maxOrder, e.Current.canvas.sortingOrder);
                    }
                }
            }
        }

        return maxOrder;
    }

    public int GetCurrentOpenWindowCount()
    {
        int count = 0;
        for (int i = 0; i <= (int)UIWindowLayer.Max; i++)
        {
            if (_layerList.TryGetValue(i, out var layerWindows) && layerWindows != null)
            {
                if (layerWindows.Count > 0)
                {
                    var e = layerWindows.GetEnumerator();
                    while (e.MoveNext())
                    {
                        if (e.Current.canvas == null)
                            continue;

                        if (e.Current.WindowName == UINameConst.MergeMain || e.Current.WindowName == UINameConst.UICurrencyGroup || e.Current.WindowName == UINameConst.UIMainHome)
                        {
                            count++;
                        }
                        else
                        {
                            if (!e.Current.gameObject.activeSelf)
                                continue;

                            count++;
                        }
                    }
                }
            }
        }

        return count;
    }

    public int extraSiblingIndex = 0;

    void LayerListInsertWindow(UIWindow window)
    {
        try
        {
            if (window != null)
            {
                if (_layerList.TryGetValue((int)window.uiWindowLayer, out var currentLayerWindows)
                    && currentLayerWindows != null
                    && currentLayerWindows.Count > 0)
                {
                    if (currentLayerWindows.Remove(window))
                    {
                    }

                    currentLayerWindows.AddLast(window);
                    Debug.Log($"{window.WindowName} sibling index is {window.transform.GetSiblingIndex()}");
                    //return;
                }
                else
                {
                    if (currentLayerWindows == null)
                    {
                        currentLayerWindows = new LinkedList<UIWindow>();
                        _layerList.Add((int)window.uiWindowLayer, currentLayerWindows);
                    }

                    currentLayerWindows.AddLast(window);
                }

                UpdateAliveWindowSiblingIndex();

                var index = window.transform.GetSiblingIndex();
                Debug.Log($"{window.WindowName} sibling index is {index}");
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            Debug.LogError($"{GetType()}.LayerListInsertWindow error, window name = {window?.name}");
        }
    }

    void LayerListRemoveWindow(UIWindow window)
    {
        try
        {
            if (window != null)
            {
                if (_layerList.TryGetValue((int)window.uiWindowLayer, out var currentLayerWindows))
                {
                    currentLayerWindows?.Remove(window);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }

    public bool GetTopWindow(out UIWindow window, out UIWindowLayer layer, int banLayerMask)
    {
        for (int i = (int)UIWindowLayer.Max; i >= 0; i--)
        {
            if (((1 << i) & banLayerMask) == 0)
            {
                if (_layerList.TryGetValue(i, out var highLayerWindows))
                {
                    if (highLayerWindows.Count > 0)
                    {
                        window = highLayerWindows.Last.Value;
                        if (window != null)
                        {
                            layer = (UIWindowLayer)i;
                            return true;
                        }
                    }
                }
            }
        }

        window = null;
        layer = UIWindowLayer.Max;
        return false;
    }

    public int LayerMask(UIWindowLayer layer)
    {
        return 1 << ((int)layer);
    }

    IEnumerator SetFullScreenIOS()
    {
        var wait = new WaitForSeconds(0.67f);
        yield return wait;
        while (true)
        {
            var screenOrientation = DragonNativeBridge.GetScreenOrientation();
            if (screenOrientation != _currentOrientation)
            {
                _currentOrientation = screenOrientation;
                EventDispatcher.Instance.DispatchEvent(EventEnum.SCREEN_ORIENTATION_CHANGED, _currentOrientation);
            }

            yield return wait;
        }
    }

    public void EnableGraphicRaycaster(bool enable)
    {
        for (int i = 0; i <= (int)UIWindowLayer.Max; i++)
        {
            if (!_layerList.ContainsKey(i))
                continue;

            var layerWindows = _layerList[i];
            if (layerWindows == null || layerWindows.Count <= 0)
                continue;

            var e = layerWindows.GetEnumerator();
            while (e.MoveNext())
            {
                var layerObj = e.Current.gameObject;
                var raycaster = layerObj.GetComponent<GraphicRaycaster>();
                if (raycaster)
                {
                    raycaster.enabled = enable;
                }
            }
        }
    }

    private readonly Vector2 _hidePosition = new Vector2(10000f, 10000f);

    public void SetCanvasGroupAlpha(bool isShow, bool anim)
    {
        float alpha = isShow ? 1.0f : 0f;

        for (int i = 0; i <= (int)UIWindowLayer.Max; i++)
        {
            if (!_layerList.ContainsKey(i))
                continue;

            var layerWindows = _layerList[i];
            if (layerWindows == null || layerWindows.Count <= 0)
                continue;

            var e = layerWindows.GetEnumerator();
            while (e.MoveNext())
            {
                var layerObj = e.Current.gameObject;
                if (e.Current.canvasGroup == null)
                {
                    e.Current.canvasGroup = e.Current.gameObject.GetOrCreateComponent<CanvasGroup>();
                }

                if (e.Current.canvasGroup == null)
                    continue;

                if (isShow && e.Current.gameObject.name.Contains("UIHomeMain"))
                    ((RectTransform)e.Current.transform).anchoredPosition = Vector2.zero;

                e.Current.canvasGroup.DOKill();
                if (anim)
                {
                    e.Current.canvasGroup.DOFade(alpha, 0.5f).OnComplete(() =>
                    {
                        if (!isShow && layerObj.name.Contains("UIHomeMain"))
                            ((RectTransform)layerObj.transform).anchoredPosition = _hidePosition;
                    });
                }
                else
                {
                    e.Current.canvasGroup.alpha = alpha;

                    if (!isShow && layerObj.name.Contains("UIHomeMain"))
                        ((RectTransform)layerObj.transform).anchoredPosition = _hidePosition;
                }
            }
        }

        UIRoot.Instance.worldCanvasGroup.DOKill();
        if (anim)
        {
            UIRoot.Instance.worldCanvasGroup.DOFade(alpha, 0.5f);
        }
        else
        {
            UIRoot.Instance.worldCanvasGroup.alpha = alpha;
        }
    }
}