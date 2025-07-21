using UnityEngine;
using DG.Tweening;
using Model;
using Zenject;
using Presenter;

namespace View {
    public class BookViewWorker : MonoBehaviour {
        [SerializeField]
        float animationTime = 1f;
        [SerializeField]
        float overlapAnimationTime = .009f;

        [SerializeField]
        PageContent[] pages;

        PageContent currentPage;

        ButtonTargetedEventsArray<MakeupViewButton> blush_events = new ButtonTargetedEventsArray<MakeupViewButton>();
        ButtonTargetedEventsArray<MakeupViewButton> lipstick_events = new ButtonTargetedEventsArray<MakeupViewButton>();
        ButtonTargetedEventsArray<MakeupViewButton> shadows_events = new ButtonTargetedEventsArray<MakeupViewButton>();

        public ButtonTargetedEventsArray<MakeupViewButton> Blush_events { get => blush_events; }
        public ButtonTargetedEventsArray<MakeupViewButton> Lipstick_events { get => lipstick_events; }
        public ButtonTargetedEventsArray<MakeupViewButton> Shadows_events { get => shadows_events; }

        [Inject]
        void Construct(DelayedStartAnnouncer startAnnouncer) {
            startAnnouncer.OnDelayedStart += DelayedStart;
        }

        void DelayedStart(DelayedStartAnnouncer startAnnouncer) {
            startAnnouncer.OnDelayedStart -= DelayedStart;
            for (int i = 1; i < pages.Length; i++) {
                PageContent page = pages[i];
                foreach (GameObject obj in page.AllItems) {
                    obj.transform.localScale = Vector3.one * .001f;
                }
            }

            currentPage = pages[0];
            for (int i = 0; i < pages.Length; i++) {
                PageContent page = pages[i];
                ButtonTargetedEventsArray<MakeupViewButton> events = GetEventsFromType(page.ItemType);
                for (int j = 0; j < page.MakeupButtons.Length; j++) {
                    page.MakeupButtons[j].SetIndex(j);
                    events.AddItem(page.MakeupButtons[j]);
                }
            }
        }

        public ButtonTargetedEventsArray<MakeupViewButton> GetEventsFromType(MakeupType type) {
            return type switch {
                MakeupType.Blush => blush_events,
                MakeupType.Lipstick => lipstick_events,
                MakeupType.Shadow => shadows_events,
                _ => null,
            };
        }

        public Sequence ShowPage(int index) {
            Sequence sequence = DOTween.Sequence();

            currentPage = pages[index];

            sequence.AppendCallback(() => currentPage.Holder.SetActive(true));

            foreach (GameObject obj in currentPage.AllItems) {
                sequence.AppendInterval(overlapAnimationTime);
                sequence.JoinCallback(() => ShowObjectAnimation(obj).Play());
            }

            return sequence;
        }
        public Sequence HideCurrentPage() {
            Sequence sequence = DOTween.Sequence();

            foreach (GameObject obj in currentPage.AllItems) {
                sequence.AppendInterval(overlapAnimationTime);
                sequence.JoinCallback(() => HideObjectAnimation(obj).Play());
            }

            sequence.AppendCallback(() => currentPage.Holder.SetActive(false));

            return sequence;
        }

        Tween HideObjectAnimation(GameObject obj) {
            return obj.transform.DOScale(0.01f, animationTime);
        }
        Tween ShowObjectAnimation(GameObject obj) {
            return obj.transform.DOScale(1f, animationTime);
        }

        void OnDestroy() {
            blush_events.RemoveAllItems();
            lipstick_events.RemoveAllItems();
            shadows_events.RemoveAllItems();
        }

        [System.Serializable]
        public class PageContent {
            [SerializeField]
            MakeupType itemType;
            [SerializeField]
            GameObject holder;
            [SerializeField]
            GameObject[] allItems;
            [SerializeField]
            MakeupViewButton[] makeupButtons;

            public MakeupType ItemType { get => itemType; }
            public GameObject Holder { get => holder; }
            public GameObject[] AllItems { get => allItems; }
            public MakeupViewButton[] MakeupButtons { get => makeupButtons; }
        }
    }
}

