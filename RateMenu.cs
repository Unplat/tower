using System;
using UnityEngine;
using UnityEngine.UI;

public class RateMenu : Menu
{
    [Header("Inherit References :")]
    [SerializeField] private Button _laterButton;
    [SerializeField] private Button _rateButton;
    [SerializeField] private Button _backButton;

    RateUs _rate;

    protected override void Awake()
    {
        base.Awake();

        _rate = GetComponent<RateUs>();
    }

    public override void SetEnable()
    {
        base.SetEnable();
        _laterButton.interactable = true;
        _rateButton.interactable = true;
        _backButton.interactable = true;
    }

    private void Start()
    {
        OnButtonPressed(_laterButton, LaterButtonListener);
        OnButtonPressed(_rateButton, RateButtonListener);
        OnButtonPressed(_backButton, HandleBackButtonPressed);
    }

    private void LaterButtonListener()
    {
        _laterButton.interactable = false;
        MenuManager.Instance.CloseMenu();
    }

    private void RateButtonListener()
    {
        _rateButton.interactable = false;
        _rate.RateNow();
    }

    private void HandleBackButtonPressed()
    {
        _backButton.interactable = false;

        MenuManager.Instance.CloseMenu();
    }
}
