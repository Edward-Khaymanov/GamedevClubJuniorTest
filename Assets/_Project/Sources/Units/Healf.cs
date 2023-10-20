using System;
using UnityEngine;

namespace ClubTest
{
    [Serializable]
    public class Healf
    {
        public event Action<float, float> HealfChanged;

        public Healf(float max)
        {
            Max = max;
            Current = max;
        }

        [field: SerializeField, Min(0)] public float Max { get; private set; }
        [field: SerializeField, Min(0)] public float Current { get; private set; }


        public void Add(float value)
        {
            if (value <= 0)
                return;

            Current = Mathf.Clamp(Current + value, 0, Max);
            HealfChanged?.Invoke(Current, Max);
        }

        public void Remove(float value)
        {
            if (value <= 0)
                return;

            Current = Mathf.Clamp(Current - value, 0, Max);
            HealfChanged?.Invoke(Current, Max);
        }
    }
}