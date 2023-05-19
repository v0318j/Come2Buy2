$(function () {
    $('#YYMM').change(function () {
        GetRent();
    });

    $('#YYMM').datepicker({
        format: 'yyyy-mm',
        minViewMode: 'year',
        language: 'zh-TW',
        autoclose: true,
        clearBtn: true
    });
    $('#YYMM').datepicker('setDate', Date());
});

function GetRent() {
    $.ajax({
        url: '/Rent/GetRentDetail',
        method: 'POST',
        data: {
            AId: $('#AId').val(),
            YYMM: $('#YYMM').val(),
        },
        success: function (resault) {
            if (!resault.Success) {
                alert(resault.Message);
            } else {
                $('#Payable').val(resault.Rent);
                $('#RentAccordion').html(GenerateRentAccordion(resault.RentDetail));
            }
        },
        error: function () {

        }
    });
}

function GenerateRentAccordion(RentDetail) {
    var Accordion = '';
    RentDetail.forEach(function (item, i) {
        Accordion += '<div class="accordion-item">';
        Accordion += '<h2 class="accordion-header" id="heading-' + i + '">';
        Accordion += '<button class="accordion-button ' + (i != 0 ? 'collapsed' : '') + '" type="button" data-bs-toggle="collapse" data-bs-target="#collapse-' + i + '" aria-expanded="true" aria-controls="collapse-' + i + '">';
        Accordion += '<div class="col-8">' + item.Item + '</div>';
        Accordion += '<div class="col-3">' + item.PayAmount + '</div>';
        Accordion += '</button>';
        Accordion += '</h2>';
        Accordion += '<div id="collapse-' + i + '" class="accordion-collapse collapse ' + (i == 0 ? 'show' : '') + '" aria-labelledby="heading-' + i + '" data-bs-parent="#RentAccordion">';
        Accordion += '<div class="accordion-body row">';
        Accordion += '<div class="col-6">付款者：' + item.Payer + '</div>';
        Accordion += '<div class="col-6">總金額：' + item.Price + '</div>';
        Accordion += '<div class="col-6">是否分期：' + item.IsInstallment + '</div>';
        Accordion += '<div class="col-6">期數：' + item.Periods + '</div>';
        Accordion += '<div class="col-12">分擔人：' + item.Names + '</div>';
        Accordion += '</div>';
        Accordion += '</div>';
        Accordion += '</div>';
    });
    return Accordion;
}