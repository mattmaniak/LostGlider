using System;
using UnityEngine;

namespace DebugUtils
{
    static class GlobalEnabler
    {
        public static readonly bool activated = true;

        public static void LogError(string message)
        {
            Debug.Log(message);
        }

        public static void LogException(Exception exception,
                                        UnityEngine.Object context = null)
        {
            if (activated)
            {
                if (context != null)
                {
                    Debug.LogException(exception);
                }
                else
                {
                    Debug.LogException(exception, context);
                }
            }
        }
    }
}
