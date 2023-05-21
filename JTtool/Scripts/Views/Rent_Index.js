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

    $('#expenseDate').datepicker({
        format: 'yyyy-mm-dd',
        language: 'zh-TW',
        autoclose: true,
        clearBtn: true
    });

    GetRentUsers();

    $('#saveBtn').click(saveExpenditure);
    $('#ExpenditureModal').on('hidden.bs.modal', function () {
        // 清除所有輸入欄位的值
        $('#expenditureId').val('');
        $('#expenseDate').val('');
        $('#item').val('');
        $('#price').val('');
        $('#isInstallment').prop('checked', false);
        $('#periods').val(0);
        $('#isAlways').prop('checked', false);
        $('#payer').find('input[name="payer"]').each(function () {
            $(this).prop('checked', false);
        });
        $('#shareIds').find('input[name="shareIds"]').each(function () {
            $(this).prop('checked', false);
        });
    });
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
            if (resault.Success) {
                $('#Payable').val(resault.Data.Rent);
                $('#RentAccordion').html(GenerateRentAccordion(resault.Data.RentDetail));
                $('.btn-update').click(GetExpenditure);
                $('.btn-delete').click(DeleteExpenditure);
            } else {
                errorAlert(resault.Message);
            }
        },
        error: function () {
            errorAlert('Error occurred while retrieving rent users.');
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
        Accordion += '<div class="col">';
        Accordion += '<div class="row">';
        Accordion += '<div class="col-6">付款者：' + item.Payer + '</div>';
        Accordion += '<div class="col-6">總金額：' + item.Price + '</div>';
        Accordion += '<div class="col-6">付款日期：' + item.ExpenseDate + '</div>';
        Accordion += '<div class="col-6">分擔人：' + item.Names + '</div>';
        Accordion += '<div class="col-6">是否分期：' + item.IsInstallment + '</div>';
        if (item.IsInstallment == '是') {
            Accordion += '<div class="col-6">期數：' + item.Periods + '</div>';
        }
        Accordion += '</div>';
        Accordion += '</div>';
        Accordion += '<div class="col-3 col-sm-2">';
        Accordion += '<div class="d-flex flex-column justify-content-center h-100">';
        if (item.Creator == $('#AId').val()) {
            Accordion += '<button type="button" class="btn btn-sm btn-primary mb-2 btn-update" data-bs-toggle="modal" data-bs-target="#ExpenditureModal" data-expenditure-id="' + item.ExpenditureId + '">修改</button>';
            if (!item.IsAlways) {
                Accordion += '<button type="button" class="btn btn-sm btn-danger btn-delete" data-expenditure-id="' + item.ExpenditureId + '">刪除</button>';
            }
        }
        Accordion += '</div>';
        Accordion += '</div>';
        Accordion += '</div>';
        Accordion += '</div>';
        Accordion += '</div>';
    });
    return Accordion;
}

function GetRentUsers() {
    $.ajax({
        url: '/Rent/GetRentUsers',
        method: 'GET',
        success: function (result) {
            if (result.Success) {
                // 將分攤者選項動態生成
                var payerContainer = $('#payer');
                payerContainer.empty(); // 清空原有的內容
                var shareIdsContainer = $('#shareIds');
                shareIdsContainer.empty(); // 清空原有的選項

                // 迴圈遍歷資料並生成 checkbox
                result.Data.forEach(function (user) {
                    var checkbox = $('<div class="col-auto">');
                    checkbox.append($('<div class="form-check form-check-payer">'));
                    checkbox.find('.form-check-payer').append($('<input class="form-check-input" type="radio" name="payer" value="' + user.Id + '">'));
                    checkbox.find('.form-check-payer').append($('<label class="form-check-label">' + user.Name + '</label>'));
                    payerContainer.append(checkbox);
                    checkbox = $('<div class="col-auto">');
                    checkbox.append($('<div class="form-check form-check-shareIds">'));
                    checkbox.find('.form-check-shareIds').append($('<input class="form-check-input" type="checkbox" name="shareIds" value="' + user.Id + '">'));
                    checkbox.find('.form-check-shareIds').append($('<label class="form-check-label">' + user.Name + '</label>'));
                    shareIdsContainer.append(checkbox);
                });
            } else {
                errorAlert(result.Message);
            }
        },
        error: function () {
            errorAlert('Error occurred while retrieving rent users.');
        }
    });
}

