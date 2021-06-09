using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// Token: 0x02000036 RID: 54
public class Replay : MonoBehaviour
{
    // Token: 0x06000113 RID: 275 RVA: 0x00006B30 File Offset: 0x00004D30
    private void Start()
    {
        if (GameState.Instance.gamemode != Gamemode.TimeTrial || GameState.Instance.ghost == GhostCycle.Ghost.Off)
        {
            UnityEngine.Object.Destroy(this);
            return;
        }
        var ghost = GameState.Instance.ghost;
        replay = new List<ReplayController.ReplayFrame>();
        var text = "pb";
        text += GameState.Instance.map;
        if (ghost == GhostCycle.Ghost.PB)
        {
            filePath = Application.persistentDataPath + "/replays/" + text + ".txt";
            if (!File.Exists(filePath))
            {
                UnityEngine.Object.Destroy(this);
                MonoBehaviour.print("couldnt find destroying");
                return;
            }
        }
        MonoBehaviour.print("path: " + filePath);
        if (ghost == GhostCycle.Ghost.Dani)
        {
            ReadTextAsset();
            return;
        }
        var streamReader = new StreamReader(filePath);
        var text2 = streamReader.ReadLine();
        text2 = text2.Replace("(", string.Empty).Replace(")", string.Empty);
        var array = Array.ConvertAll<string, int>(text2.Split(new char[]
        {
            ','
        }), new Converter<string, int>(int.Parse));
        MonoBehaviour.print(string.Concat(new object[]
        {
            "lna: ",
            array[0],
            ", ",
            array[1]
        }));
        var gameObject = UnityEngine.Object.Instantiate<GameObject>(PrefabManager.Instance.cars[array[0]]);
        gameObject.GetComponent<CarSkin>().SetSkin(array[1]);
        while ((text2 = streamReader.ReadLine()) != null)
        {
            text2 = text2.Replace("(", string.Empty).Replace(")", string.Empty);
            var array2 = Array.ConvertAll<string, float>(text2.Split(new char[]
            {
                ','
            }), new Converter<string, float>(float.Parse));
            var item = new ReplayController.ReplayFrame(new Vector3(array2[0], array2[1], array2[2]), new Vector3(array2[3], array2[4], array2[5]), array2[6], array2[7]);
            replay.Add(item);
        }
        rb = gameObject.GetComponent<Rigidbody>();
        rb.isKinematic = true;
        UnityEngine.Object.Destroy(gameObject.GetComponentInChildren<Collider>());
        var componentsInChildren = gameObject.GetComponentsInChildren<ParticleSystem>();
        for (var i = 0; i < componentsInChildren.Length; i++)
        {
            componentsInChildren[i].gameObject.SetActive(false);
        }
        var componentsInChildren2 = gameObject.GetComponentsInChildren<Suspension>();
        for (var i = 0; i < componentsInChildren2.Length; i++)
        {
            componentsInChildren2[i].showFx = false;
        }
        var componentsInChildren3 = gameObject.GetComponentsInChildren<AudioSource>();
        for (var i = 0; i < componentsInChildren3.Length; i++)
        {
            componentsInChildren3[i].enabled = false;
        }
        gameObject.AddComponent<Ghost>();
    }

    // Token: 0x06000114 RID: 276 RVA: 0x00006E08 File Offset: 0x00005008
    private void ReadTextAsset()
    {
        var textAsset = daniTimes[GameState.Instance.map];
        if (!textAsset)
        {
            UnityEngine.Object.Destroy(this);
            return;
        }
        var array = textAsset.text.Split(new string[]
        {
            Environment.NewLine
        }, StringSplitOptions.None);
        var i = 0;
        var array2 = Array.ConvertAll<string, int>(array[i++].Replace("(", string.Empty).Replace(")", string.Empty).Split(new char[]
        {
            ','
        }), new Converter<string, int>(int.Parse));
        var gameObject = UnityEngine.Object.Instantiate<GameObject>(PrefabManager.Instance.cars[array2[0]]);
        gameObject.GetComponent<CarSkin>().SetSkin(array2[1]);
        while (i < array.Length - 1)
        {
            var array3 = Array.ConvertAll<string, float>(array[i++].Replace("(", string.Empty).Replace(")", string.Empty).Split(new char[]
            {
                ','
            }), new Converter<string, float>(float.Parse));
            var item = new ReplayController.ReplayFrame(new Vector3(array3[0], array3[1], array3[2]), new Vector3(array3[3], array3[4], array3[5]), array3[6], array3[7]);
            replay.Add(item);
        }
        rb = gameObject.GetComponent<Rigidbody>();
        rb.isKinematic = true;
        UnityEngine.Object.Destroy(gameObject.GetComponentInChildren<Collider>());
        var componentsInChildren = gameObject.GetComponentsInChildren<ParticleSystem>();
        for (var j = 0; j < componentsInChildren.Length; j++)
        {
            componentsInChildren[j].gameObject.SetActive(false);
        }
        var componentsInChildren2 = gameObject.GetComponentsInChildren<Suspension>();
        for (var j = 0; j < componentsInChildren2.Length; j++)
        {
            componentsInChildren2[j].showFx = false;
        }
        var componentsInChildren3 = gameObject.GetComponentsInChildren<AudioSource>();
        for (var j = 0; j < componentsInChildren3.Length; j++)
        {
            componentsInChildren3[j].enabled = false;
        }
        gameObject.AddComponent<Ghost>();
    }

    // Token: 0x06000115 RID: 277 RVA: 0x00007008 File Offset: 0x00005208
    private void FixedUpdate()
    {
        if (currentFrame >= replay.Count - 1)
        {
            return;
        }
        rb.MovePosition(Vector3.Lerp(rb.transform.position, replay[currentFrame].pos, Time.deltaTime * 30f));
        rb.MoveRotation(Quaternion.Lerp(rb.rotation, Quaternion.Euler(replay[currentFrame].rot), Time.deltaTime * 30f));
        currentFrame++;
    }

    // Token: 0x0400012E RID: 302
    private List<ReplayController.ReplayFrame> replay;

    // Token: 0x04000130 RID: 304
    private string filePath;

    // Token: 0x04000131 RID: 305
    public TextAsset[] daniTimes;

    // Token: 0x04000132 RID: 306
    private int currentFrame;

    // Token: 0x04000134 RID: 308
    public Rigidbody rb;
}
