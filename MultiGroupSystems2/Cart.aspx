<%@ Page Title="Your Cart" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Cart.aspx.cs" Inherits="MultiGroupSystemsTester.Cart" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1 class="mb-4">Shopping Cart</h1>
    <asp:Label ID="lblEmpty" runat="server" CssClass="text-muted" Visible="false" Text="Your cart is empty."></asp:Label>

    <div class="row" runat="server" id="cartRow">
        <div class="col-lg-8">
            <asp:Repeater ID="rptCart" runat="server" OnItemCommand="rptCart_ItemCommand">
                <ItemTemplate>
                    <div class="cart-item-card mb-3 p-3">
                        <div class="row align-items-center g-3">
                            <div class="col-auto">
                                <div class="cart-item-thumb">
                                    <%# Eval("ProductName").ToString().Substring(0, 1).ToUpper() %>
                                </div>
                            </div>
                            <div class="col">
                                <div class="fw-semibold"><%# Eval("ProductName") %></div>
                                <div class="text-muted small">Unit Price: R <%# Eval("UnitPrice", "{0:N2}") %></div>
                            </div>
                            <div class="col-auto">
                                <div class="d-flex align-items-center gap-2">
                                    <asp:Button runat="server" CommandName="Decrease" CommandArgument='<%# Container.ItemIndex %>'
                                        Text="-" CssClass="btn btn-outline-secondary btn-sm" />
                                    <span class="qty-value"><%# Eval("Quantity") %></span>
                                    <asp:Button runat="server" CommandName="Increase" CommandArgument='<%# Container.ItemIndex %>'
                                        Text="+" CssClass="btn btn-outline-secondary btn-sm" />
                                </div>
                            </div>
                            <div class="col-auto text-end" style="min-width:100px;">
                                <div class="fw-semibold">R <%# Eval("Subtotal", "{0:N2}") %></div>
                            </div>
                            <div class="col-auto">
                                <asp:Button runat="server" CommandName="Remove" CommandArgument='<%# Container.ItemIndex %>'
                                    Text="Remove" CssClass="btn btn-outline-secondary btn-sm" />
                            </div>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>

            <a href="Products.aspx" runat="server" class="btn btn-outline-secondary mt-2">Continue shopping</a>
        </div>

        <div class="col-lg-4">
            <div class="cart-summary-card p-4">
                <h5 class="mb-3">Cart Summary</h5>
                <div class="d-flex justify-content-between mb-3">
                    <span>Total</span>
                    <strong><asp:Label ID="lblTotal" runat="server" /></strong>
                </div>
                <asp:HyperLink ID="lnkCheckout" runat="server" NavigateUrl="~/Checkout.aspx" CssClass="btn btn-teal w-100">Proceed to Checkout</asp:HyperLink>
            </div>
        </div>
    </div>

    <style>
        .cart-item-card {
            background: #ffffff;
            color: #172b3a; /* dark text so it stays readable in dark mode */
            border: 1px solid #E3E1DA;
            border-radius: 8px;
        }
        .cart-item-thumb {
            width: 56px;
            height: 56px;
            border-radius: 6px;
            background: #0F6E56;
            color: #ffffff;
            display: flex;
            align-items: center;
            justify-content: center;
            font-weight: 600;
            font-size: 1.25rem;
        }
        .qty-value {
            min-width: 24px;
            text-align: center;
            display: inline-block;
        }
        .cart-summary-card {
            background: #ffffff;
            color: #172b3a; /* dark text so it stays readable in dark mode */
            border: 1px solid #E3E1DA;
            border-radius: 8px;
        }
        /* Keep the "Unit Price" grey readable inside the white cards */
        .cart-item-card .text-muted,
        .cart-summary-card .text-muted {
            color: #6b7280 !important;
        }
    </style>
</asp:Content>
