<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="RegisterPage.aspx.cs" Inherits="MiniProject_Karakara.RegisterPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-- Register Card -->
    <div class="login-wrapper">
        <div class="login-container">
            <h2>Create an Account</h2>
            <asp:Label ID="lblMessage" runat="server" CssClass="message" EnableViewState="false" />
            <!-- Label -->
            <div class="form-group">
                <label for="txtUsername">Username</label>
                <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" />
                <asp:RequiredFieldValidator ID="rfvUsername" runat="server" 
                    ControlToValidate="txtUsername" CssClass="validation-message"
                    ErrorMessage="Username is required." Display="Dynamic" />
            </div>
            <div class="form-group">
                <label for="txtEmail">Email</label>
                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" />
                <asp:RequiredFieldValidator ID="rfvEmail" runat="server" 
                    ControlToValidate="txtEmail" CssClass="validation-message"
                    ErrorMessage="Email is required." Display="Dynamic" />
            </div>
            <div class="form-group">
                <label for="txtPassword">Password</label>
                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control" />
                <asp:RequiredFieldValidator ID="rfvPassword" runat="server" 
                    ControlToValidate="txtPassword" CssClass="validation-message"
                    ErrorMessage="Password is required." Display="Dynamic" />
                <asp:RegularExpressionValidator ID="revPassword" runat="server"
                    ControlToValidate="txtPassword" CssClass="validation-message"
                    ErrorMessage="Must be at least 6 characters, 1 capital letter and 1 symbol."
                    ValidationExpression="^(?=.*[A-Z])(?=.*[\W_]).{6,}$" Display="Dynamic" />
            </div>
            <div class="form-group">
                <label for="txtConfirmPassword">Confirm Password</label>
                <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password" CssClass="form-control" />
                <asp:RequiredFieldValidator ID="rfvConfirmPassword" runat="server" 
                    ControlToValidate="txtConfirmPassword" CssClass="validation-message"
                    ErrorMessage="Confirm your password." 
                    ForeColor="Red" Display="Dynamic" />
                <asp:CompareValidator ID="cvPasswords" runat="server" 
                    ControlToCompare="txtPassword" CssClass="validation-message"
                    ControlToValidate="txtConfirmPassword" 
                    ErrorMessage="Passwords do not match." Display="Dynamic" />
            </div>
            <!-- Button -->
            <asp:Button ID="btnRegister" runat="server" Text="Register" CssClass="btn-login" OnClick="btnRegister_Click" />
            <div class="register-link">
                Already have an account? <a href="LoginPage.aspx">Sign in</a>
            </div>
        </div>
    </div>
</asp:Content>
