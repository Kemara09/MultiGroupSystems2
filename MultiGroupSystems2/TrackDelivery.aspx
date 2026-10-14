<%@ Page Title="Track Delivery" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TrackDelivery.aspx.cs" Inherits="MultiGroupSystemsTester.TrackDelivery" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    
    <link rel="stylesheet" href="https://unpkg.com/leaflet@1.9.4/dist/leaflet.css" />
    
<style>
    /* Track Delivery page — light theme defaults */
    .track-delivery-page {
        --track-page-bg: #f1efe8;
        --track-card-bg: #ffffff;
        --track-text: #172b3a;
        --track-muted: #64748b;
        --track-input-bg: #ffffff;
        --track-border: #dbe3ec;
        --track-progress-bg: #e2e5e9;
        --track-progress: #00899a;
        --track-banner: #00899a;
        --track-button: #00899a;
        --track-error-bg: #fef2f2;
        --track-error-text: #b91c1c;
        --track-shadow: 0 10px 30px rgba(0, 0, 0, 0.12);

        background: var(--track-page-bg);
        color: var(--track-text);
        min-height: 70vh;
        padding: 24px 16px 48px;
        transition: background-color 0.2s, color 0.2s;
    }

    /* Dark theme */
    [data-theme="dark"] .track-delivery-page {
        --track-page-bg: #151b24;
        --track-card-bg: #202936;
        --track-text: #f1f5f9;
        --track-muted: #b7c2d0;
        --track-input-bg: #18212c;
        --track-border: #3b4757;
        --track-progress-bg: #414b59;
        --track-progress: #20b8c7;
        --track-banner: #087f90;
        --track-button: #087f90;
        --track-error-bg: #452626;
        --track-error-text: #fecaca;
        --track-shadow: 0 10px 30px rgba(0, 0, 0, 0.3);
    }

    .track-delivery-page .outer-page-wrapper {
        width: 100%;
        max-width: 650px;
        margin: 0 auto;
        overflow: hidden;
        border-radius: 20px;
        background: var(--track-card-bg);
        color: var(--track-text);
        box-shadow: var(--track-shadow);
    }

    .track-delivery-page .blue-banner-section {
        background: var(--track-banner);
        color: #ffffff;
        text-align: center;
        padding: 28px 20px;
    }

    .track-delivery-page .blue-banner-section h2 {
        margin: 0 0 8px;
        color: #ffffff;
        font-size: 1.3rem;
        font-weight: 700;
    }

    .track-delivery-page .blue-banner-section p {
        margin: 0;
        color: #ffffff;
        font-size: 0.85rem;
    }

    .track-delivery-page .track-content-area {
        padding: 30px;
        background: var(--track-card-bg);
        color: var(--track-text);
    }

    .track-delivery-page .error-msg {
        display: block;
        color: var(--track-error-text);
        margin-bottom: 12px;
        font-size: 0.9rem;
    }

    .track-delivery-page .error-msg:not(:empty) {
        background: var(--track-error-bg);
        border-radius: 8px;
        padding: 10px 12px;
    }

    .track-delivery-page .search-box-wrapper {
        display: flex;
        align-items: center;
        gap: 10px;
        margin-bottom: 22px;
    }

    .track-delivery-page .search-input {
        flex: 1;
        min-width: 0;
        width: 100%;
        height: 40px;
        padding: 0 14px;
        border: 1px solid var(--track-border);
        border-radius: 24px;
        background: var(--track-input-bg);
        color: var(--track-text);
        font-size: 0.85rem;
        outline: none;
    }

    .track-delivery-page .search-input::placeholder {
        color: var(--track-muted);
        opacity: 1;
    }

    .track-delivery-page .search-input:focus {
        border-color: var(--track-progress);
        box-shadow: 0 0 0 3px rgba(0, 137, 154, 0.15);
    }

    .track-delivery-page .btn-search {
        flex-shrink: 0;
        border: none;
        border-radius: 24px;
        background: var(--track-button);
        color: #ffffff;
        padding: 11px 19px;
        font-weight: 600;
        font-size: 0.85rem;
        cursor: pointer;
    }

    .track-delivery-page .btn-search:hover {
        filter: brightness(1.1);
    }

    .track-delivery-page .card-header {
        padding: 0;
        margin: 0 0 18px;
        border: none;
        background: transparent;
        text-align: center;
    }

    .track-delivery-page .status-heading {
        margin: 0;
        color: var(--track-text);
        font-size: 1.1rem;
        font-weight: 700;
    }

    /* Four-step delivery progress indicator */
    .track-delivery-page .stepper-wrapper {
        position: relative;
        margin: 0 12px 30px;
        padding-top: 0;
    }

    .track-delivery-page .progress-bg-bar,
    .track-delivery-page .progress-fill-bar {
        position: absolute;
        top: 11px;
        left: 6%;
        height: 6px;
        border-radius: 6px;
    }

    .track-delivery-page .progress-bg-bar {
        width: 88%;
        background: var(--track-progress-bg);
    }

    .track-delivery-page .progress-fill-bar {
        width: 0;
        background: var(--track-progress);
        transition: width 0.5s ease;
    }

    .track-delivery-page .steps-container {
        position: relative;
        display: flex;
        justify-content: space-between;
    }

    .track-delivery-page .step-item {
        z-index: 1;
        display: flex;
        flex: 1;
        min-width: 0;
        align-items: center;
        flex-direction: column;
        text-align: center;
    }

    .track-delivery-page .dot-circle {
        display: flex;
        align-items: center;
        justify-content: center;
        width: 26px;
        height: 26px;
        margin-bottom: 8px;
        border-radius: 50%;
        background: var(--track-progress-bg);
        color: transparent;
        border: 2px solid var(--track-progress-bg);
        font-size: 13px;
        font-weight: 700;
        transition: background-color 0.2s, border-color 0.2s;
    }

    .track-delivery-page .step-item.active .dot-circle,
    .track-delivery-page .step-item.completed .dot-circle {
        background: var(--track-progress);
        border-color: var(--track-progress);
        color: #ffffff;
    }

    .track-delivery-page .step-label {
        color: var(--track-muted);
        font-size: 0.72rem;
        line-height: 1.35;
    }

    .track-delivery-page .step-item.active .step-label,
    .track-delivery-page .step-item.completed .step-label {
        color: var(--track-text);
        font-weight: 700;
    }

    /* Map */
    .track-delivery-page #deliveryMap {
        display: none;
        width: 100%;
        height: 300px;
        margin-top: 28px;
        border: 1px solid var(--track-border);
        border-radius: 12px;
        overflow: hidden;
        z-index: 0;
    }

    @media (max-width: 576px) {
        .track-delivery-page {
            padding: 16px 10px 32px;
        }

        .track-delivery-page .track-content-area {
            padding: 22px 15px;
        }

        .track-delivery-page .blue-banner-section {
            padding: 24px 14px;
        }

        .track-delivery-page .blue-banner-section h2 {
            font-size: 1.1rem;
        }

        .track-delivery-page .search-box-wrapper {
            align-items: stretch;
            flex-direction: column;
        }

        .track-delivery-page .btn-search {
            width: 100%;
        }

        .track-delivery-page .stepper-wrapper {
            margin-right: 0;
            margin-left: 0;
        }

        .track-delivery-page .step-label {
            font-size: 0.65rem;
        }
    }
