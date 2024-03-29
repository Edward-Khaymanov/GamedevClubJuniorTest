using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace ClubTest
{
    [CreateAssetMenu(menuName = "_Project/EnemyTemplates")]
    public class EnemyTemplates : ScriptableObject
    {
        [field: SerializeField] public SerializedDictionary<EnemyType, Enemy> Templates { get; private set; }
    }
}