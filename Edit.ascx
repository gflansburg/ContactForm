<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Edit.ascx.cs" Inherits="Gafware.Modules.ContactForm.Edit" %>
<%@ Register Assembly="Gafware.ContactForm" Namespace="Gafware.Modules.ContactForm" TagPrefix="cc1" %>
<h2 id="H1">Question Types</h2>
<fieldset>
    <cc1:GridViewExtended ID="gv_questionTypes" runat="server" AutoGenerateColumns="False" DataKeyNames="QuestionTypeID" 
        OnRowEditing="gv_questionTypes_RowEditing" RowStyle-BackColor="#eeeeee" ShowFooterWhenEmpty="True" ShowHeaderWhenEmpty="True"
        RowStyle-Height="18" HeaderStyle-Height="30" GridLines="None" Font-Names="Arial" Font-Size="Small" CellPadding="4" ShowFooter="True" 
        OnRowCommand="gv_questionTypes_RowCommand" ForeColor="#333333" OnRowUpdating="gv_questionTypes_RowUpdating" CssClass="editTable" 
        OnRowCancelingEdit="gv_questionTypes_RowCancelingEdit" OnRowDataBound="gv_questionTypes_RowDataBound" OnRowDeleting="gv_questionTypes_RowDeleting">
        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
        <RowStyle VerticalAlign="Top" Font-Names="Arial" Font-Size="Small" BackColor="#EFF3FB" />
        <EditRowStyle BackColor="#2461BF" />
        <FooterStyle Font-Names="Arial" Font-Size="Small" BackColor="#FFFFFF" Font-Bold="True" ForeColor="#333333" />
        <HeaderStyle BackColor="#507CD1" HorizontalAlign="Left" VerticalAlign="Middle" Wrap="False" Font-Size="Small" Font-Names="Arial" ForeColor="White" Font-Bold="True" />
        <AlternatingRowStyle BackColor="White" />
        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
        <Columns>
            <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50px"> 
                <ItemTemplate>
                    <asp:ImageButton ID="editButton" runat="server" ImageUrl="~/DesktopModules/Gafware/ContactForm/images/EditIcon1_16px.gif" AlternateText="Edit Question Type" ToolTip="Edit Question Type" CommandName="Edit" Text="Edit" CausesValidation="false" /> 
                    <asp:ImageButton ID="deleteButton" runat="server" ImageUrl="~/DesktopModules/Gafware/ContactForm/images/DeleteIcon1_16px.gif" AlternateText="Delete Question Type" ToolTip="Delete Question Type" CommandName="Delete" Text="Delete" OnClientClick="return confirm('Are you sure you wish to delete this question type?');" CausesValidation="false" /> 
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:ImageButton ID="saveButton" runat="server" ImageUrl="~/DesktopModules/Gafware/ContactForm/images/save.gif" AlternateText="Save Question Type" ToolTip="Save Question Type" CommandName="Update" Text="Update" CausesValidation="true" /> 
                    <asp:ImageButton ID="cancelButton" runat="server" ImageUrl="~/DesktopModules/Gafware/ContactForm/images/DeleteIcon1_16px.gif" AlternateText="Cancel Edit" ToolTip="Cancel Edit" CommandName="Cancel" Text="Cancel" CausesValidation="false" /> 
                </EditItemTemplate>
                <FooterTemplate>
                    <asp:ImageButton ID="newButton" runat="server" ImageUrl="~/DesktopModules/Gafware/ContactForm/images/NewIcon1_16px.gif" AlternateText="New Question Type" ToolTip="New Question Type" CommandName="New" Text="New" CausesValidation="false" /> 
                    <asp:ImageButton ID="saveButton" runat="server" ImageUrl="~/DesktopModules/Gafware/ContactForm/images/save.gif" AlternateText="Save Question Type" ToolTip="Save Question Type" CommandName="Insert" Text="Insert" Visible="false" CausesValidation="true" /> 
                    <asp:ImageButton ID="cancelButton" runat="server" ImageUrl="~/DesktopModules/Gafware/ContactForm/images/DeleteIcon1_16px.gif" AlternateText="Cancel Edit" ToolTip="Cancel Insert" CommandName="Cancel" Text="Cancel" Visible="false" CausesValidation="false" /> 
                </FooterTemplate>
            </asp:TemplateField> 
            <asp:TemplateField HeaderText="Query Code" ItemStyle-Wrap="false" ItemStyle-Width="100px">
                <ItemTemplate>
                    <%# Eval("QueryCode") %>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="tbQueryCode" runat="server" Text='<%# Eval("QueryCode") %>' MaxLength="50" Width="100%"></asp:TextBox>
                </EditItemTemplate>
                <FooterTemplate>
                    <asp:TextBox ID="tbQueryCode" runat="server" Text='<%# Eval("QueryCode") %>' MaxLength="50" Width="100%"></asp:TextBox>
                </FooterTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Topic">
                <ItemTemplate>
                    <%# Eval("Topic") %>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="tbTopic" runat="server" Text='<%# Eval("Topic") %>' MaxLength="50" Width="100%"></asp:TextBox>
                </EditItemTemplate>
                <FooterTemplate>
                    <asp:TextBox ID="tbTopic" runat="server" Text='<%# Eval("Topic") %>' MaxLength="50" Width="100%"></asp:TextBox>
                </FooterTemplate>
            </asp:TemplateField>
        </Columns>
    </cc1:GridViewExtended>
