<%@ Page Title="My Orders" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MyOrders.aspx.cs" Inherits="MultiGroupSystemsTester.MyOrders" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>My Orders</h1>
    <asp:Label ID="lblMessage" runat="server" CssClass="alert alert-info d-block" Visible="false"></asp:Label>
    <asp:GridView ID="gvOrders" runat="server" CssClass="table table-striped mt-3" AutoGenerateColumns="false" Visible="false">
        <Columns>
            <asp:BoundField DataField="OrderID" HeaderText="Order #" />
            <asp:BoundField DataField="OrderDate" HeaderText="Date" DataFormatString="{0:dd MMM yyyy}" />
            <asp:BoundField DataField="TotalAmount" HeaderText="Total" DataFormatString="{0:C}" />
            <asp:BoundField DataField="OrderStatus" HeaderText="Order Status" />
            <asp:BoundField DataField="DeliveryStatus" HeaderText="Delivery Status" />
            <asp:TemplateField HeaderText="">
                <ItemTemplate>
                    <a href='TrackDelivery.aspx?orderId=<%# Eval("OrderID") %>' class="btn btn-sm btn-teal">Track</a>
                    <a href='InvoiceDownload.ashx?orderId=<%# Eval("OrderID") %>' target="_blank" class="btn btn-sm btn-navy">Invoice</a>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</asp:Content>