</style>


    <div class="track-delivery-page">

    <div class="outer-page-wrapper">
        
        <div class="blue-banner-section">
            <h2>TRACK YOUR DELIVERY</h2>
            <p>Monitor your order progress and live map location in real time.</p>
        </div>

        <div class="track-content-area">
            <asp:Label ID="lblError" runat="server" CssClass="error-msg"></asp:Label>

            <div class="search-box-wrapper">
                <asp:TextBox ID="txtOrderId" runat="server" CssClass="search-input" Placeholder="Enter Order ID (e.g., 1, 2, 3)"></asp:TextBox>
                <asp:Button ID="btnTrack" runat="server" Text="Track Order" CssClass="btn-search" OnClick="btnTrack_Click" />
            </div>

            <div class="card-header">
                <h3 class="status-heading"><asp:Label ID="lblStatusHeading" runat="server" Text="Ordered"></asp:Label></h3>
            </div>

            <div class="stepper-wrapper">
                <div class="progress-bg-bar"></div>
                <div id="progressFill" class="progress-fill-bar"></div>
                
                <div class="steps-container">
                    <div id="step1" class="step-item"><div class="dot-circle">✓</div><div class="step-label">Ordered</div></div>
                    <div id="step2" class="step-item"><div class="dot-circle">✓</div><div class="step-label">Shipped</div></div>
                    <div id="step3" class="step-item"><div class="dot-circle">✓</div><div class="step-label">Out for<br>delivery</div></div>
                    <div id="step4" class="step-item"><div class="dot-circle">✓</div><div class="step-label">Delivered</div></div>
                </div>
            </div>

            <div id="deliveryMap"></div>
        </div>
    </div>
</div>


    <!-- Leaflet Map JS Engine -->
    <script src="https://unpkg.com/leaflet@1.9.4/dist/leaflet.js"></script>
    <script>
        var map = null;
        var currentMarker = null;

        function animateTrackerToStep(targetStep) {
            var fillBar = document.getElementById('progressFill');
            var totalSteps = 4;
            var targetWidths = [0, 28, 56, 84];
            var fillPercent = targetWidths[targetStep - 1] || 0;

            for (var i = 1; i <= totalSteps; i++) {
                document.getElementById('step' + i).className = 'step-item';
            }
            document.getElementById('step1').className = 'step-item active';

            setTimeout(function () { fillBar.style.width = fillPercent + '%'; }, 100);

            var delays = [0, 350, 700, 1050];
            for (var s = 1; s <= targetStep; s++) {
                (function (stepNum) {
                    setTimeout(function () {
                        var el = document.getElementById('step' + stepNum);
                        if (stepNum < targetStep) el.className = 'step-item completed';
                        else el.className = 'step-item active';
                    }, delays[stepNum - 1]);
                })(s);
            }
        }

        function renderAddressPin(addressQuery, targetStep) {
            animateTrackerToStep(targetStep);

            var mapContainer = document.getElementById('deliveryMap');
            if (mapContainer) {
                mapContainer.style.display = 'block';
            }

            if (!map) {
                map = L.map('deliveryMap').setView([-29.8587, 31.0218], 12);
                L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
                    attribution: '© OpenStreetMap'
                }).addTo(map);
            }

            if (currentMarker) {
                map.removeLayer(currentMarker);
            }

            var geocodeUrl = "https://nominatim.openstreetmap.org/search?format=json&q=" + encodeURIComponent(addressQuery);

            fetch(geocodeUrl)
                .then(response => response.json())
                .then(data => {
                    var lat = -29.8587;
                    var lng = 31.0218;

                    if (data && data.length > 0) {
                        lat = parseFloat(data[0].lat);
                        lng = parseFloat(data[0].lon);
                    }

                    map.setView([lat, lng], 15);
                    setTimeout(function () { map.invalidateSize(); }, 300);

                    currentMarker = L.marker([lat, lng]).addTo(map)
                        .bindPopup("<b>Delivery Address:</b><br>" + addressQuery)
                        .openPopup();
                })
                .catch(err => {
                    console.error("Geocoding error:", err);
                });
        }
    </script>
</asp:Content>