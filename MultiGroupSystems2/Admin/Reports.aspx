<%@ Page Title="Reports" Language="C#" AutoEventWireup="true"
    MasterPageFile="~/Admin/AdminMaster.master"
    CodeBehind="Reports.aspx.cs" Inherits="MultiGroupSystemsTester.Reports" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" href="<%= ResolveUrl("~/Admin/Reports.css") %>" />
    <style>
        .business-reports .br-chart-wrap {
            position: relative;
            width: 100%;
            height: 320px;
            margin: 12px 0 18px;
        }

        .business-reports .br-overview .br-chart-wrap {
            height: 250px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BreadcrumbContent" runat="server">
    Reports
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div class="business-reports">
        <header class="br-heading">
            <div>
                <h1>Reports</h1>
                <p>Monitor orders, customer activity and business performance.</p>
            </div>
            <a class="br-button" href="?report=create">+ Create Report</a>
        </header>

        <% if (LoadError != null) { %>
            <p class="br-panel" role="alert"><%: LoadError %></p>
        <% } else if (ActiveReport == null) { %>
            <h2>Business overview</h2>
            <div class="br-overview">
                <% foreach (var chart in Charts) { %>
                    <section class="br-panel">
                        <h3><%: chart.Title %></h3>
                        <%= RenderChart(chart) %>
                    </section>
                <% } %>
            </div>
        <% } %>

        <nav aria-label="Business reports">
            <h2>Browse reports</h2>
            <div class="br-links">
                <a href="Reports.aspx"
                   aria-current="<%: ActiveReport == null && OpenReportId == 0 ? "page" : "false" %>">Overview</a>
                <a href="?report=saved" aria-current="<%: ActiveReport == "saved" ? "page" : "false" %>">Saved Reports</a>
                <% foreach (var report in ReportLinks) { %>
                    <a href="?report=<%: report.Key %>"
                       aria-current="<%: report.Key == ActiveReport ? "page" : "false" %>"><%: report.Value %></a>
                <% } %>
                <% if (CustomReportLinks != null) { %>
                    <% foreach (System.Data.DataRow report in CustomReportLinks.Rows) { %>
                        <a href="?saved=<%: report["ReportID"] %>"
                           aria-current="<%: (int)report["ReportID"] == OpenReportId ? "page" : "false" %>"><%: report["ReportTitle"] %></a>
                    <% } %>
                <% } %>
            </div>
            <% if (CustomLinksError != null) { %>
                <p role="status"><%: CustomLinksError %></p>
            <% } %>
        </nav>

        <% if (ActiveReport != null && LoadError == null) { %>
            <section class="br-panel">
                <h2><%: ReportTitle %></h2>
                <% if (!string.IsNullOrEmpty(ReportNote)) { %>
                    <p><%: ReportNote %></p>
                <% } %>

                <% if (ActiveReport == "create") { %>
                    <% if (SaveError != null) { %>
                        <p role="alert"><%: SaveError %></p>
                    <% } %>
                    <div class="br-fields">
                        <input type="hidden" name="reportToken" value="<%: SaveToken %>" />

                        <label for="reportTitle">Report name</label>
                        <input id="reportTitle" name="reportTitle" maxlength="200"
                               value="<%: EnteredTitle %>" placeholder="e.g. Monthly Returns" />

                        <label for="reportMetric">Calculation</label>
                        <select id="reportMetric" name="reportMetric">
                            <% foreach (var metric in CustomMetrics) { %>
                                <% if (metric.Key == EnteredMetric) { %>
                                    <option value="<%: metric.Key %>" selected><%: metric.Value %></option>
                                <% } else { %>
                                    <option value="<%: metric.Key %>"><%: metric.Value %></option>
                                <% } %>
                            <% } %>
                        </select>

                        <label for="reportChart">Chart type</label>
                        <select id="reportChart" name="reportChart">
                            <% foreach (var chartType in ChartTypes) { %>
                                <% if (chartType.Key == EnteredChartType) { %>
                                    <option value="<%: chartType.Key %>" selected><%: chartType.Value %></option>
                                <% } else { %>
                                    <option value="<%: chartType.Key %>"><%: chartType.Value %></option>
                                <% } %>
                            <% } %>
                        </select>
                        <small>Use a column or line chart for recorded order values.</small>

                        <asp:Button ID="SaveReportButton" runat="server" CssClass="br-button"
                            Text="Create Report" CausesValidation="false" OnClick="SaveReport_Click" />
                    </div>
                <% } %>

                <% foreach (var chart in Charts) { %>
                    <h3><%: chart.Title %></h3>
                    <%= RenderChart(chart) %>
                <% } %>

                <% if (Details != null) { %>
                    <% if (Details.Rows.Count == 0) { %>
                        <p>No rows found on this page.</p>
                    <% } else { %>
                        <div class="br-table">
                            <table>
                                <thead>
                                    <tr>
                                        <% foreach (System.Data.DataColumn column in Details.Columns) { %>
                                            <th scope="col"><%: column.ColumnName %></th>
                                        <% } %>
                                        <% if (ActiveReport == "saved") { %><th scope="col">Action</th><% } %>
                                    </tr>
                                </thead>
                                <tbody>
                                    <% foreach (System.Data.DataRow row in Details.Rows) { %>
                                        <tr>
                                            <% foreach (object value in row.ItemArray) { %>
                                                <td><%: FormatCell(value) %></td>
                                            <% } %>
                                            <% if (ActiveReport == "saved") { %>
                                                <td><a href="?saved=<%: row["Report ID"] %>">Open report</a></td>
                                            <% } %>
                                        </tr>
                                    <% } %>
                                </tbody>
                            </table>
                        </div>
                    <% } %>
                    <div class="br-links">
                        <% if (PageNumber > 1) { %>
                            <a href="<%: PageLink %>&amp;p=<%: PageNumber - 1 %>">Previous page</a>
                        <% } %>
                        <% if (HasNextPage) { %>
                            <a href="<%: PageLink %>&amp;p=<%: PageNumber + 1 %>">Next page</a>
                        <% } %>
                    </div>
                <% } %>
            </section>
        <% } %>
    </div>

    <script src="https://cdn.jsdelivr.net/npm/chart.js@4.4.9/dist/chart.umd.min.js"></script>
    <script>
        (function () {
            var colours = ["#2563eb", "#16a34a", "#f59e0b", "#7c3aed", "#dc2626", "#0891b2"];

            document.querySelectorAll(".business-reports .br-chart").forEach(function (canvas) {
                if (typeof Chart === "undefined") {
                    var message = document.createElement("p");
                    message.textContent = "Chart.js could not load. Check your internet connection.";
                    canvas.parentNode.replaceChild(message, canvas);
                    return;
                }

                var report = JSON.parse(canvas.getAttribute("data-chart"));
                var circular = report.type === "pie" || report.type === "doughnut";
                var formatter = new Intl.NumberFormat("en-ZA", report.currency
                    ? { style: "currency", currency: "ZAR" }
                    : { maximumFractionDigits: 0 });

                if (circular && report.values.every(function (value) { return value === 0; })) {
                    var empty = document.createElement("p");
                    empty.textContent = "All values are zero for this chart.";
                    canvas.parentNode.replaceChild(empty, canvas);
                    return;
                }

                var options = {
                    responsive: true,
                    maintainAspectRatio: false,
                    plugins: {
                        legend: { display: circular, position: "bottom" },
                        tooltip: {
                            callbacks: {
                                label: function (item) {
                                    return item.label + ": " + formatter.format(item.raw);
                                }
                            }
                        }
                    }
                };

                if (!circular) {
                    options.indexAxis = report.horizontal ? "y" : "x";
                    options.scales = {};
                    options.scales[report.horizontal ? "x" : "y"] = {
                        beginAtZero: true,
                        ticks: {
                            callback: function (value) {
                                if (!report.currency && value % 1 !== 0) return "";
                                return formatter.format(value);
                            }
                        }
                    };
                }

                new Chart(canvas, {
                    type: report.type,
                    data: {
                        labels: report.labels,
                        datasets: [{
                            label: report.title,
                            data: report.values,
                            backgroundColor: circular
                                ? report.labels.map(function (_, index) {
                                    return colours[index % colours.length];
                                })
                                : "#2563eb",
                            borderColor: circular ? "#ffffff" : "#2563eb",
                            borderWidth: circular ? 2 : 3,
                            borderRadius: report.type === "bar" ? 5 : 0,
                            tension: 0.25
                        }]
                    },
                    options: options
                });
            });
        })();
    </script>
</asp:Content>
