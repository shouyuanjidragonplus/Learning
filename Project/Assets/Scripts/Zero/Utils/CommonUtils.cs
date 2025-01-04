using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public static partial class CommonUtils
{
    public static int RandomIndexByProbability(this List<int> weights)
    {
        int sum = weights.Sum();

        int val = Mathf.CeilToInt(sum * Random.Range(0f, 1f));
        for (int i = 0; i < weights.Count; i++)
        {
            if (val <= weights[i])
                return i;
            val -= weights[i];
        }
        return -1;
    }
    public static T RandomOneByWeight<T>(this IEnumerable<T> data, Func<T, int> getWeight)
    {
        T ret = default(T);

        var totalWeight = 0;
        foreach (var item in data)
        {
            totalWeight += getWeight(item);
        }

        var randomWeight = UnityEngine.Random.Range(1, totalWeight + 1);
        var curWeight = 0;
        foreach (var item in data)
        {
            if (getWeight(item) == 0) continue;

            curWeight += getWeight(item);
            if (curWeight >= randomWeight)
            {
                ret = item;
                break;
            }
        }
        return ret;
    }
    public static int RandomIndexByWeight(List<int> weights)
    {
        int sum = weights.Sum();
        int random = Random.Range(1, sum);
        int cur_total = 0;
        for (int i = 0; i < weights.Count; i++)
        {
            cur_total += weights[i];
            if (random <= cur_total)
                return i;
        }

        return -1;
    }
    /// 随机排列数组元素
    /// </summary>
    /// <param name="myList"></param>
    /// <returns></returns>
    public static List<int> ListRandom(List<int> myList)
    {
        List<int> newList = new List<int>();
        int index = 0;
        int temp = 0;
        for (int i = 0; i < myList.Count; i++)
        {
            index = Random.Range(0, myList.Count - 1);
            if (index != i)
            {
                temp = myList[i];
                myList[i] = myList[index];
                myList[index] = temp;
            }
        }
        return myList;
    }


    public static void AddChild(Transform parent, Transform obj)
    {
        if (obj != null && parent != null)
        {
            obj.transform.SetParent(parent);
            obj.localPosition = Vector3.zero;
            obj.localRotation = Quaternion.identity;
            obj.localScale = Vector3.one;
        }
    }


    public static void SetVisible(GameObject obj, bool visible)
    {
        if (obj == null)
            return;

        SetVisible(obj.transform, visible);
    }

    public static void SetVisible(Transform obj, bool visible)
    {
        if (obj == null)
            return;

        if (obj.gameObject.activeSelf == visible)
            return;

        obj.gameObject.SetActive(visible);
    }

    public static string DisplayTimeStr(int second)
    {
        int hour = (int)second / 3600;
        int leftMinutes = (second % 3600) / 60;
        int leftSeconds = second % 60;
        if (hour == 0)
        {
            return string.Format("{0:d2}:{1:d2}", leftMinutes, leftSeconds);
        }
        if (hour >= 24)
        {
            if (hour % 24 == 0)
            {
                return $"{hour / 24}d";
            }
            return string.Format("{0}d {1:d2}h", hour / 24, hour % 24);
        }
        return string.Format("{0:d2}:{1:d2}:{2:d2}", hour, leftMinutes, leftSeconds);
    }

    public static DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1)); // 当地时区
    public static long GetTimeStamp()
    {
        long timeStamp = (long)(DateTime.Now - startTime).TotalMilliseconds;
        return timeStamp;
    }

    public static long GetUTCTime()
    {
        return ConvertDateTimeToTimeStamp(DateTime.UtcNow);
    }

    public static long GetCurTime()
    {
        return ConvertDateTimeToTimeStamp(DateTime.Now);
    }

    public static DateTime ConvertFromUnixTimestamp(ulong timestamp)
    {
        DateTime origin = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
        return origin.AddMilliseconds(timestamp);
    }

    public static long ConvertDateTimeToTimeStamp(DateTime date)
    {
        long timeStamp = (long)(date - startTime).TotalMilliseconds;
        return timeStamp;
    }

    public static bool IsSameDayWithToday(ulong millsecconds)
    {
        var startTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var dateold = startTime.AddMilliseconds(millsecconds).ToLocalTime();

        var todaydate = DateTime.UtcNow.ToLocalTime();
        if (dateold.Year == todaydate.Year && dateold.Month == todaydate.Month && dateold.Day == todaydate.Day)
        {
            return true;
        }
        return false;
    }

    public static IEnumerator DelayWork(float delayTime, Action action)
    {
        if (action != null)
        {
            yield return new WaitForSeconds(delayTime);
            action();
        }
    }

    /// <summary>
    /// 播放一次性特效
    /// </summary>
    /// <param name="trans"></param>
    /// <param name="mat"></param>
    public static void PlayOneShotParticle(GameObject prefabs, Transform trans, float delayDestroy = 0.5f)
    {
        if (prefabs == null) return;
        var obj = GameObject.Instantiate(prefabs, trans);
        if (obj == null) return;
        obj.SetActive(true);
        GameObject.Destroy(obj, delayDestroy);
    }

    /// <summary>
    /// 验证EMail是否合法
    /// </summary>
    /// <param name="email">要验证的Email</param>
    public static bool IsEmail(string email)
    {
        //如果为空，认为验证不合格
        if (string.IsNullOrEmpty(email))
        {
            return false;
        }
        //清除要验证字符串中的空格
        email = email.Trim();
        //模式字符串
        string pattern = @"^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$";
        //验证
        return new System.Text.RegularExpressions.Regex(pattern).IsMatch(email);
    }

    public static float GetAnimTime(Animator animator, string playAnimName)
    {
        float length = 1.0f;
        AnimationClip[] clips = animator.runtimeAnimatorController?.animationClips;
        if (clips != null)
        {
            foreach (AnimationClip clip in clips)
            {
                if (clip.name.Equals(playAnimName))
                {
                    length = clip.length;
                    break;
                }
            }
        }

        return length;
    }

    public static IEnumerator PlayAnimation(Animator animator, string playAnimName, string endAnimName, Action onEnd, bool isPlaySound = true)
    {
        if (animator != null)
        {
            animator.Play(playAnimName, 0, 0);
            float time = GetAnimTime(animator, playAnimName);
            yield return new WaitForSeconds(time);
            if (!string.IsNullOrEmpty(endAnimName))
            {
                animator.Play(endAnimName);
            }
            onEnd?.Invoke();
        }
    }

    public static IEnumerator DoProgressJump(Slider slider, float from, float to, float time, Action callBack = null)
    {
        int count = (int)(30 * time); // 1秒变化30次
        float timeDelta = time / count;
        float valueDelta = (to - from) / count;
        for (int i = 0; i <= count; i++)
        {
            if (i == count)
            {
                slider.value = to;
                if (callBack != null)
                {
                    callBack();
                }
            }
            else
            {
                slider.value = from + i * valueDelta;
            }
            yield return new WaitForSeconds(timeDelta);
        }
    }

    public static IEnumerator DoNumberJump(Text text, int from, int to, float time, string suffix = "", Action action = null)
    {
        int count = (int)(30 * time); // 1秒变化30次
        float timeDelta = time / count;
        float valueDelta = (float)(to - from) / count;
        for (int i = 0; i <= count; i++)
        {
            if (i == count)
            {
                text.text = to.ToString() + suffix;
            }
            else
            {
                text.text = ((int)(from + i * valueDelta)).ToString() + suffix;
            }
            yield return new WaitForSeconds(timeDelta);
        }
        if (action != null)
        {
            action();
        }
    }

    // 删除一个节点的全部子节点
    public static void DestroyAllChildren(Transform rootTransform)
    {
        foreach (Transform child in rootTransform)
        {
            UnityEngine.Object.Destroy(child.gameObject);
        }
    }

    public static Vector2 ScreenToCanvasPos(RectTransform parent, Vector3 screenPos, Camera uiCamera)
    {
        Vector2 position = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parent, screenPos, uiCamera, out position);
        return position;
    }

    public static Vector2 WorldToCanvasPos(RectTransform parent, Vector3 world, Camera uiCamera)
    {
        //world to screen
        Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(uiCamera, world);
        //screen to ui
        Vector2 position = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parent, screenPos, uiCamera, out position);
        return position;
    }

    // 判断宽屏设备
    public static bool IsWideScreenDevice()
    {
        return ((float)Screen.width / Screen.height <= 1.5f);
    }

    /// <summary>
    /// 保证最少是2位数
    /// </summary>
    /// <param name="number">Number.</param>
    public static string ConvertDateNumber(int number)
    {
        if (number < 10)
        {
            return "0" + number.ToString();
        }
        return number.ToString();
    }

    #region ScrollView扩展

    /// <summary>
    /// Horizontally align the ScrollRect to a RectTransform without scrolling past the edge of the content
    /// <param name="alignment"> 0 = left aligned, 1 = right aligned</param>
    /// </summary>
    public static void HorizontallyAlign(this ScrollRect scrollRect, RectTransform targetRectTransform, float alignment)
    {

        scrollRect.horizontalNormalizedPosition = scrollRect.CalculateHorizontalNormalizedPosition(targetRectTransform, alignment);
    }

    /// <summary>
    /// Get the normalized position of horizontally aligning the ScrollRect to a RectTransform without scrolling past the edge of the content
    /// <param name="alignment"> 0 = left aligned, 1 = right aligned</param>
    /// </summary>
    public static float CalculateHorizontalNormalizedPosition(this ScrollRect scrollRect, RectTransform targetRectTransform, float alignment)
    {

        // Get world position of the point of the traget transform
        // Alignment of 0 is left side of transform, 1 is right side
        Vector2 targetCenterLocalPosition = targetRectTransform.rect.center;
        float targetCenterOffset = targetRectTransform.rect.width * (alignment - 0.5f);
        Vector3 targetWorldPosition = targetRectTransform.TransformPoint(targetCenterLocalPosition + Vector2.right * targetCenterOffset);

        // Convert world position to position relative to scrollRect
        float targetXPosition = scrollRect.content.InverseTransformPoint(targetWorldPosition).x;

        // Calculate the new normalized position for the scroll rect by accounting
        // for the extra space on the left and right of the content where it can't 
        // be aligned without scrolling past the edge
        float contentWidth = scrollRect.content.rect.width;
        float scrollRectWidth = ((RectTransform)scrollRect.transform).rect.width;
        float newNormalizedPosition = (targetXPosition - (scrollRectWidth * 0.5f)) / (contentWidth - scrollRectWidth);

        // Clamp value between 0 and 1 so it dosen't scroll past the edge of the content
        return Mathf.Clamp01(newNormalizedPosition);
    }


    public static void Merge<TKey, TValue>(this IDictionary<TKey, TValue> first, IDictionary<TKey, TValue> second)
    {
        if (second == null || first == null) return;
        foreach (var item in second)
            if (!first.ContainsKey(item.Key))
                first.Add(item.Key, item.Value);
    }

    #endregion

    public static string FirstCharToUpper(string str)
    {
        char[] a = str.ToCharArray();
        a[0] = char.ToUpper(a[0]);
        return new string(a);
    }

    // 把配置里的奖励转换为字典 
    public static Dictionary<int, int> RewardStringToData(string rewardString, char splitChar1 = '#', char splitChar2 = ',')
    {
        var rewards = rewardString.Split(splitChar1);

        Dictionary<int, int> items = new Dictionary<int, int>();
        for (int index = 0; index < rewards.Length; index++)
        {
            var reward = rewards[index].Split(splitChar2);
            int rewardId = int.Parse(reward[0]);

            if (rewardId != -1)
                if (reward.Length == 2)
                    items[rewardId] = int.Parse(reward[1]);
                else if (reward.Length == 3)
                {
                    int value = Mathf.Min(int.Parse(reward[1]), int.Parse(reward[2]));
                    items[rewardId] = value;
                }
        }

        return items;
    }

    // 从[min, max) 中随机选择count个不重复的随机数字（不包含max)
    public static List<int> GenerateRandom(int count, int min, int max)
    {

        if (max <= min || count < 0 || (count > max - min && max - min > 0))
        {
            throw new ArgumentOutOfRangeException("Range " + min + " to " + max +
                                                  " (" + ((Int64)max - (Int64)min) + " values), or count " + count + " is illegal");
        }

        HashSet<int> candidates = new HashSet<int>();

        for (int top = max - count; top < max; top++)
        {
            UnityEngine.Random.InitState((int)System.DateTime.Now.Ticks);
            if (!candidates.Add(UnityEngine.Random.Range(min, top + 1)))
            {
                candidates.Add(top);
            }
        }

        return candidates.ToList();
    }

    public static bool IsLE_16_10()
    {
        float maxR = Mathf.Max(Screen.width, Screen.height);
        float minR = Mathf.Min(Screen.width, Screen.height);
        var ratio = (maxR / minR) <= 1.605f;
        //DebugUtil.LogError($"IsPad {Screen.width} {Screen.height} {isiPad}");
        return ratio;
    }
    public static float GetScreenRatio()
    {
        float maxR = Mathf.Max(Screen.width, Screen.height);
        float minR = Mathf.Min(Screen.width, Screen.height);
        var ratio = maxR / minR;
        //DebugUtil.LogError($"IsPad {Screen.width} {Screen.height} {isiPad}");
        return ratio;
    }
    /// <summary>
    /// 随机排列列表顺序
    /// </summary>
    /// <param name="ListT"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static List<T> RandomSortList<T>(List<T> ListT)
    {
        System.Random random = new System.Random();
        List<T> newList = new List<T>();
        foreach (T item in ListT)
        {
            newList.Insert(random.Next(newList.Count + 1), item);
        }
        return newList;
    }


    public static int StringToInt(string x)
    {
        try
        {
            return Convert.ToInt32(x);
        }
        catch (Exception ex)
        {
            Debug.LogError(ex);
            return 0;
        }
    }

    /// <summary>
    /// "1,2,3"字符串转int list
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static List<int> StringToIntList(string str)
    {
        var result = new List<int>();
        try
        {
            string[] arr = str.Split(',');
            if (arr != null)
            {
                for (int i = 0; i < arr.Length; i++)
                {
                    result.Add(StringToInt(arr[i]));
                }
            }

            return result;
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }

        return result;
    }

    public static DateTime ConvertFromTimestampWithZone(ulong timestamp)
    {
        DateTime origin = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
        return origin.AddMilliseconds(timestamp);
    }

    public static string ConnectString(string str, params object[] args)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendFormat(str, args);
        return sb.ToString();
    }

    public static void NotchAdapte(Transform transform)
    {
        if (transform == null)
            return;

        NotchAdapte(transform as RectTransform);
    }
    public static void NotchAdapte(RectTransform rectTransform)
    {
        if (rectTransform == null)
            return;

        int safeAreaOffset = (int)(Screen.height - Screen.safeArea.yMax);
        if (safeAreaOffset == 0)
            return;

        safeAreaOffset = safeAreaOffset / 2;
        float scaleRatio = 1.0f * 1366 / Screen.height;
        safeAreaOffset = (int)(safeAreaOffset * scaleRatio);
        safeAreaOffset += 15;

        rectTransform.offsetMax = new Vector2(0, -safeAreaOffset);
    }
    public static int SafeAreaOffset()
    {
        int safeAreaOffset = (int)(Screen.height - Screen.safeArea.yMax);

        return safeAreaOffset - safeAreaOffset / 2;
    }
    public static bool IsNotch()
    {
        int safeAreaOffset = (int)(Screen.height - Screen.safeArea.yMax);
        return safeAreaOffset > 0;
    }
    public static void NotchAdapte(RectTransform rectTransform, float offsetSet)
    {
        if (rectTransform == null)
            return;

        rectTransform.offsetMax = new Vector2(0, -offsetSet);
    }

    public static bool IsLevelUnlocked(int limitLevel)
    {
        return false;
    }

    public static void SafeAddDict(Dictionary<int, int> dictionary, int key, int value)
    {
        if (dictionary == null)
            return;
        if (dictionary.ContainsKey(key))
        {
            dictionary[key] = dictionary[key] + value;
        }
        else
        {
            dictionary.Add(key, value);
        }
    }

    public static void AddChild(Transform parent, Transform obj, bool resetState = true)
    {
        if (obj != null && parent != null)
        {
            obj.transform.SetParent(parent, false);
            if (resetState)
            {
                obj.localPosition = Vector3.zero;
                obj.localRotation = Quaternion.identity;
                obj.localScale = Vector3.one;
            }
        }
    }

    public static T GetComponent<T>(GameObject owner) where T : MonoBehaviour
    {
        T targetT = owner.GetComponent<T>();
        if (targetT == null)
            targetT = owner.AddComponent<T>();

        return targetT;
    }

    private static readonly DateTime TIMEZONE_1970 = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1)); // 当地时区

    public static IEnumerator DoNumberJump(Text text, int from, int to, float time, string prefix = "", string suffix = "")
    {
        int count = (int)(30 * time); // 1秒变化30次
        float timeDelta = time / count;
        float valueDelta = (float)(to - from) / count;
        for (int i = 0; i <= count; i++)
        {
            if (i == count)
            {
                text.text = prefix + to.ToString() + suffix;
            }
            else
            {
                text.text = prefix + ((int)(from + i * valueDelta)).ToString() + suffix;
            }
            yield return new WaitForSeconds(timeDelta);
        }
    }

    public static bool IsFullScreenIOS()
    {
#if UNITY_IOS && !UNITY_EDITOR
        var generation = UnityEngine.iOS.Device.generation;
        if (generation == UnityEngine.iOS.DeviceGeneration.iPhoneX ||
            generation == UnityEngine.iOS.DeviceGeneration.iPhoneXR ||
            generation == UnityEngine.iOS.DeviceGeneration.iPhoneXS ||
            generation == UnityEngine.iOS.DeviceGeneration.iPhoneXSMax)
        {
            return true;
        }
#endif
        return false;
    }

    /// <summary>
    /// usingFullFormat = false
    /// 小于一天时：显示小时：分钟：秒（无单位）。
    /// 大于一天时：显示nd nh（有单位），n为个位数时，不显示前面的0（如3h而不是03h）。
    /// 计算小时数时，被舍去的分钟数大于等于30分钟记为当前时间数+1小时，小于30分钟时不进位。当小时数最终计算为0时，时间只显示nd。（3d 23h 30m 01s应显示为4d）
    /// </summary>
    /// <param name="l"></param>
    /// <param name="usingFullFormat"></param>
    /// <returns></returns>
    public static String FormatLongToTimeStr(long l, bool usingFullFormat = false)
    {
        int hour = 0;
        int minute = 0;
        int second = 0;
        int day = 0;

        l = l < 0 ? 0 : l;
        second = (int)(l / 1000);

        if (second >= 60)
        {
            minute = second / 60;
            second = second % 60;
        }
        if (minute >= 60)
        {
            hour = minute / 60;
            minute = minute % 60;
        }

        if (hour >= 24)
        {
            day = hour / 24;
            hour = hour % 24;
        }

        if (usingFullFormat)
        {
            return day > 0 ? $"{day}d {hour:D2}:{minute:D2}" : $"{hour:D2}:{minute:D2}:{second:D2}";
        }
        else if (day > 0)
        {
            if (hour > 0)
            {
                return $"{day}d {hour}h";
            }
            else if (minute >= 30)
            {
                return $"{day}d 1h";
            }
            else
            {
                return $"{day}d";
            }
        }
        else
        {
            return $"{hour:D2}:{minute:D2}:{second:D2}";
        }
    }


    #region 判断是否是pad类设备 && dp px互转
    private static float padRatio = 0.62f;// 1200x1920 pad 0.625  // 3:4 pad 0.75
    public static bool IsPad()
    {
        return (float)Screen.width / Screen.height > padRatio;
    }

    public static float Px2Dp(float px)
    {
        return px / (Screen.dpi / 160f);
    }

    public static float Dp2Px(float dp)
    {
        return dp * (Screen.dpi / 160f);
    }
    #endregion

    public static T Find<T>(Transform transform, string name) where T : UnityEngine.Object
    {
        if (transform == null) return null;
        if (string.IsNullOrEmpty(name)) return null;
        Transform finds = transform.Find(name);
        if (finds == null)
        {
            Debug.LogWarning($"Find<T> -> Not find '{name}' in {transform.name}");
            return null;
        }
        return finds.GetComponent<T>();
    }

    public static List<T> FindChildren<T>(Transform transform, string prefixKey, int startIdx = 0) where T : UnityEngine.Object
    {
        List<T> result = new List<T>();
        if (transform == null) return result;
        for (int i = startIdx; i <= transform.childCount; i++)
        {
            T findT = Find<T>(transform, $"{prefixKey}{i}");
            if (findT == null) continue;
            result.Add(findT);
        }
        return result;
    }

    public static T FindOrCreate<T>(Transform transform, string name) where T : UnityEngine.MonoBehaviour
    {
        if (transform == null) return null;
        if (string.IsNullOrEmpty(name)) return null;
        Transform finds = transform.Find(name);
        if (finds == null)
        {
            Debug.LogError($"FindOrCreate<T> -> Not find '{name}' in {transform.name}");
            return null;
        }

        return GetComponent<T>(finds.gameObject);
    }

    public static GameObject FindObject(Transform transform, string name)
    {
        if (transform == null) return null;
        if (string.IsNullOrEmpty(name)) return null;
        Transform finds = transform.Find(name);
        if (finds == null)
        {
            Debug.LogError($"FindObject -> Not find '{name}' in {transform.name}");
            return null;
        }
        return finds.gameObject;
    }

    public static IEnumerator DelayWorkFrame(int delayFrame, Action action)
    {
        if (action != null)
        {
            for (int i = 0; i < delayFrame; i++)
                yield return new WaitForEndOfFrame();
            action();
        }
    }

    public static int GetValue<T>(Dictionary<T, int> dict, T key)
    {
        int value;
        dict.TryGetValue(key, out value);
        return value;
    }

    public static void AddValue<T>(Dictionary<T, int> dict, T key, int value)
    {
        int oldValue;
        dict.TryGetValue(key, out oldValue);
        dict[key] = oldValue + value;
    }

    public static void Swap<T>(T[] array, int a, int b)
    {
        T temp = array[a];
        array[a] = array[b];
        array[b] = temp;
    }

    public static void Swap<T>(T[] array, ref int a, ref int b)
    {
        Swap(array, a, b);
        a ^= b;
        b ^= a;
        a ^= b;
    }

    public static void AddClickListener(Button button, UnityAction action)
    {
        button.onClick.AddListener(action);
    }
    public static RectTransform AnchorPosX(this RectTransform selfRectTrans, float anchorPosX)
    {
        var anchorPos = selfRectTrans.anchoredPosition;
        anchorPos.x = anchorPosX;
        selfRectTrans.anchoredPosition = anchorPos;
        return selfRectTrans;
    }
    public static RectTransform AnchorPosY(this RectTransform selfRectTrans, float anchorPosY)
    {
        var anchorPos = selfRectTrans.anchoredPosition;
        anchorPos.y = anchorPosY;
        selfRectTrans.anchoredPosition = anchorPos;
        return selfRectTrans;
    }
}