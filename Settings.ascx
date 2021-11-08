<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Settings.ascx.cs" Inherits="Gafware.Modules.ContactForm.Settings" %>
<%@ Register TagPrefix="dnn" TagName="TextEditor" Src="~/controls/TextEditor.ascx" %>
<%@ Register TagName="label" TagPrefix="dnn" Src="~/controls/labelcontrol.ascx" %>

<h2 id="H6" class="dnnFormSectionHead"><a href="" class="dnnSectionExpanded">Mail Settings</a></h2>
<fieldset>
    <div class="dnnFormItem">
        <dnn:label ID="lblSubject" runat="server" ControlName="txtsubject" Suffix=":" />
        <asp:TextBox ID="txtsubject" runat="server" />
    </div>
    <div class="dnnFormItem">
        <dnn:Label ID="fromaddress" runat="server" ControlName="txtfromaddress" Suffix=":" />
        <asp:TextBox ID="txtfromaddress" runat="server" />
    </div>
    <div class="dnnFormItem">
        <dnn:label ID="bccaddress" runat="server" ControlName="txtbccaddress" Suffix=":" />
        <asp:TextBox ID="txtbccaddress" runat="server" />
    </div>
    <div class="dnnFormItem">
        <dnn:label ID="lblSentEmail" runat="server" ControlName="txtemailsent" Suffix=":" />
        <asp:TextBox ID="txtemailsent" runat="server" />
    </div>
    <div class="dnnFormItem">
        <dnn:label ID="lblErrorEmailsend" runat="server" ControlName="txtemailerror" Suffix=":" />
        <asp:TextBox ID="txtemailerror" runat="server" />
    </div>
    <div class="dnnFormItem">
        <dnn:Label ID="usednnsmtp" runat="server" ControlName="chkusednn" Suffix=":" />
        <asp:CheckBox runat="server" ID="chkusednn" Checked="true" AutoPostBack="true" OnCheckedChanged="chkusednn_CheckedChanged" />
    </div>
    <asp:Panel ID="pnlSmtp" runat="server" Visible="false">
        <div class="dnnFormItem">
            <dnn:Label ID="SMTPServer" runat="server" ControlName="txtSmtpServer" Suffix=":" />
            <asp:TextBox ID="txtSmtpServer" runat="server" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="smtpport" runat="server" ControlName="txtsmtpport" Suffix=":" />
            <asp:TextBox ID="txtsmtpport" runat="server" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="smtpssl" runat="server" ControlName="chksmtpssl" Suffix=":" />
            <asp:CheckBox ID="chksmtpssl" runat="server" />
        </div>
        <div class="dnnFormItem">
            <dnn:label ID="smtpuser" runat="server" ControlName="txtsmtpusername" Suffix=":" />
            <asp:TextBox ID="txtsmtpusername" runat="server" />
        </div>
        <div class="dnnFormItem">
            <dnn:label ID="smtppassword" runat="server" ControlName="txtsmtppassword" Suffix=":" />
            <asp:TextBox ID="txtsmtppassword" runat="server" TextMode="Password" />
        </div>
        <div class="dnnFormItem">
            <dnn:label ID="smtpdomain" runat="server" ControlName="txtsmtpdomain" Suffix=":" />
            <asp:TextBox ID="txtsmtpdomain" runat="server" />
        </div>
        <div class="dnnFormItem">
            <dnn:label ID="testemail" runat="server" />
            <asp:TextBox ID="txttestemail" runat="server" />
            <asp:LinkButton ID="TestSMTPSettings" runat="server" CssClass="dnnPrimaryAction" ResourceKey="TestSettings" OnClick="TestSMTPSettings_Click" />
        </div>
        <div class="dnnFormMessage dnnFormSuccess hidemelater" id="yesitdid" runat="server">Test Email Sent</div>
        <div class="dnnFormMessage dnnFormValidationSummary hidemelater" id="noitdidnt" runat="server">Error Sending test email</div>
    </asp:Panel>
</fieldset>

