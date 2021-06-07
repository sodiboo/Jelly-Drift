using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Chaos
{
#if !MOBILE
    [HideInCheatGUI]
#endif
    [Effect("chaos.view.portrait", "Portrait Mode")]
    [Description("Turns your screen to portrait mode")]
    class VerticalMobile : ChaosEffect
    {
        private void OnEnable()
        {
            Screen.autorotateToPortrait = true;
            Screen.autorotateToPortraitUpsideDown = true;
            Screen.autorotateToLandscapeLeft = false;
            Screen.autorotateToLandscapeRight = false;
            StartCoroutine(Wait3Seconds());
        }

        private void OnDisable()
        {
            Screen.autorotateToLandscapeLeft = true;
            Screen.autorotateToLandscapeRight = true;
            Screen.autorotateToPortrait = false;
            Screen.autorotateToPortraitUpsideDown = false;
            StartCoroutine(Wait3Seconds());
        }

        InputAction pause = InputManager.Instance.inputs.FindActionMap("Global").FindAction("Pause");
        IEnumerator Wait3Seconds()
        {
            MobileControls.Instance.pause.SetActive(false);
            Time.timeScale = 0f;
            yield return new WaitForSecondsRealtime(3f);
            Time.timeScale = 1f;
            pause.Enable();
            MobileControls.Instance.pause.SetActive(true);
        }


#if MOBILE
        public static bool Valid() => true;
#else
        public static bool Valid() => false;
#endif
    }
}
