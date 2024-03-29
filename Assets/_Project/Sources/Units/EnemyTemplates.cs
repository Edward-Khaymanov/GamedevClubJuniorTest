using System.Collections.Generic;
using UnityEngine;

namespace ClubTest
{
    [CreateAssetMenu(menuName = "_Project/EnemyTemplates")]
    public class EnemyTemplates : ScriptableObject
    {
        [field: SerializeField] public Dictionary<EnemyType, Enemy> Templates { get; private set; }
    }
}