<h2 id="H4" class="dnnFormSectionHead"><a href="" class="dnnSectionExpanded">Validation</a></h2>
<fieldset>
    <div class="dnnFormItem">
        <dnn:Label ID="NameFieldRequired" runat="server" ControlName="NameField" Suffix=":" />
        <asp:CheckBox runat="server" ID="NameField" Checked="true" AutoPostBack="true" OnCheckedChanged="NameField_CheckedChanged" />
    </div>
    <div class="dnnFormItem" id="divNameField" runat="server">
        <dnn:Label ID="NameFieldmsg" runat="server" ControlName="namemsg" Suffix=":" />
        <asp:TextBox runat="server" ID="namemsg"></asp:TextBox>
    </div>
    <div class="dnnFormItem">
        <dnn:Label ID="EmailFieldRequired" runat="server" ControlName="EmailField" Suffix=":" />
        <asp:CheckBox runat="server" ID="EmailField" Checked="true" AutoPostBack="true" OnCheckedChanged="EmailField_CheckedChanged" />
    </div>
    <div class="dnnFormItem" id="divEmailField" runat="server">
        <dnn:Label ID="EmailFieldMsg" runat="server" ControlName="emailmsg" Suffix=":" />
        <asp:TextBox runat="server" ID="emailmsg"></asp:TextBox>
    </div>
    <div class="dnnFormItem">
        <dnn:Label ID="AreaFieldRequired" runat="server" ControlName="AreaField" Suffix=":" />
        <asp:CheckBox runat="server" ID="AreaField" AutoPostBack="true" OnCheckedChanged="AreaField_CheckedChanged"/>
    </div>
    <div class="dnnFormItem" id="divAreaField" runat="server">
        <dnn:Label ID="AreaFieldMsg" runat="server" ControlName="areamsg" Suffix=":" />
        <asp:TextBox runat="server" ID="areamsg"></asp:TextBox>
    </div>
    <div class="dnnFormItem">
        <dnn:Label ID="AreaFieldDefault" runat="server" ControlName="AreaDefault" Suffix=":" />
        <asp:DropDownList ID="AreaDefault" runat="server" DataTextField="Topic" DataValueField="QueryCode">
        </asp:DropDownList>
    </div>
    <div class="dnnFormItem">
        <dnn:Label ID="AreaFieldVisible" runat="server" ControlName="AreaVisible" Suffix=":" />
        <asp:CheckBox runat="server" ID="AreaVisible"/>
    </div>
    <div class="dnnFormItem">
        <dnn:Label ID="PhoneFieldVisible" runat="server" ControlName="PhoneVisible" Suffix=":" />
        <asp:CheckBox runat="server" ID="PhoneVisible"/>
    </div>
    <div class="dnnFormItem">
        <dnn:Label ID="ContactNumberRequired" runat="server" ControlName="chkContactNumberField" Suffix=":" />
        <asp:CheckBox runat="server" ID="chkContactNumberField" AutoPostBack="true" OnCheckedChanged="chkContactNumberField_CheckedChanged"/>
    </div>
    <div class="dnnFormItem" id="divContactNumberField" runat="server">
        <dnn:Label ID="contactnumbermsg" runat="server" ControlName="contmsg" Suffix=":" />
        <asp:TextBox runat="server" ID="contmsg"></asp:TextBox>
    </div>
    <div class="dnnFormItem">
        <dnn:Label ID="MessageFieldRequired" runat="server" ControlName="chkMessageField" Suffix=":" />
        <asp:CheckBox runat="server" ID="chkMessageField" AutoPostBack="true" OnCheckedChanged="chkMessageField_CheckedChanged"/>
    </div>
    <div class="dnnFormItem" id="divMessageField" runat="server">
        <dnn:Label ID="messagefield" runat="server" ControlName="messagemsg" Suffix=":" />
        <asp:TextBox runat="server" ID="messagemsg"></asp:TextBox>
    </div>
    <div class="dnnFormItem">
        <dnn:Label ID="profanitycheck" runat="server" ControlName="chkprofanity" Suffix=":" />
        <asp:CheckBox runat="server" ID="chkprofanity" AutoPostBack="true" OnCheckedChanged="chkprofanity_CheckedChanged" />
    </div>
    <div class="dnnFormItem" id="divProfanityCheck" runat="server">
        <dnn:Label ID="profanitymsg" runat="server" ControlName="txtprofanitymsg" Suffix=":" />
        <asp:TextBox runat="server" ID="txtprofanitymsg"></asp:TextBox>
    </div>
    <div class="dnnFormItem">
        <dnn:Label ID="EmailValidation" runat="server" ControlName="txtemailvalidationregex" Suffix=":" />
        <asp:TextBox ID="txtemailvalidationregex" runat="server" />
        <asp:LinkButton runat="server" ID="defaultemailregex" Text="Restore default regex" OnClick="defaultemailregex_Click"></asp:LinkButton>
    </div>
    <div class="dnnFormItem">
        <dnn:Label ID="emailmessage" runat="server" ControlName="emailError" Suffix=":" />
        <asp:TextBox runat="server" ID="emailError"></asp:TextBox>
    </div>
    <div class="dnnFormItem">
        <dnn:Label ID="PhoneValidation" runat="server" ControlName="txtphonevalidationregex" Suffix=":" />
        <asp:TextBox ID="txtphonevalidationregex" runat="server" />
        <asp:LinkButton runat="server" ID="defaultphoneregex" Text="Restore default regex" OnClick="defaultphoneregex_Click"></asp:LinkButton>
    </div>
    <div class="dnnFormItem">
        <dnn:Label ID="phonemessage" runat="server" ControlName="phoneError" Suffix=":" />
        <asp:TextBox runat="server" ID="phoneError"></asp:TextBox>
    </div>
