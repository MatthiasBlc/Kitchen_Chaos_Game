using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class OptionsUI : MonoBehaviour
{

  public static OptionsUI Instance { get; private set; }

  [SerializeField] Button soundEffectsButton;
  [SerializeField] Button musicButton;
  [SerializeField] Button closeButton;
  [SerializeField] Button moveUpButton;
  [SerializeField] Button moveDownButton;
  [SerializeField] Button moveLeftButton;
  [SerializeField] Button moveRightButton;
  [SerializeField] Button interactButton;
  [SerializeField] Button interactAlternativeUpButton;
  [SerializeField] Button pauseButton;
  [SerializeField] Button gamepadInteractButton;
  [SerializeField] Button gamepadInteractAlternativeButton;
  [SerializeField] Button gamepadPauseButton;


  [SerializeField] private TextMeshProUGUI soundEffectsText;
  [SerializeField] private TextMeshProUGUI musicText;
  [SerializeField] private TextMeshProUGUI moveUpText;
  [SerializeField] private TextMeshProUGUI moveDownText;
  [SerializeField] private TextMeshProUGUI moveLeftText;
  [SerializeField] private TextMeshProUGUI moveRightText;
  [SerializeField] private TextMeshProUGUI interactText;
  [SerializeField] private TextMeshProUGUI interactAlternativeText;
  [SerializeField] private TextMeshProUGUI pauseText;
  [SerializeField] private TextMeshProUGUI gamepadInteractText;
  [SerializeField] private TextMeshProUGUI gamepadInteractAlternativeText;
  [SerializeField] private TextMeshProUGUI gamepadPauseText;
  [SerializeField] private Transform pressToRebindKeyTransform;


  private Action onCLoseButtonAction;

  private void Awake()
  {
    Instance = this;

    soundEffectsButton.onClick.AddListener(() =>
    {
      SoundManager.Instance.ChangeVolume();
      UpdateVisual();
    });
    musicButton.onClick.AddListener(() =>
    {
      MusicManager.Instance.ChangeVolume();
      UpdateVisual();
    });
    closeButton.onClick.AddListener(() =>
    {
      Hide();
      onCLoseButtonAction();
    });

    moveUpButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Move_Up); });
    moveDownButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Move_Down); });
    moveLeftButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Move_Left); });
    moveRightButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Move_Right); });
    interactButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Interact); });
    interactAlternativeUpButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.InteractAlternate); });
    pauseButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Pause); });
    gamepadInteractButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Gamepad_Interact); });
    gamepadInteractAlternativeButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Gamepad_InteractAlternate); });
    gamepadPauseButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Gamepad_Pause); });

  }

  private void Start()
  {
    KitchenGameManager.Instance.OnGameUnpaused += KitchenGameManager_OnGameUnpaused;
    UpdateVisual();

    HidePressToRebindKey();
    Hide();
  }

  private void KitchenGameManager_OnGameUnpaused(object sender, System.EventArgs e)
  {
    Hide();
  }

  private void UpdateVisual()
  {
    soundEffectsText.text = "Sound Effects: " + Mathf.Round(SoundManager.Instance.GetVolume() * 10f);
    musicText.text = "Music: " + Mathf.Round(MusicManager.Instance.GetVolume() * 10f);

    moveUpText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Up);
    moveDownText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Down);
    moveLeftText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Left);
    moveRightText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Right);
    interactText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact);
    interactAlternativeText.text = GameInput.Instance.GetBindingText(GameInput.Binding.InteractAlternate);
    pauseText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Pause);
    gamepadInteractText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Gamepad_Interact);
    gamepadInteractAlternativeText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Gamepad_InteractAlternate);
    gamepadPauseText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Gamepad_Pause);
  }

  public void Show(Action onCLoseButtonAction)
  {
    this.onCLoseButtonAction = onCLoseButtonAction;

    gameObject.SetActive(true);

    soundEffectsButton.Select();
  }

  private void Hide()
  {
    gameObject.SetActive(false);
  }

  private void ShowPressToRebindKey()
  {
    pressToRebindKeyTransform.gameObject.SetActive(true);
  }

  private void HidePressToRebindKey()
  {
    pressToRebindKeyTransform.gameObject.SetActive(false);
  }

  private void RebindBinding(GameInput.Binding binding)
  {
    ShowPressToRebindKey();
    GameInput.Instance.RebindBinding(binding, () =>
    {
      HidePressToRebindKey();
      UpdateVisual();
    });
  }
}
