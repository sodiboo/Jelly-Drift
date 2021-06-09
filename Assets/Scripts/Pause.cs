using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000030 RID: 48
public class Pause : MonoBehaviour
{
    // Token: 0x17000016 RID: 22
    // (get) Token: 0x060000F7 RID: 247 RVA: 0x000064E0 File Offset: 0x000046E0
    // (set) Token: 0x060000F8 RID: 248 RVA: 0x000064E8 File Offset: 0x000046E8
    public bool paused { get; set; }

    // Token: 0x060000F9 RID: 249 RVA: 0x000064F1 File Offset: 0x000046F1
    private void Awake() => Pause.Instance = this;

    private void OnEnable() => InputManager.Instance.pause += PauseGame;

    private void OnDisable() => InputManager.Instance.pause -= PauseGame;

    private InputManager.Layout unpausedLayout;

    public void PauseGame()
    {
        if (paused || !GameController.Instance.playing)
        {
            return;
        }
        Time.timeScale = 0f;
        paused = true;
        unpausedLayout = InputManager.Instance.layout;
        InputManager.Instance.layout = InputManager.Layout.Menu;
        Invoke("EnablePauseMenu", 0f); // next frame because otherwise input system might fire the "canceled" callback of the menu since that might be pressed this frame, depending on the order it was processing shit in
#if MOBILE
		MobileControls.Instance.pause.SetActive(false);
#endif
    }

    private void EnablePauseMenu() => pauseMenu.SetActive(true);

    // Token: 0x060000FB RID: 251 RVA: 0x00006521 File Offset: 0x00004721
    public void ResumeGame()
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
        paused = false;
        MonoBehaviour.print("resuiming game");
        InputManager.Instance.layout = unpausedLayout;
#if MOBILE
		MobileControls.Instance.pause.SetActive(true);
#endif
    }

    // Token: 0x060000FC RID: 252 RVA: 0x0000654A File Offset: 0x0000474A
    public void Recover()
    {
        Time.timeScale = 1f;
        GameController.Instance.Recover();
        ResumeGame();
    }

    // Token: 0x060000FD RID: 253 RVA: 0x00006566 File Offset: 0x00004766
    public void RestartGame()
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
        paused = false;
        GameController.Instance.RestartGame();
    }

    // Token: 0x060000FF RID: 255 RVA: 0x0000658F File Offset: 0x0000478F
    public void TogglePause()
    {
        if (!GameController.Instance.playing)
        {
            return;
        }
        if (!paused)
        {
            PauseGame();
            return;
        }
        ResumeGame();
    }

    // Token: 0x06000100 RID: 256 RVA: 0x000065B3 File Offset: 0x000047B3
    public void Quit()
    {
        ChaosController.Instance.StopChaos();
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
        paused = false;
    }

    // Token: 0x04000115 RID: 277
    public static Pause Instance;

    // Token: 0x04000116 RID: 278
    public GameObject pauseMenu;
}
