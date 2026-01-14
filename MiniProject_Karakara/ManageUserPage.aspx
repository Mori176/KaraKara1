<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ManageUserPage.aspx.cs" Inherits="MiniProject_Karakara.ManageUserPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="manage-users-wrapper">
        <div class="manage-users-container">
            <div class="manage-users-card">
                <h2>Manage Users</h2>
                <asp:Label ID="lblMessage" runat="server" CssClass="message" EnableViewState="false" />

                <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:ConnKarakara %>" 
                    DeleteCommand="DELETE FROM [Users] WHERE [Id] = @Id" 
                    InsertCommand="INSERT INTO [Users] ([username], [role], [email], [password]) VALUES (@username, @role, @email, @password)" 
                    SelectCommand="SELECT * FROM [Users] ORDER BY [Id]" 
                    UpdateCommand="UPDATE [Users] SET [username] = @username, [role] = @role, [email] = @email, [password] = @password WHERE [Id] = @Id">
                    <DeleteParameters>
                        <asp:Parameter Name="Id" Type="Int32" />
                    </DeleteParameters>
                    <InsertParameters>
                        <asp:Parameter Name="username" Type="String" />
                        <asp:Parameter Name="role" Type="String" />
                        <asp:Parameter Name="email" Type="String" />
                        <asp:Parameter Name="password" Type="String" />
                    </InsertParameters>
                    <SelectParameters>
                        <asp:Parameter DefaultValue="customer" Name="role" Type="String" />
                    </SelectParameters>
                    <UpdateParameters>
                        <asp:Parameter Name="username" Type="String" />
                        <asp:Parameter Name="role" Type="String" />
                        <asp:Parameter Name="email" Type="String" />
                        <asp:Parameter Name="password" Type="String" />
                        <asp:Parameter Name="Id" Type="Int32" />
                    </UpdateParameters>
                </asp:SqlDataSource>

                <div class="gridview-container">
                    <asp:GridView ID="gvUsers" runat="server" AllowPaging="True" 
                                  DataSourceID="SqlDataSource1" AutoGenerateColumns="false" 
                                  DataKeyNames="Id" OnRowUpdating="gvUsers_RowUpdating" 
                                  CssClass="gridview">
                        <Columns>
                            <asp:TemplateField HeaderText="No.">
                                <ItemTemplate>
                                    <%# Container.DataItemIndex + 1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="username" HeaderText="Username" />
                            <asp:BoundField DataField="role" HeaderText="Role" />
                            <asp:BoundField DataField="email" HeaderText="Email" />
                            <asp:TemplateField HeaderText="Password">
                                <ItemTemplate>
                                    *******
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" />
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:CommandField ShowEditButton="True" ShowDeleteButton="True" ButtonType="Button"
                                              EditText="Edit" DeleteText="Delete" 
                                              ControlStyle-CssClass="grid-btn" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
