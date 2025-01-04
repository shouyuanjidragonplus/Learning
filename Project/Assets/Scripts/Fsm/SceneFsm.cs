using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum StatusType
{
    Non,
    Loading,
    Login, //登录
    Game,
    Home,
    BackHome,
    Transition,
    LoadRoom,
    BackLogin,
}

public class SceneFsm : MonoSingleton<SceneFsm>
{
    public bool ClientInited = false;
    public bool ClientLogin = false;

    private Dictionary<StatusType, IFsmState> mStateDictionary = new Dictionary<StatusType, IFsmState>();

    private IFsmState currentState = null;
    private IFsmState prevState = null;

    public void AwakeProc()
    {
        mStateDictionary.Add(StatusType.Loading, new SceneFsmLoading());
        mStateDictionary.Add(StatusType.Login, new SceneFsmLogin());
        //mStateDictionary.Add(StatusType.Home, new SceneFsmHome());
        //mStateDictionary.Add(StatusType.Game, new SceneFsmGame());
        //mStateDictionary.Add(StatusType.Transition, new SceneFsmTransition());
        //mStateDictionary.Add(StatusType.BackHome, new SceneFsmBackHome());
        //mStateDictionary.Add(StatusType.LoadRoom, new SceneFsmLoadRoom());
        //mStateDictionary.Add(StatusType.BackLogin, new SceneFsmBackLogin());

        StartGame();
    }
    public void StartGame()
    {
        ChangeState(StatusType.Loading, StatusType.Login);
    }

    public void BackToLogin()
    {
        SceneFsm.Instance.ClientInited = false;
        ChangeState(StatusType.BackLogin, StatusType.Login);
    }

    public void BackHome(params object[] objs)
    {
        List<object> parm = new List<object>();
        parm.Add(StatusType.BackHome);

        if (objs != null && objs.Length > 0)
        {
            foreach (var kv in objs)
            {
                parm.Add(kv);
            }
        }

        ChangeState(StatusType.Transition, parm.ToArray());
    }

    public void EnterGame()
    {
        if (currentState == null || currentState.Type == StatusType.Login || currentState.Type == StatusType.BackLogin)
        {
            int roomId = -1;
            ChangeState(StatusType.LoadRoom, StatusType.Home, roomId);
        }
    }

    //public void TransitionGame(MergeBoardEnum boardType = MergeBoardEnum.Main, OrderType type = OrderType.None)
    //{
    //    MergeManager.Instance.Refresh(boardType);
    //    GuideSubSystem.Instance.FinishCurrent(GuideTargetType.PlayButton);
    //    SceneFsm.Instance.ChangeState(StatusType.Transition, StatusType.Game, boardType, type);
    //}

    public void ChangeState(StatusType type, params object[] objs)
    {
        if (mStateDictionary.TryGetValue(type, out IFsmState newState))
        {
            if (currentState == newState)
                return;

            StartCoroutine(ChangeState(newState, objs));
        }
    }

    public void ChangeStateForce(StatusType type, params object[] objs)
    {
        if (mStateDictionary.TryGetValue(type, out IFsmState newState))
            StartCoroutine(ChangeState(newState, objs));
    }

    IEnumerator ChangeState(IFsmState newStatus, params object[] objs)
    {
        yield return new WaitForEndOfFrame();

        prevState = currentState;

        if (currentState != null)
        {
            var oldState = currentState;
            currentState.Exit();
            EventDispatcher.Instance.DispatchEventImmediately(EventEnum.ExitFsmState, oldState);
        }

        currentState = newStatus;
        currentState.Enter(objs);
        EventDispatcher.Instance.DispatchEventImmediately(EventEnum.EnterFsmState, newStatus);
    }

    public void Update()
    {
        if (currentState == null)
            return;
        currentState.Update(Time.deltaTime);
    }
    public void LateUpdate()
    {
        if (currentState == null)
            return;
        currentState.LateUpdate(Time.deltaTime);
    }

    public StatusType GetCurrSceneType()
    {
        if (currentState == null)
            return StatusType.Non;

        return currentState.Type;
    }

    public IFsmState GetCurrentState()
    {
        return currentState;
    }
}