</fieldset>

<h2 id="H3" class="dnnFormSectionHead"><a href="" class="dnnSectionExpanded">Google reCaptcha</a></h2>
<fieldset>
    <div class="dnnFormItem">
        <dnn:Label ID="EnableRecaptcha" runat="server" ControlName="ChkEnableRecaptcha" Suffix=":" />
        <asp:CheckBox runat="server" ID="ChkEnableRecaptcha" AutoPostBack="true" OnCheckedChanged="ChkEnableRecaptcha_CheckedChanged" />
    </div>
    <asp:Panel ID="pnlRecaptcha" runat="server" Visible="false">
        <div class="dnnFormItem">
            <dnn:Label ID="PublicKey" runat="server" ControlName="txtpublickey" Suffix=":" />
            <asp:TextBox ID="txtpublickey" runat="server" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="PrivateKey" runat="server" ControlName="txtprivatekey" Suffix=":" />
            <asp:TextBox ID="txtprivateKey" runat="server" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="RecaptchaTheme" runat="server" ControlName="RecaptchaThemeDDL" Suffix=":" />
            <asp:DropDownList runat="server" ID="RecaptchaThemeDDL">
                <asp:ListItem>Light</asp:ListItem>
                <asp:ListItem>Dark</asp:ListItem>
            </asp:DropDownList>
        </div>
            <div class="dnnFormItem">
            <dnn:Label ID="blankrecaptcha" runat="server" ControlName="txtblankcaptcha" Suffix=":" />
            <asp:TextBox runat="server" ID="txtblankcaptcha"></asp:TextBox>
        </div>
            <div class="dnnFormItem">
            <dnn:Label ID="incorrectcaptcha" runat="server" ControlName="txtincorrectcaptcha" Suffix=":" />
            <asp:TextBox runat="server" ID="txtincorrectcaptcha"></asp:TextBox>
        </div>
    </asp:Panel>
</fieldset>

<h2 id="H2" class="dnnFormSectionHead"><a href="" class="dnnSectionExpanded">Reply Email</a></h2>
<fieldset>
    <div class="dnnFormItem">
        <dnn:Label ID="usereply" runat="server" ControlName="chkusereply" Suffix=":" />
        <asp:CheckBox runat="server" ID="chkusereply" AutoPostBack="true" OnCheckedChanged="chkusereply_CheckedChanged" />
    </div>
    <asp:Panel ID="pnlReply" runat="server" Visible="false">
        <div class="dnnFormItem">
            <dnn:Label ID="replyfrom" runat="server" ControlName="txtreplyfrom" Suffix=":" />
            <asp:TextBox ID="txtreplyfrom" runat="server" />
        </div>
        <div class="dnnFormItem">
            <dnn:label ID="lblAutoReplySubject" runat="server" ControlName="txtautoreplysubject" Suffix=":" />
            <asp:TextBox ID="txtautoreplysubject" runat="server" />
        </div>
        <div class="dnnFormMessage dnnFormInfo">
            <dnn:Label ID="lbltokens" runat="server" ControlName="txtReplyEmail" />
        </div>
        <div class="dnnFormItem">
            <dnn:TextEditor id="txtReplyEmail" runat="server" Width="100%"></dnn:TextEditor>
        </div>
    </asp:Panel>
</fieldset>
