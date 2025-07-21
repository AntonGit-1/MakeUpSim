using UnityEngine;
namespace View {
    public interface IButtonTargetedEventsHolder<T> {
        ButtonTargetedEvents<T> Events { get; }
    }
}

