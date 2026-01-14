<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="LoginPage.aspx.cs" Inherits="MiniProject_Karakara.LoginPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-- Login Card -->
    <div class="login-wrapper">
        <div class="login-container">
            <h2>Sign In to Your Account</h2>
            <asp:Label ID="lblMessage" runat="server" CssClass="message" EnableViewState="false" />
            <!-- Label -->
            <div class="form-group">
                <label for="txtUsername">Username</label>
                <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" />
                <asp:RequiredFieldValidator 
                    ID="rfvLoginUsername" 
                    runat="server"
                    ControlToValidate="txtUsername" 
                    CssClass="validation-message" 
                    ErrorMessage="Username is required." 
                    Display="Dynamic" />
            </div>
            <div class="form-group">
                <label for="txtPassword">Password</label>
                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control" />
                <asp:RequiredFieldValidator 
                    ID="rfvLoginPassword" 
                    runat="server"
                    ControlToValidate="txtPassword" 
                    CssClass="validation-message" 
                    ErrorMessage="Password is required." 
                    Display="Dynamic" />
            </div>
            <!-- Button -->
            <asp:Button ID="btnLogin" runat="server" Text="Sign In" CssClass="btn-login" OnClick="btnLogin_Click" />
            <div class="register-link">
                Don't have an account? <a href="RegisterPage.aspx">Sign up here</a>
            </div>
        </div>
    </div>
</asp:Content>
