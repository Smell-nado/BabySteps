using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Main : MonoBehaviour {

    public int[] gen1Reward;
    public int totalAmount;
    public int chestValue;

    public int gen1Time;
    public int gen1Timer;

    public Text chestText;
    public Text totalAmountText;
    public Text timer;
    public Text upgradeText;
    public Text chestLevelText;
    public Text payoutButtonText;

    public Button button;
    public Button upgradeButton;

    int genCurrentLevel;
    int[] upgradeCostArray;

    // Use this for initialization
    void Start () {

        gen1Reward = new int[] { 1, 2, 5, 8, 15 };
        upgradeCostArray = new int[] { 5, 18, 42, 100 };

        totalAmount = 0;

        gen1Time = 3;
        gen1Timer = gen1Time;
        genCurrentLevel = 1;

        timer.text = "Time Remaining: " + gen1Time;
        upgradeAllText();

        CheckIfUpgradeable();
    }

    public void PressButton()
    {
        button.interactable = false;

        totalAmount += chestValue;
        CheckIfUpgradeable();
        chestValue = 0;
        upgradeAllText();

        StartCoroutine("GenStart");
    }

    IEnumerator GenStart()
    {

        yield return new WaitForSeconds(1);
        gen1Timer--;
        timer.text = "Time Remaining: " + gen1Timer;

        if (gen1Timer <= 0)
        {
            EndTimer();
        }
        else
        {
            StartCoroutine("GenStart");
        }
           
    }

    public void EndTimer()
    {
        chestValue += gen1Reward[genCurrentLevel - 1];
        upgradeAllText();

        gen1Timer = gen1Time;
        timer.text = "Time Remainging: " + gen1Time;

        StopCoroutine("GenStart");
        CheckIfUpgradeable();
        button.interactable = true;

    }

    public void UpgradeButtonPress()
    {
        genCurrentLevel++;

        upgradeAllText();

        CheckIfUpgradeable();

    }

    public void CheckIfUpgradeable()
    {
        if (totalAmount >= upgradeCostArray[genCurrentLevel - 1])
        {
            upgradeButton.interactable = true;
        }
        else
        {
            upgradeButton.interactable = false;
        }
    }

    //All Timer Stuff should be separate
    public void upgradeAllText()
    {
        chestText.text = "" + chestValue;
        totalAmountText.text = "Total Amount: " + Player.TotalAmount;
        upgradeText.text = "▲ " + upgradeCostArray[genCurrentLevel - 1] + " ▲";
        chestLevelText.text = "Chest LVL: " + genCurrentLevel;
        payoutButtonText.text = "Payout: " + gen1Reward[genCurrentLevel - 1];
    }
}
