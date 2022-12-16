using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using TMPro;

public class QuestWindow : MonoBehaviour
{
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private GameObject goalPrefab;
    [SerializeField] private Transform goalsContent;
    [SerializeField] private TMP_Text xpText;
    [SerializeField] private TMP_Text coinsText;

    public void Initialize(Quest quest)
    {
        titleText.text = quest.Information.Name;
        descriptionText.text = quest.Information.Description;

        foreach (var goal in quest.Goals)
        {
            GameObject goalObj = Instantiate(goalPrefab, goalsContent);
            goalObj.transform.Find("Text").GetComponent<Text>().text = goal.GetDescription();

            GameObject countObj = goalObj.transform.Find("Count").gameObject;
            GameObject skipObj = goalObj.transform.Find("Skip").gameObject;

            if (goal.Completed)
            {
                countObj.SetActive(false);
                skipObj.SetActive(false);
                goalObj.transform.Find("Done").gameObject.SetActive(true);
            }
            else
            {
                countObj.GetComponent<TMP_Text>().text = goal.CurrentAmount + "/" + goal.RequiredAmount;

                skipObj.GetComponent<Button>().onClick.AddListener(delegate
                {
                    goal.Skip();

                    countObj.SetActive(false);
                    skipObj.SetActive(false);
                    goalObj.transform.Find("Done").gameObject.SetActive(true);
                });
            }
        }

        xpText.text = quest.Reward.XP.ToString();
        coinsText.text = quest.Reward.Currency.ToString();
    }

    public void CloseWindow()
    {
        gameObject.SetActive(false);

        for (int i = 0; i < goalsContent.childCount; i++)
        {
            Destroy(goalsContent.GetChild(i).gameObject);
        }
    }
}
