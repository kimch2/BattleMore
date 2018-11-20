using System.Collections;
using System.Collections.Generic;
using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class BugReporter : MonoBehaviour {


	public UnityEngine.UI.InputField myField;

	public void submit()
	{

		if (lastSubmissionTime > Time.time - 60)
		{
			myField.text = "Please wait at least 60 seconds between submissions" ;
			return;
		}

		if (myField.text == "" || myField.text == " You must type your bug report first!")
		{
			myField.text = " You must type your bug report first!";
			return;
		}

		//	StartCoroutine (sendEmail ());
		sendmail();
	


	}


	float lastSubmissionTime = -60;





	public void sendmail()
	{
		lastSubmissionTime = Time.time;
		SimpleEmailSender.emailSettings.STMPClient = "smtp.gmail.com";
		SimpleEmailSender.emailSettings.SMTPPort = 587;
		SimpleEmailSender.emailSettings.UserName = "battlemoredummy@gmail.com";
		SimpleEmailSender.emailSettings.UserPass = "Hadrian1!";

		myField.text += "\n\n Sending...";



		string subjectLine = "Bug Report: ";
		if (UISetter.main)
		{
			subjectLine += " " + UISetter.main.LevelTitle.text;
		}
		SimpleEmailSender.Send("battlemoreofficial@gmail.com", subjectLine, myField.text, "", SendCompletedCallback);
	}

	private void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
	{
		if (e.Cancelled || e.Error != null)
		{

			myField.text = "Something broke, please report this on the Steam Forum or the game website.";
		}
		else
		{
			print("Email successfully sent.");

			myField.text = "You bug has been reported. I'll fix it as soon as possible!";
		}
	}
}
