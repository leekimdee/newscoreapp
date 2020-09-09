var imageController = function () {
    var $lg = $('#lightgallery');
    this.initialize = function () {
        loadData($("#ImageAlbumId").val());
        registerEvents();
    }

    function registerEvents() {
        $("#ImageAlbumId").change(function () {
            $lg.data('lightGallery').destroy(true);
            loadData($(this).val());
        });
    }

    function loadData(albumId) {
        $.ajax({
            type: "GET",
            dataType: 'json',
            url: '/Image/GetImagesByAlbum',
            data: { albumId: albumId },
            success: function (result) {
                console.log(result)
                $("#lightgallery").html("");
                for (var i = 0; i < result.length; i++) {
                    $("#lightgallery").append("<a href='" + result[i].ImageUrl + "'><img src='" + result[i].ImageUrl + "' class='img-thumb' /></a>");
                }

                $lg.lightGallery({ thumbnail: true });
            },
            error: function (a, textStatus, errorThrown) {
                alert(errorThrown);
            }
        });
    }
}