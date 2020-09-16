var contactController = function () {
    this.initialize = function () {
        loadData();
        registerEvents();
    }

    function registerEvents() {
        $('#frmMaintainance').validate({
            errorClass: 'red',
            ignore: [],
            lang: 'vi',
            rules: {
                txtId: { required: true },
                txtName: { required: true }
            }
        });

        //todo: binding events to controls
        $('#ddlShowPage').on('change', function () {
            lkd.configs.pageSize = $(this).val();
            lkd.configs.pageIndex = 1;
            loadData(true);
        });

        $("#btnCreate").on('click', function () {
            resetFormMaintainance();
            $('#modal-add-edit').modal('show');

        });

        $('body').on('click', '.btn-edit', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            $.ajax({
                type: "GET",
                url: "/Admin/Contact/GetById",
                data: { id: that },
                dataType: "json",
                beforeSend: function () {
                    lkd.startLoading();
                },
                success: function (response) {
                    var data = response;
                    $('#txtId').val(data.Id);
                    $('#txtName').val(data.Name);
                    $('#txtPhone').val(data.Phone);
                    $('#txtEmail').val(data.Email);
                    $('#txtWebsite').val(data.Website);
                    $('#txtAddress').val(data.Address);
                    $('#txtOther').val(data.Other);
                    $('#txtEmbedCode').val(data.EmbedCode);
                    $('#ckStatus').prop('checked', data.Status == 1);

                    $('#modal-add-edit').modal('show');
                    lkd.stopLoading();

                },
                error: function (status) {
                    lkd.notify('Có lỗi xảy ra', 'error');
                    lkd.stopLoading();
                }
            });
        });

        $('#btnSave').on('click', function (e) {
            if ($('#frmMaintainance').valid()) {
                e.preventDefault();
                var id = $('#txtId').val();
                var name = $('#txtName').val();
                var phone = $('#txtPhone').val();
                var email = $('#txtEmail').val();
                var website = $('#txtWebsite').val();
                var address = $('#txtAddress').val();
                var other = $('#txtOther').val();
                var embedCode = $('#txtEmbedCode').val();

                var status = $('#ckStatus').prop('checked') == true ? 1 : 0;

                $.ajax({
                    type: "POST",
                    url: "/Admin/Contact/SaveEntity",
                    data: {
                        Id: id,
                        Name: name,
                        Phone: phone,
                        Email:email,
                        Website:website,
                        Address:address,
                        Other: other,
                        EmbedCode: embedCode,

                        Status: status
                    },
                    dataType: "json",
                    beforeSend: function () {
                        lkd.startLoading();
                    },
                    success: function (response) {
                        lkd.notify('Cập nhật liên hệ thành công', 'success');
                        $('#modal-add-edit').modal('hide');
                        resetFormMaintainance();

                        lkd.stopLoading();
                        loadData(true);
                    },
                    error: function () {
                        lkd.notify('Đã có lỗi xảy ra trong quá trình lưu', 'error');
                        lkd.stopLoading();
                    }
                });
                return false;
            }
        });

        $('body').on('click', '.btn-delete', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            lkd.confirm('Bạn có chắc chắn muốn xóa?', function () {
                $.ajax({
                    type: "POST",
                    url: "/Admin/Contact/Delete",
                    data: { id: that },
                    dataType: "json",
                    beforeSend: function () {
                        lkd.startLoading();
                    },
                    success: function (response) {
                        lkd.notify('Xóa thành công', 'success');
                        lkd.stopLoading();
                        loadData();
                    },
                    error: function (status) {
                        lkd.notify('Có lỗi trong quá trình xóa', 'error');
                        lkd.stopLoading();
                    }
                });
            });
        });
    }

    function loadData(isPageChanged) {
        var template = $('#table-template').html();
        var render = "";
        $.ajax({
            type: 'GET',
            data: {
                //keyword: $('#txtKeyword').val(),
                keyword: '',
                page: lkd.configs.pageIndex,
                pageSize: lkd.configs.pageSize
            },
            url: '/Admin/Contact/GetAllPaging',
            dataType: 'json',
            success: function (response) {
                $.each(response.Results, function (i, item) {
                    render += Mustache.render(template, {
                        Id: item.Id,
                        Name: item.Name,
                        Phone: item.Phone,
                        Email:item.Email,
                        Website:item.Website,
                        Address: item.Address,
                        EmbedCode: item.EmbedCode,
                        Status: lkd.getStatus(item.Status)
                    });
                });

                $('#lblTotalRecords').text(response.RowCount);

                if (render != '') {
                    $('#tbl-content').html(render);
                }

                paging.wrapPaging(response.RowCount, function () {
                    loadData();
                }, isPageChanged);
            },
            error: function (status) {
                console.log(status);
                lkd.notify('Không thể tải dữ liệu', 'error');
            }
        })
    }

    function resetFormMaintainance() {
        $('#txtId').val("");
        $('#txtName').val("");
        $('#txtPhone').val("");
        $('#txtEmail').val("");
        $('#txtWebsite').val("");
        $('#txtAddress').val("");
        $('#txtOther').val("");
        $('#txtEmbedCode').val("");
        $('#ckStatus').prop('checked', true);
    }
}