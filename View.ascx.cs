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
using DotNetNuke.Security;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Services.Localization;
using System.Reflection;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Gafware.Modules.ContactForm.Components;
using Gafware.Modules.ContactForm.Recaptcha.Web;

namespace Gafware.Modules.ContactForm
{
    /// -----------------------------------------------------------------------------
    /// <summary>
    /// The View class displays the content
    /// 
    /// Typically your view control would be used to display content or functionality in your module.
    /// 
    /// View may be the only control you have in your project depending on the complexity of your module
    /// 
    /// Because the control inherits from ContactFormModuleBase you have access to any custom properties
    /// defined there, as well as properties from DNN such as PortalId, ModuleId, TabId, UserId and many more.
    /// 
    /// </summary>
    /// -----------------------------------------------------------------------------
    public partial class View : ContactFormModuleBase, IActionable
    {
        private const string c_jqTransformKey = "jquery.plugin.jqtransform";
        private const string c_jRecaptchaAjax = "google.recaptcha.ajax";
        public const string CSS_TAG_INCLUDE_FORMAT = "<link rel=\"stylesheet\" type=\"text/css\" href=\"{0}\" />";
        public const string SCRIPT_TAG_INCLUDE_FORMAT = "<script language=\"javascript\" type=\"text/javascript\" src=\"{0}\"></script>";

