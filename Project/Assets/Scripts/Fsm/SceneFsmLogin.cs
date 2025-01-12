using DG.Tweening;
public class SceneFsmLogin : IFsmState
{
    public StatusType Type => StatusType.Login;

    public void Enter(params object[] objs)
    {
        EventDispatcher.Instance.DispatchEventImmediately(EventEnum.BackLogin);
    }

    private void OnLoginResult(bool loginResult)
    {
      
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