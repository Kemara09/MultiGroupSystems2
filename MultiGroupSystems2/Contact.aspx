<%@ Page Title="Contact" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Contact.aspx.cs" Inherits="MultiGroupSystems2.Contact" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
   
    <div class="contact-page">
    <div class="contact-hero pt-4 pb-5 px-4 px-md-5">               
        <div class="container-fluid py-3">
            <div class="row align-items-center gy-4">
                
                
                <div class="col-lg-7">            
                    <h1 class="display-4 fw-bold mb-3" style="line-height: 1.15; color: #0f172a;">  
                        YOUR NEEDS ARE OUR TOP PRIORITY
                    </h1>
                    <p class="fs-5 mb-0" style="color: #334155; font-weight: 500;">
                        Choose one of the many ways to get in touch with our team.                
                    </p>
                </div>

                
                <div class="col-lg-5 text-center">
                    <div class="hero-circle-wrapper">
                        
                        <!-- picturee -->
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

                        <p class="mb-2"><strong>Johannesburg:</strong> +27 11 467 1590</p>
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
                <asp:TextBox ID="txtMessage" runat="server" CssClass="form-control-custom" TextMode="MultiLine" placeholder="Type your message here"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvMessage" runat="server" ControlToValidate="txtMessage" 
                    ErrorMessage="Message is required." CssClass="error-text" Display="Dynamic" ValidationGroup="ContactGroup" />
            </div>

            <asp:Button ID="btnSubmit" runat="server" Text="SUBMIT" CssClass="btn-submit" 
                ValidationGroup="ContactGroup" OnClick="btnSubmit_Click" />

            <asp:Label ID="lblStatus" runat="server" CssClass="d-block text-success mt-2 text-center font-weight-bold" />

        </div>
    </asp:Panel>

        </div>


    <script>
    function openModal() {
        document.getElementById('<%= pnlModal.ClientID %>').style.display = 'flex';
    }

    function closeModal() {
        document.getElementById('<%= pnlModal.ClientID %>').style.display = 'none';
    }
    </script>







</asp:Content>
