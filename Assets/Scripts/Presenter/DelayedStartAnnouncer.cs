using System.Collections;
using UnityEngine;
using System;

namespace Presenter {
    public class DelayedStartAnnouncer {
        public event Action<DelayedStartAnnouncer> OnDelayedStart;

        public void Announce() {
            OnDelayedStart?.Invoke(this);
        }
    }
}