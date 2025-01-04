using System.Collections.Generic;
using System;
public static class IListExtensions
{
    public static T Random<T>(this IList<T> list)
    {
        if (list.Count == 0)
        {
            throw new IndexOutOfRangeException("List needs at least one entry to call Random()");
        }

        if (list.Count == 1)
        {
            return list[0];
        }

        return list[UnityEngine.Random.Range(0, list.Count)];
    }

    public static T RandomOrDefault<T>(this IList<T> list)
    {
        if (list.Count == 0)
        {
            return default(T);
        }

        return list.Random();
    }

    public static T PopLast<T>(this IList<T> list)
    {
        if (list.Count == 0)
        {
            throw new InvalidOperationException();
        }

        var t = list[list.Count - 1];

        list.RemoveAt(list.Count - 1);

        return t;
    }

    static readonly Random s_SharedRandom = new Random();

    /// <param name="elements"></param>
    /// <param name="weightSum"> 传入0时，默认累加</param>
    /// <param name="getElementWeight"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T WeightedSelection<T>(this IList<T> elements, int weightSum, Func<T, int> getElementWeight)
    {
        if (weightSum <= 0)
        {
            weightSum = 0;
            for (int i = 0; i < elements.Count; i++)
            {
                weightSum += getElementWeight(elements[i]);
            }
        }

        int index = elements.WeightedSelectionIndex(weightSum, getElementWeight);
        return elements[index];
    }

    /// <param name="elements"></param>
    /// <param name="weightSum"> 传入0时，默认累加</param>
    /// <param name="getElementWeight"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T WeightedSelection<T>(this IList<T> elements, float weightSum, Func<T, float> getElementWeight)
    {
        if (weightSum <= 0)
        {
            weightSum = 0;
            for (int i = 0; i < elements.Count; i++)
            {
                weightSum += getElementWeight(elements[i]);
            }
        }

        int index = elements.WeightedSelectionIndex(weightSum, getElementWeight);
        return elements[index];
    }

    /// <param name="elements"></param>
    /// <param name="weightSum"> 传入0时，默认累加</param>
    /// <param name="getElementWeight"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static int WeightedSelectionIndex<T>(this IList<T> elements, int weightSum, Func<T, int> getElementWeight)
    {
        if (weightSum <= 0)
        {
            weightSum = 0;
            for (int i = 0; i < elements.Count; i++)
            {
                weightSum += getElementWeight(elements[i]);
            }
        }

        int selectionIndex = 0;
        int selectionWeightIndex = UnityEngine.Random.Range(0, weightSum);
        int elementCount = elements.Count;

        if (elementCount == 0)
        {
            throw new InvalidOperationException("Cannot perform selection on an empty collection");
        }

        int itemWeight = getElementWeight(elements[selectionIndex]);
        while (selectionWeightIndex >= itemWeight)
        {
            selectionWeightIndex -= itemWeight;
            selectionIndex++;

            if (selectionIndex >= elementCount)
            {
                throw new ArgumentException("Weighted selection exceeded indexable range. Is your weightSum correct?",
                    $"weightSum:{weightSum}");
            }

            itemWeight = getElementWeight(elements[selectionIndex]);
        }

        return selectionIndex;
    }

    /// <param name="elements"></param>
    /// <param name="weightSum"> 传入0时，默认累加</param>
    /// <param name="getElementWeight"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static int WeightedSelectionIndex<T>(this IList<T> elements, float weightSum, Func<T, float> getElementWeight)
    {
        if (weightSum <= 0)
        {
            weightSum = 0;
            for (int i = 0; i < elements.Count; i++)
            {
                weightSum += getElementWeight(elements[i]);
            }
        }

        int selectionIndex = 0;

        double selectedWeight = s_SharedRandom.NextDouble() * weightSum;
        int elementCount = elements.Count;

        if (elementCount == 0)
        {
            throw new InvalidOperationException("Cannot perform selection on an empty collection");
        }

        double itemWeight = getElementWeight(elements[selectionIndex]);
        while (selectedWeight >= itemWeight)
        {
            selectedWeight -= itemWeight;
            selectionIndex++;

            if (selectionIndex >= elementCount)
            {
                throw new ArgumentException("Weighted selection exceeded indexable range. Is your weightSum correct?",
                    "weightSum");
            }

            itemWeight = getElementWeight(elements[selectionIndex]);
        }

        return selectionIndex;
    }

    public static void Shuffle<T>(this IList<T> list)
    {
        Shuffle(list, 0, list.Count);
    }

    public static void Shuffle<T>(this IList<T> list, int start, int end)
    {
        for (var n = end - 1; n > start; --n)
        {
            var k = UnityEngine.Random.Range(start, end);
            if (k == n && end > 2)
            {
                n++;
                continue;
            }
            var value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public static T TRandom<T>(this IList<T> list)
    {
        if (list.Count == 0)
        {
            throw new IndexOutOfRangeException("List needs at least one entry to call Random()");
        }

        if (list.Count == 1)
        {
            return list[0];
        }

        return list[UnityEngine.Random.Range(0, list.Count)];
    }

    public static T RandomPickOne<T>(this IList<T> list, int startIdx = 0)
    {
        if (list == null || list.Count <= 0)
        {
            return default(T);
        }

        if (list.Count == 1)
        {
            return list[0];
        }
        if (startIdx >= list.Count) startIdx = list.Count - 1;
        return list[UnityEngine.Random.Range(startIdx, list.Count)];
    }
}
