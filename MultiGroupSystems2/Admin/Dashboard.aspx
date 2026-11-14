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
                <div class="stat-icon products-icon">▣</div>
                <div class="stat-information">
                    <span>Total Products</span>
                    <strong><asp:Label ID="lblTotalProducts" runat="server" Text="0" /></strong>
                </div>
            </div>

            <!-- ORDERS -->
            <div class="stat-card">
                <div class="stat-icon orders-icon">□</div>
                <div class="stat-information">
                    <span>Total Orders</span>
                    <strong><asp:Label ID="lblTotalOrders" runat="server" Text="0" /></strong>
                </div>
            </div>

            <!-- STAFF -->
            <div class="stat-card">
                <div class="stat-icon staff-icon">♙</div>
                <div class="stat-information">
                    <span>Total Staff</span>
                    <strong><asp:Label ID="lblTotalStaff" runat="server" Text="0" /></strong>
                </div>
            </div>

            <!-- SALES -->
            <div class="stat-card">
                <div class="stat-icon sales-icon">R</div>
                <div class="stat-information">
                    <span>Total Sales</span>
                    <strong><asp:Label ID="lblTotalSales" runat="server" Text="R 0" /></strong>
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
                        <p><asp:Label ID="lblChartSub" runat="server" /></p>
                    </div>
                    <asp:DropDownList ID="ddlPeriod" runat="server" CssClass="period-select"
                        AutoPostBack="true" OnSelectedIndexChanged="ddlPeriod_SelectedIndexChanged">
                        <asp:ListItem Value="7d">Last 7 days</asp:ListItem>
                        <asp:ListItem Value="30d" Selected="True">Last 30 days</asp:ListItem>
                        <asp:ListItem Value="6m">Last 6 months</asp:ListItem>
                        <asp:ListItem Value="ytd">This year</asp:ListItem>
                    </asp:DropDownList>
                </div>

                <!-- DYNAMIC CHART -->
                <div class="chart-area">
                    <div class="chart-y-axis">
                        <span><asp:Label ID="lblY1" runat="server" /></span>
                        <span><asp:Label ID="lblY2" runat="server" /></span>
                        <span><asp:Label ID="lblY3" runat="server" /></span>
                        <span><asp:Label ID="lblY4" runat="server" /></span>
                        <span><asp:Label ID="lblY5" runat="server" /></span>
                    </div>
                    <div class="chart">
                        <div class="grid-line line-1"></div>
                        <div class="grid-line line-2"></div>
                        <div class="grid-line line-3"></div>
                        <div class="grid-line line-4"></div>
                        <div class="grid-line line-5"></div>

                        <div class="chart-bars">
                            <asp:Repeater ID="rptSalesChart" runat="server">
                                <ItemTemplate>
                                    <div class="bar-group">
                                        <div class="bar" style='<%# "height: " + Eval("BarHeight") + "%;" %>'
                                             title='<%# Eval("Tooltip") %>'></div>
                                        <span><%# Eval("Label") %></span>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
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
                        <p>Latest customer orders</p>
                    </div>
                    <a href="ViewOrders.aspx" class="view-link">View All</a>
                </div>

                <div class="table-container">
                    <table>
                        <thead>
                            <tr>
                                <th>Order ID</th>
                                <th>Customer</th>
                                <th>Product Summary</th>
                                <th>Amount</th>
                                <th>Status</th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater ID="rptRecentOrders" runat="server">
                                <ItemTemplate>
                                    <tr>
                                       <td><%# Eval("OrderID") %></td>
                                       <td><%# Eval("FirstName") %> <%# Eval("LastName") %></td>
                                        <td><%# Eval("ProductSummary") %></td>
                                        <td>R <%# Eval("TotalAmount", "{0:N0}") %></td>
                                        <td>
                                            <span class='status <%# Eval("OrderStatus").ToString().ToLower() %>'>
                                                <%# Eval("OrderStatus") %>
                                            </span>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                </div>
            </div>

            <!-- QUICK ACTIONS -->
            <div class="dashboard-card">
                <div class="card-heading">
                    <div>
                        <h2>Quick Actions</h2>
                        <p>Common management tasks</p>
                    </div>
                </div>

                <div class="quick-actions">
                    <a href="AddProduct.aspx" class="quick-action">
                        <div class="quick-icon">+</div>
                        <div>
                            <strong>Add Product</strong>
                            <span>Create a new product</span>
                        </div>
                        <span class="arrow">→</span>
                    </a>

                    <a href="StaffDataCapture.aspx" class="quick-action">
                        <div class="quick-icon">♙</div>
                        <div>
                            <strong>Add Staff</strong>
                            <span>Register a staff member</span>
                        </div>
                        <span class="arrow">→</span>
                    </a>

                    <a href="OrderLogistics.aspx" class="quick-action">
                        <div class="quick-icon">□</div>
                        <div>
                            <strong>View Orders</strong>
                            <span>Manage recent orders</span>
                        </div>
                        <span class="arrow">→</span>
                    </a>

                    <a href="Reports.aspx" class="quick-action">
                        <div class="quick-icon">▤</div>
                        <div>
                            <strong>View Reports</strong>
                            <span>Check store performance</span>
                        </div>
                        <span class="arrow">→</span>
                    </a>
                </div>
            </div>

        </section>

    </div>

</asp:Content>
