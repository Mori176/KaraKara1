<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ManageProductPage.aspx.cs" Inherits="MiniProject_Karakara.ManageProductPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="manage-products-wrapper">
        <div class="manage-products-container">
            <div class="manage-products-card">
                <h2>Manage Products</h2>
                <asp:Label ID="lblMessage" runat="server" CssClass="message" EnableViewState="false" />

                <asp:SqlDataSource ID="SqlDataSourceProducts" runat="server"
                    ConnectionString="<%$ ConnectionStrings:ConnKarakara %>"
                    SelectCommand="SELECT p.ProductID, p.ProductName, p.CategoryID, c.CategoryTitle, p.Price, p.ProductDescription, p.ProductImage FROM Products p INNER JOIN Categories c ON p.CategoryID = c.CategoryID"
                    UpdateCommand="UPDATE [Products] SET [CategoryID] = @CategoryID, [ProductName] = @ProductName, [Price] = @Price, [ProductDescription] = @ProductDescription, [ProductImage] = @ProductImage WHERE [ProductID] = @ProductID"
                    DeleteCommand="DELETE FROM [Products] WHERE [ProductID] = @ProductID" 
                    InsertCommand="INSERT INTO [Products] ([CategoryID], [ProductName], [Price], [ProductDescription], [ProductImage]) VALUES (@CategoryID, @ProductName, @Price, @ProductDescription, @ProductImage)">
                    <InsertParameters>
                        <asp:Parameter Name="CategoryID" Type="Int32" />
                        <asp:Parameter Name="ProductName" Type="String" />
                        <asp:Parameter Name="Price" Type="Double" />
                        <asp:Parameter Name="ProductDescription" Type="String" />
                        <asp:Parameter Name="ProductImage" Type="String" />
                    </InsertParameters>
                    <UpdateParameters>
                        <asp:Parameter Name="CategoryID" Type="Int32" />
                        <asp:Parameter Name="ProductName" Type="String" />
                        <asp:Parameter Name="Price" Type="Double" />
                        <asp:Parameter Name="ProductDescription" Type="String" />
                        <asp:Parameter Name="ProductImage" Type="String" />
                        <asp:Parameter Name="ProductID" Type="Int32" />
                    </UpdateParameters>
                    <DeleteParameters>
                        <asp:Parameter Name="ProductID" Type="Int32" />
                    </DeleteParameters>
                </asp:SqlDataSource>

                <asp:SqlDataSource ID="SqlDataSourceCategories" runat="server"
                                   ConnectionString="<%$ ConnectionStrings:ConnKarakara %>"
                                   SelectCommand="SELECT CategoryID, CategoryTitle FROM Categories" />


                <asp:GridView ID="gvProducts" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSourceProducts"
                    DataKeyNames="ProductID" CssClass="gridview" AllowPaging="True" OnRowUpdating="gvProducts_RowUpdating">
                    <Columns>
                        <asp:TemplateField HeaderText="No.">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="ProductName" HeaderText="Name" />
                        <asp:TemplateField HeaderText="Category">
                            <ItemTemplate>
                                <%# Eval("CategoryTitle") %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddlCategory" runat="server" 
                                    DataSourceID="SqlDataSourceCategories" 
                                    DataTextField="CategoryTitle" 
                                    DataValueField="CategoryID" 
                                    SelectedValue='<%# Bind("CategoryID") %>' />
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Price" HeaderText="Price" DataFormatString="{0:C}" />
                        <asp:BoundField DataField="ProductDescription" HeaderText="Product Description" />
                        <asp:TemplateField HeaderText="Product Image">
                            <ItemTemplate>
                                <asp:Image ID="imgProduct" runat="server" ImageUrl='<%# ResolveUrl("~/Images/" + Eval("ProductImage")) %>' Width="100" />
                            </ItemTemplate>
                            
                            <EditItemTemplate>
                                <asp:FileUpload ID="fuEditImage" runat="server" /><br />
                                Current: <asp:Image ID="imgEditPreview" runat="server" ImageUrl='<%# ResolveUrl("~/Images/" + Eval("ProductImage")) %>' Width="100" />
                            </EditItemTemplate>
                            
                            <InsertItemTemplate>
                                <asp:FileUpload ID="fuInsertImage" runat="server" />
                            </InsertItemTemplate>
                        </asp:TemplateField>

                        <asp:CommandField ShowEditButton="True" ShowDeleteButton="True" ButtonType="Button"
                                          EditText="Edit" DeleteText="Delete" 
                                          ControlStyle-CssClass="button" />
                    </Columns>
                </asp:GridView>
            </div>

            <div class="insert-product-form" style="margin-top: 50px;">
                <h2>Insert New Product</h2>
                <asp:DetailsView ID="dvInsertProduct" runat="server" DataSourceID="SqlDataSourceProducts"
                                 DefaultMode="Insert" AutoGenerateRows="False"
                                 OnItemInserting="dvInsertProduct_ItemInserting"
                                 CssClass="details-view-form">
                    <Fields>
                        <asp:BoundField DataField="ProductName" HeaderText="Name" />
                        <asp:TemplateField HeaderText="Category">
                            <ItemTemplate>
                                <%# Eval("CategoryTitle") %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddlInsertCategory" runat="server" 
                                    DataSourceID="SqlDataSourceCategories" 
                                    DataTextField="CategoryTitle" 
                                    DataValueField="CategoryID" 
                                    SelectedValue='<%# Bind("CategoryID") %>' 
                                    CssClass="form-control"/>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Price" HeaderText="Price" />
                        <asp:BoundField DataField="ProductDescription" HeaderText="Description" />
                        <asp:TemplateField HeaderText="Image">
                            <InsertItemTemplate>
                                <asp:FileUpload ID="fuInsertImage" runat="server" CssClass="form-control"/>
                            </InsertItemTemplate>
                        </asp:TemplateField>
                        <asp:CommandField ShowInsertButton="True" InsertText="Add Product" ShowCancelButton="false" ControlStyle-CssClass="buttonadd" />
                    </Fields>
                </asp:DetailsView>
            </div>
        </div>
    </div>
</asp:Content>
