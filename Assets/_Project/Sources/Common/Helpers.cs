using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ClubTest
{
    public static class Helpers
    {
        public static void DestroyChilds(this Transform target)
        {
            foreach (Transform child in target)
            {
                GameObject.Destroy(child.gameObject);
            }
        }

        public static Vector2 GetRandomPointBeetween(Vector2 min, Vector2 max)
        {
            return new Vector2(Random.Range(min.x, max.x), Random.Range(min.y, max.y));
        }

        public static T RandomSingle<T>(this IEnumerable<T> source)
        {
            if (source == null)
                return default;

            var randomIndex = Random.Range(0, source.Count());
            return source.ElementAtOrDefault(randomIndex);
        }

        public static T GetClosestToPoint<T>(this IList<T> source, Vector3 point) where T : Component
        {
            var closestDistance = float.MaxValue;
            var closestObject = default(T);

            for (int i = 0; i < source.Count; i++)
            {
                var distance = Vector3.Distance(source[i].transform.position, point);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestObject = source[i];
                }
            }

            return closestObject;
        }
    }
}