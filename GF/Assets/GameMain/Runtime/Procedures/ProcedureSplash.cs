﻿using GameFramework.Procedure;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace GameMain
{
    public class ProcedureSplash : ProcedureBase
    {
        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            // TODO: 这里可以播放一个 Splash 动画
            // ...
            //ChangeState(procedureOwner, GameEntry.Base.EditorResourceMode ? typeof(ProcedureLoadHotfixDll) : typeof(ProcedureUpdateResources));
            ChangeState(procedureOwner, typeof(ProcedureCheckUpdateInfo));
        }
    }
}