using UnityEngine;

// Token: 0x02000012 RID: 18
public class CheckPoint : MonoBehaviour
{
    // Token: 0x17000009 RID: 9
    // (get) Token: 0x06000066 RID: 102 RVA: 0x00004424 File Offset: 0x00002624
    // (set) Token: 0x06000067 RID: 103 RVA: 0x0000442C File Offset: 0x0000262C
    public int nr { get; set; }

    // Token: 0x06000068 RID: 104 RVA: 0x00004435 File Offset: 0x00002635
    private void Awake() => nr = base.transform.GetSiblingIndex();

    // Token: 0x06000069 RID: 105 RVA: 0x00004448 File Offset: 0x00002648
    private void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.layer & (LayerMask.NameToLayer("Car") | LayerMask.NameToLayer("Ghost"))) == other.gameObject.layer)
        {
            var component = other.gameObject.transform.root.GetComponent<CheckpointUser>();
            if (component)
            {
                component.CheckPoint(this);
            }
        }
    }

    // Token: 0x040000A0 RID: 160
    private readonly bool done;

    // Token: 0x040000A1 RID: 161
    public GameObject clearFx;
}
