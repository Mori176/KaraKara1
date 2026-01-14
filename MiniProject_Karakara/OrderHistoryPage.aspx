<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true"
    CodeBehind="OrderHistoryPage.aspx.cs" Inherits="MiniProject_Karakara.OrderHistoryPage" %>
    <asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    </asp:Content>
    <asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <div class="order-history-wrapper">
            <div class="order-history-container">
                <h2>Order History</h2>
                <asp:Label ID="lblMessage" runat="server" CssClass="message" Style="text-align: center;"
                    EnableViewState="false" />

                <asp:SqlDataSource ID="SqlDataSource1" runat="server"
                    ConnectionString="<%$ ConnectionStrings:ConnKarakara %>" SelectCommand="GetUserOrderHistory"
                    SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="UserID" SessionField="userId" Type="Int32" />
                    </SelectParameters>
                </asp:SqlDataSource>

                <div id="emptyOrderHistory" runat="server" visible="false" class="order-history-card"
                    style="text-align:center; padding:20px;">
                    <p style="font-weight:bold; font-size:1.2em; margin-bottom: 15px;">Your order history is empty.</p>
                    <asp:HyperLink ID="hlShopNow" runat="server" NavigateUrl="ProductPage.aspx"
                        CssClass="shop-now-button">
                        Shop Now
                    </asp:HyperLink>
                </div>

                <asp:Repeater ID="Repeater1" runat="server" OnItemDataBound="Repeater1_ItemDataBound">
                    <ItemTemplate>
                        <div class="order-history-card">
                            <div class="order-header">
                                <span class="order-number">Order: <%# Eval("OrderNumber") %></span>
                                <span class="order-date">
                                    <%# Eval("OrderDate", "{0:dd/MM/yyyy, hh:mmtt}" ).ToString().ToLower() %>
                                </span>
                            </div>

                            <asp:Repeater ID="Repeater2" runat="server">
                                <HeaderTemplate>
                                    <table class="order-table">
                                        <thead>
                                            <tr>
                                                <th>No.</th>
                                                <th>Product</th>
                                                <th>Qty</th>
                                                <th>Price</th>
                                                <th>Subtotal</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <%# (Container.ItemIndex + 1) %>
                                        </td>
                                        <td>
                                            <%# Eval("ProductName") %>
                                        </td>
                                        <td>
                                            <%# Eval("Quantity") %>
                                        </td>
                                        <td>RM <%# Eval("Price", "{0:N2}" ) %>
                                        </td>
                                        <td>RM <%# Eval("Subtotal", "{0:N2}" ) %>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </tbody>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>
                            <div class="order-summary">
                                <p>Total: RM <%# Eval("TotalAmount", "{0:N2}" ) %>
                                </p>
                                <p>Tax: RM <%# Eval("Tax", "{0:N2}" ) %>
                                </p>
                                <p>To Pay: RM <%# Eval("AmountToPay", "{0:N2}" ) %>
                                </p>
                                <p>Status: <strong>
                                        <%# Eval("Status") %>
                                    </strong></p>
                                <asp:Button ID="btnCancel" runat="server" Text="Cancel Order" OnClick="btnCancel_Click"
                                    CommandArgument='<%# Eval("OrderNumber") + "|" + Eval("Status") %>'
                                    Visible='<%# Eval("Status").ToString() != "Cancelled" %>' CssClass="cancel-btn" />
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
    </asp:Content>