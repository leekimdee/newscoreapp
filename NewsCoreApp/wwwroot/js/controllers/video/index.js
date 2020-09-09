var videoController = function () {
    var $lg = $('#video-gallery');
    this.initialize = function () {
        loadData();
        registerEvents();

        $lg.lightGallery({
            loadYoutubeThumbnail: true,
            youtubeThumbSize: 'default',
            selector: '.poster-container'
        });
    }

    function registerEvents() {

    }

    function loadData() {
        
    }
}