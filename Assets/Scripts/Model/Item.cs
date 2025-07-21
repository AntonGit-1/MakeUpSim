using UnityEngine;

namespace Model {
    public enum MakeupType { Blush, Lipstick, Shadow, Cream }
    public readonly struct Item {
        public readonly MakeupType itemType;
        public readonly int itemIndex;

        public Item(MakeupType itemType, int itemIndex) {
            this.itemType = itemType;
            this.itemIndex = itemIndex;
        }
    }
}

