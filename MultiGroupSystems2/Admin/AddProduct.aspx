<%@ Page Title="Add Product" Language="C#" AutoEventWireup="true"
MasterPageFile="~/Admin/AdminMaster.master"
CodeBehind="AddProduct.aspx.cs"
Inherits="MultiGroupSystemsTester.Admin.AddProduct" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server"> <link href="AddProduct.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="BreadcrumbContent" ContentPlaceHolderID="BreadcrumbContent" runat="server">
Add Product
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">

```
<div class="page-container">

    

    <!-- Header -->
    <header class="page-header">

        <div>
            <h1>Add Product</h1>
            <p>Add a new product to your store</p>
        </div>

        <div class="header-actions">

            <button type="button" class="cancel-button">
                Cancel
            </button>

            <button type="button" class="save-button">
                Add Product
            </button>

        </div>

    </header>


    <!-- Main Content -->
    <main class="content-area">

        <!-- Left Column -->
        <section class="left-column">

            <!-- Basic Information -->
            <div class="card">

                <div class="card-header">
                    <h2>Product Information</h2>
                    <p>
                        Enter the basic information about your product.
                    </p>
                </div>

                <div class="card-body">

                    <!-- Product Name -->
                    <div class="form-group">

                        <label for="txtProductName">
                            Product Name
                            <span class="required">*</span>
                        </label>

                        <asp:TextBox
                            ID="txtProductName"
                            runat="server"
                            CssClass="form-control"
                            placeholder="Enter product name">
                        </asp:TextBox>

                    </div>


                    <!-- Category and Price -->
                    <div class="form-row">

                        <div class="form-group">

                            <label for="ddlCategory">
                                Category
                                <span class="required">*</span>
                            </label>

                            <asp:DropDownList
                                ID="ddlCategory"
                                runat="server"
                                CssClass="form-control">

                                <asp:ListItem Value="">
                                    Select category
                                </asp:ListItem>

                                <asp:ListItem>
                                    Electronics
                                </asp:ListItem>

                                <asp:ListItem>
                                    Stationery
                                </asp:ListItem>

                                <asp:ListItem>
                                    Clothing
                                </asp:ListItem>

                                <asp:ListItem>
                                    Home &amp; Garden
                                </asp:ListItem>

                                <asp:ListItem>
                                    Accessories
                                </asp:ListItem>

                            </asp:DropDownList>

                        </div>


                        <div class="form-group">

                            <label for="txtPrice">
                                Price
                                <span class="required">*</span>
                            </label>

                            <div class="price-input">

                                <span class="currency">R</span>

                                <asp:TextBox
                                    ID="txtPrice"
                                    runat="server"
                                    CssClass="price-control"
                                    placeholder="0.00">
                                </asp:TextBox>

                            </div>

                        </div>

                    </div>


                    <!-- Description -->
                    <div class="form-group">

                        <label for="txtDescription">
                            Product Description
                        </label>

                        <asp:TextBox
                            ID="txtDescription"
                            runat="server"
                            CssClass="description-control"
                            TextMode="MultiLine"
                            Rows="6"
                            placeholder="Describe your product...">
                        </asp:TextBox>

                        <span class="field-help">
                            Provide a clear description of the product.
                        </span>

                    </div>

                </div>

            </div>


            <!-- Inventory -->
            <div class="card">

                <div class="card-header">

                    <h2>Inventory</h2>

                    <p>
                        Manage your product stock information.
                    </p>

                </div>

                <div class="card-body">

                    <div class="form-row">

                        <!-- Stock Quantity -->
                        <div class="form-group">

                            <label for="txtStock">
                                Stock Quantity
                                <span class="required">*</span>
                            </label>

                            <asp:TextBox
                                ID="txtStock"
                                runat="server"
                                CssClass="form-control"
                                placeholder="e.g. 50">
                            </asp:TextBox>

                        </div>


                        <!-- Product Status -->
                        <div class="form-group">

                            <label for="ddlStatus">
                                Product Status
                            </label>

                            <asp:DropDownList
                                ID="ddlStatus"
                                runat="server"
                                CssClass="form-control">

                                <asp:ListItem>
                                    Active
                                </asp:ListItem>

                                <asp:ListItem>
                                    Inactive
                                </asp:ListItem>

                                <asp:ListItem>
                                    Out of Stock
                                </asp:ListItem>

                            </asp:DropDownList>

                        </div>

                    </div>

                </div>

            </div>

        </section>


        <!-- Right Column -->
        <aside class="right-column">

            <!-- Product Image -->
            <div class="card">

                <div class="card-header">

                    <h2>Product Image</h2>

                    <p>
                        Upload an image of your product.
                    </p>

                </div>

                <div class="card-body">

                    <div class="image-upload">

                        <div class="upload-icon">
                            +
                        </div>

                        <h3>
                            Upload Product Image
                        </h3>

                        <p>
                            Click below to select an image
                        </p>

                        <asp:FileUpload
                            ID="fileProductImage"
                            runat="server"
                            CssClass="file-upload" />

                        <span class="upload-help">
                            JPG, PNG or JPEG
                        </span>

                    </div>

                </div>

            </div>


            <!-- Product Summary -->
            <div class="card summary-card">

                <div class="card-header">

                    <h2>Product Summary</h2>

                    <p>
                        Review your product information.
                    </p>

                </div>

                <div class="card-body">

                    <div class="summary-item">

                        <span>Product</span>

                        <strong>New Product</strong>

                    </div>


                    <div class="summary-item">

                        <span>Category</span>

                        <strong>Not selected</strong>

                    </div>


                    <div class="summary-item">

                        <span>Price</span>

                        <strong>R 0.00</strong>

                    </div>


                    <div class="summary-item">

                        <span>Stock</span>

                        <strong>0 units</strong>

                    </div>

                </div>

            </div>

        </aside>

    </main>

</div>
```

</asp:Content>
