using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UICanvasMap : MonoBehaviour
{
    public GameObject TitlePanel;
    public GameObject BattlePanel;
    public GameObject PausePanel;
    public GameObject ResultPanel;

    [SerializeField] Image[] _enemyThumbnail;
    [SerializeField] TextMeshProUGUI[] _enemyInfoText;
    [SerializeField] TextMeshProUGUI[] _resultText;

    [SerializeField] Slider _hpBar;
    [SerializeField] TextMeshProUGUI _hpp;

    [SerializeField] TextMeshProUGUI _timerText;

    private void Update()
    {
        if (Managers.Instance.Flow.State == GameState.Battle)
        {
            Managers.Instance.Flow.Timer += Time.deltaTime;
            UpdateTimerText();
        }
    }

    public void OnStartGameButtonClicked()
    {
        Managers.Instance.Flow.State = GameState.Battle;
    }
    public void OnEnemyProfileClicked()
    {
        Managers.Instance.Flow.State = GameState.BattlePause;
    }
    public void OnEnemyInfoAnywhereClicked()
    {
        Managers.Instance.Flow.State = GameState.Battle;
    }
    public void OnRestartButtonClicked()
    {
        Managers.Instance.Flow.State = GameState.Battle;
    }
    public void OnTitleButtonClicked()
    {
        Managers.Instance.Flow.State = GameState.Title;
    }

    public void UpdateEnemyInfoPanel()
    {
        Enemy enemy = Managers.Instance.Flow.CurrentEnemy;
        _enemyInfoText[0].text = enemy.Name;
        _enemyInfoText[1].text = enemy.Grade;
        _enemyInfoText[2].text = enemy.Speed.ToString("F1") + $" (회피 {100 * enemy.Speed / 5.0f} %)";
        _enemyInfoText[3].text = enemy.Health.ToString();
        _enemyInfoText[4].text = enemy.Description;

        foreach(Image image in _enemyThumbnail)
        {
            image.sprite = enemy.Thumbnail;
        }

    }
    void UpdateTimerText()
    {
        _timerText.text = Managers.Instance.Flow.Timer.ToString("F2") + " s";
    }

    public void UpdateResultInfoPanel()
    {
        _resultText[1].text = Managers.Instance.Flow.GameResult.ClearTime.ToString("F2") + " s";
    }

    public void UpdateHpView(int maxHp, int currentHp)
    {
        StartCoroutine(AnimateBarAndText(maxHp, currentHp));
    }

    IEnumerator AnimateBarAndText(int maxHp, int currentHp)
    {
        float targetFill = (float)currentHp / maxHp;
        float currentFill = 0f;

        float duration = 0.5f; // 0.5초 진행
        float elapsedTime = 0f;

        float displayedText = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            currentFill = Mathf.Lerp(_hpBar.value, targetFill, elapsedTime / duration);
            displayedText = Mathf.Lerp(100 * _hpBar.value, 100 * targetFill, elapsedTime / duration);

            _hpBar.value = currentFill;
            _hpp.text = displayedText.ToString("F2") +" %";

            yield return null;
        }

        _hpBar.value = targetFill;
        _hpp.text = (100 * targetFill).ToString("F2") + " %";
    }
}
