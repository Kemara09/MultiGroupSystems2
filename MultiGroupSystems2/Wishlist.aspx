
<%@ Page Title="My Wishlist" Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="Wishlist.aspx.cs"
    Inherits="MultiGroupSystemsTester.Wishlist" %>

<asp:Content ID="Content1"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <h1>My Wishlist</h1>

    <p class="text-muted">
        Products you have saved for later.
    </p>

    <asp:Label ID="lblMessage"
        runat="server"
        CssClass="d-block mb-3"
        Visible="false" />

    <asp:GridView ID="gvWishlist"
        runat="server"
        AutoGenerateColumns="false"
        CssClass="table table-striped align-middle"
        DataKeyNames="ProductID"
        EmptyDataText="Your wishlist is empty."
        OnRowCommand="gvWishlist_RowCommand">

        <Columns>

            <asp:BoundField
                DataField="ProductName"
                HeaderText="Product" />

            <asp:BoundField
                DataField="Category"
                HeaderText="Category" />

            <asp:BoundField
                DataField="UnitPrice"
                HeaderText="Price"
                DataFormatString="R {0:N2}" />

            <asp:BoundField
                DataField="QuantityInStock"
                HeaderText="In Stock" />

            <asp:TemplateField HeaderText="Actions">
                <ItemTemplate>
                    <div class="d-flex gap-2">

                        <asp:Button ID="btnMoveToCart"
                            runat="server"
                            Text="Add to Cart"
                            CommandName="AddToCart"
                            CommandArgument='<%# Container.DataItemIndex %>'
                            CssClass="btn btn-teal btn-sm"
                            CausesValidation="false" />

                        <asp:Button ID="btnRemove"
                            runat="server"
                            Text="Remove"
                            CommandName="RemoveWishlist"
                            CommandArgument='<%# Container.DataItemIndex %>'
                            CssClass="btn btn-outline-danger btn-sm"
                            CausesValidation="false" />

                    </div>
                </ItemTemplate>
            </asp:TemplateField>

        </Columns>

    </asp:GridView>

    <a href="Products.aspx"
       class="btn btn-navy mt-3">
        Continue Shopping
    </a>

</asp:Content>
