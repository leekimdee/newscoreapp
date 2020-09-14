var contactController = function () {
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
                txtPhone: { required: true },
                txtName: { required: true },
                txtEmail: { required: true },
                txtTitle: { required: true },
                txtContent: { required: true }
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

    function loadData() {

    }
}