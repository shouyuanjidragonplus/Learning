using UnityEngine;
public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static bool m_ShuttingDown = false;
    private static object m_Lock = new object();
    private static T m_Instance;
    public static T Instance
    {
        get
        {
            lock (m_Lock)
            {
                if (m_Instance == null)
                {
                    m_Instance = (T)FindObjectOfType(typeof(T));
                    if (m_Instance == null)
                    {
                        var singletonObject = new GameObject();
                        m_Instance = singletonObject.AddComponent<T>();
                        singletonObject.name = typeof(T) + " (Singleton)";
                        //singletonObject.hideFlags = HideFlags.HideInHierarchy;
                        if (Application.isPlaying) DontDestroyOnLoad(singletonObject);
                        (m_Instance as MonoSingleton<T>).InitImmediately();
                    }
                }

                return m_Instance;
            }
        }
    }

    // 这个方法如果override，会在Instance创建完立刻调用, 派生类可以用来默认初始化一些东西
    protected virtual void InitImmediately()
    {
    }

    private void OnApplicationQuit()
    {
        m_ShuttingDown = true;
        m_Instance = null;
    }
    private void OnDestroy()
    {
    }
}
