using System;
using System.Collections.Generic;
using System.Linq;

namespace Manager.WebApp.Helpers
{
    public static class ArrayHelper
    {
        /// <summary>
        /// Splits an array into several smaller arrays.
        /// </summary>
        /// <typeparam name="T">The type of the array.</typeparam>
        /// <param name="array">The array to split.</param>
        /// <param name="size">The size of the smaller arrays.</param>
        /// <returns>An array containing smaller arrays.</returns>
        ///  //How to use
        ///var array = new byte[] {10, 20, 30, 40, 50};
        ///var splitArray = array.Split(2);
        public static IEnumerable<IEnumerable<T>> SplitArrayCustom<T>(this T[] array, int size)
        {
            //for (var i = 0; i < (float)array.Length / size; i++)
            //{
            //    yield return array.Skip(i * size).Take(size);
            //}

            var arrLength = array.Length;
            var arrLimit = (int)Math.Ceiling((double)arrLength / size);
            var realLimitToTake = (int)Math.Round((double)arrLength / size);            
            for (var i = 0; i < size; i++)
            {
                var currentArr = array.Skip(i * realLimitToTake).Take(realLimitToTake);
                if(i == (size - 1))
                {
                    if (arrLimit > realLimitToTake)
                    {
                        var takeNumber = realLimitToTake * size;
                        var remainsArr = array.Skip(takeNumber).Take(array.Length - takeNumber);
                        if(remainsArr != null && remainsArr.Count() > 0)
                        {
                            currentArr = currentArr.Concat(remainsArr);
                        }
                    }
                }

                yield return currentArr;
            }
        }

    }
}
