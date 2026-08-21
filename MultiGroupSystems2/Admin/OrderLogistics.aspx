<%@ Page Title="Order Logistics" Language="C#" AutoEventWireup="true" MasterPageFile="~/Admin/AdminMaster.master" CodeBehind="OrderLogistics.aspx.cs" Inherits="MultiGroupSystemsTester.Admin.OrderLogistics" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="OrderLogistics.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BreadcrumbContent" runat="server">
    Order Logistics
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <section class="page-heading">
        <div>
            <h1>Order Logistics</h1>
            <p>Manage delivery, shipping and order logistics information.</p>
        </div>
        <div class="order-status">
            <span class="status-label">Order Status</span>
            <span class="status-badge">In Transit</span>
        </div>
    </section>

    <section class="summary-grid">
        <div class="summary-card">
            <div class="summary-icon blue">#</div>
            <div><span class="summary-label">Order Number</span><strong>ORD-2026-0048</strong></div>
        </div>
        <div class="summary-card">
            <div class="summary-icon green">✓</div>
            <div><span class="summary-label">Delivery Status</span><strong>In Transit</strong></div>
        </div>
        <div class="summary-card">
            <div class="summary-icon orange">◷</div>
            <div><span class="summary-label">Expected Delivery</span><strong>25 August 2026</strong></div>
        </div>
        <div class="summary-card">
            <div class="summary-icon purple">🚚</div>
            <div><span class="summary-label">Courier</span><strong>FastTrack Logistics</strong></div>
        </div>
    </section>

    <section class="content-grid">
        <div class="form-container">
            <div class="form-header">
                <div class="form-header-icon">🚚</div>
                <div>
                    <h2>Logistics Information</h2>
                    <p>Enter the delivery and shipping details for this order.</p>
                </div>
            </div>

            <div class="form-body">
                <div class="section-title">
                    <span class="section-number">1</span><span>Order Information</span>
                </div>

                <div class="form-row">
                    <div class="form-group">
                        <label>Order Number</label>
                        <input type="text" value="ORD-2026-0048" placeholder="Enter order number" />
                    </div>
                    <div class="form-group">
                        <label>Customer Name</label>
                        <input type="text" value="ABC Business Solutions" placeholder="Enter customer name" />
                    </div>
                </div>

                <div class="form-row">
                    <div class="form-group">
                        <label>Order Date</label>
                        <input type="date" value="2026-08-18" />
                    </div>
                    <div class="form-group">
                        <label>Priority</label>
                        <select>
                            <option>Normal</option>
                            <option>Low</option>
                            <option>High</option>
                            <option>Urgent</option>
                        </select>
                    </div>
                </div>

                <div class="section-title section-spacing">
                    <span class="section-number">2</span><span>Delivery Information</span>
                </div>

                <div class="form-group full-width">
                    <label>Delivery Address</label>
                    <textarea rows="3" placeholder="Enter delivery address">45 Business Park Road, Johannesburg</textarea>
                </div>

                <div class="form-row">
                    <div class="form-group">
                        <label>City</label>
                        <input type="text" value="Johannesburg" placeholder="Enter city" />
                    </div>
                    <div class="form-group">
                        <label>Postal Code</label>
                        <input type="text" value="2001" placeholder="Enter postal code" />
                    </div>
                </div>

                <div class="form-row">
                    <div class="form-group">
                        <label>Expected Delivery Date</label>
                        <input type="date" value="2026-08-25" />
                    </div>
                    <div class="form-group">
                        <label>Shipping Method</label>
                        <select>
                            <option>Standard Delivery</option>
                            <option>Express Delivery</option>
                            <option>Same Day Delivery</option>
                            <option>Collection</option>
                        </select>
                    </div>
                </div>

                <div class="section-title section-spacing">
                    <span class="section-number">3</span><span>Shipping Information</span>
                </div>

                <div class="form-row">
                    <div class="form-group">
                        <label>Courier / Logistics Provider</label>
                        <select>
                            <option>FastTrack Logistics</option>
                            <option>DHL</option>
                            <option>FedEx</option>
                            <option>Local Courier</option>
                            <option>Other</option>
                        </select>
                    </div>
                    <div class="form-group">
                        <label>Tracking Number</label>
                        <input type="text" value="FTL-84573920" placeholder="Enter tracking number" />
                    </div>
                </div>

                <div class="form-row">
                    <div class="form-group">
                        <label>Package Type</label>
                        <select>
                            <option>Standard Box</option>
                            <option>Large Box</option>
                            <option>Envelope</option>
                            <option>Pallet</option>
                            <option>Other</option>
                        </select>
                    </div>
                    <div class="form-group">
                        <label>Number of Packages</label>
                        <input type="number" value="3" placeholder="Enter quantity" />
                    </div>
                </div>

                <div class="section-title section-spacing">
                    <span class="section-number">4</span><span>Additional Information</span>
                </div>

                <div class="form-group full-width">
                    <label>Logistics Notes</label>
                    <textarea rows="4" placeholder="Enter additional logistics information">Customer requested delivery during business hours.</textarea>
                </div>

                <div class="form-actions">
                    <button type="button" class="cancel-button">Cancel</button>
                    <button type="button" class="update-button">✓ Update Logistics</button>
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
                    <div class="progress-step completed">
                        <div class="progress-icon">✓</div>
                        <div><strong>Order Placed</strong><small>18 Aug 2026</small></div>
                    </div>
                    <div class="progress-line completed-line"></div>
                    <div class="progress-step completed">
                        <div class="progress-icon">✓</div>
                        <div><strong>Processing</strong><small>19 Aug 2026</small></div>
                    </div>
                    <div class="progress-line completed-line"></div>
                    <div class="progress-step active-step">
                        <div class="progress-icon">🚚</div>
                        <div><strong>In Transit</strong><small>Current Status</small></div>
                    </div>
                    <div class="progress-line"></div>
                    <div class="progress-step">
                        <div class="progress-icon">4</div>
                        <div><strong>Delivered</strong><small>Expected 25 Aug</small></div>
                    </div>
                </div>
            </div>

            <div class="tracking-card">
                <div class="card-title">
                    <h3>Tracking Details</h3>
                    <span class="tracking-icon">#</span>
                </div>
                <div class="tracking-number">FTL-84573920</div>
                <p>Tracking number</p>
                <div class="tracking-info">
                    <div><span>Courier</span><strong>FastTrack Logistics</strong></div>
                    <div><span>Service</span><strong>Standard Delivery</strong></div>
                </div>
                <button type="button" class="tracking-button">View Tracking</button>
            </div>

            <div class="info-card">
                <div class="info-icon">i</div>
                <div>
                    <strong>Logistics Reminder</strong>
                    <p>Ensure the tracking number and delivery information are correct before updating the order.</p>
                </div>
            </div>
        </div>
    </section>

</asp:Content>