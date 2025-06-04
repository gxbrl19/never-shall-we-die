using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLevel : MonoBehaviour
{
    public int _currentLevel;
    public int _currentXP;
    int[] xpTotal = { 0, 30, 90, 180, 300, 450, 630, 840, 1080, 1350, 1650, 1980, 2340, 2730, 3150, 3600, 4080, 4590, 5130, 5700, 6300 };

    void Start()
    {
        _currentXP = GameManager.instance._xp;
        _currentLevel = GameManager.instance._level;
        if (_currentLevel == 0) { _currentLevel = 1; }
    }

    public void GainXP(int xp)
    {
        _currentXP += xp;

        if (_currentXP >= xpTotal[_currentLevel])
        {
            LevelUp();
        }

        SetXP(_currentXP);
    }

    void LevelUp()
    {
        _currentLevel += 1;
        GameManager.instance._level += _currentLevel;
        GameManager.instance._upPoints += 1; //pontos usados para as melhorias
    }

    void SetXP(int xp)
    {
        GameManager.instance._xp = xp;
    }
}
