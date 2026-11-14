<%@ Page Title="Order Logistics" Language="C#" AutoEventWireup="true" MasterPageFile="~/Admin/AdminMaster.master" CodeBehind="OrderLogistics.aspx.cs" Inherits="MultiGroupSystemsTester.Admin.OrderLogistics" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="OrderLogistics.css?v=3" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BreadcrumbContent" runat="server">
    Order Logistics
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <a class="lg-back" href="ViewOrders.aspx">&larr; Back to all orders</a>
    <div id="msgBox" runat="server" visible="false"></div>

    <asp:PlaceHolder ID="phContent" runat="server">

        <section class="page-heading">
            <div>
                <h1>Order Logistics</h1>
                <p>Review the delivery details and update the order and delivery status.</p>
            </div>
            <div class="order-status">
                <span class="status-label">Order Status</span>
                <span id="statusBadge" runat="server" class="status-badge"><asp:Literal ID="litStatusBadge" runat="server" Mode="Encode" /></span>
            </div>
        </section>

        <section class="summary-grid">
            <div class="summary-card">
                <div class="summary-icon blue">#</div>
                <div><span class="summary-label">Order Number</span><strong><asp:Literal ID="litSumOrder" runat="server" Mode="Encode" /></strong></div>
            </div>
            <div class="summary-card">
                <div class="summary-icon green">✓</div>
                <div><span class="summary-label">Delivery Status</span><strong><asp:Literal ID="litSumStatus" runat="server" Mode="Encode" /></strong></div>
            </div>
            <div class="summary-card">
                <div class="summary-icon orange">◷</div>
                <div><span class="summary-label">Expected Delivery</span><strong><asp:Literal ID="litSumDate" runat="server" Mode="Encode" /></strong></div>
            </div>
            <div class="summary-card">
                <div class="summary-icon purple">R</div>
                <div><span class="summary-label">Order Total</span><strong><asp:Literal ID="litSumTotal" runat="server" Mode="Encode" /></strong></div>
            </div>
        </section>

        <section class="content-grid">
            <div class="form-container">
                <div class="form-header">
                    <div class="form-header-icon">🚚</div>
                    <div>
                        <h2>Logistics Information</h2>
                        <p>Update the order and delivery status as the order moves to its next stage.</p>
                    </div>
                </div>

                <div class="form-body">
                    <div class="section-title">
                        <span class="section-number">1</span><span>Order Information</span>
                    </div>

                    <div class="form-row">
                        <div class="form-group">
                            <label>Order Number</label>
                            <asp:TextBox ID="txtOrderNumber" runat="server" ReadOnly="true" />
                        </div>
                        <div class="form-group">
                            <label>Customer Name</label>
                            <asp:TextBox ID="txtCustomerName" runat="server" ReadOnly="true" />
                        </div>
                    </div>

                    <div class="form-row">
                        <div class="form-group">
                            <label>Order Date</label>
                            <asp:TextBox ID="txtOrderDate" runat="server" TextMode="Date" ReadOnly="true" />
                        </div>
                        <div class="form-group">
                            <label>Payment Method</label>
                            <asp:TextBox ID="txtPayment" runat="server" ReadOnly="true" />
                        </div>
                    </div>

                    <div class="section-title section-spacing">
                        <span class="section-number">2</span><span>Delivery Information</span>
                    </div>

                    <div class="form-group full-width">
                        <label>Delivery Address</label>
                        <asp:TextBox ID="txtAddress" runat="server" TextMode="MultiLine" Rows="3" ReadOnly="true" />
                    </div>

                    <div class="form-row">
                        <div class="form-group">
                            <label>Delivery Phone</label>
                            <asp:TextBox ID="txtPhone" runat="server" ReadOnly="true" />
                        </div>
                        <div class="form-group">
                            <label>Expected Delivery Date</label>
                            <asp:TextBox ID="txtDeliveryDate" runat="server" TextMode="Date" ReadOnly="true" />
                        </div>
                    </div>

                    <div class="form-group full-width">
                        <label>Customer Delivery Instructions</label>
                        <asp:TextBox ID="txtCustomerInstructions" runat="server" TextMode="MultiLine" Rows="2" ReadOnly="true" />
                    </div>

                    <div class="section-title section-spacing">
                        <span class="section-number">3</span><span>Update Status</span>
                    </div>

                    <div class="form-row">
                        <div class="form-group">
                            <label>Order Status</label>
                            <asp:DropDownList ID="ddlOrderStatus" runat="server" />
                        </div>
                        <div class="form-group">
                            <label>Delivery Status</label>
                            <asp:DropDownList ID="ddlDeliveryStatus" runat="server" />
                        </div>
                    </div>

                    <div class="form-actions">
                        <button type="button" runat="server" id="btnCancel" class="cancel-button" onserverclick="btnCancel_Click">Cancel</button>
                        <button type="submit" runat="server" id="btnUpdate" class="update-button" onserverclick="btnUpdate_Click">✓ Update Status</button>
                    </div>
                </div>
            </div>

            <div class="right-column">
                <div class="status-card">
                    <div class="card-title">
                        <h3>Delivery Status</h3>
                        <span class="live-dot">● Live</span>
                    </div>
                    <div class="progress-container">
                        <asp:Literal ID="ltTimeline" runat="server" />
                    </div>
                </div>

                <div class="info-card">
                    <div class="info-icon">i</div>
                    <div>
                        <strong>Logistics Reminder</strong>
                        <p>Only update the status once the order has actually reached that stage. The timeline above follows the delivery status and updates when you save.</p>
                    </div>
                </div>
            </div>
        </section>

    </asp:PlaceHolder>

</asp:Content>