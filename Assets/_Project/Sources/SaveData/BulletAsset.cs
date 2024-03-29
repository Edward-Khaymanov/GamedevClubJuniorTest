using UnityEngine;

namespace ClubTest
{
    [CreateAssetMenu(menuName = "_Project/Items/Bullet")]
    public class BulletAsset : ItemAsset
    {
        [field: SerializeField] public BulletCaliber Caliber { get; set; }
    }
}