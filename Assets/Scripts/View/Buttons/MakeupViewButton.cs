using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Model;
using Data;
using Zenject;

namespace View {
    public class MakeupViewButton : MonoBehaviour, IButtonTargetedEventsHolder<MakeupViewButton> {
        [SerializeField]
        MakeupType makeupType;
        [SerializeField]
        UIButton button;

        int index;
        [SerializeField]
        Image img;

        //INJECTED
        SpriteData data;

        public ButtonTargetedEvents<MakeupViewButton> Events { get; private set; }
        public int Index { get => index; }
        public MakeupType MakeupType { get => makeupType; }

        [Inject]
        void Construct(AddressableDispatchService addressableDispatch) {
            data = addressableDispatch.Sprites_data.GetFromType(makeupType, true);
            Events = new ButtonTargetedEvents<MakeupViewButton>(this, button);
        }

        public void SetIndex(int index) {
            this.index = index;
            img.sprite = data.Sprites[index];
        }
    }
}