using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFader : MonoBase
{
    public enum State
    {
        None,
        FadeIn,
        FadeOut,
    }

    private static ScreenFader instance = null;
    public  static ScreenFader Instance
    {
        get
        {
            if (instance == null)
            {
                return InstantiatePrefab("ScreenFader").GetComponent<ScreenFader>();
            }
            return instance;
        }
    }

    private State state = State.None;
    public  float FadeTime { get; set; } = 0.5f;
    private float startTime = 0.0f;
    private Action finishCallback;

    private float ElapsedTime { get { return Time.realtimeSinceStartup - startTime; } }
    private float Factor { get { return Mathf.Min(ElapsedTime / FadeTime, 1); } }
    private bool IsFinished { get { return ElapsedTime >= FadeTime; } }

    [SerializeField] private Image fadeImage = null;

    private void Awake()
    {
        if (instance)
        {
            Destroy(this);
            return;
        }
        instance = this;
        if (!fadeImage)
        {
            fadeImage = new GameObject("FadeImage").AddComponent<Image>();
            fadeImage.transform.SetParent(transform);
            fadeImage.rectTransform.anchoredPosition = new Vector2(0, 0);
            fadeImage.rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);
            fadeImage.color = Color.black;
        }
        SetAlpha(0);
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        switch(state)
        {
            default: return;

            case State.FadeIn:
                SetAlpha(1 - Factor);
                break;

            case State.FadeOut:
                SetAlpha(Factor);
                break;
        }
        if (IsFinished)
        {
            state = State.None;
            finishCallback.SafeCall();
        }
    }

    private void SetAlpha(float a)
    {
        var color = fadeImage.color;
        color.a = a;
        fadeImage.color = color;
    }

    public void FadeIn(Action callback)
    {
        state = State.FadeIn;
        finishCallback = callback;
        startTime = Time.realtimeSinceStartup;
        SetAlpha(1);
    }

    public void FadeOut(Action callback)
    {
        state = State.FadeOut;
        finishCallback = callback;
        startTime = Time.realtimeSinceStartup;
        SetAlpha(0);
    }


}
