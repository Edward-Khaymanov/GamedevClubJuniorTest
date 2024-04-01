using UnityEngine;

namespace ClubTest
{
    [CreateAssetMenu(menuName = "_Project/Items/Weapon")]
    public class WeaponDefinition : ItemDefinition
    {
        [field: SerializeField] public WeaponStats Stats { get; set; }
        [field: SerializeField] public WeaponView View { get; set; }
    }
}