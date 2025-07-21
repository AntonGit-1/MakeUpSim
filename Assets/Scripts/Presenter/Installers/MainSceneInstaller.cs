using System.Collections;
using UnityEngine;
using Zenject;
using Data;
using View;
using Model;

namespace Presenter {
    public class MainSceneInstaller : MonoInstaller {
        [SerializeField]
        AddressableDispatchService dispatchService;
        [SerializeField]
        BookViewWorker bookViewWorker;
        [SerializeField]
        TabViewManager tabViewWorker;
        [SerializeField]
        HandView handViewWorker;
        [SerializeField]
        InputManager inputManager;
        [SerializeField]
        HouseholdItems householdItems;
        [SerializeField]
        AnimationPoints animationPoints;
        [SerializeField]
        GirlView girlView;

        DelayedStartAnnouncer startAnnouncer;

        public void Set(DelayedStartAnnouncer startAnnouncer) {
            this.startAnnouncer = startAnnouncer;
        }

        public override void InstallBindings() {
            Girl girlModel = new Girl();
            Hand hand = new Hand();

            //Data
            Container.Bind<AddressableDispatchService>().FromInstance(dispatchService).AsSingle();

            //Model
            Container.Bind<Girl>().FromInstance(girlModel).AsSingle();
            Container.Bind<Hand>().FromInstance(hand).AsSingle();

            //Presenter
            Container.Bind<DelayedStartAnnouncer>().FromInstance(startAnnouncer).AsSingle();
            Container.Bind<InputManager>().FromInstance(inputManager).AsSingle();

            //View
            Container.Bind<BookViewWorker>().FromInstance(bookViewWorker).AsSingle();
            Container.Bind<TabViewManager>().FromInstance(tabViewWorker).AsSingle();
            Container.Bind<HandView>().FromInstance(handViewWorker).AsSingle();
            Container.Bind<HouseholdItems>().FromInstance(householdItems).AsSingle();
            Container.Bind<AnimationPoints>().FromInstance(animationPoints).AsSingle();
            Container.Bind<GirlView>().FromInstance(girlView).AsSingle();
        }
    }
}