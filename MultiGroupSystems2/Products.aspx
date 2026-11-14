
<%@ Page Title="Products" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Products.aspx.cs" Inherits="MultiGroupSystemsTester.Products" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<style>
    .shop-page {
        padding: 28px 0 65px;
    }

    .shop-hero {
        background: linear-gradient(115deg, #10263e, #18485b);
        border-radius: 18px;
        padding: 42px;
        color: white;
        margin-bottom: 30px;
    }

    .shop-hero .eyebrow {
        color: #7dd5d8;
        font-size: 12px;
        font-weight: 700;
        letter-spacing: 2px;
        text-transform: uppercase;
        margin-bottom: 12px;
    }

    .shop-hero h1 {
        font-size: clamp(28px, 4vw, 42px);
        font-weight: 750;
        margin-bottom: 12px;
        color: white;
    }

    .shop-hero p {
        color: #d6e5ec;
        max-width: 570px;
        margin-bottom: 22px;
    }

    .shop-cart-link {
        display: inline-block;
        background: #008c98;
        color: white !important;
        text-decoration: none;
        padding: 11px 22px;
        border-radius: 9px;
        font-weight: 600;
    }

    .shop-cart-link:hover {
        background: #00727d;
    }

    .shop-toolbar {
        background: #f5f8fb;
        border: 1px solid #e1e8ef;
        border-radius: 14px;
        padding: 22px;
        margin-bottom: 28px;
    }

    .shop-toolbar label {
        font-weight: 600;
        color: #253c50;
        margin-bottom: 7px;
    }

    .shop-toolbar .form-control,
    .shop-toolbar .form-select {
        border-radius: 9px;
        min-height: 43px;
    }

    .shop-toolbar .btn {
        min-height: 43px;
        border-radius: 9px;
        padding-left: 20px;
        padding-right: 20px;
    }

    .shop-section-title {
        font-size: 24px;
        font-weight: 750;
        color: #10263e;
        margin: 25px 0 20px;
    }

    /* Convert the existing GridView into a card grid */
    .product-grid {
        display: block;
        width: 100%;
        border: 0;
        background: transparent;
    }

    .product-grid tbody {
        display: grid;
        grid-template-columns: repeat(3, minmax(0, 1fr));
        gap: 24px;
    }

    .product-grid tr {
        display: block;
        min-width: 0;
        border: 1px solid #e2e9ef;
        border-radius: 15px;
        background: #fff;
        overflow: hidden;
        box-shadow: 0 3px 12px rgba(20, 40, 60, 0.05);
        transition: transform .2s ease, box-shadow .2s ease;
    }

    .product-grid tr:hover {
        transform: translateY(-4px);
        box-shadow: 0 10px 28px rgba(20, 40, 60, 0.12);
    }

    .product-grid td {
        display: block;
        padding: 0;
        border: 0;
        height: 100%;
    }

    .product-grid th {
        display: none;
    }

    .shop-product-card {
        height: 100%;
        display: flex;
        flex-direction: column;
    }

    .product-image-wrap {
        height: 220px;
        padding: 22px;
        background: #f8fafc;
        display: flex;
        justify-content: center;
        align-items: center;
        border-bottom: 1px solid #edf1f5;
    }

    .product-image {
        width: 100%;
        height: 100%;
        object-fit: contain;
        transition: transform .25s ease;
    }

    .product-grid tr:hover .product-image {
        transform: scale(1.045);
    }

    .product-info {
        padding: 20px;
        display: flex;
        flex-direction: column;
        flex-grow: 1;
    }

    .product-category {
        color: #008c98;
        font-size: 11px;
        font-weight: 750;
        letter-spacing: 1px;
        text-transform: uppercase;
        margin-bottom: 8px;
    }

    .product-name {
        font-size: 17px;
        font-weight: 700;
        color: #10263e;
        margin-bottom: 10px;
        line-height: 1.4;
    }

    .product-description {
        color: #69798a;
        font-size: 13px;
        line-height: 1.6;
        margin-bottom: 15px;
        overflow-wrap: anywhere;
    }

    .product-price {
        color: #087e8b;
        font-size: 23px;
        font-weight: 800;
        margin-top: auto;
        margin-bottom: 10px;
    }

    .product-price .vat-note {
        display: block;
        font-size: 12px;
        font-weight: 600;
        color: #6b7280;
        margin-top: 2px;
    }

    .product-stock {
        font-size: 12px;
        font-weight: 600;
        margin-bottom: 15px;
    }

    .product-stock.available {
        color: #20845a;
    }

    .product-stock.unavailable {
        color: #b54b4b;
    }

    .product-order-row {
        display: flex;
        gap: 9px;
        margin-bottom: 10px;
    }

    .product-qty {
        width: 65px;
        flex-shrink: 0;
        text-align: center;
        border-radius: 8px;
    }

    .product-add-btn {
        flex: 1;
        background: #008c98;
        color: #fff;
        border: none;
        border-radius: 8px;
        font-weight: 650;
        padding: 10px;
    }

    .product-add-btn:hover {
        background: #006e79;
        color: #fff;
    }

    .product-wishlist-btn {
        width: 100%;
        border-radius: 8px;
        font-weight: 600;
        padding: 9px;
    }

    .product-add-btn:disabled {
        background: #aab7c2;
        cursor: not-allowed;
    }

    .shop-feedback {
        margin-bottom: 20px;
    }

    #backToTopBtn {
        display: none;
        position: fixed;
        bottom: 30px;
        right: 30px;
        z-index: 9999;
        background: #10263e;
        color: white;
        border: none;
        border-radius: 50%;
        width: 54px;
        height: 54px;
        cursor: pointer;
        box-shadow: 0 4px 15px rgba(0,0,0,.2);
        align-items: center;
        justify-content: center;
        font-size: 24px;
    }

    #backToTopBtn:hover {
        background: #008c98;
    }

    @media (max-width: 991px) {
        .product-grid tbody {
            grid-template-columns: repeat(2, minmax(0, 1fr));
        }
    }

    @media (max-width: 600px) {
        .shop-hero {
            padding: 28px 22px;
        }

        .product-grid tbody {
            grid-template-columns: 1fr;
        }

        .shop-toolbar {
            padding: 16px;
        }

        .product-image-wrap {
            height: 240px;
        }
    }
