using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreditMenu : Menu
{
    [Header("Inherit References :")]
    [SerializeField] private Button _backButton;

    [Space]
    [SerializeField] private TMP_Text _titleText;
    [SerializeField] private TMP_Text _descText;


    [Header("Game Database :")]
    [SerializeField] private CreditDataSO _data;

    public override void SetEnable()
    {
        base.SetEnable();

        _backButton.interactable = true;
    }

    private void Start()
    {
        OnButtonPressed(_backButton, HandleBackButtonPressed);

        _titleText.text = _data.GetTitle;
        _descText.text = _data.GetDesc;
    }

    private void HandleBackButtonPressed()
    {
        _backButton.interactable = false;

        MenuManager.Instance.CloseMenu();
    }
}
