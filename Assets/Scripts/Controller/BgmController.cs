using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgmController : MonoBase
{
    [SerializeField] private AudioSource  mainBgm;
    [SerializeField] private AudioSource finalBgm;

    private int step = 0;

    void Update()
    {
        if (step == 0)
        {
            if (GameManager.Instance.IsGameState)
            {
                if (GameManager.Instance.GameTimeFactor >= 0.7)
                {
                    mainBgm.Stop();
                    finalBgm.Play();
                    step++;
                }
            }
        }
        else if (step == 1)
        {
            if (!GameManager.Instance.IsGameState)
            {
                mainBgm.Play();
                finalBgm.Stop();
                step++;
            }
        }
    }
}
