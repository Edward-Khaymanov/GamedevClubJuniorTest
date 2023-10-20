using UnityEngine;

namespace ClubTest
{
    public class PlayerView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _head;
        [SerializeField] private SpriteRenderer _body;
        [SerializeField] private SpriteRenderer _leftArm;
        [SerializeField] private SpriteRenderer _leftArm2;
        [SerializeField] private SpriteRenderer _rightArm;
        [SerializeField] private SpriteRenderer _rightArm2;
        [SerializeField] private SpriteRenderer _leftLeg;
        [SerializeField] private SpriteRenderer _rightLeg;

        [SerializeField] private Transform _weaponPivot;

        public WeaponView WeaponView { get; private set; }

        public void EquipWeapon(WeaponView weaponView)
        {
            weaponView.transform.SetParent(_weaponPivot, false);
            WeaponView = weaponView;
        }

        public void RemoveEquipedWeapon()
        {
            if (WeaponView == null)
                return;

            Destroy(WeaponView.gameObject);
            WeaponView = null;
        }
    }
}
