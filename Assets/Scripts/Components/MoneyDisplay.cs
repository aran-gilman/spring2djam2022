using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyDisplay : MonoBehaviour
{
    public Text playerMoney;
    public GameObject notificationContainer;

    public GameObject moneyEarnedPrefab;
    public GameObject moneySpentPrefab;

    public float secondsUntilNotificationFade = 4.0f;
    public float notificationFadeDuration = 1.0f;

    public void ShowChangeNotification(int amountChanged)
    {
        GameObject go;
        string changePrefix = "";
        if (amountChanged < 0)
        {
            go = Instantiate(moneySpentPrefab, notificationContainer.transform);
        }
        else
        {
            go = Instantiate(moneyEarnedPrefab, notificationContainer.transform);
            changePrefix = "+";
        }
        go.GetComponentInChildren<Text>().text = $"{changePrefix}{amountChanged}G";

        StartCoroutine(FadeNotification(go, secondsUntilNotificationFade, notificationFadeDuration));
    }

    private PlayerState playerState;

    private int previousMoney = -1;

    private void Awake()
    {
        playerState = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerState>();
        previousMoney = playerState.playerMoney;
    }

    private void Update()
    {
        playerMoney.text = $"{playerState.playerMoney}G";

        int diff = playerState.playerMoney - previousMoney;
        if (diff != 0)
        {
            ShowChangeNotification(diff);
        }
        previousMoney = playerState.playerMoney;
    }

    private IEnumerator FadeNotification(GameObject notification, float timeUntilFade, float fadeDuration)
    {
        yield return new WaitForSeconds(timeUntilFade);

        Text notificationText = notification.GetComponentInChildren<Text>();
        Image notificationBackground = notification.GetComponent<Image>();

        for (float i = fadeDuration; i > 0; i -= Time.deltaTime)
        {
            float alpha = i / notificationFadeDuration;
            notificationText.color = new Color(notificationText.color.r, notificationText.color.g, notificationText.color.b, alpha);
            notificationBackground.color = new Color(notificationBackground.color.r, notificationBackground.color.g, notificationBackground.color.b, alpha);
            yield return null;
        }

        Destroy(notification);
    }
}
