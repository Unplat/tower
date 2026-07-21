using System;
using System.Collections.Generic; // Для работы со списками
using UnityEngine;
using YG; // Подключаем плагин Яндекс Игр (PluginYG2)

public class CubeSpawner : MonoBehaviour
{
    public static int CubeCount;
    
    // Список для отслеживания всех живых кубов на сцене
    public static List<MovingCube> SpawnedCubes = new List<MovingCube>();

    [SerializeField] private MovingCube _cubePrefab;
    [SerializeField] private MoveDirection _moveDirection;
    [SerializeField] private float _initialSpeed;
    [SerializeField] private float _maxSpeed;
    [SerializeField] private float _speedMultiplier = .05f;
    private float _currentSpeed;

    [Header("Настройки UI")]
    [SerializeField] private GameObject _gameOverPanel; // Перетащите сюда панель Game Over в инспекторе

    private void Awake()
    {
        CubeCount = 0;
        _currentSpeed = _initialSpeed;
        SpawnedCubes.Clear(); // Очищаем список при старте новой игры
    }

    public void SpawnCube()
    {
        var cube = Instantiate(_cubePrefab, transform);
        
        float x = _moveDirection == MoveDirection.X ? transform.position.x : MovingCube.LastCube.transform.position.x;
        float z = _moveDirection == MoveDirection.Z ? transform.position.z : MovingCube.LastCube.transform.position.z;
        
        cube.transform.position = new Vector3(x, MovingCube.LastCube.transform.position.y + _cubePrefab.transform.localScale.y, z);
        cube.MoveDirection = _moveDirection;
        
        CubeCount++;
        cube.SetSpeed(GetSpeed());

        // Добавляем созданный куб в наш список
        SpawnedCubes.Add(cube);
    }

    // ====================================================================
    // ВЫЗОВ РЕКЛАМЫ (ПРИВЯЗАТЬ ЭТОТ МЕТОД К КНОПКЕ ВОЗРОЖДЕНИЯ В UI ONCLICK)
    // ====================================================================
    public void OnReviveButtonClick()
    {
        // Выключаем звук игры и ставим физику на паузу на время рекламы
        AudioListener.volume = 0f;
        Time.timeScale = 0f;

        // Внутренний ID для плагина
        string rewardID = "revive_stack"; 

        // ИСПРАВЛЕНО: Вызываем рекламу строго по второму примеру из документации (с коллбэком)
        YG2.RewardedAdvShow(rewardID, GetRewardCallback); 
    }

    // Этот метод выполнится ТОЛЬКО при успешном просмотре видео до конца
    private void GetRewardCallback()
    {
        // 1. Возвращаем звук и время назад в нормальное русло
        AudioListener.volume = 1f;
        Time.timeScale = 1f;

        // 2. Прячем окно Game Over, так как игрок спасен
        if (_gameOverPanel != null) _gameOverPanel.SetActive(false);

        // 3. Запускаем механику отката блоков назад
        RollbackOnRevive();

        // 4. Сразу спавним новый блок, чтобы игра мгновенно продолжилась
        SpawnCube();
    }

    // ====================================================================
    // ВНУТРЕННЯЯ ЛОГИКА ОТКАТА БЛОКОВ
    // ====================================================================
    private void RollbackOnRevive()
    {
        // Не удаляем стартовый блок (всегда оставляем хотя бы 1 базовый куб на земле)
        int blocksToRemove = Mathf.Min(5, SpawnedCubes.Count - 1);
        if (blocksToRemove <= 0) return;

        for (int i = 0; i < blocksToRemove; i++)
        {
            int lastIndex = SpawnedCubes.Count - 1;
            MovingCube cubeToDelete = SpawnedCubes[lastIndex];

            // Удаляем из списка и физически уничтожаем со сцены
            SpawnedCubes.RemoveAt(lastIndex);
            if (cubeToDelete != null)
            {
                Destroy(cubeToDelete.gameObject);
            }

            // Уменьшаем счетчик кубов, чтобы скорректировать скорость и счет
            CubeCount--;
        }

        // Переназначаем ссылку на LastCube через созданный ранее публичный метод в MovingCube
        MovingCube.SetLastCube(SpawnedCubes[SpawnedCubes.Count - 1]);

        // Корректируем высоту спавнера (опускаем его назад на высоту удаленных блоков)
        float totalHeightOffset = _cubePrefab.transform.localScale.y * blocksToRemove;
        transform.position -= new Vector3(0, totalHeightOffset, 0);
    }

    private float GetSpeed()
    {
        float additionalSpeed = CubeCount * _speedMultiplier;
        _currentSpeed = _initialSpeed + additionalSpeed;
        _currentSpeed = Mathf.Clamp(_currentSpeed, _initialSpeed, _maxSpeed);
        return _currentSpeed;
    }

    private void OnDestroy()
    {
        CubeCount = 0;
        SpawnedCubes.Clear();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, _cubePrefab.transform.localScale);
    }
}
