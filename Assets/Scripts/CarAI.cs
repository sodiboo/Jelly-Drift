using UnityEngine;

// Token: 0x0200000C RID: 12
public class CarAI : MonoBehaviour
{
    // Token: 0x06000041 RID: 65 RVA: 0x00003654 File Offset: 0x00001854
    private void Start()
    {
        difficulty = (int)GameState.Instance.difficulty;
        MonoBehaviour.print(string.Concat(new object[]
        {
            "d: ",
            GameState.Instance.difficulty,
            ", a: ",
            difficulty
        }));
        car.engineForce = difficultyConfig[difficulty];
        base.InvokeRepeating(nameof(AdjustSpeed), 0.5f, 0.5f);
        if (GameController.Instance.finalCheckpoint != 0)
        {
            base.GetComponent<CheckpointUser>().ForceCheckpoint(0);
        }
    }

    // Token: 0x06000042 RID: 66 RVA: 0x000036F8 File Offset: 0x000018F8
    public void Recover()
    {
        car.rb.velocity = Vector3.zero;
        base.transform.position = nodes[FindClosestNode(3, base.transform)].position;
        var num = currentNode % nodes.Length;
        var num2 = (num + 1) % nodes.Length;
        var normalized = (nodes[num2].position - nodes[num].position).normalized;
        base.transform.rotation = Quaternion.LookRotation(normalized);
    }

    // Token: 0x06000043 RID: 67 RVA: 0x00003794 File Offset: 0x00001994
    private void CheckRecover()
    {
        if (!GameController.Instance.playing)
        {
            return;
        }
        if (base.transform.position.y < respawnHeight)
        {
            Recover();
        }
        if (base.IsInvoking(nameof(Recover)))
        {
            if (car.speed > 3f)
            {
                base.CancelInvoke(nameof(Recover));
            }
            return;
        }
        if (car.speed < 3f)
        {
            base.Invoke(nameof(Recover), recoverTime);
            return;
        }
        base.CancelInvoke(nameof(Recover));
    }

    // Token: 0x06000044 RID: 68 RVA: 0x00003827 File Offset: 0x00001A27
    private void Update()
    {
        if (!GameController.Instance.playing || !path)
        {
            return;
        }
        NewAI();
        CheckRecover();
    }

    // Token: 0x06000045 RID: 69 RVA: 0x0000384F File Offset: 0x00001A4F
    public void SetPath(Transform p)
    {
        path = p;
        nodes = path.GetComponentsInChildren<Transform>();
        car = base.GetComponent<Car>();
        currentNode = FindClosestNode(nodes.Length, base.transform);
    }

    // Token: 0x06000046 RID: 70 RVA: 0x00003890 File Offset: 0x00001A90
    private int FindNextTurn()
    {
        for (var i = currentNode; i < currentNode + turnLookAhead; i++)
        {
            var num = i % nodes.Length;
            var num2 = (num + 1) % nodes.Length;
            var num3 = (num2 + 1) % nodes.Length;
            var vector = nodes[num2].position - nodes[num].position;
            var vector2 = nodes[num3].position - nodes[num2].position;
            var f = Vector3.SignedAngle(vector.normalized, vector2.normalized, Vector3.up);
            if (Mathf.Abs(f) > 20f)
            {
                turnDir = (int)Mathf.Sign(f);
                nextTurnLength = FindNextStraight(num2);
                return num2;
            }
        }
        return -1;
    }

    // Token: 0x06000047 RID: 71 RVA: 0x00003970 File Offset: 0x00001B70
    private int FindNextStraight(int startNode)
    {
        for (var i = startNode; i < startNode + turnLookAhead; i++)
        {
            var num = i % nodes.Length;
            var num2 = (num + 1) % nodes.Length;
            var num3 = (num2 + 1) % nodes.Length;
            var from = nodes[num2].position - nodes[num].position;
            var to = nodes[num3].position - nodes[num2].position;
            if (Mathf.Abs(Vector3.SignedAngle(from, to, Vector3.up)) < 15f)
            {
                return num2 - startNode;
            }
        }
        return 3;
    }

