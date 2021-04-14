using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using FullSerializer;
using Proyecto26;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Assets
{
    public class MCQCreation : MonoBehaviour
    {
        public InputField getQuestion;
        public InputField getOptions;
        public InputField getAnswer;
        public Dropdown stageSelection;
        public Dropdown substageSelection;
        public Dropdown questionTypeSelection;


        private string databaseURL = "https://semaindb-default-rtdb.firebaseio.com/Questions/";
        private string qnNoDBURL = "https://semaindb-default-rtdb.firebaseio.com/MCQuestionNo/";
        private string AuthKey = "AIzaSyBqMbMl_ZIV17atZXrssFCnJERYpoffu8s";
        private string teacherPassword = "adminSE";
        private string teacherEmail = "admin@SE.com";

        public static fsSerializer serializer = new fsSerializer();

        public static string question;
        public static string answer;
        public static string options;


        public static string s1ss1;
        public static string s1ss2;
        public static string s1ss3;

        public static string s2ss1;
        public static string s2ss2;
        public static string s2ss3;

        public static string s3ss1;
        public static string s3ss2;
        public static string s3ss3;

        public static string s4ss1;
        public static string s4ss2;
        public static string s4ss3;

        public static string s5ss1;
        public static string s5ss2;
        public static string s5ss3;



        private string idToken;
        public static string localId;


        QuestionNumber questionNo = new QuestionNumber();

        private void Start()
        {

            string userData = "{\"email\":\"" + teacherEmail + "\",\"password\":\"" + teacherPassword + "\",\"returnSecureToken\":true}";
            RestClient.Post<SignResponse>("https://www.googleapis.com/identitytoolkit/v3/relyingparty/verifyPassword?key=" + AuthKey, userData).Then(
                response =>
                {
                    idToken = response.idToken;
                    localId = response.localId;
                }).Catch(error =>
                {
                    Debug.Log(error);
                });

        }
        private void RetrieveFromDatabase()
        {

            RestClient.Get<QuestionNumber>(qnNoDBURL + ".json?auth=" + idToken).Then(response =>
            {
                questionNo = response;
                PostToDatabase();


            });


        }

        private void PostToDatabase()
        {
            int stageSel = stageSelection.value + 1;
            int substageSel = substageSelection.value + 1;
            int questionTypeVal = questionTypeSelection.value;

            string questionNumberPlaceholder = null;
            QuestionNumber qNo = new QuestionNumber();

            if (stageSel == 1)
            {
                if (substageSel == 1)
                {
                    qNo.s1ss1 = questionNo.s1ss1;
                    qNo.s1ss2 = questionNo.s1ss2;
                    qNo.s1ss3 = questionNo.s1ss3;

                    qNo.s2ss1 = questionNo.s2ss1;
                    qNo.s2ss2 = questionNo.s2ss2;
                    qNo.s2ss3 = questionNo.s2ss3;

                    qNo.s3ss1 = questionNo.s3ss1;
                    qNo.s3ss2 = questionNo.s3ss2;
                    qNo.s3ss3 = questionNo.s3ss3;

                    qNo.s4ss1 = questionNo.s4ss1;
                    qNo.s4ss2 = questionNo.s4ss2;
                    qNo.s4ss3 = questionNo.s4ss3;

                    qNo.s5ss1 = questionNo.s5ss1;
                    qNo.s5ss2 = questionNo.s5ss2;
                    qNo.s5ss3 = questionNo.s5ss3;

                    questionNumberPlaceholder = questionNo.s1ss1;
                    qNo.s1ss1 = (short.Parse(questionNumberPlaceholder) + 1).ToString();


                }

                else if (substageSel == 2)
                {
                    qNo.s1ss1 = questionNo.s1ss1;
                    qNo.s1ss2 = questionNo.s1ss2;
                    qNo.s1ss3 = questionNo.s1ss3;

                    qNo.s2ss1 = questionNo.s2ss1;
                    qNo.s2ss2 = questionNo.s2ss2;
                    qNo.s2ss3 = questionNo.s2ss3;

                    qNo.s3ss1 = questionNo.s3ss1;
                    qNo.s3ss2 = questionNo.s3ss2;
                    qNo.s3ss3 = questionNo.s3ss3;

                    qNo.s4ss1 = questionNo.s4ss1;
                    qNo.s4ss2 = questionNo.s4ss2;
                    qNo.s4ss3 = questionNo.s4ss3;

                    qNo.s5ss1 = questionNo.s5ss1;
                    qNo.s5ss2 = questionNo.s5ss2;
                    qNo.s5ss3 = questionNo.s5ss3;

                    questionNumberPlaceholder = questionNo.s1ss2;
                    qNo.s1ss2 = (short.Parse(questionNumberPlaceholder) + 1).ToString();
                }

                else if (substageSel == 3)
                {
                    qNo.s1ss1 = questionNo.s1ss1;
                    qNo.s1ss2 = questionNo.s1ss2;
                    qNo.s1ss3 = questionNo.s1ss3;

                    qNo.s2ss1 = questionNo.s2ss1;
                    qNo.s2ss2 = questionNo.s2ss2;
                    qNo.s2ss3 = questionNo.s2ss3;

                    qNo.s3ss1 = questionNo.s3ss1;
                    qNo.s3ss2 = questionNo.s3ss2;
                    qNo.s3ss3 = questionNo.s3ss3;

                    qNo.s4ss1 = questionNo.s4ss1;
                    qNo.s4ss2 = questionNo.s4ss2;
                    qNo.s4ss3 = questionNo.s4ss3;

                    qNo.s5ss1 = questionNo.s5ss1;
                    qNo.s5ss2 = questionNo.s5ss2;
                    qNo.s5ss3 = questionNo.s5ss3;

                    questionNumberPlaceholder = questionNo.s1ss3;
                    qNo.s1ss3 = (short.Parse(questionNumberPlaceholder) + 1).ToString();
                }

            }

            else if (stageSel == 2)
            {
                if (substageSel == 1)
                {
                    qNo.s1ss1 = questionNo.s1ss1;
                    qNo.s1ss2 = questionNo.s1ss2;
                    qNo.s1ss3 = questionNo.s1ss3;

                    qNo.s2ss1 = questionNo.s2ss1;
                    qNo.s2ss2 = questionNo.s2ss2;
                    qNo.s2ss3 = questionNo.s2ss3;

                    qNo.s3ss1 = questionNo.s3ss1;
                    qNo.s3ss2 = questionNo.s3ss2;
                    qNo.s3ss3 = questionNo.s3ss3;

                    qNo.s4ss1 = questionNo.s4ss1;
                    qNo.s4ss2 = questionNo.s4ss2;
                    qNo.s4ss3 = questionNo.s4ss3;

                    qNo.s5ss1 = questionNo.s5ss1;
                    qNo.s5ss2 = questionNo.s5ss2;
                    qNo.s5ss3 = questionNo.s5ss3;

                    questionNumberPlaceholder = questionNo.s2ss1;
                    qNo.s2ss1 = (short.Parse(questionNumberPlaceholder) + 1).ToString();
                }

                else if (substageSel == 2)
                {
                    qNo.s1ss1 = questionNo.s1ss1;
                    qNo.s1ss2 = questionNo.s1ss2;
                    qNo.s1ss3 = questionNo.s1ss3;

                    qNo.s2ss1 = questionNo.s2ss1;
                    qNo.s2ss2 = questionNo.s2ss2;
                    qNo.s2ss3 = questionNo.s2ss3;

                    qNo.s3ss1 = questionNo.s3ss1;
                    qNo.s3ss2 = questionNo.s3ss2;
                    qNo.s3ss3 = questionNo.s3ss3;

                    qNo.s4ss1 = questionNo.s4ss1;
                    qNo.s4ss2 = questionNo.s4ss2;
                    qNo.s4ss3 = questionNo.s4ss3;

                    qNo.s5ss1 = questionNo.s5ss1;
                    qNo.s5ss2 = questionNo.s5ss2;
                    qNo.s5ss3 = questionNo.s5ss3;
                    questionNumberPlaceholder = questionNo.s2ss2;
                    qNo.s2ss2 = (short.Parse(questionNumberPlaceholder) + 1).ToString();
                }

                else if (substageSel == 3)
                {
                    qNo.s1ss1 = questionNo.s1ss1;
                    qNo.s1ss2 = questionNo.s1ss2;
                    qNo.s1ss3 = questionNo.s1ss3;

                    qNo.s2ss1 = questionNo.s2ss1;
                    qNo.s2ss2 = questionNo.s2ss2;
                    qNo.s2ss3 = questionNo.s2ss3;

                    qNo.s3ss1 = questionNo.s3ss1;
                    qNo.s3ss2 = questionNo.s3ss2;
                    qNo.s3ss3 = questionNo.s3ss3;

                    qNo.s4ss1 = questionNo.s4ss1;
                    qNo.s4ss2 = questionNo.s4ss2;
                    qNo.s4ss3 = questionNo.s4ss3;

                    qNo.s5ss1 = questionNo.s5ss1;
                    qNo.s5ss2 = questionNo.s5ss2;
                    qNo.s5ss3 = questionNo.s5ss3;

                    questionNumberPlaceholder = questionNo.s2ss3;
                    qNo.s2ss3 = (short.Parse(questionNumberPlaceholder) + 1).ToString();
                }
            }

            else if (stageSel == 3)
            {
                if (substageSel == 1)
                {
                    qNo.s1ss1 = questionNo.s1ss1;
                    qNo.s1ss2 = questionNo.s1ss2;
                    qNo.s1ss3 = questionNo.s1ss3;

                    qNo.s2ss1 = questionNo.s2ss1;
                    qNo.s2ss2 = questionNo.s2ss2;
                    qNo.s2ss3 = questionNo.s2ss3;

                    qNo.s3ss1 = questionNo.s3ss1;
                    qNo.s3ss2 = questionNo.s3ss2;
                    qNo.s3ss3 = questionNo.s3ss3;

                    qNo.s4ss1 = questionNo.s4ss1;
                    qNo.s4ss2 = questionNo.s4ss2;
                    qNo.s4ss3 = questionNo.s4ss3;

                    qNo.s5ss1 = questionNo.s5ss1;
                    qNo.s5ss2 = questionNo.s5ss2;
                    qNo.s5ss3 = questionNo.s5ss3;

                    questionNumberPlaceholder = questionNo.s3ss1;
                    qNo.s3ss1 = (short.Parse(questionNumberPlaceholder) + 1).ToString();
                }

                else if (substageSel == 2)
                {
                    qNo.s1ss1 = questionNo.s1ss1;
                    qNo.s1ss2 = questionNo.s1ss2;
                    qNo.s1ss3 = questionNo.s1ss3;

                    qNo.s2ss1 = questionNo.s2ss1;
                    qNo.s2ss2 = questionNo.s2ss2;
                    qNo.s2ss3 = questionNo.s2ss3;

                    qNo.s3ss1 = questionNo.s3ss1;
                    qNo.s3ss2 = questionNo.s3ss2;
                    qNo.s3ss3 = questionNo.s3ss3;

                    qNo.s4ss1 = questionNo.s4ss1;
                    qNo.s4ss2 = questionNo.s4ss2;
                    qNo.s4ss3 = questionNo.s4ss3;

                    qNo.s5ss1 = questionNo.s5ss1;
                    qNo.s5ss2 = questionNo.s5ss2;
                    qNo.s5ss3 = questionNo.s5ss3;

                    questionNumberPlaceholder = questionNo.s3ss2;
                    qNo.s3ss2 = (short.Parse(questionNumberPlaceholder) + 1).ToString();
                }

                else if (substageSel == 3)
                {
                    qNo.s1ss1 = questionNo.s1ss1;
                    qNo.s1ss2 = questionNo.s1ss2;
                    qNo.s1ss3 = questionNo.s1ss3;

                    qNo.s2ss1 = questionNo.s2ss1;
                    qNo.s2ss2 = questionNo.s2ss2;
                    qNo.s2ss3 = questionNo.s2ss3;

                    qNo.s3ss1 = questionNo.s3ss1;
                    qNo.s3ss2 = questionNo.s3ss2;
                    qNo.s3ss3 = questionNo.s3ss3;

                    qNo.s4ss1 = questionNo.s4ss1;
                    qNo.s4ss2 = questionNo.s4ss2;
                    qNo.s4ss3 = questionNo.s4ss3;

                    qNo.s5ss1 = questionNo.s5ss1;
                    qNo.s5ss2 = questionNo.s5ss2;
                    qNo.s5ss3 = questionNo.s5ss3;

                    questionNumberPlaceholder = questionNo.s3ss3;
                    qNo.s3ss3 = (short.Parse(questionNumberPlaceholder) + 1).ToString();
                }
            }

            else if (stageSel == 4)
            {
                if (substageSel == 1)
                {
                    qNo.s1ss1 = questionNo.s1ss1;
                    qNo.s1ss2 = questionNo.s1ss2;
                    qNo.s1ss3 = questionNo.s1ss3;

                    qNo.s2ss1 = questionNo.s2ss1;
                    qNo.s2ss2 = questionNo.s2ss2;
                    qNo.s2ss3 = questionNo.s2ss3;

                    qNo.s3ss1 = questionNo.s3ss1;
                    qNo.s3ss2 = questionNo.s3ss2;
                    qNo.s3ss3 = questionNo.s3ss3;

                    qNo.s4ss1 = questionNo.s4ss1;
                    qNo.s4ss2 = questionNo.s4ss2;
                    qNo.s4ss3 = questionNo.s4ss3;

                    qNo.s5ss1 = questionNo.s5ss1;
                    qNo.s5ss2 = questionNo.s5ss2;
                    qNo.s5ss3 = questionNo.s5ss3;

                    questionNumberPlaceholder = questionNo.s4ss1;
                    qNo.s4ss1 = (short.Parse(questionNumberPlaceholder) + 1).ToString();
                }

                else if (substageSel == 2)
                {
                    qNo.s1ss1 = questionNo.s1ss1;
                    qNo.s1ss2 = questionNo.s1ss2;
                    qNo.s1ss3 = questionNo.s1ss3;

                    qNo.s2ss1 = questionNo.s2ss1;
                    qNo.s2ss2 = questionNo.s2ss2;
                    qNo.s2ss3 = questionNo.s2ss3;

                    qNo.s3ss1 = questionNo.s3ss1;
                    qNo.s3ss2 = questionNo.s3ss2;
                    qNo.s3ss3 = questionNo.s3ss3;

                    qNo.s4ss1 = questionNo.s4ss1;
                    qNo.s4ss2 = questionNo.s4ss2;
                    qNo.s4ss3 = questionNo.s4ss3;

                    qNo.s5ss1 = questionNo.s5ss1;
                    qNo.s5ss2 = questionNo.s5ss2;
                    qNo.s5ss3 = questionNo.s5ss3;

                    questionNumberPlaceholder = questionNo.s4ss2;
                    qNo.s4ss2 = (short.Parse(questionNumberPlaceholder) + 1).ToString();
                }

                else if (substageSel == 3)
                {
                    qNo.s1ss1 = questionNo.s1ss1;
                    qNo.s1ss2 = questionNo.s1ss2;
                    qNo.s1ss3 = questionNo.s1ss3;

                    qNo.s2ss1 = questionNo.s2ss1;
                    qNo.s2ss2 = questionNo.s2ss2;
                    qNo.s2ss3 = questionNo.s2ss3;

                    qNo.s3ss1 = questionNo.s3ss1;
                    qNo.s3ss2 = questionNo.s3ss2;
                    qNo.s3ss3 = questionNo.s3ss3;

                    qNo.s4ss1 = questionNo.s4ss1;
                    qNo.s4ss2 = questionNo.s4ss2;
                    qNo.s4ss3 = questionNo.s4ss3;

                    qNo.s5ss1 = questionNo.s5ss1;
                    qNo.s5ss2 = questionNo.s5ss2;
                    qNo.s5ss3 = questionNo.s5ss3;

                    questionNumberPlaceholder = questionNo.s4ss3;
                    qNo.s4ss3 = (short.Parse(questionNumberPlaceholder) + 1).ToString();
                }
            }

            else if (stageSel == 5)
            {
                if (substageSel == 1)
                {
                    qNo.s1ss1 = questionNo.s1ss1;
                    qNo.s1ss2 = questionNo.s1ss2;
                    qNo.s1ss3 = questionNo.s1ss3;

                    qNo.s2ss1 = questionNo.s2ss1;
                    qNo.s2ss2 = questionNo.s2ss2;
                    qNo.s2ss3 = questionNo.s2ss3;

                    qNo.s3ss1 = questionNo.s3ss1;
                    qNo.s3ss2 = questionNo.s3ss2;
                    qNo.s3ss3 = questionNo.s3ss3;

                    qNo.s4ss1 = questionNo.s4ss1;
                    qNo.s4ss2 = questionNo.s4ss2;
                    qNo.s4ss3 = questionNo.s4ss3;

                    qNo.s5ss1 = questionNo.s5ss1;
                    qNo.s5ss2 = questionNo.s5ss2;
                    qNo.s5ss3 = questionNo.s5ss3;

                    questionNumberPlaceholder = questionNo.s5ss1;
                    qNo.s5ss1 = (short.Parse(questionNumberPlaceholder) + 1).ToString();
                }

                else if (substageSel == 2)
                {
                    qNo.s1ss1 = questionNo.s1ss1;
                    qNo.s1ss2 = questionNo.s1ss2;
                    qNo.s1ss3 = questionNo.s1ss3;

                    qNo.s2ss1 = questionNo.s2ss1;
                    qNo.s2ss2 = questionNo.s2ss2;
                    qNo.s2ss3 = questionNo.s2ss3;

                    qNo.s3ss1 = questionNo.s3ss1;
                    qNo.s3ss2 = questionNo.s3ss2;
                    qNo.s3ss3 = questionNo.s3ss3;

                    qNo.s4ss1 = questionNo.s4ss1;
                    qNo.s4ss2 = questionNo.s4ss2;
                    qNo.s4ss3 = questionNo.s4ss3;

                    qNo.s5ss1 = questionNo.s5ss1;
                    qNo.s5ss2 = questionNo.s5ss2;
                    qNo.s5ss3 = questionNo.s5ss3;

                    questionNumberPlaceholder = questionNo.s5ss2;
                    qNo.s5ss2 = (short.Parse(questionNumberPlaceholder) + 1).ToString();
                }

                else if (substageSel == 3)
                {
                    qNo.s1ss1 = questionNo.s1ss1;
                    qNo.s1ss2 = questionNo.s1ss2;
                    qNo.s1ss3 = questionNo.s1ss3;

                    qNo.s2ss1 = questionNo.s2ss1;
                    qNo.s2ss2 = questionNo.s2ss2;
                    qNo.s2ss3 = questionNo.s2ss3;

                    qNo.s3ss1 = questionNo.s3ss1;
                    qNo.s3ss2 = questionNo.s3ss2;
                    qNo.s3ss3 = questionNo.s3ss3;

                    qNo.s4ss1 = questionNo.s4ss1;
                    qNo.s4ss2 = questionNo.s4ss2;
                    qNo.s4ss3 = questionNo.s4ss3;

                    qNo.s5ss1 = questionNo.s5ss1;
                    qNo.s5ss2 = questionNo.s5ss2;
                    qNo.s5ss3 = questionNo.s5ss3;

                    questionNumberPlaceholder = questionNo.s5ss3;
                    qNo.s5ss3 = (short.Parse(questionNumberPlaceholder) + 1).ToString();
                }
            }



            MCQData mcqData = new MCQData();
            mcqData.Answer = getAnswer.text;
            mcqData.Question = getQuestion.text;
            mcqData.Options = getOptions.text;


            RestClient.Put(databaseURL + "Stage_" + stageSel.ToString() + "/" + "Substage_" + substageSel.ToString() +
                 "/" + questionNumberPlaceholder + "/" + ".json?auth=" + idToken, mcqData);


            RestClient.Put(qnNoDBURL + ".json?auth=" + idToken, qNo);
        }

        public void OnSubmit()
        {
            RetrieveFromDatabase();
        }

    }
}
