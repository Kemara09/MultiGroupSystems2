<%@ Page Title="Delivery & Returns" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DeliveryInfo.aspx.cs" Inherits="MultiGroupSystemsTester.DeliveryInfo" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <section class="container py-5">
        <div class="text-center text-white mb-5">
            <h1 class="display-5 fw-bold">Delivery &amp; Returns</h1>
            <p class="lead">Here's what to expect after you place an order.</p>
        </div>

        <div class="row">
            <div class="col-md-6 col-lg-3 mb-4">
                <div class="feature-card p-4 h-100">
                    <h4>Free Delivery</h4>
                    <p>Delivery is free on your order. Pick a delivery date at checkout, and we deliver between 7am and 7pm.</p>
                </div>
            </div>
            <div class="col-md-6 col-lg-3 mb-4">
                <div class="feature-card p-4 h-100">
                    <h4>Collect In Store</h4>
                    <p>Prefer to pick up? Choose Collection at checkout, select your province and collect from our pickup points, Monday to Friday, 10:00 to 17:00.</p>
                </div>
            </div>
            <div class="col-md-6 col-lg-3 mb-4">
                <div class="feature-card p-4 h-100">
                    <h4>Pay Your Way</h4>
                    <p>Pay by credit or debit card, EFT, or cash on delivery.</p>
                </div>
            </div>
            <div class="col-md-6 col-lg-3 mb-4">
                <div class="feature-card p-4 h-100">
                    <h4>Returns</h4>
                    <p>Return unused items within 7 days for a refund or exchange.Click Contact Us to start the Process</p>
                </div>
            </div>
        </div>

        <div class="text-center mt-4">
            <a href="~/Products.aspx" runat="server" class="btn btn-lg btn-primary me-2">Start Shopping</a>
            <a href="~/Contact.aspx" runat="server" class="btn btn-lg btn-outline-primary">Questions? Contact Us</a>
        </div>
    </section>
</asp:Content>