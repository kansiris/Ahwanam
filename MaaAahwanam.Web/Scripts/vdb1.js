var category = $('#category').val();
var subcategory = $('#subcategory').val();
var subid = $('#vendorsubid').val();
var id = $('#vendorid').val();

// Dropdown Change
$(document).ready(function () {
    if (category != null && category != "") {
        $('#selectedservice').val(category); //Assign value to service dropdown here
    }
});

$('#selectedservice').on('change', function () {
    var subcategory1 = $(this).val();
    $.ajax({
        url: '/VDashboard/AddSubService?type=' + category + '&&subcategory=' + subcategory1 + '&&subid=' + subid,
        type: 'post',
        contentType: 'application-json',
        success: function (data) {
            alert(data);
            //window.location.reload();
            var url = '/vdb/sidebar';
            $('.services').empty().load(url);
            $('#hname').empty().text($(this).val());
        },
        error: function (er) {
            alert("System Encountered Internal Error!!! Try Again After Some Time");
        }
    });
});