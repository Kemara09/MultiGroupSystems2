<%@ Page Title="Track Delivery" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TrackDelivery.aspx.cs" Inherits="MultiGroupSystemsTester.TrackDelivery" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Leaflet Map CSS -->
    <link rel="stylesheet" href="https://unpkg.com/leaflet@1.9.4/dist/leaflet.css" />

    <style>
        body {
            background-color: #f8fafc !important;
        }

        .body-content, .container {
            background-color: transparent !important;
            padding-top: 10px !important;
            margin-top: 0 !important;
        }

        /* Outer container with white border and shadow */
        .outer-page-wrapper {
            max-width: 780px;
            margin: 30px auto;
            background-color: #ffffff;
            border: 8px solid #ffffff;
            border-radius: 24px;
            box-shadow: 0 10px 30px rgba(0, 0, 0, 0.12);
            overflow: hidden;
        }

        /* Blue inner banner section */
        .blue-banner-section {
            background-color: #008296;
            padding: 35px 30px;
            text-align: center;
            color: #ffffff;
        }

        .blue-banner-section h2 {
            font-size: 26px;
            font-weight: 700;
            margin: 0 0 8px 0;
            color: #ffffff;
        }

        .blue-banner-section p {
            font-size: 14px;
            margin: 0;
            color: #e2e8f0;
        }

        /* Content section inside the wrapper */
        .track-content-area {
            background: #ffffff;
            padding: 30px;
            color: #0f172a;
        }

        .search-box-wrapper { 
            display: flex; 
            gap: 10px; 
            margin-bottom: 25px; 
        }

        .search-input {
            flex: 1;
            padding: 12px 18px; 
            border: 1.5px solid #e2e8f0;
            border-radius: 25px; 
            font-size: 14px; 
            outline: none; 
            transition: border-color 0.2s;
            background: #ffffff; 
            color: #0f172a;
        }

        .search-input:focus { 
            border-color: #008296; 
        }

        .btn-search {
            background: #008296; 
            color: #ffffff; 
            border: none; 
            padding: 12px 24px;
            border-radius: 25px; 
            font-weight: 600; 
            font-size: 14px; 
            cursor: pointer;
        }

        .btn-search:hover { 
            background: #006878; 
        }

        .card-header {
            text-align: center; 
            margin-bottom: 20px; 
        }

        .status-heading { 
            font-size: 22px; 
            font-weight: 700; 
            color: #0f172a; 
            margin: 0; 
        }

        .stepper-wrapper { 
            position: relative; 
            margin: 25px 10px 30px 10px; 
        }

        .progress-bg-bar {
            position: absolute; 
            top: 12px; 
            left: 8%; 
            width: 84%; 
            height: 8px;
            background-color: #e5e7eb; 
            border-radius: 4px; 
            z-index: 1;
        }

        .progress-fill-bar {
            position: absolute; 
            top: 12px; 
            left: 8%; 
            width: 0%; 
            height: 8px;
            background-color: #008296; 
            border-radius: 4px; 
            z-index: 2;
            transition: width 1.2s cubic-bezier(0.4, 0, 0.2, 1);
        }

        .steps-container { 
            position: relative; 
            z-index: 3; 
            display: flex; 
            justify-content: space-between; 
        }

        .step-item { 
            text-align: center; 
            flex: 1; 
        }

        .dot-circle {
            width: 32px; 
            height: 32px; 
            border-radius: 50%; 
            background-color: #e5e7eb;
            margin: 0 auto 10px auto; 
            display: flex; 
            align-items: center; 
            justify-content: center;
            color: transparent; 
            font-weight: bold; 
            font-size: 14px;
            transition: background-color 0.4s ease, transform 0.3s ease;
        }

        .step-item.active .dot-circle,
        .step-item.completed .dot-circle { 
            background-color: #008296; 
            color: #ffffff; 
            transform: scale(1.08); 
        }

        .step-label { 
            font-size: 13px; 
            color: #6b7280; 
            font-weight: 500; 
            line-height: 1.2; 
        }

        .step-item.active .step-label { 
            color: #0f172a; 
            font-weight: 700; 
        }

        #deliveryMap {
            width: 100%; 
            height: 380px; 
            border-radius: 16px; 
            margin-top: 25px;
            border: 1px solid #e2e8f0; 
            display: none;
        }

        .error-msg {
            color: #dc2626; 
            text-align: center; 
            font-weight: 600; 
            margin-bottom: 15px; 
            display: block; 
        }
    </style>

    <div class="outer-page-wrapper">
        <!-- Blue Inner Header Banner -->
        <div class="blue-banner-section">
            <h2>TRACK YOUR DELIVERY</h2>
            <p>Monitor your order progress and live map location in real time.</p>
        </div>

        <!-- Main Tracking Content Area -->
        <div class="track-content-area">
            <asp:Label ID="lblError" runat="server" CssClass="error-msg"></asp:Label>

            <div class="search-box-wrapper">
                <asp:TextBox ID="txtOrderId" runat="server" CssClass="search-input" Placeholder="Enter Order ID (e.g., 1, 2, 8)"></asp:TextBox>
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