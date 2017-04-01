using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GameBase
{
    public class GameRandom
    {
        public static List<int> GetIndependentRandoms(int from, int to, int num)
        {
            List<int> randoms = new List<int>();
            List<int> range = new List<int>();

            for (int i = from; i < to; ++i)
            {
                range.Add(i);
            }

            int randomCount = num;
            if (randomCount > range.Count)
            {
                randomCount = range.Count;
            }

            for (int i = 0; i < randomCount; ++i)
            {
                int randomIdx = Random.Range(0, range.Count);
                randoms.Add(range[randomIdx]);
                range.RemoveAt(randomIdx);
            }

            return randoms;
        }

        public static List<int> GetTotalRange(int total, int min, int count)
        {
            List<int> randomList = new List<int>();

            int randomCount = count;
            int logicRotal = total;
            while (randomCount > 1)
            {
                int max = logicRotal - (min * randomCount) + min;
                int randomRange1 = min;
                if (max > min)
                {
                    randomRange1 = Random.Range(min, max + 1);
                }
                randomList.Add(randomRange1);

                logicRotal -= randomRange1;
                --randomCount;
            }

            randomList.Add(logicRotal);
            return randomList;
        }

        public static int GetRandomLevel(params int[] levelRates)
        {
            int totalRate = 0;
            foreach (int levelRate in levelRates)
            {
                totalRate += levelRate;
            }

            int randomValue = Random.Range(0, totalRate);
            int rateStep = 0;

            for(int i = 0; i< levelRates.Length; ++i)
            {
                rateStep += levelRates[i];
                if (rateStep >= randomValue)
                {
                    return i;
                }
            }

            return levelRates.Length - 1;
        }
    }
}
