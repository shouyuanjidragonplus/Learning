using Framework.Base;

namespace Zero.Base
{
    /// <summary>
    /// 核心逻辑移至Plugins ，此处直接返回BaseCoroutineHelper，后面要进行扩展加强可在此处理
    /// </summary>
    public class CoroutineHelper
    {
        public static BaseCoroutineHelper Instance
        {
            get
            {
                return BaseCoroutineHelper.Instance;
            }
        }
    }
}
