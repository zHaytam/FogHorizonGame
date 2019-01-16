using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Utils
{
    public static class Lerp
    {

        public static IEnumerator OverTime(float startingValue, float endingValue, float time, Action<float> action)
        {
            float elapsedTime = 0;

            while (elapsedTime < time)
            {
                float val = Mathf.Lerp(startingValue, endingValue, elapsedTime / time);
                action.Invoke(val);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            action.Invoke(endingValue);
        }

    }
}
