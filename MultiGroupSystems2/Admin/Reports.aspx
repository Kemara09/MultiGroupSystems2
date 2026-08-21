<%@ Page Title="Reports" Language="C#" AutoEventWireup="true" MasterPageFile="~/Admin/AdminMaster.master" CodeBehind="Reports.aspx.cs" Inherits="MultiGroupSystemsTester.Reports" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Reports.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BreadcrumbContent" runat="server">
    Reports
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <section class="page-heading">
        <div>
            <h1>Reports</h1>
            <p>View, manage and monitor reports submitted by your group.</p>
        </div>
        <button type="button" class="create-button"><span>+</span> Create Report</button>
    </section>

    <section class="summary-cards">
        <div class="summary-card">
            <div class="card-icon blue">▤</div>
            <div class="card-information"><span>Total Reports</span><h2>12</h2><small>All submitted reports</small></div>
        </div>
        <div class="summary-card">
            <div class="card-icon green">✓</div>
            <div class="card-information"><span>Completed</span><h2>8</h2><small>Reports completed</small></div>
        </div>
        <div class="summary-card">
            <div class="card-icon orange">◷</div>
            <div class="card-information"><span>Pending</span><h2>3</h2><small>Awaiting completion</small></div>
        </div>
        <div class="summary-card">
            <div class="card-icon purple">♙</div>
            <div class="card-information"><span>Group Members</span><h2>5</h2><small>Active members</small></div>
        </div>
    </section>

    <section class="content-box">
        <div class="section-header">
            <div><h2>Recent Reports</h2><p>Latest reports submitted by the group</p></div>
            <button type="button" class="view-all">View All</button>
        </div>

        <div class="table-container">
            <table class="reports-table">
                <thead>
                    <tr><th>REPORT NAME</th><th>GROUP</th><th>TYPE</th><th>SUBMITTED</th><th>STATUS</th><th>ACTION</th></tr>
                </thead>
                <tbody>
                    <tr>
                        <td><div class="report-name"><div class="report-icon">📄</div><div><strong>System Analysis Report</strong><small>Multi Group Systems</small></div></div></td>
                        <td>Group 1</td><td>Analysis</td><td>15 Aug 2026</td>
                        <td><span class="status completed">● Completed</span></td>
                        <td><button type="button" class="action-button">View</button></td>
                    </tr>
                    <tr>
                        <td><div class="report-name"><div class="report-icon">📋</div><div><strong>Requirements Report</strong><small>Multi Group Systems</small></div></div></td>
                        <td>Group 1</td><td>Requirements</td><td>12 Aug 2026</td>
                        <td><span class="status completed">● Completed</span></td>
                        <td><button type="button" class="action-button">View</button></td>
                    </tr>
                    <tr>
                        <td><div class="report-name"><div class="report-icon">🎨</div><div><strong>System Design Report</strong><small>Multi Group Systems</small></div></div></td>
                        <td>Group 1</td><td>Design</td><td>10 Aug 2026</td>
                        <td><span class="status pending">● Pending</span></td>
                        <td><button type="button" class="action-button">View</button></td>
                    </tr>
                    <tr>
                        <td><div class="report-name"><div class="report-icon">🧪</div><div><strong>Testing Report</strong><small>Multi Group Systems</small></div></div></td>
                        <td>Group 1</td><td>Testing</td><td>08 Aug 2026</td>
                        <td><span class="status review">● In Review</span></td>
                        <td><button type="button" class="action-button">View</button></td>
                    </tr>
                </tbody>
            </table>
        </div>
    </section>

    <section class="categories-section">
        <div class="section-header">
            <div><h2>Report Categories</h2><p>Browse reports according to their category</p></div>
        </div>

        <div class="category-grid">
            <div class="category-card">
                <div class="category-icon blue-bg">📋</div><h3>Requirements</h3>
                <p>Functional and non-functional system requirements.</p>
                <div class="category-bottom"><span>3 Reports</span><span class="category-arrow">→</span></div>
            </div>
            <div class="category-card">
                <div class="category-icon purple-bg">🎨</div><h3>Design</h3>
                <p>System architecture, interface and design reports.</p>
                <div class="category-bottom"><span>2 Reports</span><span class="category-arrow">→</span></div>
            </div>
            <div class="category-card">
                <div class="category-icon orange-bg">🧪</div><h3>Testing</h3>
                <p>Testing documentation and system evaluation reports.</p>
                <div class="category-bottom"><span>4 Reports</span><span class="category-arrow">→</span></div>
            </div>
            <div class="category-card">
                <div class="category-icon green-bg">✓</div><h3>Final Reports</h3>
                <p>Completed project documentation and final submissions.</p>
                <div class="category-bottom"><span>3 Reports</span><span class="category-arrow">→</span></div>
            </div>
        </div>
    </section>

</asp:Content>