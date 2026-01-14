<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="HomePage.aspx.cs" Inherits="MiniProject_Karakara.HomePage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-- Hero Section -->
    <section class="hero">
        <h1>Welcome to Karäkarä</h1>
        <h3>Your Home’s New Best Friend</h3>
        <p>Explore our wide range of premium furniture and essential household items, thoughtfully selected to match your lifestyle. Whether you're furnishing a new space or refreshing your current home, we deliver quality, comfort, and style directly to your doorstep making home living more beautiful and effortless than ever.</p>
        <div style="margin-top: 40px;">
            <a href="#featured" style="color: white; font-size: 2rem; text-decoration: none;">↓</a>
        </div>
    </section>

    <!-- Featured Products -->
    <section class="featured-products" id="featured">
        <h2>Featured Products</h2>
        <div class="product-list">
            <!-- Product card -->
            <div class="product-card">
                <img src="Images/Comfort_Sofa.png" alt="Comfort Sofa" />
                <h3>Comfort Sofa</h3>
                <p>Stylish sofa for your living room.</p>
                <span class="price">RM 1,250.00</span>
                <a href="ProductPage.aspx" class="btn btn-small">View More</a>
            </div>
            <div class="product-card">
                <img src="Images/Magnus_Bed.png" alt="Magnus Bed" />
                <h3>Magnus Bed</h3>
                <p>Elegant design that brings comfort.</p>
                <span class="price">RM 2,500.00</span>
                <a href="ProductPage.aspx" class="btn btn-small">View More</a>
            </div>
            <div class="product-card">
                <img src="Images/Marble_Coffee_Table.png" alt="Marble Coffee Table" />
                <h3>Marble Coffee Table</h3>
                <p>Modern marble top with sturdy base.</p>
                <span class="price">RM 3,500.00</span>
                <a href="ProductPage.aspx" class="btn btn-small">View More</a>
            </div>
        </div>
    </section>
</asp:Content>
