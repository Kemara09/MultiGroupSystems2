<%@ Page Title="Products" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Products.aspx.cs" Inherits="MultiGroupSystemsTester.Products" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h1>Our Products</h1>

    <a href="Cart.aspx" runat="server" class="btn btn-navy mb-3">View cart</a>

    <asp:UpdatePanel ID="upProducts" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="row g-2 mb-3 align-items-center">
                <div class="col-auto">
                    <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control"
                        placeholder="Search products or category..." />
                </div>
                <div class="col-auto">
                    <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-teal" OnClick="btnSearch_Click" />
                </div>
                <div class="col-auto">
                    <asp:Button ID="btnClearSearch" runat="server" Text="Clear" CssClass="btn btn-outline-secondary" OnClick="btnClearSearch_Click" CausesValidation="false" />
                </div>
                <div class="col-auto">
                    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="upProducts">
                        <ProgressTemplate>
                            <span class="text-muted small">Searching...</span>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </div>
            </div>

            <div class="row g-2 mb-3 align-items-center">
                <div class="col-auto">
                    <label for="<%= ddlCategory.ClientID %>" class="col-form-label">Filter by category:</label>
                </div>
                <div class="col-auto">
                    <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-select"
                        AutoPostBack="true" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged">
                        <asp:ListItem Text="All Categories" Value="" />
                    </asp:DropDownList>
                </div>
            </div>

            <asp:Label ID="lblMessage" runat="server" CssClass="text-danger d-block mb-3" Visible="false"></asp:Label>
            <asp:Label ID="lblCartMsg" runat="server" CssClass="alert alert-success d-block" Visible="false"></asp:Label>

            <asp:GridView ID="gvProducts" runat="server" CssClass="table table-striped align-middle"
                AutoGenerateColumns="false" DataKeyNames="ProductID,Price,ProductName" EmptyDataText="No products available."
                OnRowCommand="gvProducts_RowCommand">
                <Columns>
                    <asp:TemplateField HeaderText="Product">
                        <ItemTemplate>
                            <a href="#" class="fw-semibold text-decoration-none"
                                data-bs-toggle="collapse" data-bs-target="#details<%# Container.DataItemIndex %>">
                                <%# Eval("ProductName") %> <small class="text-muted">(details)</small>
                            </a>
                            <div class="collapse mt-2" id="details<%# Container.DataItemIndex %>">
                                <div class="p-2 bg-light border-start border-3 border-teal small">
                                    <p class="mb-1"><strong>Description:</strong> <%# SafeText(Eval("Description")) %></p>
                                    <p class="mb-0"><strong>In stock:</strong> <%# SafeText(Eval("QuantityInStock")) %> units</p>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Category" HeaderText="Category" />
                    <asp:BoundField DataField="Price" HeaderText="Price" DataFormatString="R {0:N2}" />
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
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>