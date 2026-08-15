<%@ Page Title="Contact" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Contact.aspx.cs" Inherits="MultiGroupSystems2.Contact" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        
        .body-content {
            max-width: 100% !important;
            padding-left: 0 !important;
            padding-right: 0 !important;
            margin: 0 !important;
            width: 100% !important;
        }

        .contact-hero {
            background-color: #ADDFFF;
            color: #0f172a;
            width: 100%;
        }


        .hero-circle-wrapper {
            position: relative;
            display: inline-block;
            padding: 35px;
        }


        .hero-circle-frame {            /* circle frme  big*/
            width: 280px;
            height: 280px;
            border-radius: 50%;
            border: 8px solid rgba(255, 255, 255, 0.9);
            overflow: hidden;
            box-shadow: 0 10px 25px rgba(15, 23, 42, 0.15);
        }

        .hero-circle-frame img, 
        .floating-avatar img {
            width: 100%;
            height: 100%;
            object-fit: cover;
        }

        
        .floating-avatar {
            position: absolute;
            border-radius: 50%;
            border: 3px solid #ffffff;
            box-shadow: 0 6px 15px rgba(0, 0, 0, 0.15);
            overflow: hidden;
            background-color: #ffffff;
        }

        
        .avatar-top-left {
            width: 65px;
            height: 65px;
            top: 10px;
            left: 10px;
        }

        .avatar-top-right {
            width: 55px;
            height: 55px;
            top: 15px;
            right: 10px;
        }

        .avatar-bottom-left {
            width: 60px;
            height: 60px;
            bottom: 15px;
            left: 15px;
        }

        .avatar-bottom-right {
            width: 50px;
            height: 50px;
            bottom: 20px;
            right: 20px;
        }

        
        .info-card {                          /* contact section*/
            background-color: #f8fafc;
            border: 1px solid #e2e8f0;
            border-radius: 12px;
            height: 100%;
        }
    </style>


    <div class="contact-hero pt-4 pb-5 px-4 px-md-5">               
        <div class="container-fluid py-3">
            <div class="row align-items-center gy-4">
                
                
                <div class="col-lg-7">            
                    <h1 class="display-4 fw-bold mb-3" style="line-height: 1.15; color: #0f172a;">  <!-- heading -->
                        YOUR NEEDS ARE OUR TOP PRIORITY
                    </h1>
                    <p class="fs-5 mb-0" style="color: #334155; font-weight: 500;">
                        Choose one of the many ways to get in touch with our team.                 <!--  sub heading (can remove later) -->
                    </p>
                </div>

                
                <div class="col-lg-5 text-center">
                    <div class="hero-circle-wrapper">
                        
                        <!-- pictures -->
                        <div class="hero-circle-frame">
                            <img src="Images/Contact!.jpg" alt="Customer Support Representative" />
                        </div>

                        
                        <div class="floating-avatar avatar-top-left">
                            <img src="Images/Contact2.jpg" alt="Support Team Member 1" />
                        </div>

                        
                        <div class="floating-avatar avatar-top-right">
                            <img src="Images/Contact3.jpg" alt="Support Team Member 2" />
                        </div>

                        
                        <div class="floating-avatar avatar-bottom-left">
                            <img src="Images/contact4.jpg" alt="Support Team Member 3" />
                        </div>

                        
                        <div class="floating-avatar avatar-bottom-right">
                            <img src="Images/contact5.jpg" alt="Support Team Member 4" />
                        </div>

                    </div>
                </div>

            </div>
        </div>
    </div>

    
    <div class="py-5 px-4 px-md-5 bg-white">
        <div class="container-fluid">
            <div class="row gy-4">
                
                
                <div class="col-12 mb-2">
                    <h2 class="display-6 fw-bold text-dark mb-1">Get In Touch</h2>
                    <p class="text-secondary fs-5">Have a question or request? Reach out to our team directly.</p>
                </div>

                
                <div class="col-lg-4 col-md-6">
                    <div class="info-card p-4">
                        <h4 class="fw-bold text-dark mb-3">General Enquiries</h4>
                        <p class="mb-2"><strong>Lee Ann Pillay:</strong> +27 679 483 379 </p>
                        <p class="mb-2"><strong>Peter Trousedale:</strong> +27 836 501 682</p>
                        <p class="mb-2"><strong>Operating Hours:</strong> Mon - Fri: 8:00 AM - 5:00 PM</p>
                    </div>
                </div>

                
                <div class="col-lg-4 col-md-6">
                    <div class="info-card p-4">
                        <h4 class="fw-bold text-dark mb-3">Departmental Emails</h4>
                        <p class="mb-2"><strong>Lee Ann Pillay:</strong>  lee-ann@multigroupinc.com</p>
                        <p class="mb-2"><strong>Peter Trousedale:</strong> peter@multigroupinc.com</p>
                    </div>
                </div>

                
                <div class="col-lg-4 col-md-12">
                    <div class="info-card p-4">
                        <h4 class="fw-bold text-dark mb-3">Regional Offices</h4>

                        <p class="mb-2"><strong>Johannesburgh:</strong> +27 11 467 1590</p>
                        <p class="mb-2"><strong>Maputo, Mozambique:</strong> +258 20 608 160</p>
                        <p class="mb-2"><strong>Mbabane, Kingdom of Eswatini:</strong> +268 2404 7556 </p>
                    </div>
                </div>

            </div>
        </div>
    </div>
</asp:Content>
