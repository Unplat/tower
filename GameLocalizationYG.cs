using UnityEngine;
using UnityEngine.UI;
using YG; // Подключаем пространство имен плагина

public class GameLocalizationYG : MonoBehaviour
{
    [Header("UI Элементы экрана Game Over")]
    [SerializeField] private Text restartButtonText;
    [SerializeField] private Text reviveButtonText;
    // [SerializeField] private Text gameOverTitle;
    // [SerializeField] private Text howToPlayText;

    private void Start()
    {
        // Вызываем перевод при старте сцены
        ApplyLanguage(YG2.lang);
    }

    public void ApplyLanguage(string lang)
    {
        // Проверяем язык и подставляем нужные строчки
        if (lang == "ru")
        {
            if (restartButtonText) restartButtonText.text = "Построй Башню 3D";
            //if (reviveButtonText) reviveButtonText.text = "ВОЗРОЖДЕНИЕ";
            // if (gameOverTitle) gameOverTitle.text = "ИГРА ОКОНЧЕНА";
            // if (howToPlayText) howToPlayText.text = "Жми вовремя, чтобы ставить блоки!";
        }
        else // Для "en" и всех остальных языков, которые ваш скрипт скорректировал на "en"
        {
            if (restartButtonText) restartButtonText.text = "STACK";
            // if (reviveButtonText) reviveButtonText.text = "REVIVE";
            // if (gameOverTitle) gameOverTitle.text = "GAME OVER";
            // if (howToPlayText) howToPlayText.text = "Tap on time to stack blocks!";
        }
    }
}
