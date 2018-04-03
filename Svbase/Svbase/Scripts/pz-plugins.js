//start: dragAndDropPZ
;
(function ($, window, document) {

    var defaults = {
        fileInputId: "input[type='file']",
        dragoverStyleClass: "stripped"
    };

    $.fn.dragAndDropPZ = function (options) {
        var config = $.extend({}, defaults, options);
        var $dropZone = $(this);
        var dropZoneId = $dropZone.selector;

        $(document).on("dragover", dropZoneId, function (e) {
            e.preventDefault();
            e.stopPropagation();
            $dropZone.addClass(config.dragoverStyleClass);
            console.log("dropzone => dragover");
        });

        $(document).on("dragleave", dropZoneId, function (e) {
            e.preventDefault();
            e.stopPropagation();
            $dropZone.removeClass(config.dragoverStyleClass);
            console.log("dropzone => dragleave");
        });

        $(document).on("drop", dropZoneId, function (e) {
            e.preventDefault();
            e.stopPropagation();
            var droppedFiles = e.originalEvent.dataTransfer.files; //[0] - ?
            $(config.fileInputId).prop("files", droppedFiles);
            $dropZone.removeClass(config.dragoverStyleClass);
            console.log("dropzone => ondrop");
        });

        return $dropZone;
    };
})(jQuery, window, document);
//end: dragAndDropPZ