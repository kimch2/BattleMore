using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

public class BugReporter : MonoBehaviour {


	public UnityEngine.UI.InputField myField;

	public void submit()
	{

		StartCoroutine (sendEmail ());

	


	}

	IEnumerator sendEmail()
	{
		yield return null;

		if (myField.text == "" || myField.text == " You must type your bug report first!") {
			myField.text = " You must type your bug report first!";
	
		} else {
			MailMessage mail = new MailMessage ();

			mail.From = new MailAddress ("battlemoreofficial@gmail.com");
			mail.To.Add ("battlemoreofficial@gmail.com");
			mail.Subject = "Bug Report";
			mail.Body = myField.text;
			//mail.Headers.Add("X-SMTPAPI" xsmtpapiJSON);

			SmtpClient smtpServer = new SmtpClient ("smtp.gmail.com");
			smtpServer.Port = 587;
			smtpServer.Timeout = 5000;
			smtpServer.Credentials = new System.Net.NetworkCredential ("battlemoreofficial@gmail.com", "Hyperion1!") as ICredentialsByHost;
			smtpServer.EnableSsl = true;
			ServicePointManager.ServerCertificateValidationCallback = 
			delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) {
				return true;
			};
			smtpServer.Send (mail);
			myField.text =  "You bug has been reported. I'll fix it as soon as possible!";
		}
	}
}
