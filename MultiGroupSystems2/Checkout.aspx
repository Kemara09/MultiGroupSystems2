<%@ Page Title="Checkout" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Checkout.aspx.cs" Inherits="MultiGroupSystemsTester.Checkout" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <style>
        /* ================================
           MULTIGROUP CHECKOUT
           ================================ */

        .checkout-page {
            background: #f3f4f6;
            padding: 30px 0 50px;
            min-height: 650px;
        }

        .checkout-container {
            max-width: 1150px;
            margin: 0 auto;
            padding: 0 20px;
        }

        .checkout-title {
            font-size: 32px;
            font-weight: 600;
            color: #172b3a;
            margin-bottom: 25px;
        }

        /* Main columns */
        .checkout-layout {
            display: grid;
            grid-template-columns: minmax(0, 1fr) 340px;
            gap: 20px;
            align-items: start;
        }

        .checkout-left {
            display: flex;
            flex-direction: column;
            gap: 15px;
        }

        .checkout-right {
            position: sticky;
            top: 20px;
        }

        /* Cards */
        .checkout-card {
            background: #ffffff;
            border: 1px solid #ddd;
            border-radius: 5px;
            padding: 20px;
            box-shadow: 0 1px 2px rgba(0,0,0,.04);
        }

        .checkout-card-title {
            font-size: 15px;
            font-weight: 600;
            color: #333;
            text-transform: uppercase;
            margin-bottom: 15px;
            letter-spacing: .2px;
        }

        .checkout-card-title small {
            font-size: 12px;
            font-weight: normal;
            color: #888;
            text-transform: none;
        }

        /* Delivery method */
        .delivery-methods {
            display: grid;
            grid-template-columns: 1fr 1fr;
            gap: 12px;
        }

        .delivery-option {
            position: relative;
            border: 1px solid #ccc;
            border-radius: 5px;
            padding: 15px;
            cursor: pointer;
            transition: .2s;
            min-height: 75px;
        }

        .delivery-option:hover {
            border-color: #087f5b;
            background: #f8fffc;
        }

        .delivery-option.selected {
            border: 2px solid #087f5b;
            background: #f0faf6;
        }

        .delivery-option input {
            margin-right: 8px;
        }

        .delivery-icon {
            font-size: 24px;
            margin-right: 10px;
            vertical-align: middle;
        }

        .delivery-name {
            font-weight: 600;
            color: #333;
        }

        .delivery-description {
            display: block;
            margin-left: 35px;
            margin-top: 3px;
            color: #777;
            font-size: 11px;
        }

        /* Address */
        .address-display {
            background: #fafafa;
            border: 1px solid #e4e4e4;
            border-radius: 4px;
            padding: 15px;
        }

        .address-name {
            font-weight: 600;
            margin-bottom: 5px;
        }

        .address-text {
            line-height: 1.6;
            color: #555;
        }

        /* Date selection */
        .delivery-dates {
            display: grid;
            grid-template-columns: repeat(4, 1fr);
            gap: 10px;
        }

        .date-option {
            border: 1px solid #ccc;
            border-radius: 5px;
            padding: 13px 8px;
            text-align: center;
            background: white;
            min-height: 80px;
        }

        .date-option.selected {
            border: 2px solid #087f5b;
            background: #f0faf6;
        }

        .date-day {
            font-size: 11px;
            color: #777;
            display: block;
        }

        .date-number {
            font-size: 17px;
            font-weight: 600;
            color: #333;
            display: block;
            margin-top: 3px;
        }

        .date-label {
            font-size: 11px;
            color: #087f5b;
            display: block;
            margin-top: 4px;
        }

        /* Time */
        .delivery-time {
            margin-top: 15px;
            padding: 12px;
            background: #f7f7f7;
            border: 1px solid #ddd;
            border-radius: 4px;
        }

        .delivery-time strong {
            display: block;
            font-size: 13px;
            margin-bottom: 4px;
        }

        .delivery-time span {
            font-size: 12px;
            color: #666;
        }

        /* Forms */
        .form-label {
            font-size: 13px;
            font-weight: 500;
            color: #444;
        }

        .form-control,
        .form-select {
            border-radius: 4px;
            border: 1px solid #ccc;
        }

        .form-control:focus,
        .form-select:focus {
            border-color: #087f5b;
            box-shadow: 0 0 0 .15rem rgba(8,127,91,.12);
        }

        /* Payment */
        .payment-box {
            border: 1px solid #ccc;
            border-radius: 5px;
            padding: 15px;
            background: #fff;
        }

        .payment-label {
            font-weight: 600;
            color: #333;
        }

        .payment-icons {
            margin-top: 8px;
            font-size: 13px;
            color: #777;
        }

        /* Order summary */
        .summary-card {
            background: white;
            border: 1px solid #ddd;
            border-radius: 5px;
            padding: 20px;
        }

        .summary-title {
            font-size: 17px;
            font-weight: 600;
            margin-bottom: 15px;
            color: #333;
        }

        .summary-row {
            display: flex;
            justify-content: space-between;
            font-size: 13px;
            margin-bottom: 10px;
            color: #555;
        }

        .summary-divider {
            border-top: 1px dashed #ccc;
            margin: 15px 0;
        }

        .summary-total {
            display: flex;
            justify-content: space-between;
            font-size: 18px;
            font-weight: 700;
            color: #087f5b;
        }

        .place-order-button {
            width: 100%;
            margin-top: 18px;
            padding: 13px;
            border: none;
            border-radius: 4px;
            background: #087f5b;
            color: white;
            font-weight: 600;
            font-size: 15px;
            cursor: pointer;
        }

        .place-order-button:hover {
            background: #066b4d;
        }

        /* Items */
        .order-items-card {
            margin-top: 15px;
            background: white;
            border: 1px solid #ddd;
            border-radius: 5px;
            padding: 20px;
        }

        .order-item {
            display: flex;
            align-items: center;
            padding: 10px 0;
            border-bottom: 1px solid #eee;
        }

        .order-item:last-child {
            border-bottom: none;
        }

        .order-item-image {
            width: 55px;
            height: 55px;
            border: 1px solid #ddd;
            border-radius: 4px;
            margin-right: 12px;
            object-fit: cover;
            background: #f7f7f7;
        }

        .order-item-name {
            font-size: 13px;
            font-weight: 600;
            flex: 1;
        }

        .order-item-qty {
            font-size: 12px;
            color: #777;
            margin-right: 8px;
        }

        .order-item-price {
            font-size: 13px;
            font-weight: 600;
        }

        /* Additional options */
        .additional-option {
            padding: 5px 0;
            font-size: 13px;
            color: #555;
        }

        /* Success */
        .success-box {
            background: #e9f8f0;
            border: 1px solid #b8e5ca;
            border-radius: 5px;
            padding: 25px;
            margin-bottom: 25px;
        }

        /* Responsive */
        @media (max-width: 900px) {
            .checkout-layout {
                grid-template-columns: 1fr;
            }

            .checkout-right {
                position: static;
            }

            .delivery-dates {
                grid-template-columns: repeat(2, 1fr);
            }
        }

        @media (max-width: 600px) {
            .checkout-container {
                padding: 0 12px;
            }

            .checkout-title {
                font-size: 26px;
            }

            .delivery-methods {
                grid-template-columns: 1fr;
            }

            .delivery-dates {
                grid-template-columns: 1fr 1fr;
            }
        }
        
