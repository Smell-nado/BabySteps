using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;

public class Generator : MonoBehaviour {

    int _totalAmount;

    //Unlock
    public bool _isUnlocked;
    public int _unlockAmount;
    public bool _unlockMiscCondition;

    //Level
    int _genLevel = 1;
    [SerializeField] int[] _genUpgradeCost;
    
    //Payout
    int _genPayoutAmount;
    [SerializeField] int[] _genPayoutArray;
    public int genPayoutFactor = 1;
    //generator storage amount
    int _genStorageAmount;

    //Timer
    [SerializeField] int[] _genTimeArray;
    int _genTimer;
    public int _genTimerFactor = 1;

    //a bunch of text variables
    public Text storageAmountText;
    public Text timerText;
    public Text upgradeText;
    public Text genLevelText;
    public Text payoutText;
    public Text unlockText;

    //some button variables
    public Button generateButton;
    public Button upgradeButton;
    public Button unlockButton;

    private UnityAction<int> newTotalListener;

    void Awake()
    {
        newTotalListener = new UnityAction<int>(NewTotalAmount);
    }

    void OnEnable()
    {
        EventManager.StartListening("NewTotal", newTotalListener);
    }

    void OnDisable()
    {
        EventManager.StopListening("NewTotal", newTotalListener);
    }

    // Use this for initialization
    void Start () {
        _genTimer = _genTimeArray[_genLevel - 1];

        timerText.text = "Time Remaining: " + _genTimer;
        unlockText.text = "Unlock: " + _unlockAmount;
        UpdateText();
        UnlockGenerator();

        CheckIfUpgradeable();
    }
	
	public void PressGenerateButton()
    {
        generateButton.interactable = false;

        EventManager.TriggerEvent("AddTotal", _genStorageAmount);
        CheckIfUpgradeable();
        _genStorageAmount = 0;
        UpdateText();

        StartCoroutine("GenStart");
    }

    IEnumerator GenStart()
    {
        yield return new WaitForSeconds(1);
        _genTimer--;
        timerText.text = "Time Remaining: " + _genTimer;

        if (_genTimer <= 0)
        {
            GenEnd();
        }
        else
        {
            StartCoroutine("GenStart");
        }
    }

    public void GenEnd()
    {
        _genStorageAmount += _genPayoutArray[_genLevel - 1];
        UpdateText();

        _genTimer = _genTimeArray[_genLevel - 1];
        timerText.text = "Time Remaining: " + _genTimer;

        StopCoroutine("GenStart");
        generateButton.interactable = true;
    }

    public void PressUpgradeButton()
    {
        EventManager.TriggerEvent("AddTotal", -_genUpgradeCost[_genLevel - 1]);
        _genLevel++;
        UpdateText();

    }

    public void CheckIfUpgradeable()
    {
        if (_totalAmount >= _genUpgradeCost[_genLevel - 1])//this should be done as an event
        {
            upgradeButton.interactable = true;
        }
        else
        {
            upgradeButton.interactable = false;
        }
    }

    public void UpdateText()
    {
        storageAmountText.text = "" + _genStorageAmount;
        upgradeText.text = "▲ " + _genUpgradeCost[_genLevel - 1] + " ▲";
        genLevelText.text = "LVL: " + _genLevel;
        payoutText.text = "Generate " + _genPayoutArray[_genLevel - 1] + " Points";

    }

    public void PressUnlockButton()
    {
        EventManager.TriggerEvent("AddTotal", -_unlockAmount);
        unlockButton.gameObject.SetActive(false);
    }

    public void NewTotalAmount(int newAmount)
    {
        _totalAmount = newAmount;
        CheckIfUpgradeable();
        UnlockGenerator();
    }

    public void UnlockGenerator()
    {
        if (!_isUnlocked && _totalAmount >= _unlockAmount && _unlockMiscCondition)
        {
            unlockButton.interactable = true;
            unlockText.text = "Tap to Unlock\nCost: " + _unlockAmount + " Points";
        }
        else
        {
            unlockButton.interactable = false;
            unlockText.text = "Unlock: " + _unlockAmount;
        }
    }
}
