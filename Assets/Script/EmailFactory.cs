using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;


public class EmailFactory : MonoBehaviour
{
    [SerializeField] InputField txtData;
    public InputField receiverEmail;
    public GameObject EmailUI;
    [SerializeField] Button btnSubmit;
    [SerializeField] bool sendDirect;

    const string kSenderEmailAddress = "potate345@gmail.com";
    const string kSenderPassword = "Potato123";
    //const string kReceiverEmailAddress = "clarissa.nurawan@gmail.com";
    //Text kReceiverEmailAddress = receiverEmail.text;


    void Start()
    {
        UnityEngine.Assertions.Assert.IsNotNull(txtData);
        UnityEngine.Assertions.Assert.IsNotNull(btnSubmit);
        UnityEngine.Assertions.Assert.IsNotNull(receiverEmail);
        btnSubmit.onClick.AddListener(delegate
        {
            SendAnEmail(txtData.text);
        });
    }

    // Method 1: Direct message
    private void SendAnEmail(string message)
    {
        // Create mail
        MailMessage mail = new MailMessage();
        mail.From = new MailAddress(kSenderEmailAddress);
        //EmailFactory em = new EmailFactory();
        mail.To.Add(new MailAddress(receiverEmail.text));
        mail.Subject = "Invitation";
        mail.Body = message;

        // Setup server 
        SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
        smtpServer.Port = 587;
        smtpServer.Credentials = new NetworkCredential(
            kSenderEmailAddress, kSenderPassword) as ICredentialsByHost;
        smtpServer.EnableSsl = true;
        ServicePointManager.ServerCertificateValidationCallback =
            delegate (object s, X509Certificate certificate,
            X509Chain chain, SslPolicyErrors sslPolicyErrors) {
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
        txtData.text = "Hi, join me for a game at room: "+PhotonNetwork.room.Name;
        EmailUI.SetActive(true);
    }
}
