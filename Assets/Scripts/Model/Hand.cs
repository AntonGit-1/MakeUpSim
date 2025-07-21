using UnityEngine;
using System;

namespace Model {
    public class Hand {
        Item? currentItem = null;
        public Item? CurrentItem { 
            get => currentItem;
            private set {
                if (value == null && currentItem != null) {
                    Item item = (Item)currentItem;
                    currentItem = value;
                    OnItemReleased?.Invoke(item);
                }
                currentItem = value;
                OnItemGrabed?.Invoke((Item)currentItem);
            }
        }
        public event Action<Item> OnItemGrabed;
        public event Action<Item> OnItemReleased;

        public void Grab(Item item) {
            CurrentItem = item;
        }

        public void Release() {
            CurrentItem = null;
        }
    }
}

