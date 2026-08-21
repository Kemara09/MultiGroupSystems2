<%@ Page Title="Dashboard" Language="C#" AutoEventWireup="true"
    CodeBehind="Dashboard.aspx.cs"
    MasterPageFile="~/Admin/AdminMaster.master"
    Inherits="MultiGroupSystemsTester.Admin.Dashboard" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Dashboard.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="BreadcrumbContent" ContentPlaceHolderID="BreadcrumbContent" runat="server">
    Dashboard
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="dashboard-content">

        <!-- STATISTICS -->

        <section class="statistics-grid">

            <!-- PRODUCTS -->
            <div class="stat-card">

                <div class="stat-icon products-icon">
                    ▣
                </div>

                <div class="stat-information">

                    <span>Total Products</span>

                    <strong>248</strong>

                    <small>
                        <b>+12%</b> from last month
                    </small>

                </div>

            </div>


            <!-- ORDERS -->
            <div class="stat-card">

                <div class="stat-icon orders-icon">
                    □
                </div>

                <div class="stat-information">

                    <span>Total Orders</span>

                    <strong>1,284</strong>

                    <small>
                        <b>+8.5%</b> from last month
                    </small>

                </div>

            </div>


            <!-- STAFF -->
            <div class="stat-card">

                <div class="stat-icon staff-icon">
                    ♙
                </div>

                <div class="stat-information">

                    <span>Total Staff</span>

                    <strong>36</strong>

                    <small>
                        <b>+2</b> new this month
                    </small>

                </div>

            </div>


            <!-- SALES -->
            <div class="stat-card">

                <div class="stat-icon sales-icon">
                    R
                </div>

                <div class="stat-information">

                    <span>Total Sales</span>

                    <strong>R84,620</strong>

                    <small>
                        <b>+15.2%</b> from last month
                    </small>

                </div>

            </div>

        </section>


        <!-- MAIN DASHBOARD GRID -->

        <section class="dashboard-grid">

            <!-- SALES OVERVIEW -->

            <div class="dashboard-card sales-card">

                <div class="card-heading">

                    <div>

                        <h2>Sales Overview</h2>

                        <p>
                            Monthly sales performance
                        </p>

                    </div>

                    <select class="period-select">

                        <option>Last 7 days</option>

                        <option selected="selected">
                            Last 30 days
                        </option>

                        <option>
                            Last 6 months
                        </option>

                        <option>
                            This year
                        </option>

                    </select>

                </div>


                <!-- FAKE CHART -->

                <div class="chart-area">

                    <div class="chart-y-axis">

                        <span>R20k</span>
                        <span>R15k</span>
                        <span>R10k</span>
                        <span>R5k</span>
                        <span>R0</span>

                    </div>


                    <div class="chart">

                        <div class="grid-line line-1"></div>
                        <div class="grid-line line-2"></div>
                        <div class="grid-line line-3"></div>
                        <div class="grid-line line-4"></div>
                        <div class="grid-line line-5"></div>


                        <div class="chart-bars">

                            <div class="bar-group">
                                <div class="bar bar-1" style="height: 45%;"></div>
                                <span>Jan</span>
                            </div>

                            <div class="bar-group">
                                <div class="bar bar-2" style="height: 58%;"></div>
                                <span>Feb</span>
                            </div>

                            <div class="bar-group">
                                <div class="bar bar-3" style="height: 42%;"></div>
                                <span>Mar</span>
                            </div>

                            <div class="bar-group">
                                <div class="bar bar-4" style="height: 70%;"></div>
                                <span>Apr</span>
                            </div>

                            <div class="bar-group">
                                <div class="bar bar-5" style="height: 62%;"></div>
                                <span>May</span>
                            </div>

                            <div class="bar-group">
                                <div class="bar bar-6" style="height: 84%;"></div>
                                <span>Jun</span>
                            </div>

                            <div class="bar-group">
                                <div class="bar bar-7" style="height: 76%;"></div>
                                <span>Jul</span>
                            </div>

                            <div class="bar-group">
                                <div class="bar bar-8" style="height: 92%;"></div>
                                <span>Aug</span>
                            </div>

                        </div>

                    </div>

                </div>

            </div>


            <!-- PRODUCT OVERVIEW -->

            <div class="dashboard-card">

                <div class="card-heading">

                    <div>

                        <h2>Product Overview</h2>

                        <p>
                            Current product status
                        </p>

                    </div>

                    <a href="#" class="view-link">
                        View All
                    </a>

                </div>


                <div class="product-overview">

                    <div class="overview-row">

                        <div class="overview-label">

                            <span class="overview-dot active-dot"></span>

                            <span>Active Products</span>

                        </div>

                        <strong>214</strong>

                    </div>


                    <div class="progress-bar">

                        <div class="progress active-progress"
                            style="width: 86%;">
                        </div>

                    </div>


                    <div class="overview-row">

                        <div class="overview-label">

                            <span class="overview-dot inactive-dot"></span>

                            <span>Inactive Products</span>

                        </div>

                        <strong>22</strong>

                    </div>


                    <div class="progress-bar">

                        <div class="progress inactive-progress"
                            style="width: 9%;">
                        </div>

                    </div>


                    <div class="overview-row">

                        <div class="overview-label">

                            <span class="overview-dot stock-dot"></span>

                            <span>Out of Stock</span>

                        </div>

                        <strong>12</strong>

                    </div>


                    <div class="progress-bar">

                        <div class="progress stock-progress"
                            style="width: 5%;">
                        </div>

                    </div>

                </div>

            </div>

        </section>


        <!-- LOWER DASHBOARD GRID -->

        <section class="lower-grid">

            <!-- RECENT ORDERS -->

            <div class="dashboard-card orders-card">

                <div class="card-heading">

                    <div>

                        <h2>Recent Orders</h2>

                        <p>
                            Latest customer orders
                        </p>

                    </div>

                    <a href="#" class="view-link">
                        View All
                    </a>

                </div>


                <div class="table-container">

                    <table>

                        <thead>

                            <tr>
                                <th>Order ID</th>
                                <th>Customer</th>
                                <th>Product</th>
                                <th>Amount</th>
                                <th>Status</th>
                            </tr>

                        </thead>


                        <tbody>

                            <tr>
                                <td>#ORD-1024</td>
                                <td>John Smith</td>
                                <td>Wireless Headphones</td>
                                <td>R899</td>
                                <td>
                                    <span class="status completed">
                                        Completed
                                    </span>
                                </td>
                            </tr>


                            <tr>
                                <td>#ORD-1023</td>
                                <td>Sarah Jones</td>
                                <td>Office Backpack</td>
                                <td>R650</td>
                                <td>
                                    <span class="status pending">
                                        Pending
                                    </span>
                                </td>
                            </tr>


                            <tr>
                                <td>#ORD-1022</td>
                                <td>Michael Brown</td>
                                <td>USB Keyboard</td>
                                <td>R450</td>
                                <td>
                                    <span class="status processing">
                                        Processing
                                    </span>
                                </td>
                            </tr>


                            <tr>
                                <td>#ORD-1021</td>
                                <td>Emily Wilson</td>
                                <td>Laptop Stand</td>
                                <td>R799</td>
                                <td>
                                    <span class="status completed">
                                        Completed
                                    </span>
                                </td>
                            </tr>

                        </tbody>

                    </table>

                </div>

            </div>


            <!-- QUICK ACTIONS -->

            <div class="dashboard-card">

                <div class="card-heading">

                    <div>

                        <h2>Quick Actions</h2>

                        <p>
                            Common management tasks
                        </p>

                    </div>

                </div>


                <div class="quick-actions">

                    <a href="#" class="quick-action">

                        <div class="quick-icon">
                            +
                        </div>

                        <div>

                            <strong>Add Product</strong>

                            <span>
                                Create a new product
                            </span>

                        </div>

                        <span class="arrow">
                            →
                        </span>

                    </a>


                    <a href="#" class="quick-action">

                        <div class="quick-icon">
                            ♙
                        </div>

                        <div>

                            <strong>Add Staff</strong>

                            <span>
                                Register a staff member
                            </span>

                        </div>

                        <span class="arrow">
                            →
                        </span>

                    </a>


                    <a href="#" class="quick-action">

                        <div class="quick-icon">
                            □
                        </div>

                        <div>

                            <strong>View Orders</strong>

                            <span>
                                Manage recent orders
                            </span>

                        </div>

                        <span class="arrow">
                            →
                        </span>

                    </a>


                    <a href="#" class="quick-action">

                        <div class="quick-icon">
                            ▤
                        </div>

                        <div>

                            <strong>View Reports</strong>

                            <span>
                                Check store performance
                            </span>

                        </div>

                        <span class="arrow">
                            →
                        </span>

                    </a>

                </div>

            </div>

        </section>

    </div>

</asp:Content>