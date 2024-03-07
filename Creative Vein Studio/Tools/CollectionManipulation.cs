using System;
using System.Collections.Generic;

namespace CVStudio
{
    public sealed partial class Utils
    {
        public static class CollectionManipulation<T> where T : class
        {
            public static void CycleAndGetListItem(bool isNext, int currentIndx, List<T> collection,
                Action<T, int> cback)
            {
                currentIndx = isNext ? ++currentIndx : --currentIndx;
                currentIndx = currentIndx > (collection.Count - 1) ? 0 :
                    currentIndx < 0 ? (collection.Count - 1) : currentIndx;

                cback?.Invoke(collection[currentIndx], currentIndx);
            }
        }
    }

    [Serializable]
    public enum CycleActions
    {
        Next,
        Prev
    }
}