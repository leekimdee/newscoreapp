var functionController = function () {
    this.initialize = function () {
        loadData();
        registerEvents();
    }

    function registerEvents() {
        //Init validation
        $('#frmMaintainance').validate({
            errorClass: 'red',
            ignore: [],
            lang: 'vi',
            rules: {
                txtName: { required: true },
                txtId: { required: true }
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
                url: "/Admin/Function/GetById",
                data: { id: that },
                dataType: "json",
                beforeSend: function () {
                    lkd.startLoading();
                },
                success: function (response) {
                    var data = response;
                    $('#txtId').val(data.Id);
                    $('#txtName').val(data.Name);
                    loadParents(data.ParentId);

                    $('#txtUrl').val(data.URL);
                    $('#txtIconCss').val(data.IconCss);
                    $('#txtSortOrder').val(data.SortOrder);

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
    }

    $('#btnSave').on('click', function (e) {
        if ($('#frmMaintainance').valid()) {
            e.preventDefault();
            var id = $('#txtId').val();
            var name = $('#txtName').val();
            var parentId = $('#ddlParentId').val();
            var url = $('#txtUrl').val();
            var iconCss = $('#txtIconCss').val();
            var sortOrder = $('#txtSortOrder').val();
            var status = $('#ckStatus').prop('checked') == true ? 1 : 0;

            $.ajax({
                type: "POST",
                url: "/Admin/Function/SaveEntity",
                data: {
                    Id: id,
                    Name: name,
                    ParentId: parentId,
                    URL: url,
                    IconCss:iconCss,
                    SortOrder:sortOrder,
                    Status: status
                },
                dataType: "json",
                beforeSend: function () {
                    lkd.startLoading();
                },
                success: function (response) {
                    lkd.notify('Cập nhật chức năng thành công', 'success');
                    $('#modal-add-edit').modal('hide');
                    resetFormMaintainance();

                    lkd.stopLoading();
                    loadData(true);
                },
                error: function () {
                    lkd.notify('Có lỗi trong quá trình lưu chức năng', 'error');
                    lkd.stopLoading();
                }
            });
            return false;
        }
    });

    $('body').on('click', '.btn-delete', function (e) {
        e.preventDefault();
        var that = $(this).data('id');
        lkd.confirm('Bạn chắc chắn muốn xóa?', function () {
            $.ajax({
                type: "POST",
                url: "/Admin/Function/Delete",
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

    function resetFormMaintainance() {
        $('#txtId').val('');
        $('#txtName').val('');
        loadParents('');

        $('#txtUrl').val('');
        $('#txtIconCss').val('');
        $('#txtSortOrder').val('');

        $('#ckStatus').prop('checked', true);
    }

    function loadParents(selectedId) {
        $.ajax({
            type: 'GET',
            url: '/Admin/Function/GetAllParent',
            dataType: 'json',
            success: function (response) {
                var render = "<option value=''>--Chọn cấp cha--</option>";
                $.each(response, function (i, item) {
                    if (selectedId == item.Id) {
                        render += "<option value='" + item.Id + "' selected>" + item.Name + "</option>"
                    }
                    else
                        render += "<option value='" + item.Id + "'>" + item.Name + "</option>"
                });
                $('#ddlParentId').html(render);
            },
            error: function (status) {
                console.log(status);
                lkd.notify('Không thể tải dữ liệu cấp cha', 'error');
            }
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
            url: '/Admin/Function/GetAllPaging',
            dataType: 'json',
            success: function (response) {
                $.each(response.Results, function (i, item) {
                    render += Mustache.render(template, {
                        Id: item.Id,
                        Name: item.Name,
                        Url: item.URL,
                        ParentId: item.ParentId,
                        IconCss:item.IconCss,
                        SortOrder:item.SortOrder,
                        Status: lkd.getStatus(item.Status)
                    });
                });

                $('#lblTotalRecords').text(response.RowCount);

                if (render != '') {
                    $('#tbl-content').html(render);
                }

                wrapPaging(response.RowCount, function () {
                    loadData();
                }, isPageChanged);
            },
            error: function (xhr, status, error) {
                console.log(status);
                var err = eval("(" + xhr.responseText + ")");
                console.log(err.Message);
                lkd.notify('Không thể tải dữ liệu', 'error');
            }
        })
    }

    function wrapPaging(recordCount, callBack, changePageSize) {
        var totalsize = Math.ceil(recordCount / lkd.configs.pageSize);
        //Unbind pagination if it existed or click change pagesize
        if ($('#paginationUL a').length === 0 || changePageSize === true) {
            $('#paginationUL').empty();
            $('#paginationUL').removeData("twbs-pagination");
            $('#paginationUL').unbind("page");
        }
        //Bind Pagination Event
        $('#paginationUL').twbsPagination({
            totalPages: totalsize,
            visiblePages: 7,
            first: 'Đầu',
            prev: 'Trước',
            next: 'Tiếp',
            last: 'Cuối',
            onPageClick: function (event, p) {
                lkd.configs.pageIndex = p;
                setTimeout(callBack(), 200);
            }
        });
    }
}