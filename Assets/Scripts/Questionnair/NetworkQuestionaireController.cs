using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;


public class Answer
{
    public int question;
    public string giverName;
    public int giverID;
    public bool[] selections;

    public string ToJSON()
    {
        return JsonUtility.ToJson(this);
    }

    public static Answer FromJSON(string jsonString)
    {
        return JsonUtility.FromJson<Answer>(jsonString);
    }
}

public class NetworkQuestionaireController : QuestionaireController
{
    protected List<Answer> answerGiven = new List<Answer>();
    protected PhotonView photonView;

    protected override void Start()
    {
        if(!TryGetComponent<PhotonView>(out photonView))
        {
            LogCreator.instance.AddLog("No PhotonView; No Questionaire");
            Debug.LogError("No PhotonView for " + gameObject.name + "; No Questionaire");
            Destroy(gameObject);
        }

        base.Start();
    }

    public override void Save()
    {
        base.Save();

        if(!PhotonNetwork.IsMasterClient)
        {
            int count = 0;
            foreach (QuestionController q in questions)
            {
                Answer val = new Answer();
                val.giverID = photonView.ViewID;
                val.giverName = PhotonNetwork.NickName;
                val.question = count;
                val.selections = q.spawnedOptions.Select(o => o.selected).ToArray();

                //print(val);

                photonView.RPC(nameof(DoCountAnswersGiven), RpcTarget.MasterClient, val.ToJSON());
                count++;
            }
        }
    }

    [PunRPC]
    public void DoCountAnswersGiven(string answerAsJSON)
    {
        //print(answerAsJSON);
        Answer value = Answer.FromJSON(answerAsJSON);
        int count = 0;
        foreach(Answer a in answerGiven)
        {
            if(a.giverID == value.giverID && a.question == value.question)
            {
                // Found existing entry
                answerGiven[count] = value; // Rewrite entry
                WriteAnswersGiven();

                return; // Exit
            }
            count++;
        }
        answerGiven.Add(value);
        WriteAnswersGiven();
    }

    protected void WriteAnswersGiven()
    {
        int questionCount = 0;
        // Go through every question in this questionaire
        foreach(QuestionController qc in questions)
        {
            int optionCount = 0;
            // Go through each option in this question
            foreach (SelectionOption option in qc.spawnedOptions)
            {
                // got through each child of this selection 
                foreach(Transform t in option.transform)
                {
                    // Only work with children that are called "Extra"
                    if(t.gameObject.name == "Extra")
                    {
                        t.GetComponent<Text>().text = FindAnswersGiven(questionCount, optionCount);
                        /*print("Writing to " + qc.name + " selection " + option.name + ": " + t.GetComponent<Text>().text);*/
                    }
                }
                optionCount++;
            }
            questionCount++;
        }
    }

    protected string FindAnswersGiven(int questionNumber, int optionNumber)
    {
        string ret = "";

        foreach(Answer a in answerGiven)
        {
            if(a.question == questionNumber && a.selections[optionNumber])
            {
                ret += " ";
                ret += a.giverName;
            }
        }

        return ret;
    }
}


