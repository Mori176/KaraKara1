<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="CartPage.aspx.cs" Inherits="MiniProject_Karakara.CartPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="cart-wrapper">
        <h2>Your Cart</h2>
        <asp:Label ID="lblMessage" runat="server" CssClass="message" Style="text-align: center; padding-bottom: 10px;" EnableViewState="false" />

        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ConnKarakara %>" 
                           SelectCommand="GetUserCart" 
                           SelectCommandType="StoredProcedure"
                           DeleteCommand="DELETE FROM UserCart WHERE CartID = @CartID">
            <SelectParameters>
                <asp:SessionParameter Name="Username" SessionField="username" Type="String" />
            </SelectParameters>
            <DeleteParameters>
                <asp:Parameter Name="CartID" Type="Int32" />
            </DeleteParameters>
        </asp:SqlDataSource>
        <asp:GridView ID="gvCart" runat="server" 
                      AutoGenerateColumns="False" 
                      CssClass="cart-table"
                      EmptyDataText="Your cart is empty." 
                      DataSourceID="SqlDataSource1"
                      AllowPaging="True" 
                      PageSize="5"
                      OnRowDeleting="gvCart_RowDeleting"
                      OnRowDeleted="gvCart_Deleted"
                      PagerStyle-CssClass="cart-pager" 
                      DataKeyNames="CartID,ProductName, Quantity">
            <Columns>
                <asp:TemplateField HeaderText="No.">
                    <ItemTemplate>
                        <%# Container.DataItemIndex + 1 %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="ProductName" HeaderText="Product Name" SortExpression="ProductName" />
                <asp:BoundField DataField="Price" HeaderText="Price" SortExpression="Price" DataFormatString="RM {0:N2}" />
                <asp:BoundField DataField="Quantity" HeaderText="Quantity" SortExpression="Quantity" />
                <asp:BoundField DataField="TotalItemPrice" DataFormatString="RM {0:N2}" HeaderText="Sub Total" ReadOnly="True" SortExpression="TotalItemPrice" />
                <asp:ImageField DataImageUrlField="ProductImage" DataImageUrlFormatString="Images/{0}" HeaderText="Product Image">
                </asp:ImageField>

                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton 
                            ID="btnDelete" 
                            runat="server" 
                            CommandName="Delete" 
                            CommandArgument='<%# Eval("CartID") %>' 
                            Text="Remove" 
                            CssClass="remove-btn">
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>

            <PagerStyle CssClass="cart-pager"></PagerStyle>
        </asp:GridView>

        <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:ConnKarakara %>" SelectCommand="GetTotalBeforeTax" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:SessionParameter Name="Username" SessionField="username" Type="String" />
            </SelectParameters>
        </asp:SqlDataSource>
        <div id="summaryHide" runat="server" class="cart-summary" visible="false">
            <table>
                <tr>
                    <td>Total Amount:</td>
                    <td><asp:Label ID="lblTotal" runat="server" Text="RM 0.00"></asp:Label></td>
                </tr>
                <tr>
                    <td>Service Tax (6%):</td>
                    <td><asp:Label ID="lblTax" runat="server" Text="RM 0.00"></asp:Label></td>
                </tr>
                <tr>
                    <td><strong>Amount to Pay (after tax):</strong></td>
                    <td><strong><asp:Label ID="lblAmountToPay" runat="server" Text="RM 0.00"></asp:Label></strong></td>
                </tr>
            </table>
        </div>
        <asp:Button ID="btnConfirmOrder" runat="server" Text="Confirm Order" CssClass="confirm-order-btn" Visible="false" OnClick="btnConfirmOrder_Click" />
    </div>

</asp:Content>