/* GEOAPIFY ADDRESS AUTOCOMPLETE */

#addressSuggestions {
    position: relative;
    z-index: 1000;
    max-height: 240px;
    overflow-y: auto;
    border-radius: 4px;
}

#addressSuggestions .list-group-item {
    cursor: pointer;
    font-size: 13px;
    padding: 12px;
}

#addressSuggestions .list-group-item:hover {
    background-color: #f0faf6;
    color: #087f5b;
}

#addressSearch:focus {
    border-color: #087f5b;
    box-shadow: 0 0 0 .15rem rgba(8,127,91,.12);
}

    </style>

   <script>
       document.addEventListener("DOMContentLoaded", function () {

           var changeAddressBtn = document.getElementById("changeAddressBtn");
           var closeAddressBtn = document.getElementById("closeAddressBtn");
           var customerDetails = document.getElementById("customer-details");

           /* ==============================
              OPEN CHANGE ADDRESS
           ============================== */

           if (changeAddressBtn) {

               changeAddressBtn.addEventListener("click", function (e) {

                   e.preventDefault();

                   customerDetails.style.display = "block";

                   customerDetails.scrollIntoView({
                       behavior: "smooth",
                       block: "start"
                   });

               });

           }


           /* ==============================
              CLOSE CHANGE ADDRESS
           ============================== */

           if (closeAddressBtn) {

               closeAddressBtn.addEventListener("click", function (e) {

                   e.preventDefault();

                   customerDetails.style.display = "none";

                   changeAddressBtn.scrollIntoView({
                       behavior: "smooth",
                       block: "center"
                   });

               });

           }

       });

       document.addEventListener("DOMContentLoaded", function () {
           const search = document.getElementById("addressSearch");
           const suggestions = document.getElementById("addressSuggestions");
           const selected = document.getElementById("<%= txtSelectedAddress.ClientID %>");
    const hidden = document.getElementById("<%= hdnSelectedAddress.ClientID %>");

           const apiKey = "eb16ae629b324ef4a7189cd6a2b1e868";

    if (!search || !suggestions || !selected || !hidden) return;

    let debounceTimer;
    let controller;

    search.addEventListener("input", function () {
        clearTimeout(debounceTimer);
        if (controller) controller.abort();

        const query = search.value.trim();

        // Invalidate a previous selection when searching again
        selected.value = "";
        hidden.value = "";
        suggestions.replaceChildren();
        suggestions.style.display = "none";

        if (query.length < 3) return;

        debounceTimer = setTimeout(async function () {
            controller = new AbortController();

            try {
                const url =
                    "https://api.geoapify.com/v1/geocode/autocomplete" +
                    "?text=" + encodeURIComponent(query) +
                    "&filter=countrycode:za" +
                    "&format=json" +
                    "&apiKey=" + encodeURIComponent(apiKey);

                const response = await fetch(url, {
                    signal: controller.signal
                });

                if (!response.ok) {
                    throw new Error("Geoapify request failed: " + response.status);
                }

                const data = await response.json();

                if (search.value.trim() !== query) return;

                suggestions.replaceChildren();

                (data.results || []).forEach(function (result) {
                    const address = result.formatted;
                    if (!address) return;

                    const item = document.createElement("button");
                    item.type = "button";
                    item.className = "list-group-item list-group-item-action";
                    item.textContent = address;

                    item.addEventListener("click", function () {
                        search.value = address;
                        selected.value = address;
                        hidden.value = address;

                        suggestions.replaceChildren();
                        suggestions.style.display = "none";
                    });

                    suggestions.appendChild(item);
                });

                suggestions.style.display =
                    suggestions.children.length ? "block" : "none";

            } catch (error) {
                if (error.name !== "AbortError") {
                    console.error("Geoapify error:", error);
                }
            }
        }, 400);
    });
});

   </script>
     <div class="checkout-page">

        <div class="checkout-container">

            <h1 class="checkout-title">Checkout</h1>

            <!-- SUCCESS MESSAGE -->
            <asp:Panel ID="pnlSuccess" runat="server" CssClass="success-box" Visible="false">
            </asp:Panel>

            <!-- ERROR MESSAGE -->
            <asp:Label ID="lblError"
                runat="server"
                CssClass="alert alert-danger d-block"
                Visible="false">
            </asp:Label>


            <asp:Panel ID="pnlForm" runat="server">

                <div class="checkout-layout">

                    <!-- ==========================================
                         LEFT SIDE
                         ========================================== -->

                    <div class="checkout-left">


                        <!-- DELIVERY METHOD -->
                        <div class="checkout-card">

                            <div class="checkout-card-title">
                                Delivery Method
                            </div>

                            <div class="delivery-methods">

                                <div class="delivery-option selected">

                                    <span class="delivery-icon">🚚</span>

                                    <span class="delivery-name">
                                        Delivery
                                    </span>

                                    <span class="delivery-description">
                                        Delivered to your address
                                    </span>

                                </div>

                                <div class="delivery-option">

                                    <span class="delivery-icon">📦</span>

                                    <span class="delivery-name">
                                        Collection
                                    </span>

                                    <span class="delivery-description">
                                        Collect your order
                                    </span>

                                </div>

                            </div>

                        </div>


                        <!-- DELIVERY ADDRESS -->
                       <div class="checkout-card-title">
    Delivery Address

    <span style="float:right;">
        <a href="javascript:void(0);"
           id="changeAddressBtn"
           style="font-size:12px;
                  color:#087f5b;
                  text-decoration:none;
                  cursor:pointer;">
            ✏ Change
        </a>
    </span>
