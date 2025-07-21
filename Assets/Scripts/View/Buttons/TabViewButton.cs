using UnityEngine;
using System;

namespace View {
    public class TabViewButton : MonoBehaviour, IButtonTargetedEventsHolder<TabViewButton> {
        [SerializeField]
        int index;
        [SerializeField]
        UIButton button;
        [SerializeField]
        GameObject selected_holder, deselected_holder;

        public ButtonTargetedEvents<TabViewButton> Events { get; private set; }
        public int Index { get => index; }

        void Awake() {
            Events = new ButtonTargetedEvents<TabViewButton>(this, button);
        }

        public void SetSelected() {
            selected_holder.SetActive(true);
            deselected_holder.SetActive(false);
        }

        public void SetDeselected() {
            deselected_holder.SetActive(true);
            selected_holder.SetActive(false);
        }

        void OnDestroy() {
            Events.UnsubButton();
        }
    }
}

