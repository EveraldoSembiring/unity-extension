using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityExtension
{
    public static class RandomWithWeight
    {
        public static T Pick<T>(this List<WeightRandom<T>> randomCandidates)
        {
            if (randomCandidates == null && randomCandidates.Count == 0)
            {
                return default(T);
            }

            if (randomCandidates.Count == 1)
            {
                if (randomCandidates[0].Weight > 0)
                {
                    return randomCandidates[0].Object;
                }
                else
                {
                    return default(T);
                }
            }

            float totalWeight = 0;
            foreach (WeightRandom<T> candidate in randomCandidates)
            {
                if (candidate.Weight > 0)
                {
                    float rangeMin = totalWeight;
                    float rangeMax = totalWeight + candidate.Weight;
                    totalWeight = rangeMax;
                    candidate.Range = new Vector2(rangeMin, rangeMax);
                }
            }

            if (totalWeight == 0)
            {
                return default(T);
            }
            
            float random = UnityEngine.Random.Range(0, totalWeight);
            T result = default(T);
            foreach (var candidate in randomCandidates)
            {
                if (candidate.Range != null && candidate.Range.Value.x < random && random <= candidate.Range.Value.y)
                {
                    result = candidate.Object;
                    break;
                }
            }

            return result;
        }
    }

    public class WeightRandom<T>
    {
        public readonly float Weight;
        public readonly T Object;
        internal Vector2? Range;

        public WeightRandom(T @object, float weight)
        {
            Object = @object;
            Weight = weight;
        }
    }
}
