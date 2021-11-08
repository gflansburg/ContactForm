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
using System.Web.UI.WebControls;
using System.Collections.Generic;
using DotNetNuke.Services.Exceptions;
using Gafware.Modules.ContactForm.Components;
using DotNetNuke.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Gafware.Modules.ContactForm
{
    /// -----------------------------------------------------------------------------
    /// <summary>
    /// The EditContactForm class is used to manage content
    /// 
    /// Typically your edit control would be used to create new content, or edit existing content within your module.
    /// The ControlKey for this control is "Edit", and is defined in the manifest (.dnn) file.
    /// 
    /// Because the control inherits from ContactFormModuleBase you have access to any custom properties
    /// defined there, as well as properties from DNN such as PortalId, ModuleId, TabId, UserId and many more.
    /// 
    /// </summary>
    /// -----------------------------------------------------------------------------
    public partial class Edit : ContactFormModuleBase
    {
        private readonly INavigationManager _navigationManager;

        public Edit()
        {
            _navigationManager = DependencyProvider.GetRequiredService<INavigationManager>();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    gv_Email.DataSource = ContactController.GetContacts(PortalId);
                    gv_Email.DataBind();
                    gv_questionTypes.DataSource = QuestionTypeController.GetQuestionTypes(PortalId);
                    gv_questionTypes.DataBind();
                }
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        protected void gv_Email_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
        {
            gv_Email.EditIndex = e.NewEditIndex;
            gv_Email.DataSource = ContactController.GetContacts(PortalId);
            gv_Email.DataBind();
        }

        protected void gv_Email_RowUpdating(object sender, System.Web.UI.WebControls.GridViewUpdateEventArgs e)
        {
            int index = gv_Email.EditIndex;
            GridViewRow row = gv_Email.Rows[index];
            TextBox email = row.FindControl("tbEmailAddress") as TextBox;
            List<Contact> contacts = ContactController.GetContacts(PortalId);
            Contact contact = contacts.Find(p => p.ContactID == (int)gv_Email.DataKeys[index].Value);
            if (contact != null)
            {
                contact.PortalID = PortalId;
                contact.EmailAddress = email.Text;
                ContactController.SaveContact(contact);
            }
            gv_Email.EditIndex = -1;
            gv_Email.DataSource = contacts;
            gv_Email.DataBind();
        }

        protected void gv_Email_RowCancelingEdit(object sender, System.Web.UI.WebControls.GridViewCancelEditEventArgs e)
        {
            gv_Email.EditIndex = -1;
            gv_Email.DataSource = ContactController.GetContacts(PortalId);
            gv_Email.DataBind();
        }

        protected void gv_Email_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TextBox email = e.Row.FindControl("tbEmailAddress") as TextBox;
                if (email != null)
                {
                    ImageButton saveButton = e.Row.FindControl("saveButton") as ImageButton;
                    string js = "if ((event.which && event.which == 13) || " 
                                + "(event.keyCode && event.keyCode == 13)) "
                                + "{" + Page.ClientScript.GetPostBackEventReference(saveButton, String.Empty) + ";return false;} "
                                + "else return true;";
                    email.Attributes.Add("onkeydown", js);
                }
            }
        }

        protected void gv_questionTypes_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gv_questionTypes.EditIndex = e.NewEditIndex;
            gv_questionTypes.DataSource = QuestionTypeController.GetQuestionTypes(PortalId);
            gv_questionTypes.DataBind();
        }

        protected void gv_questionTypes_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int index = gv_questionTypes.EditIndex;
            GridViewRow row = gv_questionTypes.Rows[index];
            TextBox queryCode = row.FindControl("tbQueryCode") as TextBox;
            TextBox topic = row.FindControl("tbTopic") as TextBox;
            List<QuestionType> questionTypes = QuestionTypeController.GetQuestionTypes(PortalId);
            QuestionType questionType = questionTypes.Find(p => p.QuestionTypeID == (int)gv_questionTypes.DataKeys[index].Value);
            if (questionType != null)
            {
                questionType.PortalID = PortalId;
                questionType.QueryCode = queryCode.Text;
                questionType.Topic = topic.Text;
                QuestionTypeController.SaveQuestionType(questionType);
            }
            gv_questionTypes.EditIndex = -1;
            gv_questionTypes.DataSource = questionTypes;
            gv_questionTypes.DataBind();
            gv_Email.DataSource = ContactController.GetContacts(PortalId);
            gv_Email.DataBind();
        }

        protected void gv_questionTypes_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gv_questionTypes.EditIndex = -1;
            gv_questionTypes.DataSource = QuestionTypeController.GetQuestionTypes(PortalId);
            gv_questionTypes.DataBind();
        }

        protected void gv_questionTypes_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Footer)
            {
                TextBox topic = e.Row.FindControl("tbTopic") as TextBox;
                if (topic != null)
                {
                    ImageButton saveButton = e.Row.FindControl("saveButton") as ImageButton;
                    ImageButton newButton = e.Row.FindControl("newButton") as ImageButton;
                    string js = "if ((event.which && event.which == 13) || " 
                                + "(event.keyCode && event.keyCode == 13)) "
                                + "{" + Page.ClientScript.GetPostBackEventReference(e.Row.RowType == DataControlRowType.DataRow ? saveButton : newButton, String.Empty) + ";return false;} "
                                + "else return true;";
                    topic.Attributes.Add("onkeydown", js);
                    TextBox queryCode = e.Row.FindControl("tbQueryCode") as TextBox;
                    if (queryCode != null)
                    {
                        js = "if ((event.which && event.which == 13) || " 
                                    + "(event.keyCode && event.keyCode == 13)) "
                                    + "{$('#" + topic.ClientID + "').focus();return false;} "
                                    + "else return true;";
                        queryCode.Attributes.Add("onkeydown", js);
                    }
                }
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                for (int i = 1; i < e.Row.Cells.Count; i++)
                {
                    e.Row.Cells[i].Visible = false;
                }
            }
        }

        protected void gv_questionTypes_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int questionTypeID = (int)gv_questionTypes.DataKeys[e.RowIndex].Value;
            QuestionTypeController.DeleteQuestionType(questionTypeID);
            gv_questionTypes.DataSource = QuestionTypeController.GetQuestionTypes(PortalId);
            gv_questionTypes.DataBind();
            gv_Email.DataSource = ContactController.GetContacts(PortalId);
            gv_Email.DataBind();
        }

        protected void gv_questionTypes_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("New"))
                {
                    GridViewRow row = null;
                    if (e.CommandSource.GetType() == typeof(LinkButton))
                    {
                        LinkButton btnNew = e.CommandSource as LinkButton;
                        row = btnNew.NamingContainer as GridViewRow;
                    }
                    else if (e.CommandSource.GetType() == typeof(ImageButton))
                    {
                        ImageButton btnNew = e.CommandSource as ImageButton;
                        row = btnNew.NamingContainer as GridViewRow;
                    }
                    if (row == null)
                    {
                        return;
                    }
                    for (int i = 1; i < row.Cells.Count; i++)
                    {
                        row.Cells[i].Visible = true;
                    }
                    ImageButton newButton = row.FindControl("newButton") as ImageButton;
                    ImageButton saveButton = row.FindControl("saveButton") as ImageButton;
                    ImageButton cancelButton = row.FindControl("cancelButton") as ImageButton;
                    newButton.Visible = false;
                    saveButton.Visible = cancelButton.Visible = true;
                }
                else if (e.CommandName.Equals("Cancel"))
                {
                    GridViewRow row = null;
                    if (e.CommandSource.GetType() == typeof(LinkButton))
                    {
                        LinkButton btnNew = e.CommandSource as LinkButton;
                        row = btnNew.NamingContainer as GridViewRow;
                    }
                    else if (e.CommandSource.GetType() == typeof(ImageButton))
                    {
                        ImageButton btnNew = e.CommandSource as ImageButton;
                        row = btnNew.NamingContainer as GridViewRow;
                    }
                    if (row == null)
                    {
                        return;
                    }
                    for (int i = 1; i < row.Cells.Count; i++)
                    {
                        row.Cells[i].Visible = false;
                    }
                    ImageButton newButton = row.FindControl("newButton") as ImageButton;
                    ImageButton saveButton = row.FindControl("saveButton") as ImageButton;
                    ImageButton cancelButton = row.FindControl("cancelButton") as ImageButton;
                    newButton.Visible = true;
                    saveButton.Visible = cancelButton.Visible = false;
                }
                else if (e.CommandName.Equals("Insert"))
                {
                    GridViewRow row = null;
                    if (e.CommandSource.GetType() == typeof(LinkButton))
                    {
                        LinkButton btnNew = e.CommandSource as LinkButton;
                        row = btnNew.NamingContainer as GridViewRow;
                    }
                    else if (e.CommandSource.GetType() == typeof(ImageButton))
                    {
                        ImageButton btnNew = e.CommandSource as ImageButton;
                        row = btnNew.NamingContainer as GridViewRow;
                    }
                    if (row == null)
                    {
                        return;
                    }
                    for (int i = 1; i < row.Cells.Count; i++)
                    {
                        row.Cells[i].Visible = false;
                    }
                    ImageButton newButton = row.FindControl("newButton") as ImageButton;
                    ImageButton saveButton = row.FindControl("saveButton") as ImageButton;
                    ImageButton cancelButton = row.FindControl("cancelButton") as ImageButton;
                    newButton.Visible = true;
                    saveButton.Visible = cancelButton.Visible = false;
                    TextBox tbQueryCode = row.FindControl("tbQueryCode") as TextBox;
                    TextBox tbTopic = row.FindControl("tbTopic") as TextBox;
                    QuestionType questionType = new QuestionType();
                    questionType.PortalID = PortalId;
                    questionType.QueryCode = tbQueryCode.Text;
                    questionType.Topic = tbTopic.Text;
                    QuestionTypeController.SaveQuestionType(questionType);
                    gv_questionTypes.DataSource = QuestionTypeController.GetQuestionTypes(PortalId);
                    gv_questionTypes.DataBind();
                    gv_Email.DataSource = ContactController.GetContacts(PortalId);
                    gv_Email.DataBind();
                }
            }
            catch (Exception)
            {

            }
        }

        protected void cmdCloseEdit_Click(object sender, EventArgs e)
        {
            Response.Redirect(_navigationManager.NavigateURL());
        }
    }
}