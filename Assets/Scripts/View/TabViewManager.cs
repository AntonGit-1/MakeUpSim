using UnityEngine;
using System;

namespace View {
    public class TabViewManager : MonoBehaviour {
        int currentIndex;
        [SerializeField]
        TabViewButton[] tabViews;

        ButtonTargetedEventsArray<TabViewButton> tab_events = new ButtonTargetedEventsArray<TabViewButton>();
        public ButtonTargetedEventsArray<TabViewButton> Tab_events { get => tab_events; }

        void Awake() {
            for (int i  = 0; i < tabViews.Length; i++) {
                Tab_events.AddItem(tabViews[i]);
            }
        }

        public void TrySelectTab(int index) {
            if (currentIndex == Mathf.Clamp(currentIndex, 0, tabViews.Length - 1)) {
      
            }
            tabViews[currentIndex].SetDeselected();
            tabViews[index].SetSelected();
            currentIndex = index;
        }

        void OnDestroy() {
            Tab_events.RemoveAllItems();
        }
    }
}

