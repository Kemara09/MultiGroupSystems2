<%@ Page Title="Your Cart" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Cart.aspx.cs" Inherits="MultiGroupSystemsTester.Cart" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Your Cart</h1>
    <asp:Label ID="lblEmpty" runat="server" CssClass="text-muted" Visible="false" Text="Your cart is empty."></asp:Label>

    <asp:GridView ID="gvCart" runat="server" CssClass="table align-middle" AutoGenerateColumns="false"
        OnRowCommand="gvCart_RowCommand">
        <Columns>
            <asp:BoundField DataField="ProductName" HeaderText="Product" />
            <asp:TemplateField HeaderText="Qty">
                <ItemTemplate>
                    <div class="d-flex align-items-center gap-2">
                        <asp:Button runat="server" CommandName="Decrease" CommandArgument='<%# Container.DataItemIndex %>'
                            Text="-" CssClass="btn btn-outline-secondary btn-sm" />
                        <span style="min-width:24px; text-align:center; display:inline-block;">
                            <%# Eval("Quantity") %>
                        </span>
                        <asp:Button runat="server" CommandName="Increase" CommandArgument='<%# Container.DataItemIndex %>'
                            Text="+" CssClass="btn btn-outline-secondary btn-sm" />
                    </div>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="UnitPrice" HeaderText="Unit Price" DataFormatString="{0:C}" />
            <asp:BoundField DataField="Subtotal" HeaderText="Subtotal" DataFormatString="{0:C}" />
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Button runat="server" CommandName="Remove" CommandArgument='<%# Container.DataItemIndex %>'
                        Text="Remove" CssClass="btn btn-outline-secondary btn-sm" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>

    <p class="fs-5 mt-3">Total: <strong><asp:Label ID="lblTotal" runat="server" /></strong></p>

    <a href="Products.aspx" runat="server" class="btn btn-outline-secondary">Continue shopping</a>
    <asp:HyperLink ID="lnkCheckout" runat="server" NavigateUrl="~/Checkout.aspx" CssClass="btn btn-teal">Proceed to checkout</asp:HyperLink>
</asp:Content>