</div>

                            <div class="address-display">

                                <div class="address-name">
                                    <asp:Label ID="lblAddressName"
                                        runat="server">
                                    </asp:Label>
                                </div>

                                <div class="address-text">
                                    <asp:Label ID="lblAddress"
                                        runat="server">
                                    </asp:Label>
                                </div>

                                <div style="margin-top:8px;
                                            font-size:12px;
                                            color:#777;">

                                    Phone:
                                    <asp:Label ID="lblAddressPhone"
                                        runat="server">
                                    </asp:Label>

                                </div>

                            </div>

                        </div>


                        <!-- CUSTOMER DETAILS -->
                      <div id="customer-details"
     class="checkout-card"
     style="display:none;">

    <div class="checkout-card-title">
        Customer &amp; Delivery Details

        <span style="float:right;">
            <a href="javascript:void(0);"
               id="closeAddressBtn"
               style="font-size:12px;
                      color:#087f5b;
                      text-decoration:none;
                      cursor:pointer;">
                CANCEL
            </a>
        </span>
        
<!-- GEOAPIFY ADDRESS SEARCH -->
<div class="mt-3 mb-3">

    <label class="form-label">
        Search for a new delivery address
    </label>

    <input type="text"
           id="addressSearch"
           class="form-control"
           placeholder="Start typing your address..."
           autocomplete="off" />

    <!-- Address suggestions will appear here -->
    <div id="addressSuggestions"
         class="list-group mt-1"
         style="display:none;">
    </div>

    <label class="form-label mt-3">
        Selected delivery address
    </label>

    <asp:TextBox
        ID="txtSelectedAddress"
        runat="server"
        CssClass="form-control"
        ReadOnly="true"
        placeholder="Your selected address will appear here..." />

    <!-- Stores selected address details across postbacks -->
    <asp:HiddenField
        ID="hdnSelectedAddress"
        runat="server" />

