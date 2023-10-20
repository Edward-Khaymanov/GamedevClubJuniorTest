using UnityEngine;

namespace ClubTest
{
    public class BulletAsset : ItemAsset
    {
        [field: SerializeField] public BulletCaliber Caliber { get; set; }
    }
}