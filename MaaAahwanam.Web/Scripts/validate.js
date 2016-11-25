
$(document).ready(function () {

    $("#contact_form").validate({
        rules: {
            PersonName: {
                required: true
            },
            SenderEmailId: {
                required: true,
                email: true
            },
            SenderPhone: {
                required: true,
                number: true,
                minlength: 10,
                maxlength: 12
            },
            Country: {
                required: true
            },
            PostalCode: {
                required: true,
                number: true,
                minlength: 6
            },
            CompanyName: {
                required: true
            },

            EnquiryDetails: {
                required: true
            }

        },
        messages: {
            PersonName: {
                required: "Please enter your name"
            },
            SenderEmailId: {
                required: "Please enter your email Id "
            },
            SenderPhone: {
                required: "Please enter your phone number"
            },
            Country: {
                required: "Please select country"
            },
            PostalCode: {
                required: "Please enter your Postal Code"
            },
            CompanyName: {
                required: "Please enter your company name"
            },
            EnquiryDetails: {
                required: "Please enter your Message"
            },
        },
    });

}); 