</div>

    </div>


    <!--
        Hidden fields kept so existing C# code-behind
        does not break.
    -->
    <div style="display:none;">

        <asp:TextBox
            ID="TextBox1"
            runat="server" />

        <asp:TextBox
            ID="TextBox2"
            runat="server" />

        <asp:TextBox
            ID="TextBox3"
            runat="server" />

        <asp:TextBox
            ID="TextBox4"
            runat="server" />

        <asp:TextBox
            ID="TextBox5"
            runat="server" />

    </div>


    <!-- PHONE NUMBER -->
    <div class="row g-3">

        <div class="col-md-6">

            <label class="form-label">
                Phone number
            </label>

            <asp:TextBox
                ID="TextBox6"
                runat="server"
                CssClass="form-control" />

            <asp:RequiredFieldValidator
                runat="server"
                ControlToValidate="txtPhone"
                CssClass="text-danger"
                Display="Dynamic"
                ErrorMessage="Required." />

            <asp:RegularExpressionValidator
                runat="server"
                ControlToValidate="txtPhone"
                CssClass="text-danger"
                Display="Dynamic"
                ErrorMessage="Enter a valid 10-digit number."
                ValidationExpression="^0[0-9]{9}$" />

        </div>

    </div>


    <!-- STREET ADDRESS -->
    <div class="row g-3 mt-1">

        <div class="col-md-12">

            <label class="form-label">
                Street address
            </label>

            <asp:TextBox
                ID="txtStreetAddress"
                runat="server"
                CssClass="form-control"
                placeholder="e.g. 10 Fredman Drive" />

            <asp:RequiredFieldValidator
                runat="server"
                ControlToValidate="txtStreetAddress"
                CssClass="text-danger"
                Display="Dynamic"
                ErrorMessage="Please enter your street address." />

        </div>

    </div>


    <!-- SUBURB / CITY / PROVINCE -->
    <div class="row g-3 mt-1">

        <!-- SUBURB -->
        <div class="col-md-4">

            <label class="form-label">
                Suburb
            </label>

            <asp:TextBox
                ID="TextBox7"
                runat="server"
                CssClass="form-control" />

        </div>


        <!-- CITY -->
        <div class="col-md-4">

            <label class="form-label">
                City
            </label>

            <asp:TextBox
                ID="TextBox8"
                runat="server"
                CssClass="form-control" />

        </div>


        <!-- PROVINCE -->
        <div class="col-md-4">

            <label class="form-label">
                Province
            </label>

            <asp:DropDownList
                ID="DropDownList1"
                runat="server"
                CssClass="form-select">

                <asp:ListItem
                    Text="-- Select --"
                    Value="" />

                <asp:ListItem
                    Text="Eastern Cape"
                    Value="Eastern Cape" />

                <asp:ListItem
                    Text="Free State"
                    Value="Free State" />

                <asp:ListItem
                    Text="Gauteng"
                    Value="Gauteng" />

                <asp:ListItem
                    Text="KwaZulu-Natal"
                    Value="KwaZulu-Natal" />

                <asp:ListItem
                    Text="Limpopo"
                    Value="Limpopo" />

                <asp:ListItem
                    Text="Mpumalanga"
                    Value="Mpumalanga" />

                <asp:ListItem
                    Text="North West"
                    Value="North West" />

                <asp:ListItem
                    Text="Northern Cape"
                    Value="Northern Cape" />

                <asp:ListItem
                    Text="Western Cape"
                    Value="Western Cape" />

            </asp:DropDownList>

            <asp:RequiredFieldValidator
                runat="server"
                ControlToValidate="ddlProvince"
                InitialValue=""
                CssClass="text-danger"
                Display="Dynamic"
                ErrorMessage="Please select a province." />

        </div>

    </div>


    <!-- POSTAL CODE -->
    <div class="row g-3 mt-1">

        <div class="col-md-4">

            <label class="form-label">
                Postal code
            </label>

            <asp:TextBox
                ID="TextBox9"
                runat="server"
                CssClass="form-control" />

            <asp:RequiredFieldValidator
                runat="server"
                ControlToValidate="txtPostalCode"
                CssClass="text-danger"
                Display="Dynamic"
                ErrorMessage="Required." />

            <asp:RegularExpressionValidator
                runat="server"
                ControlToValidate="txtPostalCode"
                CssClass="text-danger"
                Display="Dynamic"
                ErrorMessage="Enter a valid 4-digit postal code."
                ValidationExpression="^[0-9]{4}$" />

        </div>

    </div>

