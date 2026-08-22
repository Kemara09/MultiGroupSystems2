<%@ Page Title="Track Delivery" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TrackDelivery.aspx.cs" Inherits="MultiGroupSystemsTester.TrackDelivery" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    
    <link rel="stylesheet" href="https://unpkg.com/leaflet@1.9.4/dist/leaflet.css" />

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