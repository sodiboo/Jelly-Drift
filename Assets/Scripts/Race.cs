using UnityEngine;

// Token: 0x0200003F RID: 63
public class Race : MonoBehaviour
{
    // Token: 0x17000022 RID: 34
    // (get) Token: 0x0600014C RID: 332 RVA: 0x00007862 File Offset: 0x00005A62
    // (set) Token: 0x0600014D RID: 333 RVA: 0x0000786A File Offset: 0x00005A6A
    public GameObject enemyCar { get; set; }

    // Token: 0x0600014E RID: 334 RVA: 0x00007874 File Offset: 0x00005A74
    private void Awake()
    {
        if (GameState.Instance.gamemode == Gamemode.TimeTrial)
        {
            ChaosEffect.HasEnemy = false;
            ChaosEffect.enemy = null;
            UnityEngine.Object.Destroy(this);
            return;
        }
        gameController = base.gameObject.GetComponent<GameController>();
        var startPos = gameController.startPos;
        enemyCar = UnityEngine.Object.Instantiate<GameObject>(enemyCarPrefab, startPos.position + startPos.forward * 10f, startPos.rotation);
        ChaosEffect.HasEnemy = true;
        ChaosEffect.enemy = enemyCar.GetComponent<CarAI>();
        ChaosEffect.enemy.SetPath(gameController.path);
    }

    // Token: 0x0600014F RID: 335 RVA: 0x000078FE File Offset: 0x00005AFE
    private void Start()
    {
        enemyCar.AddComponent<CheckpointUser>().player = false;
        GameController.Instance.currentCar.AddComponent<CheckpointUser>();
    }

    // Token: 0x04000157 RID: 343
    public GameObject enemyCarPrefab;

    // Token: 0x04000159 RID: 345
    private GameController gameController;
}
