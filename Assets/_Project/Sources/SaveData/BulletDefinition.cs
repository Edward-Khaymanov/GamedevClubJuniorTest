using UnityEngine;

namespace ClubTest
{
    [CreateAssetMenu(menuName = "_Project/Items/Bullet")]
    public class BulletDefinition : ItemDefinition
    {
        [field: SerializeField] public BulletCaliber Caliber { get; set; }
    }
}