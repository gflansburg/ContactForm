﻿/*
' Copyright (c) 2021  Gafware
'  All rights reserved.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
' 
*/

using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using Gafware.Modules.ContactForm.Components;
using DotNetNuke.Entities.Modules;
using System.Text.RegularExpressions;
using DotNetNuke.Services.Log.EventLog;
using Newtonsoft.Json;

namespace Gafware.Modules.ContactForm
{
    public class PasswordEncrypter : PortalModuleBase
    {
        static readonly string PasswordHash = "####GPH0sting_Is_The_B3st_DNN_Hosting_Pr0vider####";
        static readonly string SaltKey = "$@LTY&KEY#";
        static readonly string VIKey = "@1B2c3D4e5F61g7H8#";


        public static string Encrypt(string plainText)
        {
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            byte[] keyBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);
            var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.Zeros };
            var encryptor = symmetricKey.CreateEncryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));

            byte[] cipherTextBytes;

            using (var memoryStream = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                    cryptoStream.FlushFinalBlock();
                    cipherTextBytes = memoryStream.ToArray();
                    cryptoStream.Close();
                }
                memoryStream.Close();
            }
            return Convert.ToBase64String(cipherTextBytes);
        }

        public static string Decrypt(string encryptedText)
        {
            byte[] cipherTextBytes = Convert.FromBase64String(encryptedText);
            byte[] keyBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);
            var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.None };

            var decryptor = symmetricKey.CreateDecryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));
            var memoryStream = new MemoryStream(cipherTextBytes);
            var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            byte[] plainTextBytes = new byte[cipherTextBytes.Length];

            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
            memoryStream.Close();
            cryptoStream.Close();
            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount).TrimEnd("\0".ToCharArray());
        }
    }

    public class ContactFormModuleBase : PortalModuleBase
    {
        #region Properties
        private string _smtpserver;
        public string SMTPServerSettings
        {
            get
            {
                _smtpserver = string.Empty;
                if (Settings.Contains("SMTPServer"))
                {
                    if (!string.IsNullOrWhiteSpace(Settings["SMTPServer"].ToString()))
                    {
                        _smtpserver = Settings["SMTPServer"].ToString();
                    }
                }
                return _smtpserver;
            }
            set { _smtpserver = value; }
        }
        private string _regexforemail;
        public string RegexForEmail
        {
            get
            {
                _regexforemail = @"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?";
                return _regexforemail;
            }
            set { _regexforemail = value; }
        }
        private bool _isbcc;
        public bool IsBCC
        {
            get
            {
                if (Settings.Contains("BccAddress"))
                {
                    if (!string.IsNullOrWhiteSpace(Settings["BccAddress"].ToString()))
                    {
                        Regex emailvalidate = new Regex(RegexForEmail);
                        if (emailvalidate.IsMatch(Settings["BccAddress"].ToString()))
                        {
                            return _isbcc = true;
                        }
                    }
                }
                else
                {
                    _isbcc = false;
                }

                return _isbcc;
            }
            set { _isbcc = value; }
        }
        private string _bccaddress;
        public string GetBccAddress
        {
            get
            {
                _bccaddress = string.Empty;
                if (Settings.Contains("BccAddress"))
                {
                    if (!string.IsNullOrWhiteSpace(Settings["BccAddress"].ToString()))
                    {
                        _bccaddress = Settings["BccAddress"].ToString();
                    }
                }

                return _bccaddress;
            }
            set { _bccaddress = value; }
        }
        private string _emailsubject;
        public string GetEmailsubject
        {
            get
            {
                _emailsubject = string.Empty;
                if (Settings.Contains("Subject"))
                {
                    if (!string.IsNullOrWhiteSpace(Settings["Subject"].ToString()))
                    {
                        _emailsubject = Settings["Subject"].ToString();
                    }
                }
                return _emailsubject;
            }
            set { _emailsubject = value; }
        }
        private string _fromaddress;
        public string GetFromAddress
        {
            get
            {
                _fromaddress = string.Empty;
                if (Settings.Contains("FromAddress"))
                {
                    if (!string.IsNullOrWhiteSpace(Settings["FromAddress"].ToString()))
                    {
                        _fromaddress = Settings["FromAddress"].ToString();
                    }
                }
                return _fromaddress;
            }
            set { _fromaddress = value; }
        }
        private bool _namefield;
        public bool ValidateNameField
        {
            get
            {
                _namefield = true;

                if (Settings.Contains("NameRequired"))
                {
                    if (!string.IsNullOrWhiteSpace(Settings["NameRequired"].ToString()))
                    {
                        bool.TryParse(Settings["NameRequired"].ToString(), out _namefield);
                    }
                }
                return _namefield;


            }
            set { _namefield = value; }
        }
        private bool _areafield;
        public bool ValidateAreaField
        {
            get
            {
                _areafield = false;
                if (Settings.Contains("AreaRequired"))
                {
                    if (!string.IsNullOrWhiteSpace(Settings["AreaRequired"].ToString()))
                    {
                        bool.TryParse(Settings["AreaRequired"].ToString(), out _areafield);
                    }
                }

                return _areafield;
            }
            set { _areafield = value; }
        }
        private string _areaDefault;
        public string DefaultArea
        {
            get
            {
                _areaDefault = string.Empty;
                if (Settings.Contains("AreaDefault"))
                {
                    if (!string.IsNullOrWhiteSpace(Settings["AreaDefault"].ToString()))
                    {
                        _areaDefault = Settings["AreaDefault"].ToString();
                    }
                }
                
                return _areaDefault;
            }
            set { _areaDefault = value; }
        }
        private bool _areaVisible;
        public bool IsAreaFieldVisible
        {
            get
            {
                _areaVisible = true;
                if (Settings.Contains("AreaVisible"))
                {
                    if (!string.IsNullOrWhiteSpace(Settings["AreaVisible"].ToString()))
                    {
                        bool.TryParse(Settings["AreaVisible"].ToString(), out _areaVisible);
                    }
                }
                
                return _areaVisible;
            }
            set { _areaVisible = value; }
        }
        private bool _phoneVisible;
        public bool IsPhoneFieldVisible
        {
            get
            {
                _phoneVisible = true;
                if (Settings.Contains("PhoneVisible"))
                {
                    if (!string.IsNullOrWhiteSpace(Settings["PhoneVisible"].ToString()))
                    {
                        bool.TryParse(Settings["PhoneVisible"].ToString(), out _phoneVisible);
                    }
                }

                return _phoneVisible;
            }
            set { _phoneVisible = value; }
        }
        private bool _contactnumber;
        public bool ValidateContactNumber
        {
            get
            {
                _contactnumber = false;
                if (Settings.Contains("ContactRequired"))
                {
                    if (!string.IsNullOrWhiteSpace(Settings["ContactRequired"].ToString()))
                    {
                        bool.TryParse(Settings["ContactRequired"].ToString(), out _contactnumber);
                    }
                }

                return _contactnumber;
            }
            set { _contactnumber = value; }
        }
        private bool _validateemail;
        public bool ValidateEmailField
        {
            get
            {
                _validateemail = true;

                if (Settings.Contains("EmailRequired"))
                {
                    if (!string.IsNullOrWhiteSpace(Settings["EmailRequired"].ToString()))
                    {
                        bool.TryParse(Settings["EmailRequired"].ToString(), out _validateemail);
                    }
                }
                return _validateemail;
                
               
            }
            set { _validateemail = value; }
        }
        private bool _messagevalidate;
        public bool ValidateMessageField
        {
            get
            {
                _messagevalidate = false;
                if (Settings.Contains("MessageRequired"))
                {
                    if (!string.IsNullOrWhiteSpace(Settings["MessageRequired"].ToString()))
                    {
                        bool.TryParse(Settings["MessageRequired"].ToString(), out _messagevalidate);
                    }
                }
                return _messagevalidate;
            }
            set { _messagevalidate = value; }
        }
        private bool _enablerecaptcha;
        public bool EnableGooglereCaptcha
        {
            get
            {
                _enablerecaptcha = true;
                if (Settings.Contains("EnableGooglereCaptcha"))
                {
                    if (!string.IsNullOrWhiteSpace(Settings["EnableGooglereCaptcha"].ToString()))
                    {
                        bool.TryParse(Settings["EnableGooglereCaptcha"].ToString(), out _enablerecaptcha);
                    }
                }
                return _enablerecaptcha;
            }
            set { _enablerecaptcha = value; }
        }
        public string DefaultEmailRegex()
        {
            return @"[-0-9a-zA-Z.+_]+@[-0-9a-zA-Z.+_]+\.[a-zA-Z]{2,4}";
        }
        private string _emailregex;
        public string RegexEmail
        {
            get
            {
                _emailregex = string.Empty;


                if (Settings.Contains("RegexEmail"))
                {
                    if (!string.IsNullOrWhiteSpace(Settings["RegexEmail"].ToString()))
                    {
                        _emailregex = Settings["RegexEmail"].ToString();
                    }
                    else
                    {
                        _emailregex = DefaultEmailRegex();
                    }
                }
                else
                {
                    _emailregex = DefaultEmailRegex();
                }

                return _emailregex;
            }
            set { _emailregex = value; }
        }
        public string DefaultPhoneRegex()
        {
            return @"((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}";
        }
        private string _phoneregex;
        public string RegexPhone
        {
            get
            {
                _phoneregex = string.Empty;


                if (Settings.Contains("RegexPhone"))
                {
                    if (!string.IsNullOrWhiteSpace(Settings["RegexPhone"].ToString()))
                    {
                        _phoneregex = Settings["RegexPhone"].ToString();
                    }
                    else
                    {
                        _phoneregex = DefaultPhoneRegex();
                    }
                }
                else
                {
                    _phoneregex = DefaultPhoneRegex();
                }
                
                return _phoneregex;
            }
            set { _phoneregex = value; }
        }
        private string _recaptchaprivatekey;
        public string GetreCaptchaPrivateKey
        {
            get
            {
                _recaptchaprivatekey = string.Empty;

                if (Settings.Contains("reCaptchaPrivateKey"))
                {
                    if (!string.IsNullOrWhiteSpace(Settings["reCaptchaPrivateKey"].ToString()))
                    {
                        _recaptchaprivatekey = Settings["reCaptchaPrivateKey"].ToString();
                    }
                }
                return _recaptchaprivatekey;
            }
            set
            {
                _recaptchaprivatekey = value;
            }
        }
        private string _recaptchapublickey;
        public string GetreCaptchaPublicKey
        {
            get
            {
                _recaptchapublickey = string.Empty;

                if (Settings.Contains("reCaptchaPublicKey"))
                {
                    if (!string.IsNullOrWhiteSpace(Settings["reCaptchaPublicKey"].ToString()))
                    {
                        _recaptchapublickey = Settings["reCaptchaPublicKey"].ToString();
                    }
                }

                return _recaptchapublickey;
            }
            set { _recaptchapublickey = value; }
        }
        private string _recaptchatheme;
        public string GetreCaptchaTheme
        {
            get
            {
                _recaptchatheme = string.Empty;
                if (Settings.Contains("reCaptchaTheme"))
                {
                    if (!string.IsNullOrWhiteSpace(Settings["reCaptchaTheme"].ToString()))
                    {
                        _recaptchatheme = Settings["reCaptchaTheme"].ToString();
                    }
                }

                return _recaptchatheme;
            }
            set { _recaptchatheme = value; }
        }
        private bool _usednnsettings;
        public bool UseDnnSettings
        {
            get
            {
                _usednnsettings = true;

                if (Settings.Contains("DNNSMTPServer"))
                {
                    if (!string.IsNullOrWhiteSpace(Settings["DNNSMTPServer"].ToString()))
                    {
                        bool.TryParse(Settings["DNNSMTPServer"].ToString(), out _usednnsettings);
                    }
                }
                return _usednnsettings;
            }
            set { _usednnsettings = value; }
        }
        private string _blankrecaptcha;
        public string BlankRecaptcha
        {
            get
            {
                _blankrecaptcha = string.Empty;
                if (Settings.Contains("reCaptchaThemeblank"))
                {
                    if (!string.IsNullOrWhiteSpace(Settings["reCaptchaThemeblank"].ToString()))
                    {
                        _blankrecaptcha = Settings["reCaptchaThemeblank"].ToString();
                    }

                }

                return _blankrecaptcha;
            }
            set { _blankrecaptcha = value; }
        }
        private string _incorrectcaptcha;
        public string IncorrectCaptcha
        {
            get
            {
                _incorrectcaptcha = string.Empty;

                if (Settings.Contains("reCaptchaThemeincorret"))
                {
                    if (!string.IsNullOrWhiteSpace(Settings["reCaptchaThemeincorret"].ToString()))
                    {
                        _incorrectcaptcha = Settings["reCaptchaThemeincorret"].ToString();
                    }

                }

                return _incorrectcaptcha;
            }
            set { _incorrectcaptcha = value; }
        }
        private string _nameerror;

        public string NameError
        {
            get
            {

                _nameerror = string.Empty;
                if (Settings.Contains("NameMSG"))
                {
                    if (!string.IsNullOrWhiteSpace(Settings["NameMSG"].ToString()))
                    {
                        _nameerror = Settings["NameMSG"].ToString();
                    }

                }

                return _nameerror;
            }
            set { _nameerror = value; }
        }


        private string _areaerror;

        public string AreaError
        {
            get
            {
                _areaerror = string.Empty;

                if (Settings.Contains("AreaMSG"))
                {
                    if (!string.IsNullOrWhiteSpace(Settings["AreaMSG"].ToString()))
                    {
                        _areaerror = Settings["AreaMSG"].ToString();
                    }

                }
                return _areaerror;
            }
            set { _areaerror = value; }
        }

        private string _contacterror;

        public string ContactError
        {
            get
            {
                _contacterror = string.Empty;


                if (Settings.Contains("ContactMSG"))
                {
                    if (!string.IsNullOrWhiteSpace(Settings["ContactMSG"].ToString()))
                    {
                        _contacterror = Settings["ContactMSG"].ToString();
                    }

                }
                return _contacterror;
            }
            set { _contacterror = value; }
        }

        private string _messageerror;

        public string MessageError
        {
            get
            {
                _messageerror = string.Empty;
                if (Settings.Contains("MessageMSG"))
                {
                    if (!string.IsNullOrWhiteSpace(Settings["MessageMSG"].ToString()))
                    {
                        _messageerror = Settings["MessageMSG"].ToString();
                    }

                }
                return _messageerror;
            }
            set { _messageerror = value; }
        }

        private string _emailerrormsg;

        public string EmailErrorMSG
        {
            get
            {

                _emailerrormsg = string.Empty;
                if (Settings.Contains("EmailErrorMSG"))
                {
                    if (!string.IsNullOrWhiteSpace(Settings["EmailErrorMSG"].ToString()))
                    {
                        _emailerrormsg = Settings["EmailErrorMSG"].ToString();
                    }

                }

                return _emailerrormsg;
            }
            set { _emailerrormsg = value; }
        }

        private string _phoneerrormsg;

        public string PhoneErrorMSG
        {
            get
            {

                _phoneerrormsg = string.Empty;
                if (Settings.Contains("PhoneErrorMSG"))
                {
                    if (!string.IsNullOrWhiteSpace(Settings["PhoneErrorMSG"].ToString()))
                    {
                        _phoneerrormsg = Settings["PhoneErrorMSG"].ToString();
                    }

                }
                
                return _phoneerrormsg;
            }
            set { _phoneerrormsg = value; }
        }

        private string _emailerror;

        public string EmailError
        {
            get
            {

                _emailerror = string.Empty;
                if (Settings.Contains("EmailMSG"))
                {
                    if (!string.IsNullOrWhiteSpace(Settings["EmailMSG"].ToString()))
                    {
                        _emailerror = Settings["EmailMSG"].ToString();
                    }

                }

                return _emailerror;
            }
            set { _emailerror = value; }
        }

        private string _replyemailtemplate;
        public string ReplyEmailTemplate
        {
            get
            {

                _replyemailtemplate = string.Empty;
                if (Settings.Contains("ReplyEmailTemplate"))
                {
                    if (!string.IsNullOrWhiteSpace(Settings["ReplyEmailTemplate"].ToString()))
                    {
                        _replyemailtemplate = Settings["ReplyEmailTemplate"].ToString();
                    }
                }
                return _replyemailtemplate;
            }
            set { _replyemailtemplate = value; }
        }

        private bool _enablereply;
        public bool EnableReplyMail
        {
            get
            {
                _enablereply = false;
                if (Settings.Contains("SendReply"))
                {
                    if (!string.IsNullOrWhiteSpace(Settings["SendReply"].ToString()))
                    {
                        bool.TryParse(Settings["SendReply"].ToString(), out _enablereply);
                    }
                }

                return _enablereply;
            }
            set { _enablereply = value; }
        }

        private string _replyfromaddress;
        public string GetReplyFromAddress
        {
            get
            {
                _replyfromaddress = string.Empty;
                if (Settings.Contains("AutoReplyFromAddress"))
                {
                    if (!string.IsNullOrWhiteSpace(Settings["AutoReplyFromAddress"].ToString()))
                    {
                        _replyfromaddress = Settings["AutoReplyFromAddress"].ToString();
                    }
                }
                return _replyfromaddress;
            }
            set { _replyfromaddress = value; }
        }
        private string _smtpusername;
        public string SmtpUsername
        {
            get
            {
                _smtpusername = string.Empty;
                if (Settings.Contains("SMTPUsername"))
                {
                    if (!string.IsNullOrWhiteSpace(Settings["SMTPUsername"].ToString()))
                    {
                        _smtpusername = Settings["SMTPUsername"].ToString();
                    }
                }
                return _smtpusername;
            }
            set { _smtpusername = value; }
        }
        private string _smtppassword;
        public string SmtpPassword
        {
            get
            {
                _smtppassword = string.Empty;
                if (Settings.Contains("SMTPPassword"))
                {
                    if (!string.IsNullOrWhiteSpace(Settings["SMTPPassword"].ToString()))
                    {
                        _smtppassword = Settings["SMTPPassword"].ToString();
                    }
                }

                return _smtppassword;
            }
            set { _smtppassword = value; }
        }
        private string _smtpdomain;
        public string SmtpDomain
        {
            get
            {
                _smtpdomain = string.Empty;
                if (Settings.Contains("SMTPDomain"))
                {
                    if (!string.IsNullOrWhiteSpace(Settings["SMTPDomain"].ToString()))
                    {
                        _smtpdomain = Settings["SMTPDomain"].ToString();
                    }
                }
                
                return _smtpdomain;
            }
            set { _smtpdomain = value; }
        }
        private bool _smtpssl;
        public bool SmtpSSL
        {
            get
            {
                _smtpssl = false;
                if (Settings.Contains("SMTPSSL"))
                {
                    if (!string.IsNullOrWhiteSpace(Settings["SMTPSSL"].ToString()))
                    {
                        bool.TryParse(Settings["SMTPSSL"].ToString(), out _smtpssl);
                    }
                }
                
                return _smtpssl;
            }
            set { _smtpssl = value; }
        }
        private string _smtpportnum;
        public string SmtpPortnum
        {
            get
            {
                _smtpportnum = string.Empty;
                if (Settings.Contains("SMTPPort"))
                {
                    if (!string.IsNullOrWhiteSpace(Settings["SMTPPort"].ToString()))
                    {
                        _smtpportnum = Settings["SMTPPort"].ToString();
                    }
                }
                return _smtpportnum;
            }
            set { _smtpportnum = value; }
        }

        private bool _profanitycheck;

        public bool ProfanityCheck
        {
            get
            {
                _profanitycheck = false;

                if (Settings.Contains("EnableProfanity"))
                {
                    if (!string.IsNullOrWhiteSpace(Settings["EnableProfanity"].ToString()))
                    {
                        bool.TryParse(Settings["EnableProfanity"].ToString(), out _profanitycheck);
                    }
                }

                return _profanitycheck;
            }
            set { _profanitycheck = value; }
        }

        private bool _profanityfilter;

        public bool EnableProfanityFilter
        {
            get
            {
                _profanityfilter = false;

                if (Settings.Contains("EnableProfanityFilter"))
                {
                    if (!string.IsNullOrWhiteSpace(Settings["EnableProfanityFilter"].ToString()))
                    {
                        bool.TryParse(Settings["EnableProfanityFilter"].ToString(), out _profanityfilter);
                    }
                }

                return _profanityfilter;
            }
            set { _profanityfilter = value; }
        }

        private string _profanitymsg;

        public string ProfanityMSG
        {
            get
            {
                _profanitymsg = string.Empty;
                if (Settings.Contains("ProfanityMessage"))
                {
                    if (!string.IsNullOrWhiteSpace(Settings["ProfanityMessage"].ToString()))
                    {
                        _profanitymsg = Settings["ProfanityMessage"].ToString();
                    }

                }

                return _profanitymsg;
            }
            set { _profanitymsg = value; }
        }

        private string _emailsentok;

        public string EmailSentOK
        {
            get
            {
                _emailsentok = string.Empty;

                if (Settings.Contains("EMAILSENT"))
                {
                    if (!string.IsNullOrWhiteSpace(Settings["EMAILSENT"].ToString()))
                    {
                        _emailsentok = Settings["EMAILSENT"].ToString();
                    }

                }

                return _emailsentok;
            }
            set { _emailsentok = value; }
        }


        private string _emailnotsent;

        public string EmailNotSent
        {
            get
            {
                _emailnotsent = string.Empty;
                if (Settings.Contains("EMAILNOTSENT"))
                {
                    if (!string.IsNullOrWhiteSpace(Settings["EMAILNOTSENT"].ToString()))
                    {
                        _emailnotsent = Settings["EMAILNOTSENT"].ToString();
                    }

                }

                return _emailnotsent;
            }
            set { _emailnotsent = value; }
        }

        private string _autoreplyemailsubject;

        public string AutoReplyEmailSubject
        {
            get
            {
                _autoreplyemailsubject = string.Empty;

                if (Settings.Contains("AutoReplySubject"))
                {
                    if (!string.IsNullOrWhiteSpace(Settings["AutoReplySubject"].ToString()))
                    {
                        _autoreplyemailsubject = Settings["AutoReplySubject"].ToString();
                    }

                }

                return _autoreplyemailsubject;
            }
            set { _autoreplyemailsubject = value; }
        }
        #endregion


        protected string GetToAddress(string area)
        {
            string emailAddress = ContactController.GetContact(area, PortalId);
            string[] aryEmail = emailAddress.Split(',', ';');
            emailAddress = String.Empty;
            foreach (string email in aryEmail)
            {
                if (!String.IsNullOrEmpty(emailAddress))
                {
                    emailAddress += ";";
                }
                emailAddress += email;
            }
            return emailAddress;
        }

        public bool SendEmail(EmailObject email, ref String msg)
        {
            var Log = new EventLogController();
            try
            {
                // Save the email to the database
                EmailController.SaveEmail(email);

                //Create stringbuilder to build email body
                StringBuilder strMail = new StringBuilder();

                //Create Body for Email
                if (!String.IsNullOrEmpty(email.Name))
                {
                    strMail.Append("Submitter's Name: " + email.Name + "<br />");
                }
                if (!String.IsNullOrEmpty(email.FromAddress))
                {
                    strMail.Append("Sender Email: " + email.FromAddress + "<br />");
                }
                if (IsPhoneFieldVisible && !String.IsNullOrEmpty(email.ContactNumber))
                {
                    strMail.Append("Sender Phone: " + email.ContactNumber + "<br />");
                }
                if (IsAreaFieldVisible && !String.IsNullOrEmpty(email.Area))
                {
                    strMail.Append("Request Area: " + email.Area + "<br />");
                }
                if(strMail.Length < 0 && !String.IsNullOrEmpty(email.Message))
                {
                    strMail.Append("<br />");
                }
                if (!String.IsNullOrEmpty(email.Message))
                {
                    strMail.Append(email.Message);
                }

                //Create Mail Object
                MailMessage mail = new MailMessage();

                //Create From Address
                MailAddress FromA = new MailAddress(email.FromAddress, email.Name);

                //Create Bcc Address
                if (IsBCC)
                {
                    MailAddress bcc = new MailAddress(GetBccAddress);
                    mail.Bcc.Add(bcc);
                }
                //Set Email to HTML Body
                mail.IsBodyHtml = true;

                //Subject
                mail.Subject = GetEmailsubject;

                //Add To Recipients
                string[] aryto = GetToAddress(email.Area).Split(',', ';');
                foreach (string to in aryto)
                {
                    mail.To.Add(new MailAddress(to));
                }
                //Add Sender
                mail.From = FromA;
                mail.From = new MailAddress(email.FromAddress);

                //Add Body Text
                mail.Body = strMail.ToString();

                //Add Bcc

                //Add Reply Address
                mail.ReplyToList.Add(new MailAddress(email.FromAddress));
                mail.Sender = new MailAddress(GetFromAddress);

                //SMTP Information

                SmtpClient SmtpMail = new SmtpClient(SMTPServerSettings);

                //Send Email
                SmtpMail.Credentials = new System.Net.NetworkCredential(SmtpUsername, PasswordEncrypter.Decrypt(SmtpPassword), SmtpDomain);
                SmtpMail.Port = Convert.ToInt32(SmtpPortnum);
                SmtpMail.EnableSsl = SmtpSSL;
                SmtpMail.Send(mail);
                mail.Dispose();
                Log.AddLog("Contact Form", "Email Sent", PortalSettings, -1, EventLogController.EventLogType.ADMIN_ALERT);
                return true;

            }


            catch (Exception SendError)
            {
                //Exceptions.ProcessModuleLoadException(this, SendError);
                msg = SendError.Message;
                return false;
            }
        }

        public bool SendEmailUsingDnn(string FromAddress, string SenderAddress, string ToAddress, string Bcc, string Subject, string Body, ref string error)
        {
            try
            {
                string[] aryTo = ToAddress.Split(',', ';');
                foreach (string to in aryTo)
                {
                    DotNetNuke.Services.Mail.Mail.SendEmail(FromAddress, SenderAddress, to, Subject, Body);
                }
                if (!String.IsNullOrWhiteSpace(Bcc))
                {
                    string[] aryBcc = Bcc.Split(',', ';');
                    foreach (string bcc in aryBcc)
                    {
                        DotNetNuke.Services.Mail.Mail.SendEmail(FromAddress, SenderAddress, bcc, Subject, Body);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        public bool ReplyToUserWithDnn(EmailObject email)
        {
            //Create Body for Email
            StringBuilder strMail = new StringBuilder();
            strMail.Append(TokenMsg(ReplyEmailTemplate, email.Name, email.ContactNumber, email.FromAddress, email.Area, email.Message));
            string error = String.Empty;
            return SendEmailUsingDnn(GetReplyFromAddress, GetFromAddress, email.FromAddress, String.Empty, AutoReplyEmailSubject, strMail.ToString(), ref error);
        }

        public bool ReplyToUser(EmailObject email)
        {
            var Log = new EventLogController();
            try
            {
                //Create stringbuilder to build email body
                StringBuilder strMail = new StringBuilder();

                //Create Body for Email
                strMail.Append(TokenMsg(ReplyEmailTemplate, email.Name, email.ContactNumber, email.FromAddress, email.Area, email.Message));

                //Create Mail Object
                MailMessage mail = new MailMessage();

                //Create To Address


                //Create From Address
                MailAddress FromA = new MailAddress(GetReplyFromAddress);


                mail.IsBodyHtml = true;

                //Subject
                mail.Subject = AutoReplyEmailSubject;

                //Add To Recipients
                mail.To.Add(email.FromAddress);

                //Add Sender
                mail.From = FromA;

                //Add Body Text
                mail.Body = strMail.ToString();

                
                //SMTP Information

                SmtpClient SmtpMail = new SmtpClient(SMTPServerSettings);

                //Send Email
                SmtpMail.Credentials = new System.Net.NetworkCredential(SmtpUsername, PasswordEncrypter.Decrypt(SmtpPassword));
                SmtpMail.Port = Convert.ToInt32(SmtpPortnum);
                SmtpMail.Send(mail);
                mail.Dispose();
                Log.AddLog("Contact Form", "Reply Email Sent", PortalSettings, -1, EventLogController.EventLogType.ADMIN_ALERT);
                return true;
            }


            catch (Exception)
            {
                return false;
            }

        }

        public StringBuilder TokenMsg(string replymsg, string name, string phone, string email, string area, string message)
        {
            var stringb = new StringBuilder();
            stringb.Append(replymsg);
            stringb.Replace("[NAME]", name);
            stringb.Replace("[PHONE]", phone);
            stringb.Replace("[EMAIL]", email);
            stringb.Replace("[AREA]", area);
            stringb.Replace("[MESSAGE]", message);
            var converttext = Server.HtmlDecode(stringb.ToString());
            stringb.Clear();
            stringb.Append(converttext);

            return stringb;
        }

        public bool profanityChecker(ref string message)
        {
            var filter = new ProfanityFilter.ProfanityFilter();
            if (!EnableProfanityFilter)
            {
                var swearList = filter.DetectAllProfanities(message);
                return swearList.Count > 0;
            }
            message = filter.CensorString(message);
            return false;
        }
    }
}