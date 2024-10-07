using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class GameFlowManager
{
    GameState _gameState;

    int _maxEnemyCount;
    int _enemyCount;
    Enemy _currentEnemy;
    GameResult _gameResult;
    Vector3 _playerGenPosition;
    Vector3 _enemyGenPosition;

    public GameState State
    {
        get { return _gameState; }
        set { UpdateState(value); }
    }
    public GameResult GameResult { get { return _gameResult; } }
    public Enemy CurrentEnemy { get { return _currentEnemy; } }


    void UpdateState(GameState newState)
    {
        GameState oldValue = _gameState;
        _gameState = newState;
        switch (newState)
        {
            case GameState.Title:
                MoveToTitle();
                break;

            case GameState.Battle:
                if (oldValue == GameState.BattlePause)
                    Resume();
                else if (oldValue == GameState.Battle)
                    Restart();
                else
                    StartGame();
                break;

            case GameState.BattlePause:
                Pause();
                break;

            case GameState.BattleEnd:
                EndGame();
                break;
        }
        Managers.Instance.UI.UpdatePanel(newState);
    }

    // 게임 시작 시 한 번만 사용
    public void Initialize()
    {
        _gameState = GameState.Title;
        _maxEnemyCount = DataIO.EnemyDatas.Count;
        _enemyCount = 0;
        _playerGenPosition = new Vector3(-10f, -5.6f, 0f);
        _enemyGenPosition = new Vector3(10f, -5.6f, 0f);
    }
    public void InitializeLater()
    {
        GenPlayer();
    }

    // 주요 메서드
    public void StartGame()
    {
        Debug.Log($"{MethodBase.GetCurrentMethod().Name}()");
        GenNextEnemy();
    }
    public void GenPlayer()
    {
        GameObject PlayerGameObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Player"), _playerGenPosition, Quaternion.identity);
    }

    public void GenNextEnemy()
    {
        Debug.Log($"{MethodBase.GetCurrentMethod().Name}()");

        // 최대 적 수 이상이 될 경우 게임 종료
        if ( _enemyCount >= _maxEnemyCount)
        {
            State = GameState.BattleEnd;
            return;
        }

        // 새 적 생성 및 데이터 세팅
        _currentEnemy = DataIO.EnemyDatas[_enemyCount];
        GameObject EnemyGameObject = MonoBehaviour.Instantiate(_currentEnemy.GameObject, _enemyGenPosition, Quaternion.identity);

        Managers.Instance.Action.InvokeOnEnemyGen(); // UI 업데이트 등

        _enemyCount++;
    }
    public void EndGame()
    {
        Debug.Log($"{MethodBase.GetCurrentMethod().Name}()");
        SaveGameResult();
        ResetGameState();
    }
    public void Pause()
    {
        Debug.Log($"{MethodBase.GetCurrentMethod().Name}()");
    }
    public void Resume()
    {
        Debug.Log($"{MethodBase.GetCurrentMethod().Name}()");
    }
    public void Restart()
    {
        Debug.Log($"{MethodBase.GetCurrentMethod().Name}()");
    }
    public void MoveToTitle()
    {
        Debug.Log($"{MethodBase.GetCurrentMethod().Name}()");
    }
    // 보조 메서드
    public void ResetGameState()
    {
        Debug.Log($"{MethodBase.GetCurrentMethod().Name}()");
    }
    public void SaveGameResult()
    {
        Debug.Log($"{MethodBase.GetCurrentMethod().Name}()");
        _gameResult.ClearTime = 99.99f; // 임시지정
    }
}

public enum GameState
{
    Title = 0,
    Battle = 1,
    BattlePause = 2,
    BattleEnd = 3,
}

public struct GameResult
{
    public float ClearTime;
}