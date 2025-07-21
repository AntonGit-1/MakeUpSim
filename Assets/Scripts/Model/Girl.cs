using UnityEngine;
using System;

namespace Model {
    public class Girl {
        bool hasAcne = true;
        // Индекс -1 - майкапа нет
        int currentLipstickIndex = -1;
        int currentBlushIndex = -1;
        int currentShadowsIndex = -1;

        public bool HasAcne { 
            get => hasAcne;
            private set {
                hasAcne = value;
                OnAcneChanged?.Invoke(hasAcne);
            }
        }
        public int CurrentLipstickIndex { 
            get => currentLipstickIndex;
            private set {
                currentLipstickIndex = value;
                OnLipstickChanged?.Invoke(currentLipstickIndex);
            }
        }
        public int CurrentBlushIndex { 
            get => currentBlushIndex;
            private set {
                currentBlushIndex = value;
                OnBlushChanged?.Invoke(currentBlushIndex);
            }
        }
        public int CurrentShadowsIndex { 
            get => currentShadowsIndex;
            private set {
                currentShadowsIndex = value;
                OnShadowsChanged?.Invoke(currentShadowsIndex);
            }
        }

        public event Action<int> OnLipstickChanged;
        public event Action<int> OnShadowsChanged;
        public event Action<int> OnBlushChanged;
        public event Action<bool> OnAcneChanged;

        public void ApplyFromHand(Hand hand) {
            if (hand.CurrentItem == null) {
                return;
            }

            Item item = (Item)hand.CurrentItem;
            switch (item.itemType) {
                case MakeupType.Blush:
                    CurrentBlushIndex = item.itemIndex;
                    break;
                case MakeupType.Cream:
                    HasAcne = false;
                    break;
                case MakeupType.Lipstick:
                    CurrentLipstickIndex = item.itemIndex;
                    break;
                case MakeupType.Shadow:
                    CurrentShadowsIndex = item.itemIndex;
                    break;
            }
            hand.Release();
        }

        public void Reset() {
            HasAcne = true;
            CurrentLipstickIndex = -1;
            CurrentBlushIndex = -1;
            CurrentShadowsIndex = -1;
        }
    }
}

