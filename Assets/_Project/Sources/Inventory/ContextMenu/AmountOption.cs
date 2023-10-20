using UnityEngine.EventSystems;

namespace ClubTest
{
    public class AmountOption : BaseContextMenuOption, IPointerClickHandler
    {
        //[SerializeField] private TMP_Text _amountSource;

        //public event Action<int> Selected;

        public void OnPointerClick(PointerEventData eventData)
        {
            //Selected?.Invoke();
        }
    }
}