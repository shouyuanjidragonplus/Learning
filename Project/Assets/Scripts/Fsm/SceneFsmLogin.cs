using DG.Tweening;
public class SceneFsmLogin : IFsmState
{
    public StatusType Type => StatusType.Login;

    public void Enter(params object[] objs)
    {
        WaitingManager.Instance.OpenWindow(10.0f, 0.5f, () =>
        {
            LoadingController.HideLoading();
            UIManager.Instance.OpenView(UINameConst.UILogin);
        });
        
        UILoginController.RunOnce();
        AccountManager.Instance.Login(OnLoginResult, () =>
        {
            WaitingManager.Instance.CloseWindow();
            LoadingController.HideLoading();
            UIManager.Instance.OpenView(UINameConst.UILogin);
        });
        EventDispatcher.Instance.DispatchEventImmediately(EventEnum.BackLogin);
    }

    private void OnLoginResult(bool loginResult)
    {
        if (loginResult)
        {
            UIPopupSaveProgressController.SendLoginEvent();
        }
 
        UILoginController.EnterGame();
    }
    public void Update(float deltaTime)
    {
    }
    public void LateUpdate(float deltaTime)
    {
        
    }
    public void Exit()
    {
        DOTween.KillAll(true);
    }
}