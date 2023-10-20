using System;
using UnityEngine.EventSystems;

namespace ClubTest
{
    public class SelectableOption : BaseContextMenuOption, IPointerClickHandler
    {
        public event Action Selected;

        public void OnPointerClick(PointerEventData eventData)
        {
            Selected?.Invoke();
        }
    }
}