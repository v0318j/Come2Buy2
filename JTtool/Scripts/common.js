$(function () {
    removeBanner();
});

function removeBanner() {
    if ($('body > script:last').attr('src') != undefined && $('body > script:last').attr('src').indexOf('http://ads.mgmt.somee.com/') > -1) {
        $('body > script:last').remove();
        $('body > center:last').remove();
        var removeBanner = false;
        var tryRemove = 0;
        while (!removeBanner) {
            setTimeout(function () {
                if ($('body > div:last').attr('onmouseover') == 'S_ssac();') {
                    for (var i = 0; i < 3; i++) {
                        $('body > div:last').remove();
                    }
                    removeBanner = true;
                }
            }, 100);
            if (tryRemove < 20) {
                tryRemove++;
            } else {
                break;
            }
        }
    }
}

function errorAlert(text) {
    Swal.fire({
        icon: 'error',
        html: text,
    })
}

function successAlert(text) {
    Swal.fire({
        icon: 'success',
        html: text,
    })
}

function checkAlert(action,doIfConfirmed) {
    Swal.fire({
        icon: 'question',
        title: '確定要' + action +'嗎？',
        showCancelButton: true,
        confirmButtonText: action,
    }).then((result) => {
        if (result.isConfirmed) {
            doIfConfirmed();
        }
    })
}