</fieldset>

<h2 id="H2">Email Addresss</h2>
<fieldset>
    <asp:GridView ID="gv_Email" runat="server" AutoGenerateColumns="False" DataKeyNames="ContactID" 
        OnRowEditing="gv_Email_RowEditing" RowStyle-BackColor="#eeeeee" ShowHeaderWhenEmpty="True" CssClass="editTable" 
        RowStyle-Height="18" HeaderStyle-Height="30" GridLines="None" Font-Names="Arial" Font-Size="Small" CellPadding="4" ShowFooter="False" 
        ForeColor="#333333" OnRowUpdating="gv_Email_RowUpdating" OnRowCancelingEdit="gv_Email_RowCancelingEdit" OnRowDataBound="gv_Email_RowDataBound">
        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
        <RowStyle VerticalAlign="Top" Font-Names="Arial" Font-Size="Small" BackColor="#EFF3FB" />
        <EditRowStyle BackColor="#2461BF" />
        <FooterStyle Font-Names="Arial" Font-Size="Small" BackColor="#FFFFFF" Font-Bold="True" ForeColor="#333333" />
        <HeaderStyle BackColor="#507CD1" HorizontalAlign="Left" VerticalAlign="Middle" Wrap="False" Font-Size="Small" Font-Names="Arial" ForeColor="White" Font-Bold="True" />
        <AlternatingRowStyle BackColor="White" />
        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
        <Columns>
            <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50px"> 
                <ItemTemplate>
                    <asp:ImageButton ID="editButton" runat="server" ImageUrl="~/DesktopModules/Gafware/ContactForm/images/EditIcon1_16px.gif" AlternateText="Edit Contact" ToolTip="Edit Contact" CommandName="Edit" Text="Edit" /> 
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:ImageButton ID="saveButton" runat="server" ImageUrl="~/DesktopModules/Gafware/ContactForm/images/save.gif" AlternateText="Save Contact" ToolTip="Save Contact" CommandName="Update" Text="Update" /> 
                    <asp:ImageButton ID="cancelButton" runat="server" ImageUrl="~/DesktopModules/Gafware/ContactForm/images/DeleteIcon1_16px.gif" AlternateText="Cancel Edit" ToolTip="Cancel Edit" CommandName="Cancel" Text="Cancel" /> 
                </EditItemTemplate>
            </asp:TemplateField> 
            <asp:TemplateField HeaderText="Query Code" ItemStyle-Wrap="false">
                <ItemTemplate>
                    <%# Eval("QueryCode") %>
                </ItemTemplate>
                <EditItemTemplate>
                    <span style="color: white;"><%# Eval("QueryCode") %></span>
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Email Addresses">
                <ItemTemplate>
                    <%# Eval("EmailAddress") %>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="tbEmailAddress" runat="server" Text='<%# Eval("EmailAddress") %>' MaxLength="255" Width="100%"></asp:TextBox>
                </EditItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</fieldset>
<br />
<div class="CloseEditModeLink">
	<asp:LinkButton ID="cmdCloseEdit" runat="server" Text="Close" OnClick="cmdCloseEdit_Click" CssClass="dnnPrimaryAction" />
</div>