</style>

<div class="shop-page">

    <section class="shop-hero">
        <div class="eyebrow">MULTIGROUP PRODUCT CATALOGUE</div>
        <h1>Explore Our Products</h1>
        <p>
            Discover quality technology, security equipment,
            industrial solutions and essential business products,
            all in one place.
        </p>
        <a href="Cart.aspx" class="shop-cart-link">
            View Shopping Cart &rarr;
        </a>
    </section>

    <asp:UpdatePanel ID="upProducts" runat="server"
        UpdateMode="Conditional">
        <ContentTemplate>

            <div class="shop-toolbar">
                <div class="row g-3 align-items-end">

                    <div class="col-lg-6">
                        <label>Search products</label>
                        <asp:TextBox ID="txtSearch" runat="server"
                            CssClass="form-control"
                            placeholder="Search products or categories..." />
                    </div>

                    <div class="col-lg-3">
                        <label>Category</label>
                        <asp:DropDownList ID="ddlCategory"
                            runat="server"
                            CssClass="form-select"
                            AutoPostBack="true"
                            OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged">
                            <asp:ListItem Text="All Categories" Value="" />
                        </asp:DropDownList>
                    </div>

                    <div class="col-lg-3">
                        <div class="d-flex gap-2">
                            <asp:Button ID="btnSearch" runat="server"
                                Text="Search"
                                CssClass="btn btn-teal flex-grow-1"
                                OnClick="btnSearch_Click" />

                            <asp:Button ID="btnClearSearch"
                                runat="server"
                                Text="Clear"
                                CssClass="btn btn-outline-secondary"
                                OnClick="btnClearSearch_Click"
                                CausesValidation="false" />
                        </div>
                    </div>

                </div>

                <asp:UpdateProgress ID="UpdateProgress1"
                    runat="server"
                    AssociatedUpdatePanelID="upProducts">
                    <ProgressTemplate>
                        <div class="text-muted small mt-2">
                            Loading products...
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </div>

            <div class="shop-feedback">
                <asp:Label ID="lblMessage"
                    runat="server"
                    CssClass="alert alert-danger d-block"
                    Visible="false" />

                <asp:Label ID="lblCartMsg"
                    runat="server"
                    CssClass="alert alert-success d-block"
                    Visible="false" />
            </div>

            <h2 class="shop-section-title">Our Catalogue</h2>

          <asp:GridView ID="gvProducts"
    runat="server"
    CssClass="product-grid"
    AutoGenerateColumns="false"
    ShowHeader="false"
    GridLines="None"
    DataKeyNames="ProductID,Price,ProductName"
    EmptyDataText="No products available."
    AllowPaging="true"
    PageSize="12"
    PagerSettings-Visible="false"
    OnPageIndexChanging="gvProducts_PageIndexChanging"
    OnRowCommand="gvProducts_RowCommand">

                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>

                            <article class="shop-product-card">

                                <div class="product-image-wrap">
                                    <asp:Image ID="imgProduct"
                                        runat="server"
                                        CssClass="product-image"
                                        ImageUrl='<%# Eval("ImagePath") %>'
                                        AlternateText='<%# Eval("ProductName") %>' />
                                </div>

                                <div class="product-info">

                                    <div class="product-category">
                                        <%#: Eval("Category") %>
                                    </div>

                                    <div class="product-name">
                                        <%#: Eval("ProductName") %>
                                    </div>

                                    <div class="product-description">
                                        <%# SafeText(Eval("Description")) %>
                                    </div>

                                    <div class="product-price">
                                        <%# PriceInclVat(Eval("Price")) %>
                                        <span class="vat-note"><%# PriceExclVat(Eval("Price")) %></span>
                                    </div>

                                    <div class='product-stock <%# Convert.ToInt32(Eval("QuantityInStock")) > 0 ? "available" : "unavailable" %>'>
                                        <%# Convert.ToInt32(Eval("QuantityInStock")) > 0
                                            ? "In stock: " + Eval("QuantityInStock") + " available"
                                            : "Out of stock" %>
                                    </div>

                                    <div class="product-order-row">

                                        <asp:TextBox ID="txtQty"
                                            runat="server"
                                            CssClass="form-control product-qty"
                                            TextMode="Number"
                                            min="1"
                                            Text="1" />

                                        <asp:Button ID="btnAddToCart"
                                            runat="server"
                                            CommandName="AddToCart"
                                            CommandArgument='<%# Container.DataItemIndex %>'
                                            Text="Add to Cart"
                                            CssClass="product-add-btn"
                                            Enabled='<%# Convert.ToInt32(Eval("QuantityInStock")) > 0 %>' />

                                    </div>

                                    <asp:Button ID="btnWishlist"
                                        runat="server"
                                        CommandName="AddToWishlist"
                                        CommandArgument='<%# Container.DataItemIndex %>'
                                        Text="&#9829; Add to Wishlist"
                                        CssClass="btn btn-outline-danger product-wishlist-btn"
                                        CausesValidation="false" />

                                </div>

                            </article>

                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            
