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
        _enemyInfoText[2].text = enemy.Speed.ToString("1");
        _enemyInfoText[3].text = enemy.Health.ToString();
        _enemyInfoText[4].text = enemy.Description;

        foreach(Image image in _enemyThumbnail)
        {
            image.sprite = enemy.Thumbnail;
        }

    }
    public void UpdateResultInfoPanel()
    {
        GameResult result = Managers.Instance.Flow.GameResult;
        _resultText[1].text = result.ClearTime.ToString("1");
    }
    public void UpdateHpView(int maxHp, int currentHp)
    {
        _hpBar.value = (float)currentHp / maxHp;
    }
}
