using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using UnityEngine;

namespace ClubTest
{
    [CreateAssetMenu(menuName = "_Project/LevelData")]
    public class LevelData : ScriptableObject
    {
        [field: SerializeField] public SerializedDictionary<EnemyType, int> EnemyTypeAmountToSpawn { get; private set; }
        [field: SerializeField] public SerializedDictionary<EnemyType, List<DropedItem>> EnemyDropList { get; private set; }

    }
}