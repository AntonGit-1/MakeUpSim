using UnityEngine.Events;
using System;

namespace View {
    public class ButtonTargetedEvents<T> {
        T owner;
        UIButton button;

        public event Action<T> OnItemClick;
        public event Action<T> OnItemDoubleClick;
        public event Action<T> OnItemEnter;
        public event Action<T> OnItemExit;
        public event Action<T> OnItemPressDown;
        public event Action<T> OnItemPressUp;
        public event Action<T> OnItemDragBegin;
        public event Action<T> OnItemDragEnd;
        public event Action<T> OnItemHoldBegin;
        public event Action<T> OnItemHoldEnd;

        public UIButton Button => button;

        public ButtonTargetedEvents(T owner, UIButton button) {
            this.owner = owner;
            this.button = button;
            SubButton();
        }

        void AnnounceOnClick() => OnItemClick?.Invoke(owner);
        void AnnounceOnDoubleClick() => OnItemDoubleClick?.Invoke(owner);
        void AnnounceOnEnter() => OnItemEnter?.Invoke(owner);
        void AnnounceOnExit() => OnItemExit?.Invoke(owner);
        void AnnounceOnPressDown() => OnItemPressDown?.Invoke(owner);
        void AnnounceOnPressUp() => OnItemPressUp?.Invoke(owner);
        void AnnounceOnDragBegin() => OnItemDragBegin?.Invoke(owner);
        void AnnounceOnDragEnd() => OnItemDragEnd?.Invoke(owner);
        void AnnounceOnHoldBegin() => OnItemHoldBegin?.Invoke(owner);
        void AnnounceOnHoldEnd() => OnItemHoldEnd?.Invoke(owner);

        void SubButton() {
            button.OnClick.AddListener(AnnounceOnClick);
            button.OnDoubleClick.AddListener(AnnounceOnDoubleClick);
            button.OnEnter.AddListener(AnnounceOnEnter);
            button.OnExit.AddListener(AnnounceOnExit);
            button.OnPressDown.AddListener(AnnounceOnPressDown);
            button.OnPressUp.AddListener(AnnounceOnPressUp);
            button.OnDragBegin.AddListener(AnnounceOnDragBegin);
            button.OnDragEnd.AddListener(AnnounceOnDragEnd);
            button.OnHoldBegin.AddListener(AnnounceOnHoldBegin);
            button.OnHoldEnd.AddListener(AnnounceOnHoldEnd);
        }
        public void UnsubButton() {
            button.OnClick.RemoveListener(AnnounceOnClick);
            button.OnDoubleClick.RemoveListener(AnnounceOnDoubleClick);
            button.OnEnter.RemoveListener(AnnounceOnEnter);
            button.OnExit.RemoveListener(AnnounceOnExit);
            button.OnPressDown.RemoveListener(AnnounceOnPressDown);
            button.OnPressUp.RemoveListener(AnnounceOnPressUp);
            button.OnDragBegin.RemoveListener(AnnounceOnDragBegin);
            button.OnDragEnd.RemoveListener(AnnounceOnDragEnd);
            button.OnHoldBegin.RemoveListener(AnnounceOnHoldBegin);
            button.OnHoldEnd.RemoveListener(AnnounceOnHoldEnd);
        }
    }
}