function saveExpenditure() {
    // 獲取表單資料
    var formData = {
        ExpenditureId: $('#expenditureId').val(),
        PayerId: $('input[name="payer"]:checked').val() == undefined ? null : $('input[name="payer"]:checked').val(),
        ShareIds: $('input[name="shareIds"]:checked').map(function () {
            return $(this).val();
        }).get(),
        Item: $('#item').val(),
        Price: parseInt($('#price').val()),
        ExpenseDate: $('#expenseDate').val(),
        IsInstallment: $('#isInstallment').prop('checked'),
        Periods: parseInt($('#periods').val()),
        IsAlways: $('#isAlways').prop('checked'),
        AId: $('#AId').val()
    };

    // 發送 AJAX POST 請求
    $.ajax({
        url: $('#expenditureId').val() == '' ? '/Rent/AddExpenditure' : '/Rent/UpdateExpenditure',
        method: 'POST',
        data: JSON.stringify(formData),
        contentType: 'application/json',
        success: function (response) {
            if (response.Success) {
                // 新增成功，執行相應的操作
                successAlert('儲存成功');
                $('#closeExpenditureModal').click();
                GetRent();
                // 清除表單資料或重新載入頁面等
            } else {
                // 新增失敗，顯示錯誤訊息
                errorAlert(response.Message);
            }
        },
        error: function () {
            // 請求失敗處理
            errorAlert('請求失敗');
        }
    });
}

function GetExpenditure() {
    $.ajax({
        url: '/Rent/GetExpenditure?id=' + $(this).data('expenditure-id'),
        method: 'GET',
        success: function (result) {
            if (result.Success) {
                var expenditure = result.Data;

                // Populate the modal fields with the retrieved data
                $('#expenditureId').val(expenditure.Id);
                $('#expenseDate').datepicker('setDate', new Date(parseInt(expenditure.ExpenseDate.match(/\d+/)[0])));
                $('#item').val(expenditure.Item);
                $('#price').val(expenditure.Price);
                $('#isInstallment').prop('checked', expenditure.IsInstallment);
                $('#periods').val(expenditure.Periods);
                $('#isAlways').prop('checked', expenditure.IsAlways);

                $('#payer').find('input[name="payer"]').each(function () {
                    if (result.Data.PayerId == parseInt($(this).val())) {
                        $(this).prop('checked', true);
                    } else {
                        $(this).prop('checked', false);
                    }
                });

                $('#shareIds').find('input[name="shareIds"]').each(function () {
                    if (result.Data.ShareIds.includes(parseInt($(this).val()))) {
                        $(this).prop('checked', true);
                    } else {
                        $(this).prop('checked', false);
                    }
                });
            } else {
                errorAlert(result.Message);
            }
        },
        error: function () {
            errorAlert('Error occurred while retrieving rent users.');
        }
    });
}

function DeleteExpenditure() {
    var expenditureId = $(this).data('expenditure-id');
    checkAlert('刪除', function () {
        $.ajax({
            url: '/Rent/DeleteExpenditure',
            method: 'POST',
            data: {
                ExpenditureId: expenditureId
            },
            success: function (result) {
                if (result.Success) {
                    successAlert('刪除成功');
                    GetRent();
                } else {
                    errorAlert(result.Message);
                }
            },
            error: function () {
                errorAlert('Error occurred while retrieving rent users.');
            }
        });
    });
}