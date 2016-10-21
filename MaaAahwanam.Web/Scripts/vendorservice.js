﻿//@*Image Popup*@
//<script>
    function imagepopup(obj) {       
        $('body').on('click', 'img', function () {
            var imgsrc = $(this).attr('src');

            $('#myModal img').attr('src', $(this).attr('src'));
        })
    }
    
//</script>

//@*Enable/disable buttons*@
//<script>
        window.onload = function () {
            var url = window.location.href;
            var op = url.substring(url.lastIndexOf('=') + 1);
            var up = window.location.search.substring(1);
            var servicetype = $("#type").val();
            if (servicetype != 'Venue' && servicetype != 'Others' && servicetype != 'InvitationCards' && servicetype != 'EventOrganisers' ) {
                var hdn = $("#hdnservices").val();
                if (hdn != '') {
                    var valArr = hdn.split(","), i = 0, size = valArr.length,
                    $options = $('#ddlservice option');

                    for (i; i < size; i++) {
                        $options.filter('[value="' + valArr[i] + '"]').prop('selected', true);
                    }
                }
            }
         
            //alert(op);
            $("#veglunch").hide(); 
            $("#nonveglunch").hide();
            $("#vegdinner").hide();
            $("#nonvegdinner").hide(); 
            $("#menuitem").hide();
            //var stat = up.contains("vid");
            //alert(url); alert(up.trimLeft("vid")); alert(op); //alert(stat);
            if (op == 'display') {
                //alert("view purpose");
                $("#btnadd").hide();
                $('#btn').hide();
                $("#btncancel").hide();
                $('#btnback').show();
                $('.deletelink').hide();
                $('#fileUpload').hide();
                $('#msg').hide();
                $('.form-control').attr("disabled", "disabled");
                itemsdisable1();
            }
            if (op == 'add') {
                //alert("Add NEw");
                $("[id^=Item1]").prop("disabled", true);
                $("[id^=Item2]").val("");
                $('#btn').hide();
                $("#btncancel").show();
                $('#btnback').hide();
                $('.deletelink').hide();
                $('#fileUpload').show();
                $('#msg').show();
                $("#btnadd").show();
                $("#imagesdisplay").hide();
            }
            //op == url
            if (up == 'vid=' + op || op == url) {
                alert("hi");
                $('#btn').show();
                $("#btncancel").show();
                $('#btnback').hide();
                $('.deletelink').show();
                $('#fileUpload').show();
                $('#msg').show();
                $('.form-control').removeAttr("disabled");
                $("#dealcheck").attr("disabled", "disabled");
                $("#btnadd").hide(); itemsdisable1();
            }
            if (op == 'adddeal' || op == 'adddeal#') {
                $("#btnadd").show().val("Add Deal").html("Add Deal");
                $('#btn').hide();
                $("#btncancel").show();
                $('#btnback').hide();
                $('.deletelink').hide();
                $('#fileUpload').hide();
                $('#msg').hide();
                //$('.form-control').attr("disabled", "disabled");
                $("[id^=Item1]").prop("disabled", true);
                $("[id^=Item2]").prop("disabled", true);
                $("#dealcheck").prop('checked', true).attr("disabled", "disabled");
                $("#dealdata").css({ "display": "block" });
                $("#ddlservice").attr("disabled", "disabled");
                var hdnservice = $("#hdnservices").val();
                if (hdnservice != null || hdnservice != '') {
                    $("#Item3_VendorCategory").val(hdnservice);
                }
                
                if (servicetype == 'Venue') {
                    var type = $("#Item2_VenueType").val();
                    $("#Item3_VendorCategory").val(type);
                    if (type == 'Banquet Hall') {
                        $("#dealprice").show();
                        $("#dealservicecost").hide();
                        itemsdisable1();
                    }
                    else {
                        $("#dealprice").hide();
                        $("#dealservicecost").show();
                    }
                }
                else if (servicetype == 'BeautyServices') {
                    $("#dealbeautyservicecost").show();
                }
                else if (servicetype == 'Catering') {
                    $("#dealcateringcost").show();
                }
                else if (servicetype == 'InvitationCards') {
                    $("#dealinvitation").show();
                }
                else {
                    $("#dealservicecost").show();
                }
                
            }
            if (op == 'displaydeal') {
                //alert("view purpose");
                $("#btnadd").hide();
                $('#btn').hide();
                $("#btncancel").hide();
                $('#btnback').show();
                $('.deletelink').hide();
                $('#fileUpload').hide();
                $('#msg').hide();
                $('.form-control').attr("disabled", "disabled");
                $("#dealcheck").prop('checked', true).attr("disabled", "disabled");
                $("#dealdata").css({ "display": "block" });
                if (servicetype == 'Venue') {
                    var type = $("#Item2_VenueType").val();
                    if (type == 'Banquet Hall') {
                        
                        $("#dealprice").show();
                        $("#dealservicecost").hide();
                        itemsdisable1();
                    }
                    else {
                        $("#dealprice").hide();
                        $("#dealservicecost").show();
                    }
                }
                else if (servicetype == 'BeautyServices') {
                    $("#dealbeautyservicecost").show();
                }
                else if (servicetype == 'Catering') {
                    $("#dealcateringcost").show();
                }
                else if (servicetype == 'InvitationCards') {
                    $("#dealinvitation").show();
                }
                else {
                    $("#dealservicecost").show();
                }

            }
            if (op == 'editdeal') {
                $('#btn').show();
                $("#btncancel").show();
                $('#btnback').hide();
                $('.deletelink').show();
                $('#fileUpload').show();
                $('#msg').show();
                $('.form-control').removeAttr("disabled");
                $("#btnadd").hide();
                
                $("[id^=Item1]").prop("disabled", true);
                $("[id^=Item2]").prop("disabled", true); $("#ddlservice").attr("disabled", "disabled"); 
                $("#dealcheck").prop('checked', true).attr("disabled", "disabled");
                $("#dealdata").css({ "display": "block" });
                if (servicetype == 'Venue') {
                    var type = $("#Item2_VenueType").val();
                    if (type == 'Banquet Hall') {
                        $("#dealprice").show();
                        $("#dealservicecost").hide();
                        itemsdisable1();
                    }
                    else {
                        $("#dealprice").hide();
                        $("#dealservicecost").show();
                    }
                }
                else if (servicetype == 'BeautyServices') {
                    $("#dealbeautyservicecost").show();
                }
                else if (servicetype == 'Catering') {
                    $("#dealcateringcost").show();
                }
                else if (servicetype == 'InvitationCards') {
                    $("#dealinvitation").show();
                }
                else {
                    $("#dealservicecost").show();
                }


            }
            $(".overlay").hide();
        }
