<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ProductPage.aspx.cs" Inherits="MiniProject_Karakara.ProductPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="product-page">
        <h2>Our Products</h2>

        <div class="category-filter">
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ConnKarakara %>" SelectCommand="SELECT [CategoryID], [CategoryTitle] FROM [Categories]"></asp:SqlDataSource>
            <asp:DropDownList 
                ID="ddlCategory" 
                runat="server" 
                AutoPostBack="True" 
                OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged" 
                CssClass="form-dropdown" DataSourceID="SqlDataSource1" DataTextField="CategoryTitle" DataValueField="CategoryID">
            </asp:DropDownList>
        </div>

        <div class="add-cart-section">
            Quantity: &nbsp;
            <asp:TextBox ID="txtQuantity" runat="server" CssClass="quantity-box" TextMode="Number" />
            <asp:Button ID="btnAddToCart" runat="server" Text="Add to Cart" CssClass="btn-addcart" OnClick="btnAddToCart_Click" />
        </div>
        <asp:Label ID="lblMessage" runat="server" CssClass="message" EnableViewState="false" />

        <div class="product-lists">
            <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:ConnKarakara %>" SelectCommand="SELECT [ProductID], [ProductName], [Price], [ProductImage], [ProductDescription] FROM [Products] WHERE (@CategoryID = 0 OR [CategoryID] = @CategoryID)">
                <SelectParameters>
                    <asp:ControlParameter ControlID="ddlCategory" Name="CategoryID" PropertyName="SelectedValue" Type="Int32" DefaultValue="0" />
                </SelectParameters>
            </asp:SqlDataSource>

            <!-- List View to display Products -->
            <asp:ListView ID="lvProducts" runat="server" DataSourceID="SqlDataSource2">
                <LayoutTemplate>
                    <div class="product-grid">
                        <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                    </div>
                </LayoutTemplate>
                <ItemTemplate>
                    <div class='<%# GetProductCardCssClass((int)Eval("ProductID")) %>'>
                        <img src='Images/<%# Eval("ProductImage") %>' alt='<%# Eval("ProductName") %>' class="product-image" />
                        <h3><asp:Label ID="lblProductName" runat="server" Text='<%# Eval("ProductName") %>'></asp:Label></h3>
                        <p class="price">RM <asp:Label ID="lblPrice" runat="server" Text='<%# Eval("Price", "{0:N2}") %>'></asp:Label></p>
                        <asp:Button ID="btnSelect" runat="server" Text="Select" CommandArgument='<%# Eval("ProductID") %>' OnClick="btnSelect_Click" CssClass="select-btn" />
                    </div>
                </ItemTemplate>
            </asp:ListView>
        </div>
    </div>
</asp:Content>
