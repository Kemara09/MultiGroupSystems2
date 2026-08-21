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


        .popup-trigger {
            color: #0284c7;
            font-weight: 600;
            cursor: pointer;
            text-decoration: underline;
        }

        .popup-trigger:hover {
            color: #0369a1;
        }

        .modal-overlay {
            display: none;
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            background-color: rgba(0, 0, 0, 0.5);
            z-index: 1050;
            justify-content: center;
            align-items: center;
        }

        .modal-card {
            background: #ffffff;
            width: 100%;
            max-width: 500px;
            padding: 30px;
            border-radius: 16px;
            box-shadow: 0 10px 25px rgba(0,0,0,0.2);
            position: relative;
            box-sizing: border-box;
        }

        .modal-close {
            position: absolute;
            top: 15px;
            right: 20px;
            font-size: 24px;
            font-weight: bold;
            color: #94a3b8;
            cursor: pointer;
        }

        .modal-close:hover {
            color: #111827;
        }

        .modal-card h2 {
            font-size: 1.4rem;
            font-weight: 700;
            color: #111827;
            margin-bottom: 20px;
            text-align: center;
        }
        .form-group {
            margin-bottom: 15px;
            text-align: left;
        }

        .form-group label {
            display: block;
            font-size: 0.85rem;
            font-weight: 600;
            color: #475569;
            margin-bottom: 5px;
        }

        .form-control-custom {
            width: 100%;
            padding: 10px 14px;
            border: 1px solid #cbd5e1;
            border-radius: 6px;
            font-size: 0.95rem;
            outline: none;
            box-sizing: border-box;
        }

        .form-control-custom:focus {
            border-color: #0284c7;
        }

        textarea.form-control-custom {
            resize: vertical;
            height: 100px;
        }

        .btn-submit {
            width: 100%;
            background-color: #2563eb;
            color: white;
            border: none;
            padding: 12px;
            border-radius: 6px;
            font-weight: 600;
            font-size: 1rem;
            cursor: pointer;
            margin-top: 10px;
            transition: background 0.2s;
        }

        .btn-submit:hover {
            background-color: #1d4ed8;
        }

        .error-text {
            color: #dc2626;
            font-size: 0.75rem;
            margin-top: 3px;
            display: block;
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
                   <p class="text-secondary fs-5"> Have a question or request? <a href="javascript:void(0);" class="popup-trigger" onclick="openModal()">Click here</a> to send us a direct inquiry.
                   </p>
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

   <asp:Panel ID="pnlModal" runat="server" CssClass="modal-overlay" style="display: none;">
        <div class="modal-card">
           <button type="button" class="modal-close" onclick="closeModal()">&times;</button>
            <h2>Need help with your request?</h2>
            
            
                <div class="form-group">
                <label>First Name</label>
                <asp:TextBox ID="txtFirstName" runat="server" CssClass="form-control-custom" placeholder="Enter your first name"></asp:TextBox>
            </div>

            <div class="form-group">
                <label>Surname</label>
                <asp:TextBox ID="txtSurname" runat="server" CssClass="form-control-custom" placeholder="Enter your surname"></asp:TextBox>
            </div>

            <div class="form-group">
                <label>Email <span style="color:red;">*</span></label>
                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control-custom" TextMode="Email" placeholder="Enter your email address"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail" 
                    ErrorMessage="Email is required." CssClass="error-text" Display="Dynamic" ValidationGroup="ContactGroup" />
            </div>

            <div class="form-group">
                <label>Message / Request <span style="color:red;">*</span></label>
                <asp:TextBox ID="txtMessage" runat="server" CssClass="form-control-custom" TextMode="MultiLine" placeholder="Type your message here..."></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvMessage" runat="server" ControlToValidate="txtMessage" 
                    ErrorMessage="Message is required." CssClass="error-text" Display="Dynamic" ValidationGroup="ContactGroup" />
            </div>

            <asp:Button ID="btnSubmit" runat="server" Text="SUBMIT" CssClass="btn-submit" 
                ValidationGroup="ContactGroup" OnClick="btnSubmit_Click" />

            <asp:Label ID="lblStatus" runat="server" CssClass="d-block text-success mt-2 text-center font-weight-bold" />

        </div>
    </asp:Panel>


    <script>
    function openModal() {
        document.getElementById('<%= pnlModal.ClientID %>').style.display = 'flex';
    }

    function closeModal() {
        document.getElementById('<%= pnlModal.ClientID %>').style.display = 'none';
    }
    </script>







</asp:Content>
