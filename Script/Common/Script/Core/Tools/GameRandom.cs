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

    }
}
