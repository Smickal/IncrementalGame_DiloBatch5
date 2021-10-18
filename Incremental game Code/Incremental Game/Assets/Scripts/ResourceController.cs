using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceController : MonoBehaviour
{
   
    public Text resourceDescription;
    public Text resourceUpgradeCost;
    public Text resourceUnlockCost;
    public Button resourceButton;
    public Image ResourceImage;

    private ResourceConfig _config;

    int _level = 1;
    public bool IsUnlocked{get; private set;}

    private void Start() {
        resourceButton.onClick.AddListener (() =>
        {
            if (IsUnlocked)
            {
                UpgradeLevel ();
            }
            else
            {
                UnlockResource ();
            }
        });
    }

    public void SetConfig(ResourceConfig config)
    {
        _config = config;

        resourceDescription.text = $"{_config.name}\nLv. {_level}\n";
        resourceUnlockCost.text = "Unlock Cost\n"+ _config.unlockCost;
        resourceUpgradeCost.text = "Upgrade Cost\n" + GetUpgradeCost();

        SetUnlocked(_config.unlockCost == 0);
    }
    
    public double GetOutput()
    {
        return _config.output * _level;
    }

    public double GetUpgradeCost()
    {
        return _config.UpgradeCost * _level;
    }

    public double GetUnlockCost()
    {
        return _config.unlockCost;
    }

    public void UpgradeLevel()
    {
        Debug.Log("Total Gold =" + GameManager.Instance.TotalGold  );
        double upgradeCost = GetUpgradeCost();
        Debug.Log("uipgrade cost"  + upgradeCost);
        if (GameManager.Instance.TotalGold < upgradeCost)
        {
            return;
        }

        GameManager.Instance.AddGold (-upgradeCost);
        _level++;
        resourceUpgradeCost.text = $"Upgrade Cost\n{ GetUpgradeCost() }";
        resourceDescription.text = $"{ _config.name } Lv. { _level }\n+{ GetOutput ().ToString ("0") }";
    }

    public void UnlockResource ()
    {
        double unlockCost = GetUnlockCost ();
        if (GameManager.Instance.TotalGold < unlockCost)
        {
            return;
        }
 
        SetUnlocked (true);
        GameManager.Instance.ShowNextResource ();
        AchievementController.Instance.UnlockAchievement(AchievementType.UnlockResource, _config.name);
    }

 
    public void SetUnlocked (bool unlocked)
    {
        IsUnlocked = unlocked;
        ResourceImage.color = IsUnlocked ? Color.white : Color.grey;
        resourceUnlockCost.gameObject.SetActive (!unlocked);
        resourceUpgradeCost.gameObject.SetActive (unlocked);
    }

}


