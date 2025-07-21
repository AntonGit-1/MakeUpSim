using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace View {
    public class CoverScreen : MonoBehaviour {
        [SerializeField]
        float fadeTime = .3f;
        [SerializeField]
        Image img;

        public void TurnOff() {
            img.DOFade(0, fadeTime);
        }
        public void TurnOn() {
            gameObject.SetActive(true);
            img.color = Color.black;
        }
    }
}