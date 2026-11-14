<%@ Page Title="Access Denied" Language="C#" AutoEventWireup="true"
    MasterPageFile="~/Admin/AdminMaster.master"
    CodeBehind="AccessDenied.aspx.cs"
    Inherits="MultiGroupSystemsTester.Admin.AccessDenied" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .denied-card {
            max-width: 520px;
            margin: 70px auto;
            padding: 40px 32px;
            text-align: center;
            background: #ffffff;
            border: 1px solid #e2e8f0;
            border-radius: 12px;
        }

        .denied-icon {
            width: 56px;
            height: 56px;
            margin: 0 auto 18px;
            display: flex;
            align-items: center;
            justify-content: center;
            font-size: 26px;
            background: #fee2e2;
            border-radius: 50%;
        }

        .denied-card h1 {
            margin: 0 0 8px;
            font-size: 22px;
        }

        .denied-card p {
            margin: 0 0 24px;
            color: #64748b;
            font-size: 14px;
        }

        .denied-link {
            display: inline-block;
            padding: 10px 22px;
            background: #2563eb;
            color: #ffffff;
            font-size: 14px;
            font-weight: 600;
            text-decoration: none;
            border-radius: 8px;
        }

        .denied-link:hover {
            background: #1d4ed8;
        }

        html[data-theme="dark"] .denied-card {
            background: #111a2e;
            border-color: #1f2a44;
        }

        html[data-theme="dark"] .denied-icon {
            background: #3b1d24;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BreadcrumbContent" runat="server">
    Access Denied
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div class="denied-card">
        <div class="denied-icon">🔒</div>
        <h1>Access denied</h1>
        <p>Your role doesn't have permission to open that page. If you think this is a mistake, please contact a system administrator.</p>
        <a href="Dashboard.aspx" class="denied-link">Back to Dashboard</a>
    </div>
</asp:Content>
