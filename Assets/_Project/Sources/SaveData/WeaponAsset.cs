using UnityEngine;

namespace ClubTest
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Items/Weapon")]
    public class WeaponAsset : ItemAsset
    {
        [field: SerializeField] public WeaponStats Stats { get; set; }
        [field: SerializeField] public WeaponView View { get; set; }
    }
}