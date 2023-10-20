using System.Collections.Generic;
using UnityEngine;

namespace ClubTest
{
    public class UnitDelector
    {
        private readonly Collider2D[] _colliders;

        public UnitDelector()
        {
            _colliders = new Collider2D[CONSTANTS.UNIT_DETECTOR_MAX_UNITS];
        }

        public IEnumerable<T> GetComponentsAround<T>(Vector2 center, float range, LayerMask targetLayers) where T : Unit
        {
            var collidersCount = Physics2D.OverlapCircleNonAlloc(center, range, _colliders, targetLayers);

            for (int i = 0; i < collidersCount; i++)
            {
                if (_colliders[i].TryGetComponent(out T component))
                    yield return component;
            }
        }

        public T GetComponentAround<T>(Vector2 center, float range, LayerMask targetLayers) where T : Unit
        {
            var collidersCount = Physics2D.OverlapCircleNonAlloc(center, range, _colliders, targetLayers);

            for (int i = 0; i < collidersCount; i++)
            {
                if (_colliders[i].TryGetComponent(out T component))
                    return component;
            }

            return default;
        }
    }
}