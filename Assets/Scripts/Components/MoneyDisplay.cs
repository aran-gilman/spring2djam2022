using UnityEngine;
using UnityEngine.UI;

public class MoneyDisplay : MonoBehaviour
{
    public Text playerMoney;
    public Text amountDue;
    public Text daysRemaining;

    private PlayerState playerState;

    private void Awake()
    {
        playerState = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerState>();
    }

    private void Update()
    {
        playerMoney.text = $"{playerState.playerMoney}G";
        amountDue.text = $"Due:\n{playerState.amountDue}G";
        daysRemaining.text = $"{playerState.daysRemaining} Days";
    }
}
