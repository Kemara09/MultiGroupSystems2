<%@ Page Title="Products" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Products.aspx.cs" Inherits="MultiGroupSystemsTester.Products" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Our Products</h1>
     <a href="Cart.aspx" runat="server" class="btn btn-navy mb-3">View cart</a>
    <asp:Label ID="lblMessage" runat="server" CssClass="text-danger d-block mb-3" Visible="false"></asp:Label>
    <asp:Label ID="lblCartMsg" runat="server" CssClass="alert alert-success d-block" Visible="false"></asp:Label>

    <asp:GridView ID="gvProducts" runat="server" CssClass="table table-striped align-middle" ClientIDMode="Static"
    AutoGenerateColumns="false" DataKeyNames="ProductID,Price,ProductName" EmptyDataText="No products available."
    OnRowCommand="gvProducts_RowCommand" OnRowDataBound="gvProducts_RowDataBound">
        <Columns>
           <asp:TemplateField HeaderText="Product">
    <ItemTemplate>
        <a href="#" class="fw-semibold text-decoration-none" onclick="toggleDetails(<%# Container.DataItemIndex %>); return false;">
            <%# Eval("ProductName") %> <small class="text-muted">(details)</small>
        </a>
    </ItemTemplate>
</asp:TemplateField>
            <asp:BoundField DataField="Category" HeaderText="Category" />
            <asp:BoundField DataField="Price" HeaderText="Price" DataFormatString="{0:C}" />
            <asp:TemplateField HeaderText="Order">
                <ItemTemplate>
                    <div class="d-flex gap-2 align-items-center">
                        <asp:TextBox ID="txtQty" runat="server" CssClass="form-control form-control-sm" style="width:70px" Text="1" />
                        <asp:Button ID="btnAddToCart" runat="server" CommandName="AddToCart"
                            CommandArgument='<%# Container.DataItemIndex %>' Text="Add to cart" CssClass="btn btn-teal btn-sm" />
                    </div>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
  <script>
function toggleDetails(index) {
    var row = document.getElementById('detailsRow' + index);
    if (row) {
        row.style.display = (row.style.display === 'none') ? '' : 'none';
    }
    }
  </script>
</asp:Content>