function GetQuote(quotationsList) {
    $.ajax({
        url: '/HomePage/GetQuote',
        type: "POST",
        datatype: "json",
        data: { quotationsList: quotationsList },
        success: function (response) {
            if (response == "Success")
            { alert("Quotation Sent to Vendor"); $(".view-details-alert").css("display", "none"); }
            else if (response == "exceeded")
                alert("Quotation Limit Exceeded!!! please Login/Sign up to Raise Quotation");
            else if (response == "Fail")
                alert("Cannot Raise Quotation at the moment")
        },
        error: function (er) {
            errormsg("Something Went Wrong!!!Try Again Later");
        }
    })
}

function GetNewQuote(quotationsList) {
    $.ajax({
        url: '/NVendors/GetQuote',
        type: "POST",
        datatype: "json",
        data: { quotationsList: quotationsList },
        success: function (response) {
            if (response == "Success")
            { alert("Quotation Sent to Vendor"); $(".view-details-alert").css("display", "none"); }
            else if (response == "exceeded")
                alert("Quotation Limit Exceeded!!! please Login/Sign up to Raise Quotation");
            else if (response == "Fail")
                alert("Cannot Raise Quotation at the moment")
        },
        error: function (er) {
            errormsg("Something Went Wrong!!!Try Again Later");
        }
    })
}