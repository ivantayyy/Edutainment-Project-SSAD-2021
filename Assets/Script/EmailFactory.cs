using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace Assets
{
    public class EmailFactory : MonoBehaviour
    {
        [SerializeField] InputField txtData;
        public InputField receiverEmail;
        public GameObject EmailUI;
        [SerializeField] Button btnSubmit;
        [SerializeField] bool sendDirect;
        private GameObject teacherObject;
        private bool isTeacher;

        const string kSenderEmailAddress = "ButterflyImpact.Adm@gmail.com";
        const string kSenderPassword = "Admin.123";

        void Start()
        {
            //teacherObject = GameObject.Find("TeacherObject");
           // isTeacher = teacherObject.GetComponent<isTeacherObject>().isTeacher;

            UnityEngine.Assertions.Assert.IsNotNull(txtData);
            UnityEngine.Assertions.Assert.IsNotNull(btnSubmit);
            UnityEngine.Assertions.Assert.IsNotNull(receiverEmail);
            btnSubmit.onClick.AddListener(delegate
            {
                SendAnEmail(txtData.text);
            });
        }

        // Method 1: Direct message
        public void SendAnEmail(string message)
        {
            // Create mail
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(kSenderEmailAddress);
            mail.To.Add(new MailAddress(receiverEmail.text));
            mail.Subject = "Invitation to join room in Butterfly Impact!";
            mail.Body = message;

            // Setup server 
            SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
            smtpServer.Port = 587;
            smtpServer.Credentials = new NetworkCredential(
                kSenderEmailAddress, kSenderPassword) as ICredentialsByHost;
            smtpServer.EnableSsl = true;
            ServicePointManager.ServerCertificateValidationCallback =
                delegate (object s, X509Certificate certificate,
                X509Chain chain, SslPolicyErrors sslPolicyErrors)
                {
                    Debug.Log("Email success!");
                    return true;
                };

            // Send mail to server, print results
            try
            {
                smtpServer.Send(mail);
            }
            catch (System.Exception e)
            {
                Debug.Log("Email error: " + e.Message);
            }
            finally
            {
                Debug.Log("Email sent!");
            }
        }

        public void cancelEmail()
        {
            txtData.text = "";
            receiverEmail.text = "";
            EmailUI.SetActive(false);
        }

        public void inviteEmail()
        {
           // if (!isTeacher)
           //     txtData.text = "Hi, join me for a game at room: " + PhotonNetwork.room.Name;
            EmailUI.SetActive(true);
        }
    }
}