using System.Collections.Generic;
using UnityEngine;

namespace ClubTest
{
    public class EnemyView : MonoBehaviour
    {
        private List<SpriteRenderer> _bodyParts;

        private void Awake()
        {
            _bodyParts = new List<SpriteRenderer>(GetComponentsInChildren<SpriteRenderer>());
        }

        public void ChangeColor(Color color)
        {
            foreach (var part in _bodyParts)
            {
                part.color = color;
            }
        }
    }
}