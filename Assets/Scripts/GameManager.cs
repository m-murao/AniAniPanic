using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBase
{
    private enum State
    {
        None,
        Title,
        Count,
        Game,
        FinishFadeOut,
        FinishFadeIn,
        Result,
        Return,
    }

    private static GameManager instance = null;
    public  static GameManager Instance { get { return instance ?? new GameObject("GameManager").AddComponent<GameManager>(); } }

    private void Awake()
    {
        if (instance)
        {
            Destroy(this);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private State state = State.Title;
    private void SetState(State s) { state = s; startTime = Time.realtimeSinceStartup; }
    public bool IsCountState  { get { return state == State.Count ; } }

    public bool IsGameState   { get { return state == State.Game  ; } }
    public bool IsResultState { get { return state == State.Result; } }

    public bool IsFinishFadeInState { get { return state == State.FinishFadeIn; } }

    private bool IsRemainGameTimeZero { get { return state > State.Game; } }

    private const float CountMaxTime = 3.5f;
    private const float GameMaxTime = 30;
    private float startTime = 0;
    public float ElapsedTime { get { return Time.realtimeSinceStartup - startTime; } }
    public int CountTime { get { return Mathf.Max(0, Mathf.CeilToInt(CountMaxTime - ElapsedTime - 0.5f)); } }
    public float GameTimeFactor { get { return Mathf.Min(ElapsedTime / GameMaxTime, 1); } }
    public float RemainGameTime { get { return IsRemainGameTimeZero ? 0 : IsGameState ? Mathf.Max(0, GameMaxTime - ElapsedTime) : GameMaxTime; } }
    public bool IsGameFinished { get { return ElapsedTime >= GameMaxTime; } }

    public int GamePoint { get; set; }

    [RuntimeInitializeOnLoadMethod]
    private static void ProjectInitializer()
    {
        Application.targetFrameRate = 60;

        ScreenFader.Instance.FadeTime = 1;
        ScreenFader.Instance.FadeIn(null);

        switch(SceneManager.GetActiveScene().name)
        {
            case "GameScene":
                Instance.SetState(State.Count);
                break;
            default:
                Instance.SetState(State.Title);
                break;
        }
    }

    public void GameStart()
    {
        SetState(State.Game);
        GamePoint = 0;
    }

    private void LoadScene(string sceneName, Action callback)
    {
        void SceneLoaded(Scene s, LoadSceneMode mode)
        {
            SceneManager.sceneLoaded -= SceneLoaded;
            callback.SafeCall();
        }

        SceneManager.sceneLoaded += SceneLoaded;
        SceneManager.LoadSceneAsync(sceneName);
    }


    private void Update()
    {
        switch (state)
        {
            case State.Title : ExecTitle (); break;
            case State.Count : ExecCount (); break;
            case State.Game  : ExecGame  (); break;
            case State.Result: ExecResult(); break;
        }
    }

    private void ExecTitle()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SetState(State.None);
            ScreenFader.Instance.FadeOut(() =>
            {
                LoadScene("GameScene", () =>
                {
                    ScreenFader.Instance.FadeIn(() =>
                    {
                        SetState(State.Count);
                    });
                });
            });
        }
    }

    private void ExecCount()
    {
        if (ElapsedTime >= CountMaxTime)
        {
            PlaySe(SE.Start);
            SetState(State.Game);
        }
    }

    private void ExecGame()
    {
        if (IsGameFinished)
        {
            PlaySe(SE.Finish);
            SetState(State.FinishFadeOut);
            ScreenFader.Instance.FadeOut(() =>
            {
                SetState(State.FinishFadeIn);
                ScreenFader.Instance.FadeIn(() =>
                {
                    SetState(State.Result);
                });
            });
        }
    }

    private void ExecResult()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SetState(State.Return);
            ScreenFader.Instance.FadeOut(() =>
            {
                LoadScene("TitleScene", () =>
                {
                    ScreenFader.Instance.FadeIn(() =>
                    {
                        SetState(State.Title);
                    });
                });
            });
        }
    }

}