//</script>

//@*image Zoom*@
//<script>
    function Large(obj) {
        var imgbox = document.getElementById("imgbox");
        imgbox.style.visibility = 'visible';
        var img = document.createElement("img");

        img.src = obj.src;
        img.style.width = '400px';
        img.style.height = '400px';

        if (img.addEventListener) {
            img.addEventListener('mouseout', Out, false);
        } else {
            img.attachEvent('onmouseout', Out);
        }
        imgbox.innerHTML = '';
        imgbox.appendChild(img);
        imgbox.style.left = (getElementLeft(obj) - 50) + 'px';
        imgbox.style.top = (getElementTop(obj) - 50) + 'px';
    }
function Out() {
    document.getElementById("imgbox").style.visibility = 'hidden';
}
//</script>

//@*items enable/disable on type selection*@
//<script>
    function itemsdisable() {
        var type = $("#Item2_VenueType").val();
        if (type == 'Banquet Hall') {
            $("#veglunch").show();
            $("#nonveglunch").show();
            $("#vegdinner").show();
            $("#nonvegdinner").show();
            $("#menuitem").show();
        }
        else {
            $("#veglunch").hide();
            $("#nonveglunch").hide();
            $("#vegdinner").hide();
            $("#nonvegdinner").hide();
            $("#menuitem").hide();
            $("#Item2_VegLunchCost").val("");
            $("#Item2_NonVegLunchCost").val("");
            $("#Item2_VegDinnerCost").val("");
            $("#Item2_NonVegDinnerCost").val("");
            $("#Item2_Menuwiththenoofitems").val("");
        }
    }
//</script>

//@*item enable/disable on food type selection*@
//<script>
    function itemsdisable1() {
        var type1 = $("#Item2_VenueType").val();
        var type2 = $("#Item2_Food").val(); 
        if (type1 == 'Banquet Hall') {
            if (type2 == 'Veg') {
                $("#veglunch").show();
                $("#nonveglunch").hide();
                $("#vegdinner").show();
                $("#nonvegdinner").hide();
            }
            else {
                $("#veglunch").hide();
                $("#nonveglunch").show();
                $("#vegdinner").hide();
                $("#nonvegdinner").show();
            }
        }
        
        else {
            
            $("#menuitem").hide();
            $("#Item2_VegLunchCost").val("");
            $("#Item2_NonVegLunchCost").val("");
            $("#Item2_VegDinnerCost").val("");
            $("#Item2_NonVegDinnerCost").val("");
            $("#Item2_Menuwiththenoofitems").val("");
        }
    }
//</script>

//@*E-Mail ID check*@
//<script>
        function check() {
            var emailId = $("#Item1_EmailId").val();
            var label = $("#label");
            $.ajax({
                url: window.location.href.split("/")[0] + '//' + window.location.href.split("/")[2] + '/' + window.location.href.split("/")[3] + '/CreateVendor/checkemail',
                type: 'POST',
                data: JSON.stringify({ emailid: emailId }),
                dataType: 'json',
                contentType: 'application/json',
                success: function (data) {
                    if (data == "exists") {
                        $("#Item1_EmailId").focus();
                        $("#btn").prop("disabled", true);
                        $("#lblshow").show();
                        label.html("E-Mail ID Already Taken!!! Try Another");
                    }
                    else {
                        $("#btn").prop("disabled", false);
                        $("#lblshow").hide();
                        label.html("");
                    }
                },
                error: function (data)
                { alert("Failed!!!"); }
            });
        }
//</script>

 