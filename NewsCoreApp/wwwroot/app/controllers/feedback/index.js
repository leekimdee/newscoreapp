var feedbackController = function () {
    this.initialize = function () {
        loadData();
        registerEvents();
    }

    function registerEvents() {
        //todo: binding events to controls
        $('#ddlShowPage').on('change', function () {
            lkd.configs.pageSize = $(this).val();
            lkd.configs.pageIndex = 1;
            loadData(true);
        });

        $('body').on('click', '.btn-detail', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            $.ajax({
                type: "GET",
                url: "/Admin/Feedback/GetById",
                data: { id: that },
                dataType: "json",
                beforeSend: function () {
                    lkd.startLoading();
                },
                success: function (response) {
                    var data = response;
                    $('#hidId').val(data.Id);
                    $('#txtTitle').val(data.Title);
                    $('#txtName').val(data.Name);
                    $('#txtPhone').val(data.Phone);
                    $('#txtEmail').val(data.Email);
                    $('#ckStatus').prop('checked', data.Status == 1);

                    $('#modal-detail').modal('show');
                    lkd.stopLoading();

                },
                error: function (status) {
                    lkd.notify('Có lỗi xảy ra', 'error');
                    lkd.stopLoading();
                }
            });
        });

        $('body').on('click', '.btn-delete', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            lkd.confirm('Bạn chắc chắn muốn xóa?', function () {
                $.ajax({
                    type: "POST",
                    url: "/Admin/Feedback/Delete",
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
                        lkd.notify('Có lỗi xảy ra trong quá trình xóa', 'error');
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
            url: '/Admin/Feedback/GetAllPaging',
            dataType: 'json',
            success: function (response) {
                $.each(response.Results, function (i, item) {
                    render += Mustache.render(template, {
                        Id: item.Id,
                        Title: item.Title,
                        Name: item.Name,
                        Phone: item.Phone,
                        Email: item.Email,
                        CreatedDate: lkd.dateTimeFormatJson(item.CreatedDate),
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
        $('#hidId').val(0);
        $('#txtTitle').val('');
        $('#txtName').val('');
        $('#txtPhone').val('');
        $('#txtEmail').val('');
        $('#ckStatus').prop('checked', true);
    }
}