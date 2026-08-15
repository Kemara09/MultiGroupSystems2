<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="MultiGroupSystems2._Default" %>

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

</asp:Content>
