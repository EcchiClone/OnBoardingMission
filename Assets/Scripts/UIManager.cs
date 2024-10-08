using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager
{
    string UICanvasPath = DataIO.Paths["UICanvas"];
    string DamageCanvasPath = DataIO.Paths["DamageCanvas"];
    string UICanvasParentName = "[Canvas]";
    GameObject UICanvas;
    UICanvasMap UIMap;
    public void Initialize()
    {
        UICanvas = MonoBehaviour.Instantiate(Resources.Load<GameObject>(UICanvasPath), GameObject.Find(UICanvasParentName).transform);
        UIMap = UICanvas.GetComponent<UICanvasMap>();
        UIMap.UpdateHpView(1, 1);
    }
    public void InitializeLater()
    {
        Managers.Instance.Action.OnEnemyGen += UpdateEnemyInfo;
        Managers.Instance.Action.OnEnemyGen += UpdateHpBar;

        Managers.Instance.Action.OnEndGame += UpdateResultPanel;

        UpdatePanel(Managers.Instance.Flow.State);
    }
    public void UpdatePanel(GameState state)
    {
        UIMap.TitlePanel.SetActive(state == GameState.Title ? true : false);
        UIMap.BattlePanel.SetActive(state == GameState.Title ? false : true);
        UIMap.PausePanel.SetActive(state == GameState.BattlePause ? true : false);
        UIMap.ResultPanel.SetActive(state == GameState.BattleEnd ? true : false);
    }
    public void UpdateEnemyInfo()
    {
        UIMap.UpdateEnemyInfoPanel();
    }
    public void UpdateHpBar(int maxHp, int currentHp)
    {
        UIMap.UpdateHpView(maxHp, currentHp);
    }
    public void UpdateResultPanel()
    {
        UIMap.UpdateResultInfoPanel();
    }
    public void UpdateHpBar()
    {
        UIMap.UpdateHpView(
            Managers.Instance.Flow.CurrentEnemyController.GetEnemyData().Health,
            Managers.Instance.Flow.CurrentEnemyController.GetCurrentHp() );
    }
    public void PopupDamageSkin(int damage, Transform enemyTransform)
    {
        GameObject DamageCanvas = MonoBehaviour.Instantiate(Resources.Load<GameObject>(DamageCanvasPath), enemyTransform.position + Vector3.up * 4f, Quaternion.identity);
        DamageCanvas.transform.SetParent(GameObject.Find(UICanvasParentName).transform);
        if(DamageCanvas.TryGetComponent(out DamageCanvas damageCanvas))
        {
            damageCanvas.SetDamageText(damage);
        }
    }
}
