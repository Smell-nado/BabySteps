using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;

public class Player : MonoBehaviour {

    public static int TotalAmount;

    public Text totalAmountText;

    private UnityAction<int> totalChangeListener;

    void Awake()
    {
        totalChangeListener = new UnityAction<int>(AddAmountToTotal);
    }

    void OnEnable()
    {
        EventManager.StartListening("AddTotal", totalChangeListener);
    }

    void OnDisable()
    {
        EventManager.StopListening("AddTotal", totalChangeListener);
    }

    // Use this for initialization
    void Start ()
    {
        UpdateTotalAmount();
    }

    public void UpdateTotalAmount()
    {
        totalAmountText.text = "Total Amount: " + TotalAmount;
        EventManager.TriggerEvent("NewTotal", TotalAmount);
    }

    public void AddAmountToTotal (int amountToAdd)
    {
        TotalAmount += amountToAdd;
        UpdateTotalAmount();
    }
}
