<%@ Page Title="View Orders" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" CodeBehind="ViewOrders.aspx.cs" Inherits="MultiGroupSystemsTester.ViewOrders" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="ViewOrders.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="vo-wrapper">
        <h2 class="vo-title">All Orders</h2>

        <div class="vo-filters">
            <asp:TextBox ID="txtSearch" runat="server" CssClass="vo-input" placeholder="Search by order ID, customer ID or name" />
            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="vo-input">
                <asp:ListItem Text="All statuses" Value="" />
            </asp:DropDownList>
            <asp:Button ID="btnFilter" runat="server" Text="Filter" CssClass="vo-btn" OnClick="btnFilter_Click" />
            <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="vo-btn vo-btn-secondary" OnClick="btnClear_Click" />
        </div>

        <asp:Label ID="lblMessage" runat="server" CssClass="vo-message" Visible="false" />

        <div class="vo-table-wrap">
            <asp:GridView ID="gvOrders" runat="server"
                AutoGenerateColumns="false"
                CssClass="vo-grid"
                AllowPaging="true" PageSize="15"
                OnPageIndexChanging="gvOrders_PageIndexChanging"
                EmptyDataText="No orders found."
                GridLines="None">
                <Columns>
                    <asp:BoundField DataField="OrderID" HeaderText="Order #" />
                    <asp:TemplateField HeaderText="Customer">
                        <ItemTemplate>
                            <%# Eval("CustomerName") %>
                            <br />
                            <small>ID: <%# Eval("CustomerID") %></small>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="OrderDate" HeaderText="Order Date" DataFormatString="{0:yyyy-MM-dd HH:mm}" />
                    <asp:BoundField DataField="TotalAmount" HeaderText="Total" DataFormatString="R {0:N2}" />
                    <asp:BoundField DataField="PaymentMethod" HeaderText="Payment" />
                    <asp:BoundField DataField="OrderStatus" HeaderText="Order Status" />
                    <asp:BoundField DataField="DeliveryStatus" HeaderText="Delivery Status" />
                    <asp:BoundField DataField="DeliveryDate" HeaderText="Delivery Date" DataFormatString="{0:yyyy-MM-dd}" NullDisplayText="Not set" />
                    <asp:BoundField DataField="DeliverySuburb" HeaderText="Suburb" />
                    <asp:BoundField DataField="DeliveryProvince" HeaderText="Province" />
                    <asp:TemplateField HeaderText="">
                        <ItemTemplate>
                            <%-- Change "orderId" if OrderLogistics reads a different query string name --%>
                            <asp:HyperLink runat="server" CssClass="vo-link" Text="Update"
                                NavigateUrl='<%# "~/Admin/OrderLogistics.aspx?orderId=" + Eval("OrderID") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <PagerStyle CssClass="vo-pager" />
            </asp:GridView>
        </div>
    </div>
</asp:Content>