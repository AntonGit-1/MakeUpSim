using UnityEngine;
using Data;
using Zenject;
using View;

namespace Presenter {
    public class Bootstrap : MonoBehaviour {
        [SerializeField]
        CoverScreen coverScreen;
        [SerializeField]
        AddressableDispatchService dispatchService;
        [SerializeField]
        MainSceneInstaller installer;
        [SerializeField]
        SceneContext sceneContext;

        async void Start() {
            coverScreen.TurnOn();
            await dispatchService.LoadAll();
            DelayedStartAnnouncer startAnnouncer = new DelayedStartAnnouncer();
            installer.Set(startAnnouncer);
            sceneContext.Run();
            startAnnouncer.Announce();
            coverScreen.TurnOff();
        }
    }

}
