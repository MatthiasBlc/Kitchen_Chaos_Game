using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterSound : MonoBehaviour
{

  [SerializeField] private StoveCounter stoveCounter;
  private AudioSource audiosource;
  private float warningSoundTimer;
  private bool playWarningSound;

  private void Awake()
  {
    audiosource = GetComponent<AudioSource>();
  }

  private void Start()
  {
    stoveCounter.OnStateChanged += stoveCounter_OnStateChanged;
    stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;
  }

  private void StoveCounter_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
  {

    float burnShowProgressAmount = .5f;
    playWarningSound = stoveCounter.IsFried() && e.progressNormalized >= burnShowProgressAmount;
  }

  private void stoveCounter_OnStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e)
  {
    bool playSound = e.state == StoveCounter.State.Frying || e.state == StoveCounter.State.Fried;
    if (playSound)
    {
      audiosource.Play();
    }
    else
    {
      audiosource.Pause();
    }
  }

  private void Update()
  {
    if (playWarningSound)
    {

      warningSoundTimer -= Time.deltaTime;
      if (warningSoundTimer <= 0f)
      {
        float warningSoundTimerMax = .2f;
        warningSoundTimer = warningSoundTimerMax;

        SoundManager.Instance.PlayWarningSound(stoveCounter.transform.position);
      }
    }
  }
}
