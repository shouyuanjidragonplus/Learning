using GameFramework.Procedure;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

public class ProcedureSplash : ProcedureBase
{
    protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

        // TODO: 这里可以播放一个 Splash 动画
        // ...

        // if (GF.Base.EditorResourceMode)
        // {
        //     ChangeState<ProcedureUpdateResources>(procedureOwner);
        // }
        // else
        // {
            ChangeState<ProcedureLoadHotfixDll>(procedureOwner);
        //}
    }
}