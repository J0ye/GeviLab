using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionaireController : MonoBehaviour
{
    public List<QuestionController> questions = new List<QuestionController>();
    public bool closeOnStart = true;

    protected int activeQuestion = 0;
    protected bool state = false;
    // Start is called before the first frame update
    void Start()
    {
        LoadInputFromSessionData();
        Session.Instance().ClearAllLoginEvents();
        Session.Instance().OnLogin += ReactToLogin;
    }

    public void ChangeDisplayState()
    {
        if(state)
        {
            Close();
        } else
        {
            Open();
        }
    }

    public void Save()
    {
        Questionaire questionaire = new Questionaire(0, questions);
        questionaire.SaveToSession();
    }
    public void LoadInputFromSessionData()
    {
        OpenAll();
        if (Session.CheckUser() && Session.Instance().questionair.questions.Length > 0)
        {
            Question[] fromMemory = Session.Instance().questionair.questions;
            for (int i = 0; i < fromMemory.Length; i++)
            {
                if (i < questions.Count)
                {
                    questions[i].LoadInputFromSession(fromMemory[i]);
                }
            }
        }
        Close();
        if (!closeOnStart) Open();
    }

    public void ReactToLogin(object sender, EventArgs e)
    {
        LoadInputFromSessionData();
    }


    /// <summary>
    /// Creates listener for the select event of selectable options.
    /// Has a delay because the seleactable option are getting created at the same time.
    /// </summary>
    /// <returns></returns>
    public IEnumerator RegisterSaveAction()
    {
        yield return new WaitForSeconds(0.1f);
        /*Debug.Log("Questions: " + questions.Count);
        foreach(QuestionController quest in questions)
        {
            Debug.Log("Options: " + quest.spawnedOptions.Count);
            foreach (SelectionOption so in quest.spawnedOptions)
            {
                so.OnSelect.AddListener(() => Save());
                Debug.Log("Registered save on: " + so.name);
            }
        }*/
    }

    /// <summary>
    /// Display every child obejct that isnt a question, but the first question.
    /// This allows extra elements like buttons to be opend and closed as well.
    /// </summary>
    public void Open()
    {
        // Open all children of questionaire
        OpenAll();

        // Close all questions but the first
        foreach(QuestionController qc in questions)
        {
            if(qc == questions[0])
            {
                questions[0].gameObject.SetActive(true);
                questions[0].Open();
                activeQuestion = 0;
            }
            else
            {
                qc.gameObject.SetActive(false);
            }
        }
        state = true;
    }

    public void OpenAll()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
        state = true;
    }

    public void Close()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
        state = false;
    }

    public void Next()
    {
        Save();
        if (activeQuestion < questions.Count-1)
        {
            questions[activeQuestion].Close();
            activeQuestion++;
            activeQuestion = Mathf.Clamp(activeQuestion, 0, questions.Count - 1);
            questions[activeQuestion].Open();
        }
    }

    public void Previous()
    {
        Save();
        if (activeQuestion > 0)
        {
            questions[activeQuestion].Close();
            activeQuestion--;
            activeQuestion = Mathf.Clamp(activeQuestion, 0, questions.Count-1);
            questions[activeQuestion].Open();
        }
    }
}

[Serializable]
public class Questionaire
{
    public int id;
    public Question[] questions;

    public Questionaire()
    {
        id = 0;
        questions = new Question[0];
    }

    public Questionaire(int newID, List<QuestionController> controllers)
    {
        id = newID;

        questions = new Question[0];
        foreach(QuestionController qc in controllers)
        {
            Question[] temp = new Question[questions.Length + 1];
            for(int i = 0; i < questions.Length; i++)
            {
                temp[i] = questions[i];
            }
            temp[questions.Length] = qc.GetQuestion();
            questions = temp;
        }
    }

    public void SaveToGlobal()
    {
        foreach(Questionaire q in global.Instance().questionaires)
        {
            if(q.id == id)
            {
                q.questions = questions;
                return;
            }
        }
        global.Instance().questionaires.Add(this);
    }

    public void SaveToSession()
    {
        Session.Instance().questionair = this;
    }
}
