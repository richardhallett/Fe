using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fe
{
    static class Utils
    {
        public static void RadixSort64<T>(ulong[] keys, T[] values, int length)
        {
            // Base 10 Radix Sort
            const int radix = 10;

            // Find the largest number in all the keys
            ulong maxValue = 0;
            for (int i = 0; i < keys.Length; i++)
            {
                var num = keys[i];
                if (num > maxValue)
                    maxValue = num;
            }

            // Buckets to hold the indexes back to the keys array
            int[][] buckets = new int[radix][];
            for (int i = 0; i < radix; i++)
            {
                buckets[i] = new int[length];
            }

            int[] counts = new int[radix]; // The number of key indices stored in each bucket

            // Temporary arrays used to copy after we've sorted
            ulong[] tmpKeys = new ulong[length];
            T[] tmpValues = new T[length];

            int exp = 1;
            double place = 1;
            // Repeat until we hit max places of the max value.
            while (place <= maxValue)
            {
                // For every key, put the index next to it's LSD
                var prevKey = keys[0];
                bool sorted = true;

                for (int i = 0; i < length; i++)
                {
                    var key = keys[i];
                    sorted &= (prevKey <= key);
                    int digit = (int)(keys[i] / place % radix);

                    // Store the key index
                    buckets[digit][counts[digit]] = i;

                    // Increase bucket count
                    counts[digit]++;

                    prevKey = key;
                }

                if (sorted)
                {
                    break;
                }

                // For each bucket take the keys and fill keys/values
                int keyIndex = 0;
                for (int i = 0; i < buckets.Length; i++)
                {
                    var bucket = buckets[i];
                    for (int j = 0; j < counts[i]; j++)
                    {
                        int bucketKey = bucket[j];
                        tmpKeys[keyIndex] = keys[bucketKey];
                        tmpValues[keyIndex] = values[bucketKey];
                        keyIndex++;
                    }

                    // Reset counter for the bucket
                    counts[i] = 0;
                }

                tmpKeys.CopyTo(keys, 0);
                tmpValues.CopyTo(values, 0);

                // Calculate the places
                place = Math.Pow(radix, exp);
                exp += 1;
            }
        }
    }
}