        private void ImportPlugins()
        {
            //load the plugin client scripts on every page load
            if (!Page.ClientScript.IsClientScriptBlockRegistered(c_jqTransformKey))
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), c_jqTransformKey, String.Format(SCRIPT_TAG_INCLUDE_FORMAT, String.Concat(this.ControlPath, "js/jqTransform/jquery.jqtransform.js")), false);
            }
            if (!Page.ClientScript.IsClientScriptBlockRegistered(c_jqTransformKey) && EnableGooglereCaptcha)
            {
                string scriptToRenderCaptcha = @" <script src=""https://www.google.com/recaptcha/api.js?onload=onLoadreCaptcha&render=explicit"" async defer></script>";
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), c_jRecaptchaAjax, scriptToRenderCaptcha , false);
            }
            Page.Header.Controls.Add(new System.Web.UI.LiteralControl(String.Format(CSS_TAG_INCLUDE_FORMAT, String.Concat(this.ControlPath, "js/jqTransform/jqtransform.css"))));
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ImportPlugins();
                if (!IsPostBack)
                {
                    DotNetNuke.Entities.Users.UserInfo currentUser = DotNetNuke.Entities.Users.UserController.Instance.GetCurrentUserInfo();
                    if (currentUser.UserID > -1)
                    {
                        txtemail.Text = currentUser.Email;
                        txtfirstname.Text = currentUser.FirstName;
                        txtlastname.Text = currentUser.LastName;
                    }

                    reqArea.Visible = ValidateAreaField;
                    reqEmail.Visible = ValidateEmailField;
                    reqFirstName.Visible = reqLastName.Visible = ValidateNameField;
                    reqMessage.Visible = ValidateMessageField;
                    reqPhone.Visible = ValidateContactNumber;

                    rfvArea.Enabled = ValidateAreaField;
                    rfvContactNumber.Enabled = ValidateContactNumber;
                    rfvEmail.Enabled = ValidateEmailField;
                    rfvFirstName.Enabled = rfvLastName.Enabled = ValidateNameField;
                    rfvMessage.Enabled = ValidateMessageField;
                    rfvRecaptcha.EnableClientScript = EnableGooglereCaptcha;
                    cvProfanity.EnableClientScript = ProfanityCheck;
                    cvRecaptcha.Enabled = EnableGooglereCaptcha;

                    revContactNumber.Enabled = ValidateContactNumber && !String.IsNullOrWhiteSpace(RegexPhone);
                    revContactNumber.ValidationExpression = RegexPhone;
                    revEmail.Enabled = ValidateEmailField && !String.IsNullOrWhiteSpace(RegexEmail);
                    revEmail.ValidationExpression = RegexEmail;

                    rfvArea.ErrorMessage = AreaError;
                    rfvContactNumber.ErrorMessage = ContactError;
                    rfvEmail.ErrorMessage = EmailError;
                    rfvFirstName.ErrorMessage = NameError;
                    rfvLastName.ErrorMessage = NameError;
                    rfvMessage.ErrorMessage = MessageError;
                    rfvRecaptcha.ErrorMessage = BlankRecaptcha;
                    cvProfanity.ErrorMessage = ProfanityMSG;
                    revContactNumber.ErrorMessage = PhoneErrorMSG;
                    revEmail.ErrorMessage = EmailErrorMSG;

                    if (ValidateAreaField)
                    {
                        ddarea.CssClass = "dnnFormRequired";
                    }
                    if (ValidateContactNumber)
                    {
                        txtcontactno.CssClass = "NormalTextBox dnnFormRequired";
                    }
                    if (ValidateEmailField)
                    {
                        txtemail.CssClass = "NormalTextBox dnnFormRequired";
                    }
                    if (ValidateMessageField)
                    {
                        txtmessage.CssClass = "NormalTextBox dnnFormRequired";
                    }
                    if (ValidateNameField)
                    {
                        txtfirstname.CssClass = txtlastname.CssClass = "NormalTextBox dnnFormRequired";
                    }
                    List<QuestionType> questionTypes = QuestionTypeController.GetQuestionTypes(PortalId);
                    ddarea.DataSource = questionTypes;
                    ddarea.DataBind();
                    ddarea.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select one", ""));
                    if (ddarea.Items.FindByValue(DefaultArea) != null)
                    {
                        ddarea.SelectedValue = DefaultArea;
                    }
                    pnlArea.Visible = IsAreaFieldVisible;
                    pnlPhone.Visible = IsPhoneFieldVisible;
			        if (questionTypes.Count == 0 || String.IsNullOrEmpty(GetFromAddress))
                    {
				        DotNetNuke.UI.Skins.Skin.AddModuleMessage(this, this.LocalizeString("MissingModuleSettings.Text"), DotNetNuke.UI.Skins.Controls.ModuleMessage.ModuleMessageType.RedError);
			        }
                }
                if (EnableGooglereCaptcha)
                {
                    Recaptcha1.Enabled = true;
                    Recaptcha1.PrivateKey = GetreCaptchaPrivateKey;
                    Recaptcha1.PublicKey = GetreCaptchaPublicKey;
                    switch (GetreCaptchaTheme)
                    {
                        case "Dark":
                            Recaptcha1.Theme = RecaptchaTheme.Dark;
                            break;
                        case "Light":
                            Recaptcha1.Theme = RecaptchaTheme.Light;
                            break;
                        default:
                            Recaptcha1.Theme = RecaptchaTheme.Light;
                            break;
                    }
                }
                else
                {
                    Recaptcha1.Enabled = false;
                    Recaptcha1.Visible = false;
                }
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        public void VerifyFields()
        {
            if (!string.IsNullOrWhiteSpace(txtmessage.Text) && ProfanityCheck)
            {
                if (profanityChecker(txtmessage.Text))
                {
                    cvProfanity.IsValid = false;
                }
            }
            if (EnableGooglereCaptcha)
            {
                cvRecaptcha.IsValid = true;
                if (!Recaptcha1.Validate())
                {
                    if (!string.IsNullOrWhiteSpace(Recaptcha1.Response))
                    {
                        cvRecaptcha.ErrorMessage = IncorrectCaptcha;
                        cvRecaptcha.IsValid = false;
                    }
                    else
                    {
                        cvRecaptcha.ErrorMessage = BlankRecaptcha;
                        cvRecaptcha.IsValid = false;
                    }
                }
            }
        }

        public ModuleActionCollection ModuleActions
        {
            get
            {
                var actions = new ModuleActionCollection
                    {
                        {
                            GetNextActionID(), Localization.GetString("EditModule", LocalResourceFile), "", "", "",
                            EditUrl(), false, SecurityAccessLevel.Edit, true, false
                        }
                    };
                return actions;
            }
        }

        protected void btnsubmit_Click(object sender, EventArgs e)
        {
            EmailObject email = new EmailObject();
            email.PortalID = PortalId;
            email.Name = txtfirstname.Text + " " + txtlastname.Text;
            email.ContactNumber = txtcontactno.Text;
            email.FromAddress = txtemail.Text;
            email.BccAddress = GetBccAddress;
            email.Message = txtmessage.Text;
            email.Area = ddarea.SelectedValue;
            try
            {
                clearerrors();
                VerifyFields();
                if (Page.IsValid)
                {
                    if (!UseDnnSettings)
                    {
                        String msg = String.Empty;
                        if (SendEmail(email, ref msg))
                        {
                            pnlMessage.Visible = true;
                            emailsent.CssClass = "emailsent";
                            emailsent.Text = EmailSentOK;
                            if (EnableReplyMail)
                            {
                                ReplyToUser(email);
                            }
                            ClearForm();
                        }
                        else
                        {
                            pnlMessage.Visible = true;
                            emailsent.CssClass = "emailnotsent";
                            emailsent.Text = msg; // EmailNotSent;
                        }
                    }
                    else
                    {
                        System.Text.StringBuilder strMail = new System.Text.StringBuilder();
                
                        //Create Body for Email
	                    strMail.Append("Submitter's Name: " + email.Name + "<br />");
	                    strMail.Append("Sender Email: " + email.FromAddress + "<br />");
                        strMail.Append("Sender Phone: " + email.ContactNumber + "<br /><br />");
                        strMail.Append(email.Message);
                        string error = String.Empty;
                        if (SendEmailUsingDnn(email.FromAddress, GetFromAddress, GetToAddress(email.Area), email.BccAddress, GetEmailsubject, strMail.ToString(), ref error))
                        {
                            // Save the email to the database
                            EmailController.SaveEmail(email);
                            pnlMessage.Visible = true;
                            emailsent.CssClass = "emailsent";
                            emailsent.Text = EmailSentOK;
                            if (EnableReplyMail)
                            {
                                ReplyToUserWithDnn(email);
                            }
                            ClearForm();
                        }
                        else
                        {
                            pnlMessage.Visible = true;
                            emailsent.CssClass = "emailnotsent";
                            emailsent.Text = (String.IsNullOrEmpty(error) ? EmailNotSent : error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var name = MethodBase.GetCurrentMethod().Name;
                pnlMessage.Visible = true;
                emailsent.CssClass = "emailnotsent";
                emailsent.Text = ex.Message;
            }
        }

        public void ClearForm()
        {
            txtfirstname.Text = String.Empty;
            txtlastname.Text = String.Empty;
            txtcontactno.Text = String.Empty;
            txtemail.Text = String.Empty;
            txtmessage.Text = String.Empty;
            if (ddarea.Items.FindByValue(DefaultArea) != null)
            {
                ddarea.SelectedValue = DefaultArea;
            }
        }

        public void clearerrors()
        {
            pnlMessage.Visible = false;
            emailsent.Text = string.Empty;
        }
    }
}