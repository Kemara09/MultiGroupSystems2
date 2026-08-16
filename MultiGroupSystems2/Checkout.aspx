<%@ Page Title="Checkout" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Checkout.aspx.cs" Inherits="MultiGroupSystemsTester.Checkout" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Checkout</h1>
    <asp:Panel ID="pnlSuccess" runat="server" CssClass="alert alert-success" Visible="false"></asp:Panel>
    <asp:Label ID="lblError" runat="server" CssClass="alert alert-danger d-block" Visible="false"></asp:Label>

    <asp:Panel ID="pnlForm" runat="server">
        <div class="row g-3">
            <div class="col-md-6">
                <label class="form-label">First name</label>
                <asp:TextBox ID="txtFirstName" runat="server" CssClass="form-control" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtFirstName" CssClass="text-danger" Display="Dynamic" ErrorMessage="Required." />
            </div>
            <div class="col-md-6">
                <label class="form-label">Last name</label>
                <asp:TextBox ID="txtLastName" runat="server" CssClass="form-control" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtLastName" CssClass="text-danger" Display="Dynamic" ErrorMessage="Required." />
            </div>
            <div class="col-md-6">
                <label class="form-label">Email</label>
                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtEmail" CssClass="text-danger" Display="Dynamic" ErrorMessage="Required." />
                <asp:RegularExpressionValidator runat="server" ControlToValidate="txtEmail" CssClass="text-danger" Display="Dynamic" ErrorMessage="Invalid email." ValidationExpression="^[^@\s]+@[^@\s]+\.[^@\s]+$" />
            </div>
            <div class="col-md-6">
                <label class="form-label">Phone number</label>
                <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtPhone" CssClass="text-danger" Display="Dynamic" ErrorMessage="Required." />
                <asp:RegularExpressionValidator runat="server" ControlToValidate="txtPhone" CssClass="text-danger" Display="Dynamic" ErrorMessage="Enter a valid 10-digit number (e.g. 0312050565)." ValidationExpression="^0[0-9]{9}$" />
            </div>
            <div class="col-md-3">
                <label class="form-label">Street number</label>
                <asp:TextBox ID="txtStreetNumber" runat="server" CssClass="form-control" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtStreetNumber" CssClass="text-danger" Display="Dynamic" ErrorMessage="Required." />
                <asp:RegularExpressionValidator runat="server" ControlToValidate="txtStreetNumber" CssClass="text-danger" Display="Dynamic" ErrorMessage="Digits only." ValidationExpression="^[0-9]+$" />
            </div>
            <div class="col-md-9">
                <label class="form-label">Street name</label>
                <asp:TextBox ID="txtStreetName" runat="server" CssClass="form-control" />
            </div>
            <div class="col-md-4">
                <label class="form-label">Suburb</label>
                <asp:TextBox ID="txtSuburb" runat="server" CssClass="form-control" />
            </div>
            <div class="col-md-4">
                <label class="form-label">City</label>
                <asp:TextBox ID="txtCity" runat="server" CssClass="form-control" />
            </div>
            <div class="col-md-4">
                <label class="form-label">Province</label>
                <asp:DropDownList ID="ddlProvince" runat="server" CssClass="form-select">
                    <asp:ListItem Text="-- Select --" Value="" />
                    <asp:ListItem Text="Eastern Cape" Value="Eastern Cape" />
                    <asp:ListItem Text="Free State" Value="Free State" />
                    <asp:ListItem Text="Gauteng" Value="Gauteng" />
                    <asp:ListItem Text="KwaZulu-Natal" Value="KwaZulu-Natal" />
                    <asp:ListItem Text="Limpopo" Value="Limpopo" />
                    <asp:ListItem Text="Mpumalanga" Value="Mpumalanga" />
                    <asp:ListItem Text="North West" Value="North West" />
                    <asp:ListItem Text="Northern Cape" Value="Northern Cape" />
                    <asp:ListItem Text="Western Cape" Value="Western Cape" />
                </asp:DropDownList>
                <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlProvince" InitialValue="" CssClass="text-danger" Display="Dynamic" ErrorMessage="Please select a province." />
            </div>
           
            <div class="col-md-4">
                <label class="form-label">Postal code</label>
                <asp:TextBox ID="txtPostalCode" runat="server" CssClass="form-control" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtPostalCode" CssClass="text-danger" Display="Dynamic" ErrorMessage="Required." />
                <asp:RegularExpressionValidator runat="server" ControlToValidate="txtPostalCode" CssClass="text-danger" Display="Dynamic" ErrorMessage="Enter a valid 4-digit postal code." ValidationExpression="^[0-9]{4}$" />
            </div>

            <div class="col-md-6">
                <label class="form-label">Payment method</label>
                <asp:DropDownList ID="ddlPaymentMethod" runat="server" CssClass="form-select" AutoPostBack="true" OnSelectedIndexChanged="ddlPaymentMethod_SelectedIndexChanged">
                    <asp:ListItem Text="-- Select --" Value="" />
                    <asp:ListItem Text="Credit / Debit Card" Value="Card" />
                    <asp:ListItem Text="EFT" Value="EFT" />
                    <asp:ListItem Text="Cash on Delivery" Value="Cash on Delivery" />
                </asp:DropDownList>
            </div>

            <div class="col-12">
                <asp:Panel ID="pnlCardDetails" runat="server" CssClass="border rounded p-3 mt-2" Visible="false">
                    <p class="text-muted small mb-2">Demo only — card details are not stored or processed.</p>
                    <div class="row g-3">
                        <div class="col-md-6">
                            <label class="form-label">Name on card</label>
                            <asp:TextBox ID="txtCardName" runat="server" CssClass="form-control" />
                        </div>
                        <div class="col-md-6">
                            <label class="form-label">Card number</label>
                            <asp:TextBox ID="txtCardNumber" runat="server" CssClass="form-control" MaxLength="19" placeholder="0000 0000 0000 0000" />
                        </div>
                        <div class="col-md-3">
                            <label class="form-label">Expiry (MM/YY)</label>
                            <asp:TextBox ID="txtCardExpiry" runat="server" CssClass="form-control" MaxLength="5" placeholder="MM/YY" />
                        </div>
                        <div class="col-md-3">
                            <label class="form-label">CVV</label>
                            <asp:TextBox ID="txtCardCVV" runat="server" CssClass="form-control" MaxLength="3" TextMode="Password" />
                        </div>
                    </div>
                </asp:Panel>

                <asp:Panel ID="pnlEFTDetails" runat="server" CssClass="alert alert-info mt-2" Visible="false">
                    <p class="mb-1"><strong>Please make payment to:</strong></p>
                    <p class="mb-0">Bank: Standard Bank &middot; Account Name: Multigroup Inc.<br />
                    Account Number: 123 456 789 &middot; Branch Code: 051001</p>
                    <p class="mb-0 mt-2">Please use your order number as the payment reference once your order is placed.</p>
                </asp:Panel>
            </div>
        </div>
        <asp:Button ID="btnPlaceOrder" runat="server" Text="Place order" CssClass="btn btn-teal mt-3" OnClick="btnPlaceOrder_Click" />
    </asp:Panel>
</asp:Content>