using System.Collections.ObjectModel;

namespace PeopleWith;

public partial class PrivacyPolicy : ContentPage
{
	ObservableCollection<privacy> PrivacyList = new ObservableCollection<privacy>(); 
	public PrivacyPolicy()
	{
		try
		{
            InitializeComponent();

			//Item one 
			var PrivOne = new privacy();
			PrivOne.Title = "";
            PrivOne.Body = "PeopleWith Limited is a company incorporated in Northern Ireland (the Company).\r\n\r\nThe following terms and conditions form part of the agreement between you the �User� of the PeopleWith application (�Application�) and the Company. This agreement also summarises how you may use the Application and you how the Company may collect, receive, protect and utilise your information.\r\n\r\nThe Application is available to download via the Apple App Store and the Google Play Store. After downloading the Application, you will be required to create a user account which is specific to the Application. You will be sent confirmation of the registration process by email.\r\n\r\nBy agreeing to be bound by these Terms and Condition the Company gives you consent to use the Application. If you do not agree to these Terms and Conditions, in whole or in part you may not use or access any part of the Application or the PeopleWith platform and supporting materials.\r\n\r\nWe may amend this Policy at any time. Any changes we may make will be posted on this page, please check back frequently. Your continued use of the services after posting will constitute your acceptance of, and agreement to, any changes. You will be notified by email and/or application notification.\r\n\r\nPeopleWith Limited is registered with the Information Commissioners Office (ICO), with registration reference: ZA745747\r\n\r\nIf you have any questions about this Policy, please contact info@peoplewith.com. We will endeavour to respond to all requests within 7 working days.";
            PrivacyList.Add(PrivOne);

            //Item two 
            var PrivTwo = new privacy();
            PrivTwo.Title = "YOUR RIGHT TO USE THE PEOPLEWITH APPLICATION";
            PrivTwo.Body = "By agreeing to these Terms and Conditions, the Company hereby grants you a non-exclusive, non-transferable right to use the Application, solely for your own personal purposes, subject to the terms and conditions of this Agreement. All rights not expressly granted to you are reserved by the Company.\r\n\r\nYou may not access the Application if you are a direct competitor of the Company, except with the Company�s prior written consent. In addition, you may not access the Application for purposes of monitoring its availability, performance or functionality, or for any other benchmarking or competitive purposes.\r\n\r\nYou shall not (i) license, sublicense, sell, resell, transfer, assign, distribute or otherwise commercially exploit or make available to any third party the Application or the content in any way; (ii) modify or make derivative works based upon the Application or the content; (iii) create Internet 'links' to the Application or 'frame' or 'mirror' any Application content on any other server or wireless or Internet-based device; or (iv) reverse engineer or access the Application in order to (a) build a competitive product or service, (b) build a product using similar ideas, features, functions or graphics of the Application, or (c) copy any ideas, features, functions or graphics of the Application.\r\n\r\nYou may use the Application only for your own personal purposes and shall not: (i) send spam or otherwise duplicative or unsolicited messages in violation of applicable laws; (ii) send or store infringing, obscene, threatening, libellous, or otherwise unlawful material, including material or in violation of third party privacy rights; (iii) send or store material containing software viruses, worms, Trojan horses or other harmful computer code, files, scripts, agents or programs; (iv) interfere with or disrupt the integrity or performance of the Application or the data contained there in; or (v) attempt to gain unauthorised access to the Service or its related systems or networks.";
		    PrivacyList.Add(PrivTwo);

            //Item Three 
            var PrivThree = new privacy();
            PrivThree.Title = "YOUR RESPONSIBILITIES";
            PrivThree.Body = "You are responsible for all activity occurring under your User account and shall abide by all applicable local, state, national and foreign laws, treaties and regulations in connection with your use of the Application, including those related to data privacy, international communications and the transmission of technical or personal data.\r\n\r\nYou shall: (i) notify Company immediately of any unauthorised use of any password or account or any other known or suspected breach of security; (ii) report to Company immediately and use reasonable efforts to stop immediately any copying or distribution of Content that is known or suspected by you; and (iii) not impersonate another User or provide false identity information to gain access to or use the Application.";
            PrivacyList.Add(PrivThree);

            //Item Four 
            var PrivFour = new privacy();
            PrivFour.Title = "INTELLECTUAL PROPERTY OWNERSHIP";
            PrivFour.Body = "The Company alone shall own all right, title and interest, including all related Intellectual Property Rights, in and to the Company technology, content and the Application and any suggestions, ideas, enhancement requests, feedback, recommendations or other information provided by you or any other party relating to the Application. This Agreement is not a sale and does not convey to you any rights of ownership in or related to the Application, the or the Intellectual Property Rights owned by the Company. The Company name, the Company logo, and the product names associated with the Application are trademarks of the Company, and no right or license is granted to use them.";
            PrivacyList.Add(PrivFour);

            //Item Five 
            var PrivFive = new privacy();
            PrivFive.Title = "ACCOUNT INFORMATION AND DATA";
            PrivFive.Body = "The Company respects the privacy and confidentiality of all users who engage with the Application, Organisation, Marketing or Projects.\r\n\r\nFor the purposes of applicable data protection laws, The Company is the �Data processor� for the purposes described in this policy. You, the user, remain the �Data Controller�.\r\n\r\nThe Company adheres to the seven key principles that underpin Data Protection legislation:\r\nLawfulness, fairness and transparencyPurpose limitationData minimisationAccuracyStorage limitationIntegrity and confidentiality (security)Accountability\r\n\r\nThese principles are central to how the Company, manage, store and process data. The Company strives to ensure that all data that is shared with us or within the Application is treated with respect for personal, and client, privacy and protected in line with all our legal responsibilities and recognised best practice standards and processes.\r\n\r\nThe Company will only collect the minimum levels of personal data necessary to support operational processes and functionality within the Application and will only use your personal data as described within this policy.\r\n\r\nProtecting your privacy important to us. For that reason, any of your personal data will only be collected, stored and processed by the Application in compliance with the General Data Protection Regulation 2016/679 (�GDPR�)."; 
            PrivacyList.Add(PrivFive);

            //Item Six 
            var PrivSix = new privacy();
            PrivSix.Title = "INFORMATION YOU GIVE THE COMPANY:";
            PrivSix.Body = "You may give the Company information about yourself by when you register for an account in respect of the Application. The information you give the Company may include:\r\nEmail Address (Required)Password (Required)Gender (Required)Age/Date of Birth (or age-range) (Required)Name (Optional)Address, Town or City (Optional)Race/Ethnicity (Optional)Phone number (Optional)Communication Preferences (Optional);and any feedback on your medical condition you might provide which remains optional.\r\n\r\nThe minimum level of data required to register for the service includes, email address, password, date of birth and gender. We need your date of birth to ensure you are of legal age to use the service in your jurisdiction, and your gender to tailor your experience and access to resources.\r\n\r\nShould the purpose of data collection change, you will be informed and opt-in consent re-obtained.\r\n\r\nPersonally Identifiable Information is never accessed by the Company and is utilised for the purposes of personalisation within the Application. In certain circumstances and dependant on Application functionality, consent is required for you to share your information outside of the Application. Consent will be requested and recorded for information governance, auditing and validation.\r\n\r\nYou (the User) have the ability to opt out of data processing activities. Where opting-out will result in your services being negatively impacted, your account will be terminated. To opt out, please email: info@peoplewith.com with �Data Processing Opt-Out� in the subject line. Please contact us at: support@peoplewith.com if you require any assistance with opting-out. The Company will endeavour to respond to your request within 7 working days.\r\n\r\nContact and personal information will not be utilised by the Company for any other reason than explicitly outlined and will not be shared with any Third Parties.\r\n\r\nVisiting the Application, sending us emails, and completing in-app registration constitutes electronic communications. You consent to receive electronic communications, and you agree that all agreements, notices, disclosures, and other communications we provide to you electronically, via email and on the Application, satisfy any legal requirement that such communication be in writing (See: Marketing Activities).\r\n\r\nAll information you supply remains anonymous, safe and secure. The only information that will be used by the Company will be the anonymised information based on your health profile and treatment outcomes.\r\n\r\nYou acknowledge and accept that the Company owns all right, title and interest in and to any data derived by the Company from the Application from such anonymised data.\r\n\r\nUsers have rights to their data which the Company must respect and comply with to the best of our ability. The Company must ensure individuals can exercise their rights in the following ways in accordance with GDPR:\r\nRight to be informed: Keeping a record of how the Company uses personal data to demonstrate compliance with the need for accountability and transparency.Right of access: Enabling Users to access their personal data and supplementary information. Allowing individuals to be aware of and verify the lawfulness of the processing activities.Right to rectification: The Company must rectify or amend the personal data of the individual if requested because it is inaccurate or incomplete. This must be done without delay, and no later than one month.Right to erasure: Individuals have a right to have their data erased and for processing to cease. The User has the right to delete or remove individual personal data any time. The User can remove their data by deleting their profile in the Application.Right to restrict processing: The Company must comply with any User request to restrict, block, or otherwise suppress the processing of personal. The Company is permitted to store personal data if it has been restricted, but not process it further.Right to object: The Company respects the right of an individual to object to data processing based on legitimate interest or the performance of a public interest task. The Company respects the right of an individual to object to direct marketing, including profiling.You (the User) have the right to request access to your data please place your request in an email to dpo@peoplewith.com quoting �Right of Access� in the email subject line. The Company will endeavour to respond to your request within 7 working days.\r\n\r\nWe do not knowingly collect Personal Data online from individuals under 18. If you become aware that a child has provided us with Personal Data without parental consent, please contact us at: support@peoplewith.com. If we become aware that an individual under 18 has provided us with Personal Data without parental consent, we will take steps to remove the data and cancel that individual�s account.";
            PrivacyList.Add(PrivSix);

            //Item Seven 
            var PrivSeven = new privacy();
            PrivSeven.Title = "AUTOMATICALLY SHARED DATA";
            PrivSeven.Body = "Data automatically captured by the Company�s Web and Application is anonymised and utilised for the purposes outlined below:\r\nUsage data from use of the web and application including information about your device, information about your visit and how you use the web or application.Analytics including bug reporting and problem identification.Service improvements and product development.You (the User) are not able to opt out of automatically shared data because data collected is anonymised and aggregated for service improvements. Individuals data cannot be identified and therefore cannot be removed from aggregated service improvement analytics.";
            PrivacyList.Add(PrivSeven);

            //Item Eight
            var PrivEight = new privacy();
            PrivEight.Title = "MARKETING ACTIVITIES";
            PrivEight.Body = "The Company do not utilise or share any information you supply to inform marketing activities by Third Parties. The Company may from time to time communicate with you via email for the purposes outlined below:\r\nHealth related research.Notifications on changes to the Terms and Conditions of Use, Privacy and Data Policy.Newsletter containing New Features and Product Development Announcements.Company Specific Events related to your profile.Other products or services supplied by the Company.You (the User) have the ability to opt out of all marketing activities above, apart from �Notifications on changes to the Terms and Conditions of Use, Privacy and Data Policy� because changes must be agreed to in order to continue the use of the service.\r\n\r\nTo opt out, please email: info@peoplewith.com with �Unsubscribe� in the subject line. Please contact us at: support@peoplewith.com if you require any assistance with unsubscribing. The Company will endeavour to respond to your request within 7 working days.";
            PrivacyList.Add(PrivEight);

            //Item Nine
            var PrivNine = new privacy();
            PrivNine.Title = "DATA RETENTION, SECURITY AND STORAGE";
            PrivNine.Body = "The Company retains the data you the user input into the Application for the term and duration of use of the Application in order to ensure normal Application functionality. Upon termination of Application usage, the Company will retain data for a period of two years unless specifically requested to erase the data under data protection laws.\r\n\r\nOn the event that you the user decide to delete your Account via the Application, systematic processes will permanently delete all data within forty-eight hours. Deletion process cannot be reverse once implemented.\r\n\r\nThe Company utilise ISO27001 certified cloud computing platform Microsoft Azure as its cloud storage provide. Adequate organisational and technical safeguards are implemented to ensure data security.\r\n\r\nData is encrypted at all stages within the PeopleWith Platform, in-flight and at rest. Data encryption is implemented in the following ways:\r\nTransparent Data Encryption: Data moving through PeopleWith�s Platform, from Azure Storage and Databases to the front end of the User Interface, is encrypted utilizing Azure�s Transparent Data Encryption protocols. When data arrives at is destination, it is unencrypted and presented accordingly.Database Encryption: Data stored in a rest state in PeopleWith�s databases are encrypted with Database Encryption Keys implemented, dynamically rotated & updated on a period basis to ensure maximum defence against data breach.Data is encrypted in-flight utilising Organisation Level Domain Verification Secure Socket Layer for maximum verification and security to facilitate the processing of data through the internet.The Company is currently completing Cyber Security Essentials framework and seeking to complete ISO27001 Certification from UK certifier BSI in 2024.\r\n\r\nThe Company has put in place appropriate security measures to prevent your personal data from being accidentally lost, used or accessed in an unauthorised way, altered or disclosed. In addition, the Company limit access to your data to those designated employees and contractors who have a business need to know. They will only process your data on instruction, and they are subject to a confidentiality agreement.\r\n\r\nThe Company have put in place procedures to deal with any suspected personal data breach and will notify you and The Information Commissioner�s Office of a breach where we are legally required to do so. Additionally, you have the right to contact the Information Commissioner�s Office whose details can be found at: https://ico.org.uk.";
            PrivacyList.Add(PrivNine);

            //Item Ten 
            var PrivTen = new privacy();
            PrivTen.Title = "TERMINATION";
            PrivTen.Body = "You, the User, are entitled to stop using the Application and to terminate this agreement at any time. To do this you must deactivate your account via the Application. This can be done by access the �Delete Account� option from �Profile�.\r\n\r\nIn the event that you feel that you can no longer agree to the present Terms and Conditions you must stop using the Application immediately.\r\n\r\nThe Company has the right to terminate this agreement by providing you with two weeks� notice by email.\r\n\r\nEither party�s right to terminate the agreement for cause remain unaffected. Cause that entitles the Company to terminate the agreement may be, in particular but not exclusively, User violation under terms outlined in paragraph 2.\r\n\r\nIf you the User or Company deactivates your User account or withdraws your right to access the Application, the Company will delete any personal data stored about you in accordance with our Privacy Policy.\r\n\r\nIf we become aware that an individual under 18 has provided us with Personal Data without parental consent, we will take steps to remove the data and cancel that individual�s account.";
            PrivacyList.Add(PrivTen);

            //Item Eleven 
            var PrivEleven = new privacy();
            PrivEleven.Title = "MUTUAL INDEMNIFICATION";
            PrivEleven.Body = "The Application is not a medical diagnosis tool, nor does the Company claim that the Application be used as a diagnostic tool. If you have concerns regarding your health or are worried about the progression of symptoms you should seek professional medical advice.\r\n\r\nYou shall indemnify and hold the Company, its officers, directors, employees, attorneys and agents harmless from and against any and all claims, costs, damages, losses, liabilities and expenses (including attorneys' fees and costs) arising out of or in connection with: (i) a claim alleging that use of the your data infringes the rights of, or has caused harm to, a third party; (ii) a claim, which if true, would constitute a violation by you of your representations and warranties.\r\n\r\nThe Company shall indemnify and hold you harmless from and against any and all claims, costs, damages, losses, liabilities and expenses (including attorneys' fees and costs) arising out of or in connection with: (i) a claim alleging that the Application directly infringes copyright or trademark of a third party; (ii) a claim, which if true, would constitute a violation by the Company of its representations or warranties.";
            PrivacyList.Add(PrivEleven);

            //Item Tweleve 
            var PrivTweleve = new privacy();
            PrivTweleve.Title = "MODIFICATION TO TERMS";
            PrivTweleve.Body = "The Company reserves the right to modify the terms and conditions of this Agreement or its policies relating to the Application at any time, effective upon posting of an updated version of this Agreement on the Web or Application. You are responsible for regularly reviewing this Agreement. Continued use of the Application after any such changes shall constitute your consent to such changes.\r\n\r\nWhere the purpose of data collection or processing activities change, you will be informed via email and opt-in consent reobtained.";
            PrivacyList.Add(PrivTweleve);

            //Item Thirteen
            var PrivThirteen = new privacy();
            PrivThirteen.Title = "GENERAL";
            PrivThirteen.Body = "This Agreement shall be governed by and construed in accordance with the laws of England.";
            PrivacyList.Add(PrivThirteen);

            //Item Fourteen 
            var PrivFourteen = new privacy();
            PrivFourteen.Title = "CONTACT";
            PrivFourteen.Body = "If you have any questions about this Policy, please contact info@peoplewith.com. We will endeavour to respond to all requests within 7 working days.\r\n\r\nThe Company can be contacted by post at the registered address:\r\n\r\nPeopleWith Limited\r\nUnit D10\r\nOmagh Enterprise Centre,\r\n41 � 43 Great Northern Road,\r\nOmagh, Co. Tyrone\r\nBT78 5LU\r\nNorthern Ireland\r\n\r\nThe Company Data Protection Officer can be contacted at the above address or:\r\n\r\nChris Johnston\r\nData Protection Officer (DPO)\r\ndpo@peoplewith.com\r\n\r\nPeopleWith Limited Terms and Conditions of Use and Data Policy � September 2023 - Version 1.6";
            PrivacyList.Add(PrivFourteen);

            //Item Fifteen
            var PrivFifteen = new privacy();
            PrivFifteen.Title = "PeopleWith SfE BES Challenge Terms and Conditions:";
            PrivFifteen.Body = "No purchase necessary. All entries will be validated as part of the competition winners review process.\r\n\r\nChallenge prize draw is only open to delegates attending the SfE BES Glasgow SEC Conference, 2023.\r\n\r\nTotal value of the prize draw is up to and not exceeding �250.00 redeemable against a health related device.\r\n\r\nEntrants will be entered into the prize draw and one winner will be selected at random.\r\n\r\nUpon completion of the Challenge draw, all entrant data will be deleted and destroyed. Only the winner will be contacted to arrange the prize delivery.\r\n\r\nPeopleWith retain the right to terminate the prize draw based on a minimum number of participants not being reached.";
            PrivacyList.Add(PrivFifteen);

            PrivPolicy.ItemsSource = PrivacyList; 
        }
		catch (Exception Ex) 
		{

		}
	}
}