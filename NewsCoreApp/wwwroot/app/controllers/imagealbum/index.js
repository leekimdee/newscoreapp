var imageAlbumController = function () {
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
                txtTitle: { required: true },
                txtSortOrder: { number: true }
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
                url: "/Admin/ImageAlbum/GetById",
                data: { id: that },
                dataType: "json",
                beforeSend: function () {
                    lkd.startLoading();
                },
                success: function (response) {
                    var data = response;
                    $('#hidId').val(data.Id);
                    $('#txtTitle').val(data.Title);
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

        $('#btnSave').on('click', function (e) {
            if ($('#frmMaintainance').valid()) {
                e.preventDefault();
                var id = $('#hidId').val();
                var title = $('#txtTitle').val();
                var sortOrder = $('#txtSortOrder').val();
                var status = $('#ckStatus').prop('checked') == true ? 1 : 0;

                $.ajax({
                    type: "POST",
                    url: "/Admin/ImageAlbum/SaveEntity",
                    data: {
                        Id: id,
                        Title: title,
                        SortOrder: sortOrder,
                        Status: status
                    },
                    dataType: "json",
                    beforeSend: function () {
                        lkd.startLoading();
                    },
                    success: function (response) {
                        lkd.notify('Update album successful', 'success');
                        $('#modal-add-edit').modal('hide');
                        resetFormMaintainance();

                        lkd.stopLoading();
                        loadData(true);
                    },
                    error: function () {
                        lkd.notify('Has an error in save album progress', 'error');
                        lkd.stopLoading();
                    }
                });
                return false;
            }
        });

        $('body').on('click', '.btn-delete', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            lkd.confirm('Are you sure to delete?', function () {
                $.ajax({
                    type: "POST",
                    url: "/Admin/ImageAlbum/Delete",
                    data: { id: that },
                    dataType: "json",
                    beforeSend: function () {
                        lkd.startLoading();
                    },
                    success: function (response) {
                        lkd.notify('Delete successful', 'success');
                        lkd.stopLoading();
                        loadData();
                    },
                    error: function (status) {
                        lkd.notify('Has an error in delete progress', 'error');
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
            url: '/admin/imagealbum/GetAllPaging',
            dataType: 'json',
            success: function (response) {
                $.each(response.Results, function (i, item) {
                    render += Mustache.render(template, {
                        Id: item.Id,
                        Title: item.Title,
                        SortOrder: item.SortOrder,
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
                lkd.notify('Cannot loading data', 'error');
            }
        })
    }

    function resetFormMaintainance() {
        $('#hidId').val(0);
        $('#txtTitle').val('');
        $('#txtSortOrder').val('');
        $('#ckStatus').prop('checked', true);
    }
}