using UnityEngine;
using UnityEngine.UI;

public class MoneyDisplay : MonoBehaviour
{
    public Text text;

    private PlayerState playerState;

    private void Awake()
    {
        playerState = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerState>();
    }

    private void Update()
    {
        text.text = $"{playerState.playerMoney}G";
    }
}
