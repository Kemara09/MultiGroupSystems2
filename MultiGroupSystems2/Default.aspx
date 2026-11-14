<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="MultiGroupSystemsTester._Default" %>

<asp:Content ID="TitleContentPage" ContentPlaceHolderID="TitleContent" runat="server">Multigroup Inc. - Home</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

  <div class="home-page">
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
        <h1 class="display-4 fw-bold">Welcome to Multigroup Inc.</h1>
        <p class="lead">Browse our range, order online and get it delivered or collect it at a given pick up point.</p>
        <a href="~/Products.aspx" runat="server" class="btn btn-lg btn-primary mt-3">Shop Now</a>
    </div>
</section>

   <section class="container py-5">
    <div class="row text-center">
        <div class="col-md-4 mb-4">
            <a href="~/Products.aspx?category=YourTopCategory" runat="server" class="text-decoration-none d-block h-100">
                <div class="feature-card p-4 h-100 feature-card-link">
                    <h4>Popular Picks</h4>
                    <p>See what other customers are buying most. <span class="card-arrow">Browse now &rarr;</span></p>
                </div>
            </a>
        </div>
        <div class="col-md-4 mb-4">
            <a href="~/DeliveryInfo.aspx" runat="server" class="text-decoration-none d-block h-100">
                <div class="feature-card p-4 h-100 feature-card-link">
                    <h4>Delivery You Can Count On</h4>
                    <p>Find out how fast we deliver and how we handle returns. <span class="card-arrow">See how it works &rarr;</span></p>
                </div>
            </a>
        </div>
        <div class="col-md-4 mb-4">
    <a href="~/Account/Register.aspx" runat="server" class="text-decoration-none d-block h-100">
        <div class="feature-card p-4 h-100 feature-card-link">
            <h4>Create Your Free Account</h4>
            <p>Check out faster and get delivery updates. <span class="card-arrow">Sign up &rarr;</span></p>
        </div>
    </a>
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
      </div>

</asp:Content>