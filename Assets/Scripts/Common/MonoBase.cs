using UnityEngine;

public class MonoBase : MonoBehaviour
{
    public enum SE
    {
        Attack = 0,
        Start,
        Finish,
        Miss
    }
    private readonly static string[] SENames = new[]
    {
        "attack",
        "start",
        "finish",
        "miss",
    };

    public static void PlaySe(SE se)
    {
        var obj = InstantiatePrefab($"SE/{SENames[(int)se]}");
        if (obj) obj.AddComponent<SeAutoDestroy>();
    }

    public static GameObject InstantiatePrefab(string path, Transform parent = null)
    {
        var prefab = Resources.Load<GameObject>($"Prefabs/{path}");
        var obj = Instantiate(prefab);
        if (parent) obj.transform.SetParent(parent);
        obj.transform.localPosition = prefab.transform.localPosition;
        obj.transform.localRotation = prefab.transform.localRotation;
        obj.transform.localScale    = prefab.transform.localScale   ;
        return obj;
    }
}
