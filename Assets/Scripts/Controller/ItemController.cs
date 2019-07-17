using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBase
{
    public enum Type
    {
        Haneru = 0,
        Ran,
        Hinako,
        Ichika,
    }

    public class ItemData
    {
        public readonly Type Type;
        public readonly int Weight;
        public readonly int Point;
        public readonly bool HitOnce;

        public ItemData(Type type, int weight, int point, bool hitOnce)
        {
            Type = type;
            Weight = weight;
            Point = point;
            HitOnce = hitOnce;
        }
    }

    private readonly static ItemData[] lotDatas = new[]
    {
        new ItemData(Type.Haneru, 1000, -5,  true),
        new ItemData(Type.Ran   , 5000,  4,  true),
        new ItemData(Type.Hinako, 2000,  1, false),
    };
    private static int totalWeight = 0;

    [SerializeField] private Animator[] itemAnimators;

    private ItemData CurrentItem { get; set; }

    private Animator CurrentAnim { get { return CurrentItem == null ? null : itemAnimators[(int)CurrentItem.Type]; } }

    private bool IsAnimFinished { get { return CurrentItem == null || CurrentAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1; } }

    private bool isHit = false;

    private int CurrentSpeed { get { return Mathf.Max(1, Mathf.FloorToInt(GameManager.Instance.GameTimeFactor *  5)); } }
    private int CurrentWave  { get { return Mathf.Max(1, Mathf.FloorToInt(GameManager.Instance.GameTimeFactor * 10)); } }

    private static ItemData ItemLot()
    {
        if (totalWeight == 0)
        {
            foreach (var data in lotDatas)
            {
                totalWeight += data.Weight;
            }
        }

        ItemData ret = null;
        var rand = Random.Range(1, totalWeight);
        foreach(var data in lotDatas)
        {
            rand -= data.Weight;
            if (rand <= 0)
            {
                ret = data;
                break;
            }
        }

        return ret;
    }

    private void Awake()
    {
        foreach(var item in itemAnimators)
        {
            item.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (GameManager.Instance.IsGameState && IsAnimFinished)
        {
            if (CurrentAnim && CurrentAnim.gameObject.activeSelf)
            {
                CurrentAnim.gameObject.SetActive(false);
            }
            CurrentItem = null;

            var rand = Random.Range(1, 100 / CurrentWave * Application.targetFrameRate);
            if (rand < Application.targetFrameRate / 2)
            {
                RandomStart();
            }
        }
    }

    public void RandomStart()
    {
        isHit = false;
        CurrentItem = ItemLot();
        CurrentAnim.gameObject.SetActive(true);
        CurrentAnim.speed = CurrentSpeed;
    }

    public void Hit()
    {
        if (isHit) return;
        if (!GameManager.Instance.IsGameState) return;
        isHit = CurrentItem.HitOnce;
        GameManager.Instance.GamePoint += CurrentItem.Point;
        if (CurrentItem.Type == Type.Haneru) PlaySe(SE.Miss);
        else PlaySe(SE.Attack);
    }
}
