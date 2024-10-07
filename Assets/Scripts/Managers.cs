using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    private static Managers _instance;
    private GameFlowManager _Flow;
    private UIManager _UI;
    private ActionManager _Action;
    public static Managers Instance { get { return _instance; } } // 현재, _instance의 참조가 없는 경우는 대응하지 않음
    public GameFlowManager Flow { get { return _Flow; } }
    public UIManager UI { get { return _UI; } }
    public ActionManager Action { get { return _Action; } }

    string EnemyDataPath = "Assets/Resources/Datas/MonsterData.csv";


    private void Awake()
    {
        if (_instance == null) { _instance = this; DontDestroyOnLoad(gameObject); }
        else { Destroy(this); return; }

        InitializeGame();
    }
    public void InitializeGame()
    {
        DataIO.CsvParseEnemy(EnemyDataPath);

        _instance._Action = new ActionManager();

        _instance._Flow = new GameFlowManager();
        _Flow.Initialize();

        _instance._UI = new UIManager();
        _UI.Initialize();

        _Flow.InitializeLater();
        _UI.InitializeLater();

    }
}
