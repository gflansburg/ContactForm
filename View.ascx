<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="View.ascx.cs" Inherits="Gafware.Modules.ContactForm.View" %>
<%@ Register TagPrefix="cc1" Namespace="Gafware.Modules.ContactForm.Recaptcha.Web.UI.Controls" Assembly="Gafware.ContactForm" %>
<div id="dnnContactForm" class="dnnContactForm dnnForm dnnClear">
    <fieldset>
        <div class="dnnFormItem">
            <div class="dnnLabel" style="position: relative;"><label><span id="reqFirstName" runat="server" class="required">*</span> First Name:</label></div>
            <asp:TextBox CssClass="NormalTextBox" ID="txtfirstname" MaxLength="50" runat="server" ValidationGroup="ContactForm"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvFirstName" runat="server" ControlToValidate="txtfirstname" Display="Dynamic" CssClass="dnnFormMessage dnnFormError" ValidationGroup="ContactForm" />
        </div>
        <div class="dnnFormItem">
            <div class="dnnLabel" style="position: relative;"><label><span id="reqLastName" runat="server" class="required">*</span> Last Name:</label></div>
            <asp:TextBox CssClass="NormalTextBox" ID="txtlastname" MaxLength="50" runat="server" ValidationGroup="ContactForm"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvLastName" runat="server" ControlToValidate="txtlastname" Display="Dynamic" CssClass="dnnFormMessage dnnFormError" ValidationGroup="ContactForm" />
        </div>
        <div class="dnnFormItem">
            <div class="dnnLabel" style="position: relative;"><label><span id="reqEmail" runat="server" class="required">*</span> Your Return Email Address:</label></div>
            <asp:TextBox CssClass="NormalTextBox" ID="txtemail" Columns="52" MaxLength="255" runat="server" ValidationGroup="ContactForm"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtemail" Display="Dynamic" CssClass="dnnFormMessage dnnFormError" ValidationGroup="ContactForm" />
            <asp:RegularExpressionValidator ID="revEmail" runat="server" Display="Dynamic" CssClass="dnnFormMessage dnnFormError" ValidationGroup="ContactForm" ControlToValidate="txtemail"></asp:RegularExpressionValidator>
        </div>
        <div class="dnnFormItem">
            <div class="dnnLabel" style="position: relative;"><label><span id="reqMessage" runat="server" class="required">*</span> Your Message:</label></div>
            <asp:TextBox TextMode="MultiLine" CssClass="NormalTextBox" Columns="67" Rows="8" ID="txtmessage" MaxLength="2000" runat="server" ValidationGroup="ContactForm"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvMessage" runat="server" ControlToValidate="txtmessage" InitialValue="" Display="Dynamic" CssClass="dnnFormMessage dnnFormError" ValidationGroup="ContactForm" />
            <asp:CustomValidator ID="cvProfanity" runat="server" ControlToValidate="txtmessage" Display="Dynamic" CssClass="dnnFormMessage dnnFormError" ValidationGroup="ContactForm" />
        </div>
        <asp:Panel ID="pnlArea" runat="server">
            <div class="dnnFormItem">
                <div class="dnnLabel" style="position: relative;"><label><span id="reqArea" runat="server" class="required">*</span> What type of question is this?</label></div>
                <asp:DropDownList ID="ddarea" runat="server" DataTextField="Topic" DataValueField="QueryCode" ValidationGroup="ContactForm"></asp:DropDownList>
                <asp:RequiredFieldValidator ID="rfvArea" runat="server" ControlToValidate="ddarea" InitialValue="" Display="Dynamic" CssClass="dnnFormMessage dnnFormError" ValidationGroup="ContactForm" />
            </div>
        </asp:Panel>
        <asp:Panel ID="pnlPhone" runat="server">
            <div style="text-align: center; width: 100%; margin-top: 10px; margin-bottom: 5px;"><label><strong>Include the following <em>(if available)</em> for the quickest service.</strong></label></div>
            <div class="dnnFormItem">
                <div class="dnnLabel" style="position: relative;"><label><span id="reqPhone" runat="server" class="required">*</span> Your Phone #:</label></div>
                <asp:TextBox CssClass="NormalTextBox" ID="txtcontactno" MaxLength="20" runat="server" ValidationGroup="ContactForm"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvContactNumber" runat="server" ControlToValidate="txtcontactno" InitialValue="" Display="Dynamic" CssClass="dnnFormMessage dnnFormError" ValidationGroup="ContactForm" />
                <asp:RegularExpressionValidator ID="revContactNumber" runat="server" ValidationGroup="ContactForm" ControlToValidate="txtcontactno" Display="Dynamic" CssClass="dnnFormMessage dnnFormError"></asp:RegularExpressionValidator>
            </div>
        </asp:Panel>
        <div style="text-align: center; width: 302px; margin: 0 auto 0 auto;"><cc1:GoogleReCaptcha ID="Recaptcha1"  runat="server" /></div>
        <br style="clear: both;" />
        <div style="text-align: center; width: 100%;"><asp:CustomValidator ID="rfvRecaptcha" runat="server" Display="Dynamic" CssClass="dnnFormMessage dnnFormError" ValidationGroup="ContactForm" ClientValidationFunction="validateRecaptchaLength" Enabled="false" /></div>
        <div style="text-align: center; width: 100%;"><asp:CustomValidator ID="cvRecaptcha" runat="server" Display="Dynamic" CssClass="dnnFormMessage dnnFormError" ValidationGroup="ContactForm" Enabled="false" /></div>
        <br style="clear: both;" />
        <asp:Panel ID="pnlMessage" runat="server" Visible="false">
            <div id="divMessage" class="centerDiv"><label><asp:Label runat="server" ID="emailsent" CssClass="emailsent"></asp:Label></label></div>
        </asp:Panel>
        <label><em><span class="required">*</span> Required fields.</em></label>
        <div id="divCommands" class="dnnClear commands">
            <ol id="olCommands">
                <li><asp:LinkButton ID="btnSubmit" runat="server" CssClass="dnnPrimaryAction" OnClick="btnsubmit_Click" Text="Send Email" CausesValidation="true" ValidationGroup="ContactForm" /></li>
            </ol>
        </div>
    </fieldset>
