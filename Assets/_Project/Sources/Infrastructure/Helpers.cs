using UnityEngine;

namespace ClubTest
{
    public static class Helpers
    {
        public static Vector2 GetRandomPointBeetween(Vector2 min, Vector2 max)
        {
            return new Vector2(Random.Range(min.x, max.x), Random.Range(min.y, max.y));
        }
    }
}