</div>

                            <div class="row g-3">

                                <div class="col-md-6">

                                    <label class="form-label">
                                        First name
                                    </label>

                                    <asp:TextBox
                                        ID="txtFirstName"
                                        runat="server"
                                        CssClass="form-control" />

                                    <asp:RequiredFieldValidator
                                        runat="server"
                                        ControlToValidate="txtFirstName"
                                        CssClass="text-danger"
                                        Display="Dynamic"
                                        ErrorMessage="Required." />

                                </div>


                                <div class="col-md-6">

                                    <label class="form-label">
                                        Last name
                                    </label>

                                    <asp:TextBox
                                        ID="txtLastName"
                                        runat="server"
                                        CssClass="form-control" />

                                    <asp:RequiredFieldValidator
                                        runat="server"
                                        ControlToValidate="txtLastName"
                                        CssClass="text-danger"
                                        Display="Dynamic"
                                        ErrorMessage="Required." />

                                </div>


                                <div class="col-md-6">

                                    <label class="form-label">
                                        Email
                                    </label>

                                    <asp:TextBox
                                        ID="txtEmail"
                                        runat="server"
                                        CssClass="form-control" />

                                    <asp:RequiredFieldValidator
                                        runat="server"
                                        ControlToValidate="txtEmail"
                                        CssClass="text-danger"
                                        Display="Dynamic"
                                        ErrorMessage="Required." />

                                    <asp:RegularExpressionValidator
                                        runat="server"
                                        ControlToValidate="txtEmail"
                                        CssClass="text-danger"
                                        Display="Dynamic"
                                        ErrorMessage="Invalid email."
                                        ValidationExpression="^[^@\s]+@[^@\s]+\.[^@\s]+$" />

                                </div>


                                <div class="col-md-6">

                                    <label class="form-label">
                                        Phone number
                                    </label>

                                    <asp:TextBox
                                        ID="txtPhone"
                                        runat="server"
                                        CssClass="form-control" />

                                    <asp:RequiredFieldValidator
                                        runat="server"
                                        ControlToValidate="txtPhone"
                                        CssClass="text-danger"
                                        Display="Dynamic"
                                        ErrorMessage="Required." />

                                    <asp:RegularExpressionValidator
                                        runat="server"
                                        ControlToValidate="txtPhone"
                                        CssClass="text-danger"
                                        Display="Dynamic"
                                        ErrorMessage="Enter a valid 10-digit number."
                                        ValidationExpression="^0[0-9]{9}$" />

                                </div>


                                <div class="col-md-3">

                                    <label class="form-label">
                                        Street number
                                    </label>

                                    <asp:TextBox
                                        ID="txtStreetNumber"
                                        runat="server"
                                        CssClass="form-control" />

                                    <asp:RequiredFieldValidator
                                        runat="server"
                                        ControlToValidate="txtStreetNumber"
                                        CssClass="text-danger"
                                        Display="Dynamic"
                                        ErrorMessage="Required." />

                                </div>


                                <div class="col-md-9">

                                    <label class="form-label">
                                        Street name
                                    </label>

                                    <asp:TextBox
                                        ID="txtStreetName"
                                        runat="server"
                                        CssClass="form-control" />

                                </div>


                                <div class="col-md-4">

                                    <label class="form-label">
                                        Suburb
                                    </label>

                                    <asp:TextBox
                                        ID="txtSuburb"
                                        runat="server"
                                        CssClass="form-control" />

                                </div>


                                <div class="col-md-4">

                                    <label class="form-label">
                                        City
                                    </label>

                                    <asp:TextBox
                                        ID="txtCity"
                                        runat="server"
                                        CssClass="form-control" />

                                </div>


                                <div class="col-md-4">

                                    <label class="form-label">
                                        Province
                                    </label>

                                    <asp:DropDownList
                                        ID="ddlProvince"
                                        runat="server"
                                        CssClass="form-select">

                                        <asp:ListItem
                                            Text="-- Select --"
                                            Value="" />

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

                                    <asp:RequiredFieldValidator
                                        runat="server"
                                        ControlToValidate="ddlProvince"
                                        InitialValue=""
                                        CssClass="text-danger"
                                        Display="Dynamic"
                                        ErrorMessage="Please select a province." />

                                </div>


                                <div class="col-md-4">

                                    <label class="form-label">
                                        Postal code
                                    </label>

                                    <asp:TextBox
                                        ID="txtPostalCode"
                                        runat="server"
                                        CssClass="form-control" />

                                    <asp:RequiredFieldValidator
                                        runat="server"
                                        ControlToValidate="txtPostalCode"
                                        CssClass="text-danger"
                                        Display="Dynamic"
                                        ErrorMessage="Required." />

                                    <asp:RegularExpressionValidator
                                        runat="server"
                                        ControlToValidate="txtPostalCode"
                                        CssClass="text-danger"
                                        Display="Dynamic"
                                        ErrorMessage="Enter a valid 4-digit postal code."
                                        ValidationExpression="^[0-9]{4}$" />

                                </div>

                            </div>

                        </div>


                        <!-- DELIVERY DATE -->
                        <div class="checkout-card">

                            <div class="checkout-card-title">
                                Delivery Date &amp; Time
                            </div>

                            <div class="delivery-dates">

                                <div class="date-option selected">
                                    <span class="date-day">Today</span>
                                    <span class="date-number">
                                        <%= DateTime.Now.Day %>
                                    </span>
                                    <span class="date-label">
                                        Next Day
                                    </span>
                                </div>

                                <div class="date-option">
                                    <span class="date-day">Tomorrow</span>
                                    <span class="date-number">
                                        <%= DateTime.Now.AddDays(1).Day %>
                                    </span>
                                    <span class="date-label">
                                        Available
                                    </span>
                                </div>

                                <div class="date-option">
                                    <span class="date-day">
                                        <%= DateTime.Now.AddDays(2).ToString("ddd") %>
                                    </span>
                                    <span class="date-number">
                                        <%= DateTime.Now.AddDays(2).Day %>
                                    </span>
                                    <span class="date-label">
                                        Available
                                    </span>
                                </div>

                                <div class="date-option">
                                    <span class="date-day">
                                        <%= DateTime.Now.AddDays(3).ToString("ddd") %>
                                    </span>
                                    <span class="date-number">
                                        <%= DateTime.Now.AddDays(3).Day %>
                                    </span>
                                    <span class="date-label">
                                        Available
                                    </span>
                                </div>

                            </div>

                            <div class="delivery-time">

                                <strong>
                                    Delivery time
                                </strong>

                                <span>
                                    7am - 7pm
                                </span>

                            </div>

                        </div>


                        <!-- PAYMENT METHOD -->
                        <div class="checkout-card">

                            <div class="checkout-card-title">
                                Payment Method
                            </div>

                            <div class="payment-box">

                                <label class="payment-label">
                                    Select your payment method
                                </label>

                                <asp:DropDownList
                                    ID="ddlPaymentMethod"
                                    runat="server"
                                    CssClass="form-select mt-2"
                                    AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlPaymentMethod_SelectedIndexChanged">

                                    <asp:ListItem
                                        Text="-- Select payment method --"
                                        Value="" />

                                    <asp:ListItem
                                        Text="Credit / Debit Card"
                                        Value="Card" />

                                    <asp:ListItem
                                        Text="EFT"
                                        Value="EFT" />

                                    <asp:ListItem
                                        Text="Cash on Delivery"
                                        Value="Cash on Delivery" />

                                </asp:DropDownList>

                                <div class="payment-icons">
                                    💳 Visa &nbsp;&nbsp; Mastercard &nbsp;&nbsp; American Express
                                </div>

                            </div>


                            <!-- CARD DETAILS -->
                            <asp:Panel
                                ID="pnlCardDetails"
                                runat="server"
                                CssClass="border rounded p-3 mt-3"
                                Visible="false">

                                <p class="text-muted small mb-2">
                                    Demo only — card details are not stored or processed.
                                </p>

                                <div class="row g-3">

                                    <div class="col-md-6">

                                        <label class="form-label">
                                            Name on card
                                        </label>

                                        <asp:TextBox
                                            ID="txtCardName"
                                            runat="server"
                                            CssClass="form-control" />

                                    </div>

                                    <div class="col-md-6">

                                        <label class="form-label">
                                            Card number
                                        </label>

                                        <asp:TextBox
                                            ID="txtCardNumber"
                                            runat="server"
                                            CssClass="form-control"
                                            MaxLength="19"
                                            placeholder="0000 0000 0000 0000" />

                                    </div>

                                    <div class="col-md-3">

                                        <label class="form-label">
                                            Expiry
                                        </label>

                                        <asp:TextBox
                                            ID="txtCardExpiry"
                                            runat="server"
                                            CssClass="form-control"
                                            MaxLength="5"
                                            placeholder="MM/YY" />

                                    </div>

                                    <div class="col-md-3">

                                        <label class="form-label">
                                            CVV
                                        </label>

                                        <asp:TextBox
                                            ID="txtCardCVV"
                                            runat="server"
                                            CssClass="form-control"
                                            MaxLength="3"
                                            TextMode="Password" />

                                    </div>

                                </div>

                            </asp:Panel>


                            <!-- EFT -->
                            <asp:Panel
                                ID="pnlEFTDetails"
                                runat="server"
                                CssClass="alert alert-info mt-3"
                                Visible="false">

                                <p class="mb-1">
                                    <strong>Please make payment to:</strong>
                                </p>

                                <p class="mb-0">
                                    Bank: Standard Bank<br />
                                    Account Name: Multigroup Inc.<br />
                                    Account Number: 123 456 789<br />
                                    Branch Code: 051001
                                </p>

                                <p class="mb-0 mt-2">
                                    Please use your order number as the payment reference once your order is placed.
                                </p>

                            </asp:Panel>

                        </div>


                        <!-- ADDITIONAL INFORMATION -->
                        <div class="checkout-card">

                            <div class="checkout-card-title">
                                Additional Information
                            </div>

                            <div class="additional-option">

                                <asp:CheckBox
                                    ID="chkDeliveryInstructions"
                                    runat="server" />

                                <label for="<%= chkDeliveryInstructions.ClientID %>">
                                    Add delivery instructions
                                </label>

                            </div>

                            <asp:TextBox
                                ID="txtDeliveryInstructions"
                                runat="server"
                                CssClass="form-control mt-2"
                                TextMode="MultiLine"
                                Rows="3"
                                placeholder="Optional delivery instructions..."
                                Visible="false" />

                        </div>


                    </div>


                    <!-- ==========================================
                         RIGHT SIDE
                         ========================================== -->

                    <div class="checkout-right">


                        <!-- ORDER SUMMARY -->
                        <div class="summary-card">

                            <div class="summary-title">
                                Order Summary
                            </div>

                            <div class="summary-row">

                                <span>
                                    Items
                                </span>

                                <strong>
                                    <asp:Label
                                        ID="lblItemCount"
                                        runat="server">
                                    </asp:Label>
                                </strong>

                            </div>

                            <div class="summary-row">

                                <span>
                                    Subtotal
                                </span>

                                <strong>
                                    R
                                    <asp:Label
                                        ID="lblSubtotal"
                                        runat="server">
                                    </asp:Label>
                                </strong>

                            </div>

                            <div class="summary-row">

                                <span>
                                    VAT
                                </span>

                                <strong>
                                    R
                                    <asp:Label
                                        ID="lblVAT"
                                        runat="server">
                                    </asp:Label>
                                </strong>

                            </div>

                            <div class="summary-row">

                                <span>
                                    Delivery
                                </span>

                                <strong>
                                    Free
                                </strong>

                            </div>

                            <div class="summary-divider"></div>

                            <div class="summary-total">

                                <span>
                                    TO PAY:
                                </span>

                                <span>
                                    R
                                    <asp:Label
                                        ID="lblGrandTotal"
                                        runat="server">
                                    </asp:Label>
                                </span>

                            </div>


                            <asp:Button
                                ID="btnPlaceOrder"
                                runat="server"
                                Text="Place Order"
                                CssClass="place-order-button"
                                OnClick="btnPlaceOrder_Click" />

                        </div>


                        <!-- ITEMS -->
                        <div class="order-items-card">

                            <div class="summary-title"
                                 style="font-size:15px;">

                                Items in Your Order

                            </div>

                            <asp:Repeater
                                ID="rptOrderItems"
                                runat="server">

                                <ItemTemplate>

                                    <div class="order-item">

                                        <div class="order-item-image"
                                             style="display:flex;
                                                    align-items:center;
                                                    justify-content:center;
                                                    font-size:20px;">

                                            🛒

                                        </div>

                                        <div class="order-item-name">

                                            <%# Eval("ProductName") %>

                                        </div>

                                        <div class="order-item-qty">

                                            x<%# Eval("Quantity") %>

                                        </div>

                                        <div class="order-item-price">

                                            R<%# Convert.ToDecimal(Eval("Subtotal")).ToString("N2") %>

                                        </div>

                                    </div>

                                </ItemTemplate>

                            </asp:Repeater>

                        </div>

                    </div>

                </div>

            </asp:Panel>

        </div>

    </div>

</asp:Content>