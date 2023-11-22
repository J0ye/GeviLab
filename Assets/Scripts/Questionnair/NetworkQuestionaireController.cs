using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public struct Answer
{
    public int question;
    public int giverID;
    public bool[] selections;
}

public class NetworkQuestionaireController : QuestionaireController
{

    protected List<Answer> answerGiven = new List<Answer>();
    protected PhotonView photonView;

    protected override void Start()
    {
        if(ConnectionManager.instance == null)
        {
            LogCreator.instance.AddLog("No Connection Manager; No Questionaire");
            Debug.LogError("No Connection Manager for " + gameObject.name + "; No Questionaire");
            Destroy(gameObject);
        }
        else
        {
            photonView = ConnectionManager.instance.photonView;
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
                val.question = count;
                val.selections = q.spawnedOptions.Select(o => o.selected).ToArray();

                print(val);

                photonView.RPC(nameof(DoCountAnswersGiven), RpcTarget.MasterClient, val);
                count++;
            }
        }
    }

    [PunRPC]
    public void DoCountAnswersGiven(Answer value)
    {
        int count = 0;
        foreach(Answer a in answerGiven)
        {
            if(a.giverID == value.giverID && a.question == value.question)
            {
                // Found existing entry
                answerGiven[count] = value; // Rewrite entry

                return; // Exit
            }
            count++;
        }

        answerGiven.Add(value);
    }
}
