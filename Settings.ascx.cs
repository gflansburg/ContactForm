/*
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
using System.Net.Mail;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Exceptions;
using Gafware.Modules.ContactForm.Components;

namespace Gafware.Modules.ContactForm
{
    /// -----------------------------------------------------------------------------
    /// <summary>
    /// The Settings class manages Module Settings
    /// 
    /// Typically your settings control would be used to manage settings for your module.
    /// There are two types of settings, ModuleSettings, and TabModuleSettings.
    /// 
    /// ModuleSettings apply to all "copies" of a module on a site, no matter which page the module is on. 
    /// 
    /// TabModuleSettings apply only to the current module on the current page, if you copy that module to
    /// another page the settings are not transferred.
    /// 
    /// If you happen to save both TabModuleSettings and ModuleSettings, TabModuleSettings overrides ModuleSettings.
    /// 
    /// Below we have some examples of how to access these settings but you will need to uncomment to use.
    /// 
    /// Because the control inherits from ContactFormSettingsBase you have access to any custom properties
    /// defined there, as well as properties from DNN such as PortalId, ModuleId, TabId, UserId and many more.
    /// </summary>
    /// -----------------------------------------------------------------------------
    public partial class Settings : ContactFormModuleSettingsBase
    {
        #region Base Method Implementations

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// LoadSettings loads the settings from the Database and displays them
        /// </summary>
        /// -----------------------------------------------------------------------------
        public override void LoadSettings()
        {
            yesitdid.Visible = false;
            noitdidnt.Visible = false;

            try
            {
                if (Page.IsPostBack == false)
                {
                    AreaDefault.DataSource = QuestionTypeController.GetQuestionTypes(PortalId);
                    AreaDefault.DataBind();
                    AreaDefault.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select one", ""));
                    txtSmtpServer.Text = SMTPServerSettings;
                    chkusednn.Checked = UseDnnSettings;
                    txtsmtpusername.Text = SmtpUsername;
                    txtsmtppassword.Text = PasswordEncrypter.Decrypt(SmtpPassword);
                    txtsmtpdomain.Text = SmtpDomain;
                    chksmtpssl.Checked = SmtpSSL;
                    txtsmtpport.Text = SmtpPortnum;
                    txtfromaddress.Text = GetFromAddress;
                    txtreplyfrom.Text = GetReplyFromAddress;
                    chkusereply.Checked = EnableReplyMail;
                    txtbccaddress.Text = GetBccAddress;
                    txtReplyEmail.Text = ReplyEmailTemplate;
                    ChkEnableRecaptcha.Checked = EnableGooglereCaptcha;
                    txtprivateKey.Text = GetreCaptchaPrivateKey;
                    txtpublickey.Text = GetreCaptchaPublicKey;
                    RecaptchaThemeDDL.Items.FindByValue(GetreCaptchaTheme).Selected = true;
                    NameField.Checked = ValidateNameField;
                    EmailField.Checked = ValidateEmailField;
                    AreaField.Checked = ValidateAreaField;
                    chkContactNumberField.Checked = ValidateContactNumber;
                    chkMessageField.Checked = ValidateMessageField;
                    txtemailvalidationregex.Text = RegexEmail;
                    txtphonevalidationregex.Text = RegexPhone;
                    txtincorrectcaptcha.Text = IncorrectCaptcha;
                    txtblankcaptcha.Text = BlankRecaptcha;
                    namemsg.Text = NameError;
                    areamsg.Text = AreaError;
                    contmsg.Text = ContactError;
                    messagemsg.Text = MessageError;
                    emailError.Text = EmailErrorMSG;
                    phoneError.Text = PhoneErrorMSG;
                    emailmsg.Text = EmailMSG;
                    chkprofanity.Checked = ProfanityCheck;
                    txtprofanitymsg.Text = ProfanityMSG;
                    txtemailsent.Text = EmailSentOK;
                    txtemailerror.Text = EmailNotSent;
                    txtsubject.Text = GetEmailsubject;
                    txtautoreplysubject.Text = AutoReplyEmailSubject;
                    AreaVisible.Checked = IsAreaFieldVisible;
                    if (AreaDefault.Items.FindByValue(DefaultArea) != null)
                    {
                        AreaDefault.SelectedValue = DefaultArea;
                    }
                    PhoneVisible.Checked = IsPhoneFieldVisible;
                    pnlSmtp.Visible = !chkusednn.Checked;
                    pnlRecaptcha.Visible = ChkEnableRecaptcha.Checked;
                    divNameField.Visible = NameField.Checked;
                    divEmailField.Visible = EmailField.Checked;
                    divAreaField.Visible = AreaField.Checked;
                    divContactNumberField.Visible = chkContactNumberField.Checked;
                    divMessageField.Visible = chkMessageField.Checked;
                    divProfanityCheck.Visible = chkprofanity.Checked;
                    pnlReply.Visible = chkusereply.Checked;
                }
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// UpdateSettings saves the modified settings to the Database
        /// </summary>
        /// -----------------------------------------------------------------------------
        public override void UpdateSettings()
        {
            var encPass = new PasswordEncrypter();
            try
            {
                
                var modules = new ModuleController();
                /********************************************
                 * Save Contact Form SMTP Settings    
                 *******************************************/
                modules.UpdateTabModuleSetting(TabModuleId, "DNNSMTPServer", chkusednn.Checked.ToString());
                modules.UpdateModuleSetting(ModuleId, "SMTPServer", txtSmtpServer.Text);
                modules.UpdateModuleSetting(ModuleId, "SMTPPort", txtsmtpport.Text);
                modules.UpdateModuleSetting(ModuleId, "SMTPUsername", txtsmtpusername.Text);
                modules.UpdateModuleSetting(ModuleId, "SMTPPassword", PasswordEncrypter.Encrypt(txtsmtppassword.Text));
                modules.UpdateModuleSetting(ModuleId, "SMTPDomain", txtsmtpdomain.Text);
                modules.UpdateModuleSetting(ModuleId, "SMTPSSL", chksmtpssl.Checked.ToString());
                modules.UpdateTabModuleSetting(TabModuleId, "EMAILSENT", txtemailsent.Text);
                modules.UpdateTabModuleSetting(TabModuleId, "EMAILNOTSENT", txtemailerror.Text);

                /********************************************
                * Save Contact Form SMTP Settings  END  
                *******************************************/

                /********************************************
                * Save Contact Form Email Settings    
                *******************************************/
                modules.UpdateTabModuleSetting(TabModuleId, "FromAddress", txtfromaddress.Text);
                modules.UpdateTabModuleSetting(TabModuleId, "AutoReplyFromAddress", txtreplyfrom.Text);
                modules.UpdateTabModuleSetting(TabModuleId, "BccAddress", txtbccaddress.Text);
                modules.UpdateTabModuleSetting(TabModuleId, "AutoReplySubject", txtautoreplysubject.Text);
                /********************************************
                * Save Contact Form Email Settings END
                *******************************************/

                /********************************************
                * Save Contact Form Reply Template     
                *******************************************/
                modules.UpdateTabModuleSetting(TabModuleId, "SendReply", chkusereply.Checked.ToString());
                modules.UpdateTabModuleSetting(TabModuleId, "ReplyEmailTemplate", txtReplyEmail.Text);
                /********************************************
                * Save Contact Form Reply Template END    
                *******************************************/

                /********************************************
                * Save Contact Form Validation      
                *******************************************/
                modules.UpdateTabModuleSetting(TabModuleId, "NameRequired", NameField.Checked.ToString());
                modules.UpdateTabModuleSetting(TabModuleId, "EmailRequired", EmailField.Checked.ToString());
                modules.UpdateTabModuleSetting(TabModuleId, "AreaRequired", AreaField.Checked.ToString());
                modules.UpdateTabModuleSetting(TabModuleId, "ContactRequired", chkContactNumberField.Checked.ToString());
                modules.UpdateTabModuleSetting(TabModuleId, "MessageRequired", chkMessageField.Checked.ToString());
                modules.UpdateTabModuleSetting(TabModuleId, "RegexEmail", txtemailvalidationregex.Text);
                modules.UpdateTabModuleSetting(TabModuleId, "RegexPhone", txtphonevalidationregex.Text);
                modules.UpdateTabModuleSetting(TabModuleId, "NameMSG", namemsg.Text);
                modules.UpdateTabModuleSetting(TabModuleId, "AreaMSG", areamsg.Text);
                modules.UpdateTabModuleSetting(TabModuleId, "ContactMSG", contmsg.Text);
                modules.UpdateTabModuleSetting(TabModuleId, "MessageMSG", messagemsg.Text);
                modules.UpdateTabModuleSetting(TabModuleId, "EmailMSG", emailmsg.Text);
                modules.UpdateTabModuleSetting(TabModuleId, "EmailErrorMSG", emailError.Text);
                modules.UpdateTabModuleSetting(TabModuleId, "PhoneErrorMSG", phoneError.Text);
                modules.UpdateTabModuleSetting(TabModuleId, "EnableProfanity", chkprofanity.Checked.ToString());
                modules.UpdateTabModuleSetting(TabModuleId, "ProfanityMessage", txtprofanitymsg.Text);
                modules.UpdateTabModuleSetting(TabModuleId, "AreaDefault", AreaDefault.SelectedValue);
                modules.UpdateTabModuleSetting(TabModuleId, "AreaVisible", AreaVisible.Checked.ToString());
                modules.UpdateTabModuleSetting(TabModuleId, "PhoneVisible", PhoneVisible.Checked.ToString());
                modules.UpdateTabModuleSetting(TabModuleId, "Subject", txtsubject.Text);

                /********************************************
                * Save Contact Form Validation  END    
                *******************************************/

                /********************************************
                * Save Contact Form Google reCaptcha Settings   
               *******************************************/
                modules.UpdateTabModuleSetting(TabModuleId, "EnableGooglereCaptcha", ChkEnableRecaptcha.Checked.ToString());
                modules.UpdateModuleSetting(ModuleId, "reCaptchaPublicKey", txtpublickey.Text);
                modules.UpdateModuleSetting(ModuleId, "reCaptchaPrivateKey", txtprivateKey.Text);
                modules.UpdateTabModuleSetting(TabModuleId, "reCaptchaTheme", RecaptchaThemeDDL.SelectedItem.Text);
                modules.UpdateTabModuleSetting(TabModuleId, "reCaptchaThemeblank", txtblankcaptcha.Text);
                modules.UpdateTabModuleSetting(TabModuleId, "reCaptchaThemeincorret", txtincorrectcaptcha.Text);
                /********************************************
                * Save Contact Form Google reCaptcha Settings  END    
                *******************************************/

                /********************************************
                * Delete Contact Form Global Settings   
                *******************************************/
                modules.DeleteModuleSetting(ModuleId, "EMAILSENT");
                modules.DeleteModuleSetting(ModuleId, "EMAILNOTSENT");
                modules.DeleteModuleSetting(ModuleId, "FromAddress");
                modules.DeleteModuleSetting(ModuleId, "AutoReplyFromAddress");
                modules.DeleteModuleSetting(ModuleId, "BccAddress");
                modules.DeleteModuleSetting(ModuleId, "AutoReplySubject");
                modules.DeleteModuleSetting(ModuleId, "SendReply");
                modules.DeleteModuleSetting(ModuleId, "ReplyEmailTemplate");
                modules.DeleteModuleSetting(ModuleId, "NameRequired");
                modules.DeleteModuleSetting(ModuleId, "EmailRequired");
                modules.DeleteModuleSetting(ModuleId, "AreaRequired");
                modules.DeleteModuleSetting(ModuleId, "ContactRequired");
                modules.DeleteModuleSetting(ModuleId, "MessageRequired");
                modules.DeleteModuleSetting(ModuleId, "RegexEmail");
                modules.DeleteModuleSetting(ModuleId, "RegexPhone");
                modules.DeleteModuleSetting(ModuleId, "NameMSG");
                modules.DeleteModuleSetting(ModuleId, "AreaMSG");
                modules.DeleteModuleSetting(ModuleId, "ContactMSG");
                modules.DeleteModuleSetting(ModuleId, "MessageMSG");
                modules.DeleteModuleSetting(ModuleId, "EmailMSG");
                modules.DeleteModuleSetting(ModuleId, "EmailErrorMSG");
                modules.DeleteModuleSetting(ModuleId, "PhoneErrorMSG");
                modules.DeleteModuleSetting(ModuleId, "EnableProfanity");
                modules.DeleteModuleSetting(ModuleId, "ProfanityMessage");
                modules.DeleteModuleSetting(ModuleId, "AreaDefault");
                modules.DeleteModuleSetting(ModuleId, "AreaVisible");
                modules.DeleteModuleSetting(ModuleId, "PhoneVisible");
                modules.DeleteModuleSetting(ModuleId, "Subject");
                modules.DeleteModuleSetting(ModuleId, "EnableGooglereCaptcha");
                modules.DeleteModuleSetting(ModuleId, "reCaptchaTheme");
                modules.DeleteModuleSetting(ModuleId, "reCaptchaThemeblank");
                modules.DeleteModuleSetting(ModuleId, "reCaptchaThemeincorret");
                /********************************************
                * Delete Contact Form Global Settings End
                *******************************************/

            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        #endregion
        #region Test SMTP Settings
        protected void TestSMTPSettings_Click(object sender, EventArgs e)
        {
            if (chkusednn.Checked)
            {
                try
                {
                    DotNetNuke.Services.Mail.Mail.SendEmail(smtpuser.Text, smtpuser.Text, "Test Email", "This is a test email");
                    yesitdid.Visible = true;
                    noitdidnt.Visible = false;
                }
                catch (Exception)
                {
                    noitdidnt.Visible = true;
                    yesitdid.Visible = false;
                }

            }
            else
            {
                try
                {
                    MailMessage mail = new MailMessage();
                    mail.Subject = "Test Email";
                    mail.To.Add(txttestemail.Text);
                    MailAddress FromA = new MailAddress(txtfromaddress.Text);
                    mail.From = FromA;
                    mail.Body = "This is a test email";
                    SmtpClient SmtpMail = new SmtpClient(txtSmtpServer.Text, Convert.ToInt32(txtsmtpport.Text));
                    SmtpMail.EnableSsl = chksmtpssl.Checked;
                    if (!String.IsNullOrEmpty(txtsmtpusername.Text) && !String.IsNullOrEmpty(txtsmtppassword.Text))
                    {
                        if (!String.IsNullOrEmpty(txtsmtpdomain.Text))
                        {
                            SmtpMail.Credentials = new System.Net.NetworkCredential(txtsmtpusername.Text, txtsmtppassword.Text, txtsmtpdomain.Text);
                        }
                        else
                        {
                            SmtpMail.Credentials = new System.Net.NetworkCredential(txtsmtpusername.Text, txtsmtppassword.Text);
                        }
                    }
                    SmtpMail.Send(mail);
                    mail.Dispose();
                    yesitdid.Visible = true;
                    noitdidnt.Visible = false;
                }
                catch (Exception)
                {
                    noitdidnt.Visible = true;
                    yesitdid.Visible = false;
                }
            }
        }
        #endregion

        protected void defaultemailregex_Click(object sender, EventArgs e)
        {
            txtemailvalidationregex.Text = DefaultEmailRegex();
        }

        protected void defaultphoneregex_Click(object sender, EventArgs e)
        {
            txtphonevalidationregex.Text = DefaultPhoneRegex();
        }

        protected void chkusednn_CheckedChanged(object sender, EventArgs e)
        {
            pnlSmtp.Visible = !chkusednn.Checked;
        }

        protected void ChkEnableRecaptcha_CheckedChanged(object sender, EventArgs e)
        {
            pnlRecaptcha.Visible = ChkEnableRecaptcha.Checked;
        }

        protected void NameField_CheckedChanged(object sender, EventArgs e)
        {
            divNameField.Visible = NameField.Checked;
        }

        protected void EmailField_CheckedChanged(object sender, EventArgs e)
        {
            divEmailField.Visible = EmailField.Checked;
        }

        protected void AreaField_CheckedChanged(object sender, EventArgs e)
        {
            divAreaField.Visible = AreaField.Checked;
        }

        protected void chkContactNumberField_CheckedChanged(object sender, EventArgs e)
        {
            divContactNumberField.Visible = chkContactNumberField.Checked;
        }

        protected void chkMessageField_CheckedChanged(object sender, EventArgs e)
        {
            divMessageField.Visible = chkMessageField.Checked;
        }

        protected void chkprofanity_CheckedChanged(object sender, EventArgs e)
        {
            divProfanityCheck.Visible = chkprofanity.Checked;
        }

        protected void chkusereply_CheckedChanged(object sender, EventArgs e)
        {
            pnlReply.Visible = chkusereply.Checked;
        }
    }
}