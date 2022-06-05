var user = {
    init: function () {
        user.registerEvents();
    },
    registerEvents: function () {
        $('.btn btn-sm btn-success').off('click').on('click', function (e) {
            e.preventDefault();
        });
    }
}