    // Token: 0x06000048 RID: 72 RVA: 0x00003A18 File Offset: 0x00001C18
    private void NewAI()
    {
        var num = FindClosestNode(maxLookAhead, base.transform);
        currentNode = num;
        var num2 = (num + 1) % nodes.Length;
        if (currentNode > nextTurnStart + nextTurnLength)
        {
            nextTurnStart = FindNextTurn();
        }
        if (num2 < nextTurnStart)
        {
            xOffset = 0.13f * turnDir;
        }
        else if (num2 >= nextTurnStart && num2 < nextTurnStart + nextTurnLength)
        {
            xOffset = -0.13f * turnDir;
        }
        else
        {
            xOffset = 0f;
        }
        var b = Vector3.Cross(nodes[num2].position - nodes[num].position, Vector3.up) * xOffset;
        var vector = nodes[num2].position + b - base.transform.position;
        vector = base.transform.InverseTransformDirection(vector);
        var num3 = 1f + Mathf.Clamp(car.speed * 0.01f * speedSteerMultiplier, 0f, 1f);
        car.steering = Mathf.Clamp(vector.x * 0.05f * num3, -1f, 1f) * num3;
        car.throttle = 1f;
        car.throttle = 1f - Mathf.Abs(car.steering * Mathf.Clamp(car.speed - maxTurnSpeed, 0f, 100f) * 0.06f);
    }

    // Token: 0x06000049 RID: 73 RVA: 0x00003BE0 File Offset: 0x00001DE0
    private void AdjustSpeed()
    {
        var num = FindClosestNode(nodes.Length, base.transform) / (float)nodes.Length;
        var num2 = FindClosestNode(nodes.Length, GameController.Instance.currentCar.transform) / (float)nodes.Length;
        var num3 = num - num2;
        if (num3 < 0f)
        {
            num3 *= speedupM;
        }
        if (num3 > 0f)
        {
            num3 *= slowdownM;
        }
        var num4 = difficultyConfig[difficulty] - Mathf.Clamp(num3 * 1000f * speedAdjustMultiplier, -8000f, 4000f);
        num4 = Mathf.Clamp(num4, 1000f, 8000f);
        car.engineForce = num4;
    }

    // Token: 0x0600004A RID: 74 RVA: 0x00003CA4 File Offset: 0x00001EA4
    public int FindClosestNode(int maxLook, Transform target)
    {
        var num = float.PositiveInfinity;
        var result = 0;
        for (var i = 0; i < maxLook; i++)
        {
            var num2 = (currentNode + i) % nodes.Length;
            var num3 = Vector3.Distance(target.position, nodes[num2].position);
            if (num3 < num)
            {
                num = num3;
                result = num2;
            }
        }
        return result;
    }

    // Token: 0x04000069 RID: 105
    [ExecuteInEditMode]
    public Transform path;

    // Token: 0x0400006A RID: 106
    public Transform[] nodes;

    // Token: 0x0400006B RID: 107
    public Car car;

    // Token: 0x04000070 RID: 112
    private readonly int maxLookAhead = 6;

    // Token: 0x04000072 RID: 114
    public int respawnHeight;

    // Token: 0x04000073 RID: 115
    private int difficulty;

    // Token: 0x04000074 RID: 116
    public int[] difficultyConfig;

    // Token: 0x04000075 RID: 117
    private readonly float recoverTime = 1.5f;

    // Token: 0x04000076 RID: 118
    private readonly int turnLookAhead = 6;

    // Token: 0x04000077 RID: 119
    private int turnDir;

    // Token: 0x04000078 RID: 120
    private int nextTurnStart;

    // Token: 0x04000079 RID: 121
    private int nextTurnLength;

    // Token: 0x0400007A RID: 122
    public float xOffset;

    // Token: 0x0400007B RID: 123
    public float speedSteerMultiplier = 1f;

    // Token: 0x0400007C RID: 124
    public float steerMultiplier = 1f;

    // Token: 0x0400007D RID: 125
    public int maxTurnSpeed = 50;

    // Token: 0x0400007E RID: 126
    private readonly float speedAdjustMultiplier = 5f;

    // Token: 0x0400007F RID: 127
    private readonly float speedupM = 15f;

    // Token: 0x04000080 RID: 128
    private readonly float slowdownM = 5f;

    // Token: 0x04000081 RID: 129
    public int currentNode;
}