</div>
<script language="javascript" type="text/javascript">/*<![CDATA[*/
    function validateRecaptchaLength(oSrc, args) {
        var text = $('#<%= Recaptcha1.ClientID %> #recaptcha_response_field').val();
        args.IsValid = (text.length > 0);
    }
	function onLoadreCaptcha() {
		try {
			$("#<%= Recaptcha1.ClientID %>_Ctrl").empty();
			grecaptcha.render('<%= Recaptcha1.ClientID %>_Ctrl', {
				'sitekey' : '<%= GetreCaptchaPublicKey %>',
				'theme' : '<%= GetreCaptchaTheme %>'
			});
		}
		catch(e) { }
	}
    (function ($, Sys) {
        function setupDnnContactFormSiteSettings() {
            //jQuery('select, input:text').jqTransform();
        }
        $(document).ready(function () {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(hideBlockingScreen);
            var isBlockingScreen = true;
            function showBlockingScreen(message) {
                if (!isBlockingScreen) {
                    isBlockingScreen = true;
                    if (message) {
                        $.blockUI({ message: '<h2><img src="/Images/loading.gif" /> ' + message + '...</h2>' });
                    } else {
                        $(".se-pre-con").show();
                    }
                }
            }
            function hideBlockingScreen() {
                //if (isBlockingScreen)
                {
                    // Animate loader off screen
                    isBlockingScreen = false;
                    $(".se-pre-con").fadeOut("fast");;
                    $.unblockUI();
                }
            }
            $(window).ready(function () {
                hideBlockingScreen();
            });
            $(window).load(function () {
                //hideBlockingScreen();
            });
            $(window).unload(function () {
                //showBlockingScreen();
            });
            var ignore_onbeforeunload = false;
            $('a[href^=mailto]').on('click', function () {
                ignore_onbeforeunload = true;
            });
            window.addEventListener("beforeunload", function (event) {
                if (!ignore_onbeforeunload) {
                    showBlockingScreen();
                }
                ignore_onbeforeunload = false;
            });
            $('#<%= btnSubmit.ClientID %>').click(function (e) {
                //showBlockingScreen("Sending");
            });
            setupDnnContactFormSiteSettings();
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
                setTimeout(function() {
                    $('#divMessage').show(); 
                    setTimeout(function() {
                        $('#divMessage').fadeOut('slow', function () {
                            $('#divMessage').hide();
                        }); 
                    }, 5000);
                }, 500);
                setupDnnContactFormSiteSettings();
                if(<%= EnableGooglereCaptcha.ToString().ToLower() %>) {
                    onLoadreCaptcha();
                }
        });
            setTimeout(onLoadreCaptcha, 1000);
        });

    }(jQuery, window.Sys));
    /*]]>*/
</script>