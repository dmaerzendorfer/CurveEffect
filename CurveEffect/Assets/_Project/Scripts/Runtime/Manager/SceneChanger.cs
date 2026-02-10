using System;
using System.Collections;
using _Project.Scripts.Runtime.Utility;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using PrimeTween;

namespace _Project.Scripts.Runtime.Manager
{
    public class SceneChanger : SingletonMonoBehaviour<SceneChanger>
    {
        public Image fadeImage;
        public TweenSettings<float> fadeTweenSettings;
        public UnityEvent onTransitionDone = new UnityEvent();
        public UnityEvent onAsyncLoadDone = new UnityEvent();


        public bool IsTransitioning { get; set; } = false;


        public override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
        }

        public void ChangeToScene(int sceneId, Action onCompleteCallback = null)
        {
            if (IsTransitioning) return;
            //for animation use postprocessing -> lensdistortion + color adjustment to fade + panini projection distance
            StartCoroutine(DoTransition(sceneId, onCompleteCallback));
        }

        [EditorAttributes.Button()]
        public void ChangeToNextScene()
        {
            var scene = (SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCountInBuildSettings;
            ChangeToScene(scene);
        }

        private IEnumerator DoTransition(int sceneId, Action onCompleteCallback)
        {
            IsTransitioning = true;
            fadeImage.raycastTarget = true;
            //fade to black
            yield return Tween.Alpha(fadeImage, fadeTweenSettings).ToYieldInstruction();

            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneId);
            // Wait until the asynchronous scene fully loads
            while (!asyncLoad.isDone)
            {
                yield return null;
            }

            onAsyncLoadDone.Invoke();

            //fade back in
            yield return Tween.Alpha(fadeImage, fadeTweenSettings.WithDirection(false, true)).ToYieldInstruction();

            IsTransitioning = false;
            fadeImage.raycastTarget = false;
            onCompleteCallback?.Invoke();
            onTransitionDone.Invoke();
        }
    }
}