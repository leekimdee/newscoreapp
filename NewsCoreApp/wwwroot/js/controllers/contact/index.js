﻿var contactController = function () {
    this.initialize = function () {
        loadData();
        registerEvents();
    }

    function registerEvents() {
        $('#frmContact').validate({
            errorClass: 'red',
            ignore: [],
            lang: 'vi',
            rules: {
                Phone: { required: true },
                Name: { required: true },
                Email: { required: true },
                Title: { required: true },
                Content: { required: true }
            }
        });

        $('#btnSubmit_V2').on('click', function (e) {
            if ($('#frmContact').valid()) {
                e.preventDefault();
                var name = $('#txtName').val();
                var phone = $('#txtPhone').val();
                var email = $('#txtEmail').val();
                var title = $('#txtTitle').val();
                var content = $('#txtContent').val();
                var status = 1;

                $.ajax({
                    type: "POST",
                    url: "/Contact/SubmitContact",
                    data: {
                        Name: name,
                        Phone: phone,
                        Email: email,
                        Title: title,
                        Content: content,
                        Status: status
                    },
                    dataType: "json",
                    beforeSend: function () {

                    },
                    success: function (response) {
                        console.log(response);
                    },
                    error: function () {

                    }
                });

                return false;
            }
        });

        $('#btnSubmit_V3').on('click', function (e) {
            if ($('#frmContact').valid()) {
                e.preventDefault();
                var name = $('#txtName').val();
                var phone = $('#txtPhone').val();
                var email = $('#txtEmail').val();
                var title = $('#txtTitle').val();
                var content = $('#txtContent').val();
                var status = 1;

                grecaptcha.ready(function () {
                    grecaptcha.execute('6LfS9csZAAAAAIPB8ysydIySKdeaDW1osl4ShCsb', { action: 'submit' }).then(function (token) {
                        // Add your logic to submit to your backend server here.
                        $.ajax({
                            type: "POST",
                            url: "/Contact/SubmitContact",
                            data: {
                                Name: name,
                                Phone: phone,
                                Email: email,
                                Title: title,
                                Content: content,
                                Status: status
                            },
                            dataType: "json",
                            beforeSend: function () {

                            },
                            success: function (response) {
                                console.log(response);
                            },
                            error: function () {

                            }
                        });
                    });
                });

                
                return false;
            }
        });
    }

    function initMap() {
        var uluru = { lat: parseFloat($('#hidLat').val()), lng: parseFloat($('#hidLng').val()) };
        var map = new google.maps.Map(document.getElementById('map'), {
            zoom: 17,
            center: uluru
        });

        var contentString = $('#hidAddress').val();

        var infowindow = new google.maps.InfoWindow({
            content: contentString
        });

        var marker = new google.maps.Marker({
            position: uluru,
            map: map,
            title: $('#hidName').val()
        });
        infowindow.open(map, marker);
    }

    function loadData() {
        $("#name").text($('#hidName').val());
        $("#phone").text("Điện thoại: " + $('#hidPhone').val());
        $("#address").text("Địa chỉ: " + $('#hidAddress').val());
        $("#email").text("Email: " + $('#hidEmail').val());
        $("#map").attr("src", $('#hidEmbedCode').val());
    }
}