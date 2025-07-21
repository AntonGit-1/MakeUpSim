using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace View {
    public class ButtonTargetedEventsArray<T> {
        List<IButtonTargetedEventsHolder<T>> items = new List<IButtonTargetedEventsHolder<T>>();

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

        public int Count => items.Count;
        public bool TryGetItemAt(int index, out T item) {
            if (index >= 0 && index < items.Count) {
                item = (T)items[index];
                return true;
            }
            item = default;
            return false;
        }

        public void AddItem(IButtonTargetedEventsHolder<T> item) {
            if (item != null && !items.Contains(item)) {
                items.Add(item);
                SubItem(item);
            }
        }
        public void RemoveItem(IButtonTargetedEventsHolder<T> item) {
            if (item != null && items.Contains(item)) {
                items.Remove(item);
                UnsubItem(item);
            }
        }
        public void RemoveAllItems() {
            foreach (IButtonTargetedEventsHolder<T> item in items) {
                UnsubItem(item);
            }
            items.Clear();
        }

        void SubItem(IButtonTargetedEventsHolder<T> item) {
            item.Events.OnItemClick += AnnounceOnClick;
            item.Events.OnItemDoubleClick += AnnounceOnDoubleClick;
            item.Events.OnItemEnter += AnnounceOnEnter;
            item.Events.OnItemExit += AnnounceOnExit;
            item.Events.OnItemPressDown += AnnounceOnPressDown;
            item.Events.OnItemPressUp += AnnounceOnPressUp;
            item.Events.OnItemDragBegin += AnnounceOnDragBegin;
            item.Events.OnItemDragEnd += AnnounceOnDragEnd;
            item.Events.OnItemHoldBegin += AnnounceOnHoldBegin;
            item.Events.OnItemHoldEnd += AnnounceOnHoldEnd;
        }
        void UnsubItem(IButtonTargetedEventsHolder<T> item) {
            item.Events.OnItemClick -= AnnounceOnClick;
            item.Events.OnItemDoubleClick -= AnnounceOnDoubleClick;
            item.Events.OnItemEnter -= AnnounceOnEnter;
            item.Events.OnItemExit -= AnnounceOnExit;
            item.Events.OnItemPressDown -= AnnounceOnPressDown;
            item.Events.OnItemPressUp -= AnnounceOnPressUp;
            item.Events.OnItemDragBegin -= AnnounceOnDragBegin;
            item.Events.OnItemDragEnd -= AnnounceOnDragEnd;
            item.Events.OnItemHoldBegin -= AnnounceOnHoldBegin;
            item.Events.OnItemHoldEnd -= AnnounceOnHoldEnd;
        }

        void AnnounceOnClick(T item) => OnItemClick?.Invoke(item);
        void AnnounceOnDoubleClick(T item) => OnItemDoubleClick?.Invoke(item);
        void AnnounceOnEnter(T item) => OnItemEnter?.Invoke(item);
        void AnnounceOnExit(T item) => OnItemExit?.Invoke(item);
        void AnnounceOnPressDown(T item) => OnItemPressDown?.Invoke(item);
        void AnnounceOnPressUp(T item) => OnItemPressUp?.Invoke(item);
        void AnnounceOnDragBegin(T item) => OnItemDragBegin?.Invoke(item);
        void AnnounceOnDragEnd(T item) => OnItemDragEnd?.Invoke(item);
        void AnnounceOnHoldBegin(T item) => OnItemHoldBegin?.Invoke(item);
        void AnnounceOnHoldEnd(T item) => OnItemHoldEnd?.Invoke(item);
    }
}
