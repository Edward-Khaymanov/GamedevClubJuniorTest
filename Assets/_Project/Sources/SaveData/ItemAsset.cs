using UnityEngine;

namespace ClubTest
{
    [CreateAssetMenu(fileName = "Item", menuName = "Items/Item")]
    public abstract class ItemAsset : ScriptableObject
    {
        [field: SerializeField] public int Id { get; set; }
        [field: SerializeField] public string Name { get; set; }
        [field: SerializeField] public Sprite Icon { get; set; }
        [field: SerializeField, Min(1)] public int MaxInStack { get; set; }
    }
}