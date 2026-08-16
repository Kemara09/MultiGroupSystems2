<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="MultiGroupSystemsTester._Default" %>

<asp:Content ID="TitleContentPage" ContentPlaceHolderID="TitleContent" runat="server">MultigroupSystems - Home</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

   <style>
    
    .body-content {
        max-width: 100% !important;
        padding-left: 0 !important;
        padding-right: 0 !important;
        margin: 0 !important;
        width: 100% !important;
    }

    
    .hero-video-section {
        position: relative;
        width: 100%;
        height: 85vh;
        min-height: 550px;
        overflow: hidden;
        background-color: #0f172a;
        color: #ffffff;
        display: flex;
        align-items: center;
    }

    .bg-video {
        position: absolute;
        top: 50%;
        left: 50%;
        width: 100%;
        height: 100%;
        object-fit: cover;
        transform: translate(-50%, -50%);
        z-index: 1;
    }

   
    .video-overlay {
        position: absolute;
        top: 0;
        left: 0;
        width: 100%;
        height: 100%;
        background: linear-gradient(90deg, rgba(15, 23, 42, 0.92) 0%, rgba(15, 23, 42, 0.65) 50%, rgba(15, 23, 42, 0.4) 100%);
        z-index: 2;
    }

    
    .hero-content {
        position: relative;
        z-index: 3;
        width: 100%;
    }
</style>


<div class="hero-video-section">
    
    
    <video class="bg-video" autoplay loop muted playsinline>
        <source src="Images/homePage.mp4" type="video/mp4" />
        Your browser does not support the video tag.
    </video>

    
    <div class="video-overlay"></div>

    
    <div class="hero-content px-4 px-md-5">
        <div class="container-fluid">
            <div class="row">
                <div class="col-lg-7 col-xl-6">
                    
                    <h1 class="display-3 fw-bold mb-4" style="line-height: 1.1; letter-spacing: -1px;">
                        MULTIGROUP INCORPORATED 
                    </h1>
                    
                    <p class="fs-5 text-white-50 mb-4" style="max-width: 540px;">
                        A leading provider of comprehensive security solutions and IT services
                    </p>

                </div>
            </div>
        </div>
    </div>

</div>
    <section class="hero-section text-white text-center">
        <div class="container py-5">
            <h1 class="display-4 fw-bold">Welcome to MultigroupSystems</h1>
            <p class="lead">Streamlined order management, built for reliability and speed.</p>
            <a href="~/Products.aspx" runat="server" class="btn btn-lg btn-primary mt-3">Shop Now</a>
        </div>
    </section>

    <section class="container py-5">
        <div class="row text-center">
            <div class="col-md-4 mb-4">
                <div class="feature-card p-4 h-100">
                    <h4>Order Tracking</h4>
                    <p>Real-time visibility into every order, from placement to delivery.</p>
                </div>
            </div>
            <div class="col-md-4 mb-4">
                <div class="feature-card p-4 h-100">
                    <h4>Secure Access</h4>
                    <p>Role-based logins keep customer and company data protected.</p>
                </div>
            </div>
            <div class="col-md-4 mb-4">
                <div class="feature-card p-4 h-100">
                    <h4>Reliable Reporting</h4>
                    <p>Business intelligence built directly on live system data.</p>
                </div>
            </div>
        </div>
    </section>

    <section class="cta-section text-center py-5">
        <div class="container">
            <h2>Ready to see it in action?</h2>
            <p class="mb-4">Contact us to learn how MultigroupSystems can support your operations.</p>
            <a href="~/Contact.aspx" runat="server" class="btn btn-outline-primary btn-lg">Contact Us</a>
        </div>
    </section>

</asp:Content>