<div class="shop-pagination mt-4">

    <asp:Label ID="lblProductCount"
        runat="server"
        CssClass="text-muted small d-block text-center mb-3" />

    <div class="d-flex justify-content-center align-items-center gap-3 flex-wrap">

        <asp:Button ID="btnPrevious"
            runat="server"
            Text="← Previous"
            CssClass="btn btn-outline-secondary"
            OnClick="btnPrevious_Click"
            CausesValidation="false" />

        <asp:Label ID="lblPageInfo"
            runat="server"
            CssClass="fw-semibold text-navy" />

        <asp:Button ID="btnNext"
            runat="server"
            Text="Next →"
            CssClass="btn btn-teal"
            OnClick="btnNext_Click"
            CausesValidation="false" />

    </div>
</div>


        </ContentTemplate>
    </asp:UpdatePanel>

</div>

<button type="button" id="backToTopBtn"
    title="Back to Top"
    aria-label="Back to top">
    &#8593;
</button>

<script>
    (function () {
        var button = document.getElementById("backToTopBtn");

        function updateBackToTop() {
            var scrollableHeight =
                document.documentElement.scrollHeight - window.innerHeight;

            button.style.display =
                scrollableHeight > 0 &&
                    window.scrollY >= scrollableHeight / 2
                    ? "flex" : "none";
        }

        window.addEventListener("scroll", updateBackToTop);
        updateBackToTop();

        button.addEventListener("click", function () {
            window.scrollTo({
                top: 0,
                behavior: "smooth"
            });
        });
    })();
</script>

</asp:Content>
