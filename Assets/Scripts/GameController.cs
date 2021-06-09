using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

// Token: 0x0200001A RID: 26
public class GameController : MonoBehaviour
{
    // Token: 0x1700000A RID: 10
    // (get) Token: 0x0600008B RID: 139 RVA: 0x00005210 File Offset: 0x00003410
    // (set) Token: 0x0600008C RID: 140 RVA: 0x00005218 File Offset: 0x00003418
    public Transform startPos { get; set; }

    // Token: 0x1700000B RID: 11
    // (get) Token: 0x0600008D RID: 141 RVA: 0x00005221 File Offset: 0x00003421
    // (set) Token: 0x0600008E RID: 142 RVA: 0x00005229 File Offset: 0x00003429
    public GameObject currentCar { get; set; }

    // Token: 0x1700000C RID: 12
    // (get) Token: 0x0600008F RID: 143 RVA: 0x00005232 File Offset: 0x00003432
    // (set) Token: 0x06000090 RID: 144 RVA: 0x0000523A File Offset: 0x0000343A
    public bool playing { get; set; }

    private InputAction action;

    // Token: 0x06000091 RID: 145 RVA: 0x00005244 File Offset: 0x00003444
    private void Awake()
    {
        GameController.Instance = this;
        Time.timeScale = 1f;
        startPos = checkPoints.GetChild(0);
        base.Invoke(nameof(StartRace), startTime);
        currentCar = UnityEngine.Object.Instantiate<GameObject>(PrefabManager.Instance.cars[GameState.Instance.car], startPos.position, startPos.rotation);
        currentCar.GetComponent<CarSkin>().SetSkin(GameState.Instance.skin);
        action = PrefabManager.Instance.inputs.FindActionMap("Menu").FindAction("Cancel");
        action.performed += Paused;
        InputManager.Instance.layout = InputManager.Layout.None;
    }

    private void OnDestroy() => action.performed -= Paused;

    // Token: 0x06000092 RID: 146 RVA: 0x000052D4 File Offset: 0x000034D4
    private void Start()
    {
        AssignCar();
        ChaosController.Instance.RegisterChaos();
    }

    public void AssignCar()
    {
        CameraController.Instance.AssignTarget(currentCar.transform);
        ShakeController.Instance.car = currentCar.GetComponent<Car>();
        ReplayController.Instance.car = currentCar.GetComponent<Car>();
        currentCar.AddComponent<CheckpointUser>();
    }

    // Token: 0x06000093 RID: 147 RVA: 0x0000532C File Offset: 0x0000352C
    private void StartRace()
    {
        playing = true;
        Timer.Instance.StartTimer();
        currentCar.AddComponent<InputListener>().car = currentCar.GetComponent<Car>();
        InputManager.Instance.layout = InputManager.Layout.Car;
        if (GameState.Instance.gamemode == Gamemode.Chaos) ChaosController.Instance.StartChaos();
    }

    private void Paused(InputAction.CallbackContext context)
    {
        var pressed = context.ReadValueAsButton();
        if (IsInvoking("ShowFinishScreen"))
        {
            if (pressed)
            {
                CancelInvoke("ShowFinishScreen");
                ShowFinishScreen();
            }
            return;
        }
        if (pressed && !Pause.Instance.paused) Pause.Instance.TogglePause();
    }

    // Token: 0x06000096 RID: 150 RVA: 0x000053A4 File Offset: 0x000035A4
    public void RestartGame() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    // Token: 0x06000097 RID: 151 RVA: 0x000053C4 File Offset: 0x000035C4
    public void Recover()
    {
        var component = currentCar.GetComponent<CheckpointUser>();
        if (!component)
        {
            return;
        }
        MonoBehaviour.print("cur check: " + component.GetCurrentCheckpoint(finalCheckpoint == 0));
        var child = checkPoints.GetChild(component.GetCurrentCheckpoint(finalCheckpoint == 0));
        currentCar.GetComponent<Rigidbody>().velocity = Vector3.zero;
        currentCar.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        currentCar.transform.rotation = child.rotation;
        currentCar.transform.position = child.position;
    }

    // Token: 0x06000098 RID: 152 RVA: 0x0000547C File Offset: 0x0000367C
    public void FinishRace(bool win, Transform car)
    {
        ChaosController.Instance.StopChaos();
        if (!playing)
        {
            return;
        }
        victory = win;
        playing = false;
        Time.timeScale = 0.3f;
        base.Invoke(nameof(ShowFinishScreen), 1f);
        if (endCamera)
        {
            endCamera.target = car;
            endCamera.gameObject.SetActive(true);
            CameraController.Instance.gameObject.SetActive(false);
        }
    }

    // Token: 0x06000099 RID: 153 RVA: 0x000054F4 File Offset: 0x000036F4
    public void ShowFinishScreen() => FinishController.Instance.Open(victory);

    // Token: 0x040000C3 RID: 195
    public Transform path;

    // Token: 0x040000C4 RID: 196
    public Transform checkPoints;

    // Token: 0x040000C5 RID: 197
    public LookAtTarget endCamera;

    // Token: 0x040000C6 RID: 198
    public int finalCheckpoint;

    // Token: 0x040000C8 RID: 200
    public float startTime = 1.5f;

    // Token: 0x040000C9 RID: 201
    public static GameController Instance;

    // Token: 0x040000CA RID: 202
    private bool victory;
}
