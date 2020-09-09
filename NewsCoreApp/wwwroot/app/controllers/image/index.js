var imageController = function () {
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
                txtTitle: { required: true },
                ddlAlbumId: { required: true }
            }
        });

        $('#frmAddMulti').validate({
            errorClass: 'red',
            ignore: [],
            lang: 'vi',
            rules: {
                ddlAlbumIdMulti: { required: true }
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
            //initTreeDropDownCategory();
            $('#modal-add-edit').modal('show');

        });

        $("#btnCreateMulti").on('click', function () {
            resetFormMulti();
            $('#modal-add-multi').modal('show');
        });

        $('#btnSelectImg').on('click', function () {
            $('#fileInputImage').click();
        });

        $('#btnSelectImg-Multi').on('click', function () {
            $('#fileInputImage-Multi').click();
        });

        $("#fileInputImage").on('change', function () {
            var fileUpload = $(this).get(0);
            var files = fileUpload.files;
            var data = new FormData();
            for (var i = 0; i < files.length; i++) {
                data.append(files[i].name, files[i]);
            }
            $.ajax({
                type: "POST",
                url: "/Admin/Upload/UploadImage",
                contentType: false,
                processData: false,
                data: data,
                success: function (path) {
                    $('#txtImage').val(path);
                    lkd.notify('Upload image succesful!', 'success');
                },
                error: function () {
                    lkd.notify('There was error uploading files!', 'error');
                }
            });
        });

        $("#fileInputImage-Multi").on('change', function () {
            var fileUpload = $(this).get(0);
            var files = fileUpload.files;
            var data = new FormData();
            var strImageUrlList = "";
            for (var i = 0; i < files.length; i++) {
                data.append(files[i].name, files[i]);
            }
            $.ajax({
                type: "POST",
                url: "/Admin/Upload/UploadMultiImage",
                contentType: false,
                processData: false,
                data: data,
                success: function (pathList) {
                    $.each(pathList, function (i, item) {
                        $('#filePathList').append(item);
                        $('#filePathList').append("<br />");
                        
                        if (strImageUrlList == "") {
                            strImageUrlList = item;
                        }
                        else
                            strImageUrlList += "," + item;
                    });
                    $("#hidImageUrlList").val(strImageUrlList);
                    lkd.notify('Upload image succesful!', 'success');
                },
                error: function () {
                    lkd.notify('There was error uploading files!', 'error');
                }
            });
        });

        $('body').on('click', '.btn-edit', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            $.ajax({
                type: "GET",
                url: "/Admin/Image/GetById",
                data: { id: that },
                dataType: "json",
                beforeSend: function () {
                    lkd.startLoading();
                },
                success: function (response) {
                    var data = response;
                    $('#hidId').val(data.Id);
                    $('#txtTitle').val(data.Title);
                    loadAlbums(data.ImageAlbumId);

                    $('#txtImage').val(data.ImageUrl);

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
            var id = $('#hidId').val();
            var title = $('#txtTitle').val();
            var albumId = $('#ddlAlbumId').val();
            var imageUrl = $('#txtImage').val();
            var status = $('#ckStatus').prop('checked') == true ? 1 : 0;

            $.ajax({
                type: "POST",
                url: "/Admin/Image/SaveEntity",
                data: {
                    Id: id,
                    Title: title,
                    ImageAlbumId: albumId,
                    ImageUrl: imageUrl,
                    Status: status
                },
                dataType: "json",
                beforeSend: function () {
                    lkd.startLoading();
                },
                success: function (response) {
                    lkd.notify('Update image successful', 'success');
                    $('#modal-add-edit').modal('hide');
                    resetFormMaintainance();

                    lkd.stopLoading();
                    loadData(true);
                },
                error: function () {
                    lkd.notify('Has an error in save image progress', 'error');
                    lkd.stopLoading();
                }
            });
            return false;
        }
    });

    $('#btnSave-Multi').on('click', function (e) {
        if ($('#frmAddMulti').valid()) {
            e.preventDefault();
            var images = [];
            var image;
            var arrImageUrl = $('#hidImageUrlList').val().split(',');
            var albumId = $('#ddlAlbumId-Multi').val();
            var status = $('#ckStatus-Multi').prop('checked') == true ? 1 : 0;
            for (var i = 0; i < arrImageUrl.length; i++) {
                image = {
                    Title: "Title",
                    ImageAlbumId: albumId,
                    ImageUrl: arrImageUrl[i],
                    Status: status
                }
                images.push(image);
            }            

            $.ajax({
                type: "POST",
                url: "/Admin/Image/SaveMulti",
                data: {
                    imageListVm: images
                },
                dataType: "json",
                beforeSend: function () {
                    lkd.startLoading();
                },
                success: function (response) {
                    lkd.notify('Update image successful', 'success');
                    $('#modal-add-multi').modal('hide');
                    resetFormMulti();

                    lkd.stopLoading();
                    loadData(true);
                },
                error: function () {
                    lkd.notify('Has an error in save image progress', 'error');
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
                url: "/Admin/Image/Delete",
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

    function initDropDownAlbum(selectedId) {
        $.ajax({
            url: "/Admin/ImageAlbum/GetAll",
            type: 'GET',
            dataType: 'json',
            async: false,
            success: function (response) {
                var data = [];
                $.each(response, function (i, item) {
                    data.push({
                        id: item.Id,
                        text: item.Title,
                        sortOrder: item.SortOrder
                    });
                });
                //var arr = lkd.unflattern(data);
                //$('#ddlCategoryIdM').combotree({
                //    data: arr
                //});
                //if (selectedId != undefined) {
                //    $('#ddlCategoryIdM').combotree('setValue', selectedId);
                //}
            }
        });
    }

    function resetFormMaintainance() {
        $('#hidId').val(0);
        $('#txtTitle').val('');
        loadAlbums('');

        $('#txtImage').val('');

        $('#ckStatus').prop('checked', true);
    }

    function resetFormMulti() {
        loadAlbums('');

        $('#filePathList').html('');

        $('#ckStatus-Multi').prop('checked', true);
    }

    function loadAlbums(selectedId) {
        $.ajax({
            type: 'GET',
            url: '/admin/imagealbum/GetAll',
            dataType: 'json',
            success: function (response) {
                var render = "<option value=''>--Chọn album--</option>";
                $.each(response, function (i, item) {
                    if (selectedId == item.Id) {
                        render += "<option value='" + item.Id + "' selected>" + item.Title + "</option>"
                    }
                    else
                        render += "<option value='" + item.Id + "'>" + item.Title + "</option>"
                });
                $('#ddlAlbumId').html(render);
                $('#ddlAlbumId-Multi').html(render);
            },
            error: function (status) {
                console.log(status);
                lkd.notify('Cannot loading image album data', 'error');
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
            url: '/admin/image/GetAllPaging',
            dataType: 'json',
            success: function (response) {
                $.each(response.Results, function (i, item) {
                    render += Mustache.render(template, {
                        Id: item.Id,
                        Title: item.Title,
                        ImageUrl: item.ImageUrl,
                        AlbumId: item.ImageAlbum.Title,
                        CreatedDate: lkd.dateTimeFormatJson(item.CreatedDate),
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
            error: function (status) {
                console.log(status);
                lkd.notify('Cannot loading data', 'error');
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