var category = $('#category').val();
var subcategory = $('#subcategory').val();
var subid = $('#vendorsubid').val();
var id = $('#vendorid').val();
var PackageID = '';

//Profile Pic Section
$('#spc').on('click', function () {
    $('#profileimage').click();
});

$('#profileimage').change(function () {
    var ext = $('#profileimage').val().split('.').pop().toLowerCase();
    if ($.inArray(ext, ['gif', 'png', 'jpg', 'jpeg']) == -1) {
        alert('Invalid File Type');
    }
    else {
        var data = new FormData();
        var files = $("#profileimage").get(0).files;
        if (files.length > 0) {
            data.append("helpSectionImages", files[0]);
        }
        $.ajax({
            url: '/vdb/UploadProfilePic',
            type: "POST",
            processData: false,
            contentType: false,
            data: data,
            success: function (response) {
                $('#profilepic1').removeAttr('src');
                $('#profilepic1').attr('src', '/ProfilePictures/' + response + '');
            }
        });
    }
});

// Add/Update Services Section
$(document).on('click', '.subcatclose', function () {
    var pdiv = $(this).parent('div');
    var val1 = $(this).attr('value').split(',');
    var text = val1[1];
    var vsid = val1[0];
    var type = val1[2];
    var r = confirm("Do you want to delete" + text);
    if (r == true) {
        $.ajax({
            url: '/vdb/deleteservice?vsid=' + vsid + '&&type=' + type,
            type: 'POST',
            contentType: 'application/json',
            success: function (result) {
                if (result = 'success') {
                    alert("service removed");
                    pdiv.remove();
                    location.href = '/home';
                }
                else { alert(result); return location.href = '/home';}
            },
        })
    }
});

// Dropdown Change
$(document).ready(function () {
    if (subcategory != null && subcategory != "") {
        $('#selectedservice').val(subcategory); //Assign value to service dropdown here
    }
});

$('#selectedservice').on('change', function () {
    var subcategory1 = $(this).val();
    $.ajax({
        url: '/vdb/AddSubService?type=' + category + '&&subcategory=' + subcategory1 + '&&subid=' + subid + '&&id=' + id,
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

//Slider images Section
var image;
$('.sliderdiv').on('click', function () {
    var thisimage = $(this);
    var image = $(this).children('img').attr('href');
    var file1 = $(this).find('input').attr('id');
    image = document.getElementById("" + file1 + "");
    image.click();
    image.onchange = function () {
        var file = $('#' + file1 + '').get(0).files;
        var data = new FormData;
        if (file.length > 0) {
            data.append("helpSectionImages", file[0]);
        }
        $.ajax({
            url: '/vdb/UploadImages?vid=' + subid + '&&id=' + id + '&&type=Slider',
            type: "POST",
            contentType: false, // Not to set any content header
            processData: false, // Not to process data
            data: data,
            success: function (result) {
                thisimage.children('img').remove();
                thisimage.append('<img src="/vendorimages/' + result + '" href="s2" style="width:100px;height:100px;padding:0px">')
                //thisimage.attr('border', 'none');
            },
            error: function (err) {
                alert("System Encountered Internal Error!!! Try Again After Some Time");
            }
        });
    }
});

$('.sliderdiv1').on('click', function () {
    var thisimage = $(this);
    var image = $(this).children('img').attr('href');
    var file1 = $(this).find('input').attr('id');
    image = document.getElementById("" + file1 + "");
    image.click();
    image.onchange = function () {
        var file = $('#' + file1 + '').get(0).files;
        var data = new FormData;
        if (file.length > 0) {
            data.append("helpSectionImages", file[0]);
        }
        $.ajax({
            url: '/vdb/UploadImages?vid=' + subid + '&&id=' + id + '&&type=image',
            type: "POST",
            contentType: false, // Not to set any content header
            processData: false, // Not to process data
            data: data,
            success: function (result) {
                thisimage.children('img').remove();
                thisimage.append('<img src="/vendorimages/' + result + '" href="s2" style="width:100px;height:100px;padding:0px">')
                //thisimage.attr('border', 'none');
            },
            error: function (err) {
                alert("System Encountered Internal Error!!! Try Again After Some Time");
            }
        });
    }
});

function SaveImages(thisimage,subid,id,type) {
    $.ajax({
        url: '/vdb/UploadImages?vid=' + subid + '&&id=' + id + '&&type='+type,
        type: "POST",
        contentType: false, // Not to set any content header
        processData: false, // Not to process data
        data: data,
        success: function (result) {
            thisimage.children('img').remove();
            thisimage.append('<img src="/vendorimages/' + result + '" href="s2" style="width:100px;height:100px;padding:0px">')
            //thisimage.attr('border', 'none');
        },
        error: function (err) {
            alert("System Encountered Internal Error!!! Try Again After Some Time");
        }
    });
}

//Policy Section
$("#savepolicy").click(function () {
    var policycheck;
    var chkArray = [];
    $(".kscpolicy:checked").each(function () {
        chkArray.push($(this).val());
        var selected;
        selected = chkArray.join(',');
        /* check if there is selected checkboxes, by default the length is 1 as it contains one single comma */
        if (selected.length > 0) {
            //  alert("You have selected " + selected);
            policycheck = selected;
        } else {
            alert("Please at least check one of the checkbox");
        }
    });
    var Tax = $("#Tax").val();
    var Decoration_starting_costs = $("#Decoration_starting_costs").val();
    var Rooms_Count = $("#Rooms_Count").val();
    var Advance_Amount = $("#Advance_Amount").val();
    var Room_Average_Price = $("#Room_Average_Price").val();
    $.ajax({
        url: '/vdb/updatepolicy?policycheck=' + policycheck + '&&id=' + id + '&&vid=' + subid + '&&Tax=' + Tax + '&&Decoration_starting_costs=' + Decoration_starting_costs + '&&Rooms_Count=' + Rooms_Count + '&&Advance_Amount=' + Advance_Amount + '&&Room_Average_Price=' + Room_Average_Price,
        type: 'post',
        contentType: 'application-json',
        success: function (data) {
            alert(data);
        },
        error: function (er) {
            alert("System Encountered Internal Error!!! Try Again After Some Time");
        }
    });

})

//Amenity Section
$(document).on('change', '.amenityselection', function () {
    var timeslot='';  var favorite = [];
    $.each($("#amenitytable input[type='checkbox']:checked"), function () {
        favorite.push($(this).val());
        timeslot = favorite.join(',');
    })
    $('#selectedamenities').val(timeslot);
});

//Packages Section

//Add Package
$("#anp").click(function () {
    $('.overlay').show();
    $('#loadermsg').text("Adding Package...");
    var div = document.getElementById("pkgsdiv");
    var package = {
        VendorId: id,
        VendorSubId: subid,
        VendorType: category,
        VendorSubType: subcategory
    }
    $.ajax({
        url: '/vdb/AddPackage',
        type: 'post',
        datatype: 'json',
        data: package,
        success: function (data) {
            //alert(data); 
            if (data > 0) {
                var childdiv = "<div class='allpkgs'><div class='package-box'><div class='container-fluid'><div class='row'><div class='col-md-6 col-xs-6'><input type='text' class='form-control' id='pkgname' placeholder='Package Name' value=''></div><div class='col-md-6 col-xs-6'><div class='row' id='sectiontimeslot'><div class='col-md-4 col-xs-6'><input type='radio' name='checkradio' id='Veg' value='Veg'><label for='Veg'>Veg</label><br><input type='radio' name='checkradio' id='NonVeg' value='NonVeg'><label for='NonVeg'>Non-Veg</label></div><input type='hidden' id='pkgslots' value=''><div class='col-md-4 col-xs-4'><input type='checkbox' class='slotcheck' name='slot' id='Break_Fast' value='Break Fast'><label for='Break_Fast'>Break Fast</label><br><input type='checkbox' class='slotcheck' name='slot' id='Lunch' value='Lunch'><label for='Lunch'>Lunch</label></div><div class='col-md-4 col-xs-4'><input type='checkbox' class='slotcheck' name='slot' id='Dinner' value='Dinner'><label for='Dinner'>Dinner</label><br><input type='checkbox' class='slotcheck' name='slot' id='High_Tea' value='High Tea'><label for='High_Tea'>High Tea</label></div></div></div></div><div class='row' id='pkgpricesection'><div class='col-md-3 col-xs-3'><span>Normal Days ₹</span><span><input type='number' id='ndays' name='ndays' placeholder='₹100' style='width:75%;color:black' value=''></span></div><div class='col-md-3 col-xs-3'><span>Peak Days ₹</span><span><input type='number' id='pdays' name='pdays' placeholder='₹100' style='width:75%;color:black' value=''></span></div><div class='col-md-3 col-xs-3'><span>Holidays ₹</span><span><input type='number' id='holidays' name='holidays' placeholder='₹100' style='width:75%;color:black' value=''></span></div><div class='col-md-3 col-xs-3'><span>Choice Days ₹</span><span><input type='number' id='cdays' name='cdays' placeholder='₹100' style='width:75%;color:black' value=''></span></div></div><div class='row' align='center' style='padding-top:10px'><textarea id='pkgdesc' class='form-control' name='pkgdesc' placeholder='Enter Package Description' cols='3'></textarea></div></div></div><div class='package-list'><button id='pillbutton' class='button pkgitem' value='Welcome_Drinks'>Welcome Drinks(0) <i class='fa fa-close'></i> </button><button id='pillbutton' class='button pkgitem' value='Starters'>Starters(0)</button><button id='pillbutton' class='button pkgitem' value='Rice'>Rice(0)</button><button id='pillbutton' class='button pkgitem' value='Bread'>Bread(0)</button><button id='pillbutton' class='button pkgitem' value='Curries'>Curries(0)</button><button id='pillbutton' class='button pkgitem' value='Fry_Dry'>Fry/Dry(0)</button><button id='pillbutton' class='button pkgitem' value='Salads'>Salads(0)</button><button id='pillbutton' class='button pkgitem' value='Soups'>Soups(0)</button><button id='pillbutton' class='button pkgitem' value='Deserts'>Deserts(0)</button><button id='pillbutton' class='button pkgitem' value='Beverages'>Beverages(0)</button><button id='pillbutton' class='button pkgitem' value='Fruits'>Fruits(0)</button></div><div align='right' class='pkgsave'><input type='hidden' class='pkgmenuitems' value=''><input type='hidden' class='selpkgitems'><input type='hidden' class='availablepkgitems' value=''><a class='addcourse' style='padding: 17px;margin-top: 3px; color:#dc3545;cursor:pointer'>Add Course</a><a class='viewmenu' style='padding: 17px;margin-top: 3px; color:#dc3545;cursor:pointer'>View Menu</a><a class='pkgclone' style='padding: 17px;margin-top: 3px; color:#dc3545;cursor:pointer'>Duplicate</a><input type='hidden' class='packageid' value='" + data + "' /><a class='updatepkg' style='padding: 17px;margin-top: 3px; color:#dc3545; cursor: pointer;'>Update</a><a class='pkgremove' style='padding: 17px;margin-top: 3px; color:#dc3545;cursor:pointer'>Remove</a></div></div>";
                $('#pkgsdiv').append(childdiv).focus();
                $('.overlay').hide();
                location.reload();
            }
            else {
                alert('Failed To Add Package');
            }
        }
    });
});

//Package Checkbox Selection
$(document).on('change', '.slotcheck', function () {
    var thisdiv = $(this).parents("div.package-box");
    if (thisdiv.find("input[type='checkbox']").is(':checked')) {
        var favorite = [];
        $.each(thisdiv.find("input[type='checkbox']:checked"), function () {
            favorite.push($(this).val());
            timeslot = favorite.join(", ");
        })
        thisdiv.find('#pkgslots').val(favorite);
    }
});

//Loading Menu on Load
var pkgheadings = 'Welcome Drinks,Starters,Rice,Bread,Curries,Fry/Dry,Salads,Soups,Deserts,Beverages,Fruits';
var totallist = '';//$('.pkgmenuitems').val();

//Add Course Button
$(document).on('click', '.addcourse', function () {
//$('.addcourse').click(function(){
    var parentdiv = $(this).parent('div.pkgsave').prev('div.package-list').find('div#extracourse');
    var btn = '<button id="pillbutton" class="button extraitem" value="">New Course(0)</button>';
    parentdiv.append(btn);
});

var pdiv = '';
//Adding Selected Lists to Course
function Addpkgitems(mtype) {
    var type = $('#pkgmodal .selectedpkg').val();
    var maxcount = $('#pkgmodal .maxcount').val();
    var selecteditems = '';
    var itemstable = $('#pkgmodal').find('table.menuitems');
    var ctype = $('#pkgmodal .pkgitemsection p#ctype').text();
    if (mtype != 'old')
        ctype = type.replace(' ','_');
    $(itemstable).find('tr').each(function () {
        if ($(this).find("input[type='checkbox']").is(':checked')) {
            var checkText = $(this).find('label').text();
            if (selecteditems != '')
                selecteditems = selecteditems + '!' + checkText;
            else
                selecteditems = checkText;
        }
    });
    selecteditems = selecteditems.trim(' ');
    var finallist = (totallist != '') ? totallist.split(',') : [];

    var value = pdiv.find('.selpkgitems').val();
    if (value != '') {
        if (value.indexOf(ctype) != -1) {
                var tlist = totallist.split(',');
                for (var j = 0; j < tlist.length; j++) {
                    if (tlist[j].startsWith(ctype)) {
                        tlist[j] = ctype + '(' + selecteditems + ')';
                    }
                }
                totallist = tlist.join(',');
            }
            else {
                finallist.push(ctype + '(' + selecteditems + ')');
                totallist = finallist.join(',');
            }
    }
    else {
        finallist.push(ctype + '(' + selecteditems + ')');
        totallist = finallist.join(',');
    }

    pdiv.find('.selpkgitems').val(totallist);
    if(mtype == 'old')
    {
        if (pdiv.find(".package-list button[value = " + ctype.replace('/', '_') + "]").replaceWith('<button id="pillbutton" class="button pkgitem" value=' + ctype + '>' + type + '(' + maxcount + ')</button>'));
    }
    else
    {
        if (pdiv.find(".package-list #extracourse button:contains(" + mtype + ")").replaceWith('<button id="pillbutton" class="button extraitem" value=' + ctype + '>' + type + '(' + maxcount + ')</button>'));
    }
}

// Loading Package Menu
$(document).on('click', '.pkgitem', function () {
    $('.overlay').show();
    $('#loadermsg').text("Fetching Menu Items");
    var parentdiv = $(this).parent('div.package-list').prev('div.package-box');
    var checkbox = parentdiv.find('input[type=radio]:checked').val();//$("input[name='checkradio']:checked").val();
    var pkgid = $(this).parent("div.allpkgs").find("div.pkgsave").find("input.packageid").val();
    PackageID = pkgid;
    var pkgsavediv = $(this).parent('div.package-list').next("div.pkgsave");
    if (checkbox == undefined) {
        alert("Select Menu Type")
    }
    else {
        var val = $(this).val();
        if(val == "Fry_Dry") val="Fry/Dry";
        var pkgid = $(this).text().split('(')[0]; // Course name
        $('#pkgmodal .pkgitemsection #ctype').text(val);
        $('#pkgmodal .maxcount').val($(this).text().split('(')[1].split(')')[0]);
        $('#pkgmodal .selectedpkg').val(pkgid);
        $('#pkgmodal .add').attr('onclick', 'Addpkgitems("old")');
        pdiv = '';
        pdiv = $(this).parent('div.package-list').parent('div.allpkgs');
        var exists = $(this).text().split('(')[1].split(')')[0];
        filteritems(checkbox, val.replace('_',' '),pkgid,pkgsavediv);
    }
});

// Loading Extra Menus
$(document).on('click', '.extraitem', function () {
    $('.overlay').show();
    $('#loadermsg').text("Fetching Menu Items");
    var parentdiv = $(this).parents('div.package-list').prev('div.package-box');
    var checkbox = parentdiv.find('input[type=radio]:checked').val();//$("input[name='checkradio']:checked").val();
    var pkgid = $(this).parents("div.allpkgs").find("div.pkgsave").find("input.packageid").val();
    PackageID = pkgid;
    var pkgsavediv = $(this).parents("div.allpkgs").find("div.pkgsave");
    if (checkbox == undefined) {
        alert("Select Menu Type")
    }
    else {
        var val = $(this).text().split('(')[0];
        //var pkgid = $(this).text(); // Course name
        $('#pkgmodal .pkgitemsection #ctype').text(val);
        $('#pkgmodal .maxcount').val($(this).text().split('(')[1].split(')')[0]);
        $('#pkgmodal .selectedpkg').val(val);
        $('#pkgmodal .add').attr('onclick', 'Addpkgitems("' + val + '")');
        pdiv = '';
        pdiv = $(this).parents('div.package-list').parent('div.allpkgs');
        var exists = $(this).text().split('(')[1].split(')')[0];
        filteritems(checkbox, val, 'New', pkgsavediv);
    }
});
//$('.writemsg.modal').modal('attach events', '.writemsg.button', 'show');

//Filtering Menu and loading menu items
function filteritems(type, selectedpkg, pkgid, pkgsavediv) {
    var selpkg = selectedpkg.replace(' ', '_').replace('/', '_').toLowerCase();
    var welcome_drinks = ""; var starters = ""; var rice = ""; var bread = ""; var curries = "";
    var fry_dry = ""; var salads = ""; var soups = ""; var tandoori_kababs = ""; var deserts = ""; var beverages = ""; var fruits = "";
    var tabletr = $('#pkgmodal .pkgitemsection table.menuitems'); var commondiv = "";
    var itemsdiv = $('#pkgmodal .pkgitemsection table.menuitems tr#pkgitemstr');
    tabletr.empty();
    var url = ''
    if (pkgid == 'New')
        url = "/vdb/GetPackageExtraItem?menuitem=" + selectedpkg + "&&id=" + id + "&&subid=" + subid + "&&pkgid="+PackageID;
    else
        url = "/vdb/GetPackagemenuItem?menuitem=" + selectedpkg + "&&category=" + type + "&&vsid=" + subid + "";

    $.get(url,
        function (data) {
            commondiv = data;
            var getlist = pkgsavediv.find('input.selpkgitems').val() + ',' + pkgsavediv.find('input.pkgmenuitems').val();//$('.selpkgitems').val() + ',' + $('.pkgmenuitems').val();
            var newitem = '';
            if(pkgid == 'New')
                newitem='<tr class="newtr"><td style="padding:0"><input type="textbox" class="form-control newitem" /><div align="center"><button class="btn btn-success sni">+ New Item</button>  <button class="btn btn-success saveall1">Save All</button></div></td></tr>';
            else
                newitem = '<tr class="newtr"><td style="padding:0"><input type="textbox" class="form-control newitem" /><div align="center"><button class="btn btn-success sni">+ New Item</button>  <button class="btn btn-success saveall">Save All</button></div></td></tr>';

            for (var i = 0; i < commondiv.split('!').length; i++) {
                var row = $("<tr/>");
                if (getlist != null && getlist != ',' && commondiv != '') {
                    if (getlist.includes(commondiv.split('!')[i])) {
                        row.append('<td class="menuitems" style="padding-top:0;padding:0"> <a title="Remove ' + commondiv.split('!')[i] + '?" class="removemitem" style="color:red;cursor:pointer">X</a> <input type="checkbox" class="icheck" style="margin-left:5px;" checked id="' + commondiv.split('!')[i] + '"/>  <label for="' + commondiv.split('!')[i] + '">' + commondiv.split('!')[i] + '</label></td>');
                    }
                    else {
                        row.append('<td class="menuitems" style="padding-top:0;padding:0"> <a title="Remove ' + commondiv.split('!')[i] + '?" class="removemitem" style="color:red;cursor:pointer">X</a> <input type="checkbox" class="icheck" style="margin-left:5px;" id="' + commondiv.split('!')[i] + '"/>  <label for="' + commondiv.split('!')[i] + '">' + commondiv.split('!')[i] + '</label></td>');
                    }
                }
                else if(commondiv != ''){
                    row.append('<td class="menuitems" style="padding-top:0;padding:0"> <a title="Remove ' + commondiv.split('!')[i] + '?" class="removemitem" style="color:red;cursor:pointer">X</a> <input type="checkbox" style="margin-left:5px;" id="' + commondiv.split('!')[i] + '"/>  <label for="' + commondiv.split('!')[i] + '">' + commondiv.split('!')[i] + '</label></td>');
                }
                tabletr.append(row);
            }
            tabletr.append(newitem);
            $('.overlay').hide();
            $('#pkgmodal').modal('show');
        });

}

//Adding new item to List
$(document).on('click','.sni',function(){
    var item = $(this).parents("td").find("input").val();
    if (item != '') {
        var nitem = '<tr><td class="menuitems" style="padding:0"><a title="'+item+'" class="removemitem" style="color:red;cursor:pointer">X</a> <input type="checkbox" style="margin-left:5px;"/>  <label>' + item + '</label></td></tr>';
        $(this).parents("tr.newtr").before(nitem);
        var totalitems = '';
        var favorite = [];
        $.each($('#pkgmodal .pkgitemsection table.menuitems input[type=checkbox]'), function () {
            favorite.push($(this).next("label").text());
            totalitems = favorite.join('!');
        });
        $(this).parents("td").find("input").val('').focus();
    }
    else {
        alert("Item Name Cannot be Blank");
        $(this).parents("td").find("input").focus();
    }

});


//Save All for existing items
$(document).on('click','.saveall',function(){
    $('.overlay').show();
    $('#loadermsg').text("Updating items list...");
    var type = $('#pkgmodal .selectedpkg').val();
    var ctype = $('#pkgmodal #ctype').text();
    var pkgcategory = $("input[type='radio']:checked").val();
    var totalitems = '';var favorite = [];
    $.each($('#pkgmodal .pkgitemsection table.menuitems input[type=checkbox]'), function () {
        favorite.push($(this).next("label").text());
        totalitems = favorite.join('!');
    });
    totalitems = totalitems;
    //alert(totalitems);
    var PackageMenu = {
        VendorID :subid,
        VendorMasterID:id,
        Category: pkgcategory,
        Welcome_Drinks:totalitems,
        Starters:totalitems,
        Rice:totalitems,
        Bread:totalitems,
        Curries:totalitems,
        Fry_Dry:totalitems,
        Salads:totalitems,
        Soups:totalitems,
        Deserts:totalitems,
        Beverages:totalitems,
        Fruits:totalitems,
    }
    //alert(ctype);
    $.ajax({
        url:'/vdb/UpdateMenuItems',
        type: 'POST',
        data: JSON.stringify({ PackageMenu: PackageMenu, type: ctype }),
        dataType: 'json',
        contentType: 'application/json',
        success:function(data){
            $('.overlay').hide();
            alert(type + '!!!Select items and click Add');
            //$('#pkgmodal').modal('hide');
        }
    });
});

//Save All for Extra Items
$(document).on('click','.saveall1',function(){
    $('.overlay').show();
    $('#loadermsg').text("Updating items list...");
    var type = $('#pkgmodal .selectedpkg').val();
    var pkgcategory = $("input[type='radio']:checked").val();
    var totalitems = '';var favorite = [];
    $.each($('#pkgmodal .pkgitemsection table.menuitems input[type=checkbox]'), function () {
        favorite.push($(this).next("label").text());
        totalitems = favorite.join('!');
    });
    totalitems = type + '(' + totalitems + ')';
    //alert(totalitems);
    var Package = {
        PackageID:PackageID,
        VendorSubId: subid,
        VendorId: id,
        Category: pkgcategory,
        extramenuitems: totalitems,
    }
    //alert(Package.extramenuitems);
    $.ajax({
        url:'/vdb/NewCourse',
        type: 'POST',
        data: JSON.stringify({ Package: Package, type: type }),
        dataType: 'json',
        contentType: 'application/json',
        success:function(data){
            $('.overlay').hide();
            alert(data + '!!!Select items and click Add');
            //$('#pkgmodal').modal('hide');
        }
    });
});

//View Menu
$(document).on('click', '.viewmenu', function () {
//$('.viewmenu').click(function () {
    var packagename = $('#pkgname').val();
    var pkgcategory = $("input[type='radio']:checked").val();
    var peakdays = $('#pdays').val();
    var normaldays = $('#ndays').val();
    var holidays = $('#holidays').val();
    var choicedays = $('#cdays').val();
    var pkgdesc = $('#pkgdesc').val();
    var maindiv = '';
    var menuitems = $(this).parents('div.pkgsave').find('input.pkgmenuitems').val();
    if (menuitems != '' && menuitems != null) {
        var count = menuitems.split(',').length;
        for (var i = 0; i < count; i++) {
            var totalitem = menuitems.split(',')[i];
            var heading = totalitem.split('(')[0];
            var items = totalitem.split('(')[1].split(')')[0].split('!');
            maindiv+= '<div class="heading">'+heading+'</div><div class="items">'+items+'</div><br/>';
        }
    }
    else {
        maindiv = "No Menu Items";
    }
    $('#viewmenuitems .pkgname').text(packagename);
    $('#viewmenuitems .pkgtype').text(pkgcategory);
    $('#viewmenuitems #peakdays').text(peakdays);
    $('#viewmenuitems #normaldays').text(normaldays);
    $('#viewmenuitems #holidays').text(holidays);
    $('#viewmenuitems #choicedays').text(choicedays);
    $('#viewmenuitems #pkgitems').empty().append(maindiv);
    $('#viewmenuitems').modal('show');
});

//Saving Package
$(document).on('click', '.addpkg', function () {
//$('.addpkg').click(function () {
    //validation('AddPackage');
    $('.overlay').show();
    $('#loadermsg').text("Saving Package...");
    var parentdiv = $(this).parent('div.pkgsave').parent('div.allpkgs');
    var allbuttons = parentdiv.find(".package-list button").text();
    var list=[];
    for (var i = 0; i < allbuttons.split(')').length; i++) {
        if(allbuttons.split(')')[i] != '' && allbuttons.split(')')[i].split('(')[1]!=0)
            list.push(allbuttons.split(')')[i]+')');
    }
    var packagename = parentdiv.find('#pkgname').val();
    var pkgcategory = parentdiv.find("input[type='radio']:checked").val();
    var peakdays = parentdiv.find('#pdays').val();
    var normaldays = parentdiv.find('#ndays').val();
    var holidays = parentdiv.find('#holidays').val();
    var choicedays = parentdiv.find('#cdays').val();
    var timeslot = parentdiv.find('#pkgslots').val();
    var pkgdescription = parentdiv.find('#pkgdesc').val();
    var menuitems = $(this).parent('div').find('input.selpkgitems').val();//pkgdesc;
    var menu = list.join(','); //$(this).parent('div').find('input.selpkgitems').val();
    //alert(pkgdesc);
    if (packagename == '') {
        parentdiv.find('#pkgname').focus();
        alert("Enter Package Name");
    }
    else if (pkgcategory == undefined) {
        alert("Select Package Category");
    }
    else if (timeslot == '') {
        alert("Select TimeSlots");
    }
    else if (normaldays == '') {
        parentdiv.find('#ndays').focus();
        alert("Enter Normal Days Price");
    }
    else if (peakdays == '') {
        parentdiv.find('#pdays').focus();
        alert("Enter Peak Days Price");
    }
    else if (holidays == '') {
        parentdiv.find('#holidays').focus();
        alert("Enter Holidays Price");
    }
    else if (choicedays == '') {
        parentdiv.find('#cdays').focus();
        alert("Enter Choice Days Price");
    }
    else {
        var package = {
            PackageName: packagename,
            Category: pkgcategory,
            PackageDescription: pkgdescription,
            peakdays: peakdays,
            normaldays: normaldays,
            holidays: holidays,
            choicedays: choicedays,
            VendorId: id,
            VendorSubId: subid,
            VendorType: category,
            VendorSubType: subcategory,
            timeslot: timeslot,
            menuitems : menuitems,
            menu : menu
        }
        $.ajax({
            url: '/vdb/AddPackage',
            type: 'post',
            datatype: 'json',
            data: package,
            success: function (data) {
                alert(data);
                $('.overlay').hide();
            }
        });
    }
});

//Updating Package
$(document).on('click', '.updatepkg', function () {
    var menuitems='';
    var parentdiv = $(this).parent('div.pkgsave').parent('div.allpkgs');
    var allbuttons = parentdiv.find(".package-list button").text();
    var list=[];
    for (var i = 0; i < allbuttons.split(')').length; i++) {
        if(allbuttons.split(')')[i] != '' && allbuttons.split(')')[i].split('(')[1]!=0)
            list.push(allbuttons.split(')')[i]+')');
    }
    var allVal = '';
    parentdiv.find(".package-list button").each(function () {
        var textvalue = $(this).text();
        if (textvalue != '') {
            if ($(this).text().split('(')[1].split(')')[0] != 0)
                allVal += ',' + $(this).val();
        }
    });
    var allbuttonsvals = allVal.substring(1, allVal.length);
    var newlists = '';
    if (parentdiv.find('.selpkgitems').val() != '') {
        newlists = parentdiv.find('.selpkgitems').val() + ',' + parentdiv.find('.pkgmenuitems').val();
    }
    else {
        newlists = parentdiv.find('.pkgmenuitems').val();
    }
    //var newlists = parentdiv.find('.selpkgitems').val() + ',' + parentdiv.find('.pkgmenuitems').val();
    var dblist = parentdiv.find('.pkgmenuitems').val();
    var selectedlist = parentdiv.find('.availablepkgitems').val();
    var splitteddblist = dblist.split(',');
    var a =[];var b=[];
    for (var i = 0; i < newlists.split(',').length; i++) {
        var thisval = newlists.split(',')[i].split('(')[0]
        if(thisval != '')
            a.push(thisval);
    }
    var availableitems = a;
    if (selectedlist != undefined) {
        for (var i = 0; i < availableitems.length; i++) {
            var value = $.inArray(availableitems[i].replace('/', '_'), selectedlist.split(','));
            if (b.indexOf(value) == -1) {
                //alert(b.indexOf(value));
                if (value != -1) {
                    b.push(value);
                    splitteddblist[value] = newlists.split(',')[i];
                }
                else {
                    splitteddblist.push(newlists.split(',')[i]);
                    //alert(newlists.split(',')[i]);
                }
            }
        }
        menuitems = splitteddblist.join(',');
    }
    else {
        menuitems = newlists;
    }
    alert(menuitems);
    var pkgid = $(this).prev('input.packageid').val();
    var packagename = parentdiv.find('#pkgname').val();
    var pkgcategory = parentdiv.find("input[type=radio]:checked").val();
    var peakdays = parentdiv.find('#pdays').val();
    var normaldays = parentdiv.find('#ndays').val();
    var holidays = parentdiv.find('#holidays').val();
    var choicedays = parentdiv.find('#cdays').val();
    var timeslot = parentdiv.find('#pkgslots').val();
    var pkgdescription = parentdiv.find('#pkgdesc').val();
    //var menuitems = $(this).parent('div').find('input.selpkgitems').val();
    var menu = list.join(',');//$(this).parent('div').find('input.selpkgitems').val();
    if (packagename == '') {
        $(packagename).focus();
        alert("Enter Package Name");
    }
    else if (pkgcategory == undefined) {
        alert("Select Package Category");
    }
    else if (timeslot == '') {
        alert("Select TimeSlots");
    }
    else if (normaldays == '') {
        $('#ndays').focus();
        alert("Enter Normal Days Price");
    }
    else if (peakdays == '') {
        $('#pdays').focus();
        alert("Enter Peak Days Price");
    }
    else if (holidays == '') {
        $('#holidays').focus();
        alert("Enter Holidays Price");
    }
    else if (choicedays == '') {
        $('#cdays').focus();
        alert("Enter Choice Days Price");
    }
    else if (menuitems == '') {
        alert("Select Menu Items");
    }
    else {
        $('.overlay').show();
        $('#loadermsg').text("Updating Package...");
        var package = {
            PackageID:pkgid,
            PackageName: packagename,
            Category: pkgcategory,
            PackageDescription: pkgdescription,
            peakdays: peakdays,
            normaldays: normaldays,
            holidays: holidays,
            choicedays: choicedays,
            VendorId: id,
            VendorSubId: subid,
            VendorType: category,
            VendorSubType: subcategory,
            timeslot: timeslot,
            menuitems : menuitems,
            menu:menu
        }        
        $.ajax({
            url: '/vdb/UpdatePackage',
            type: 'post',
            datatype: 'json',
            data: package,
            success: function (data) {
                alert(data);
                $('.overlay').hide();
            }
        });
    }
});

//Copy Package
$(document).on('change', '.copypkg', function () {
    $('.overlay').show();
    $('#loadermsg').text("Copying Package...");
    var selectedid = $(this).val();
    var pkgid = $(this).parent('div.pkgsave').find('input.packageid').val();
    $.ajax({
        url: '/vdb/CopyPackage?pkgid=' + pkgid + '&&id=' + id + '&&selectedid=' + selectedid + '&&subid=' + subid,
        type: 'post',
        datatype: 'json',
        success: function (data) {
            if (data > 0) {
                alert("Package Copied Successfully!!!");
            }
            else {
                alert(data);
            }
            $('.overlay').hide();
        }
    });
});

// Cloning Package
$(document).on('click', '.pkgclone', function () {
    $('.overlay').show();
    $('#loadermsg').text("Duplicating Package...");
    var pkgid = $(this).parent('div.pkgsave').find('input.packageid').val();
    $.ajax({
        url: '/vdb/DuplicatePackage?pkgid=' + pkgid + '&&id=' + id + '&&subid=' + subid,
        type: 'post',
        datatype: 'json',
        success: function (data) {
            if (data > 0) {
                alert("Package Duplicated Successfully!!!");
            }
            else {
                alert(data);
            }
            location.reload();
        }
    });
});

//Remove Package
$(document).on('click', '.pkgremove', function () {
    var pkgid = $(this).parent('div.pkgsave').find('input.packageid').val();
    var totaldiv = $(this).parent('div.pkgsave').parent('div.allpkgs');
    $.ajax({
        url: '/vdb/DeletePackage?pkgid=' + pkgid,
        type: 'post',
        datatype: 'json',
        success: function (data) {
            if (data == 'success') {
                totaldiv.css('display', 'none');
                alert('Package Removed Successfully!!!');
                $('.overlay').hide();
            }
        }
    });
});

function duplicatepkg(thisdiv, pkgid) {
    var parentdiv = document.getElementById("pkgsdiv");
    thisdiv.parents('div.allpkgs').clone().find("input.packageid").attr('value', pkgid).end().insertAfter(parentdiv);
}

// Removing Item from list
$(document).on('click','.removemitem',function(){
    $('.overlay').show();
    $('#loadermsg').text("Removing item from list...");
    var item = $(this).next("input").next("label").text();
    var selpkg = $('#pkgmodal .selectedpkg').val();
    var totalitems = '';
    var favorite = [];
    var pkgcategory = $("input[type='radio']:checked").val();
    $.each($('#pkgmodal .pkgitemsection table.menuitems input[type=checkbox]'), function () {
        favorite.push($(this).next("label").text());
        totalitems = favorite.join('!');
    })
    var tr = $(this).parents("tr");//.remove();
    totalitems = totalitems.replace('!'+item,'');
    var PackageMenu = {
        VendorID :subid,
        VendorMasterID:id,
        Category: pkgcategory,
        Welcome_Drinks:totalitems,
        Starters:totalitems,
        Rice:totalitems,
        Bread:totalitems,
        Curries:totalitems,
        Fry_Dry:totalitems,
        Salads:totalitems,
        Soups:totalitems,
        Deserts:totalitems,
        Beverages:totalitems,
        Fruits:totalitems
    }
    $.ajax({
        url:'/vdb/UpdateMenuItems',
        type: 'POST',
        data: JSON.stringify({ PackageMenu: PackageMenu, type: selpkg }),
        dataType: 'json',
        contentType: 'application/json',
        success:function(data){
            $('.overlay').hide();
            alert(data);
            tr.remove();
        }
    });
});

// Rental Package

//Package Checkbox Selection
$(document).on('change', '.slotcheck1', function () {
    var thisdiv = $(this).parents("div.package-box");
    if (thisdiv.find("input[type='checkbox']").is(':checked')) {
        var favorite = [];
        $.each(thisdiv.find("input[type='checkbox']:checked"), function () {
            favorite.push($(this).val());
            timeslot = favorite.join(", ");
        })
        thisdiv.find('#rpkgslots').val(favorite);
    }
});

// Add Rental/Update Package
$(document).on('click', '.rsavepkg', function () {
    var type = $(this).text();    
    var parentdiv = $(this).parent('div.rpkgsave').parent('div.rallpkgs');
    var packagename = parentdiv.find('#pkgname').val();
    var peakdays = parentdiv.find('#pdays').val();
    var normaldays = parentdiv.find('#ndays').val();
    var holidays = parentdiv.find('#holidays').val();
    var choicedays = parentdiv.find('#cdays').val();
    var timeslot = parentdiv.find('#rpkgslots').val();
    var pkgdescription = parentdiv.find('#pkgdesc').val();
    var pkgid = $(this).prev('input.packageid').val();
    if (packagename == '') {
        parentdiv.find('#pkgname').focus();
        alert("Enter Package Name");
    }
    else if (timeslot == '') {
        alert("Select TimeSlots");
    }
    else if (normaldays == '') {
        parentdiv.find('#ndays').focus();
        alert("Enter Normal Days Price");
    }
    else if (peakdays == '') {
        parentdiv.find('#pdays').focus();
        alert("Enter Peak Days Price");
    }
    else if (holidays == '') {
        parentdiv.find('#holidays').focus();
        alert("Enter Holidays Price");
    }
    else if (choicedays == '') {
        parentdiv.find('#cdays').focus();
        alert("Enter Choice Days Price");
    }
    else {
        $('.overlay').show();
        $('#loadermsg').text("Saving Package...");
        var package = {
            PackageID: pkgid,
            PackageName: packagename,
            PackageDescription: pkgdescription,
            peakdays: peakdays,
            normaldays: normaldays,
            holidays: holidays,
            choicedays: choicedays,
            VendorId: id,
            VendorSubId: subid,
            VendorType: category,
            VendorSubType: subcategory,
            timeslot: timeslot,
        }
        var url = "";
        if (type == 'Save')
            url = '/vdb/AddRentalPackage';
        else if (type == 'Update')
            url = '/vdb/UpdateRentalPackage';
        $.ajax({
            url: url,
            type: 'post',
            datatype: 'json',
            data: package,
            success: function (data) {
                //if (type == 'Save') {
                    if (data > 0) {
                        alert("Rental Package "+type+"d Successfully!!!");
                    }
                    else {
                        alert("Failed!!!");
                    }
                //}
                //else if (type == 'Update') {
                //    alert(data);
                //}
                $('.overlay').hide();
            }
        });
    }
});

//Remove Rental Package
$(document).on('click', '.rpkgremove', function () {
    var pkgid = $(this).parent('div.rpkgsave').find('input.packageid').val();
    var totaldiv = $(this).parent('div.rpkgsave').parent('div.rallpkgs');
    $.ajax({
        url: '/vdb/DeletePackage?pkgid=' + pkgid,
        type: 'post',
        datatype: 'json',
        success: function (data) {
            if (data == 'success') {
                totaldiv.css('display', 'none');
                alert('Package Removed Successfully!!!');
                $('.overlay').hide();
            }
        }
    });
});

// Cloning Rental Package
$(document).on('click', '.rpkgclone', function () {
    $('.overlay').show();
    $('#loadermsg').text("Duplicating Package...");
    var pkgid = $(this).parent('div.rpkgsave').find('input.packageid').val();
    $.ajax({
        url: '/vdb/DuplicateRentalPackage?pkgid=' + pkgid + '&&id=' + id + '&&subid=' + subid,
        type: 'post',
        datatype: 'json',
        success: function (data) {
            if (data > 0) {
                alert("Rental Package Duplicated Successfully!!!");
            }
            else {
                alert(data);
            }
            location.reload();
        }
    });
});

// Updating Address,Amenities details
function updatedetails(val) {
    var hdname = $('#hdname').val();
    var Dimentions = $('#Dimentions').val();
    var Minimumseatingcapacity = $('#Minimumseatingcapacity').val();
    var Maximumcapacity = $('#Maximumcapacity').val();
    var selectedamenuities = $('#selectedamenities').val();
    //var vsid = subid;
    var Address = $('#Address').val();
    var Landmark = $('#Landmark').val();
    var City = $('#City').val();
    var ZipCode = $('#ZipCode').val();
    var Description = $('#mainDescription').val();
    $.ajax({
        url: '/VDashboard/UpdateAmenities?selectedamenities=' + selectedamenuities + '&&hdname=' + hdname + '&&vsid=' + subid + '&&Dimentions=' + Dimentions + '&&Minimumseatingcapacity=' + Minimumseatingcapacity + '&&Maximumcapacity=' + Maximumcapacity + '&&Description=' + Description + '&&Address=' + Address + '&&Landmark=' + Landmark + '&&City=' + City + '&&ZipCode=' + ZipCode + '&&command=' + val,
        type: 'post',
        contentType: 'application-json',
        success: function (data) {
            alert(data);
        },
        error: function (er) {
            alert("System Encountered Internal Error!!! Try Again After Some Time");
        }
    });
}


//Filtering Services
$('#btnsearch').click(function () {
    window.localStorage.removeItem("guests1");
    window.localStorage.removeItem("location1");
    window.localStorage.removeItem("eventtype1");
    window.localStorage.removeItem("eventdate1");

    var location = 'Hyderabad';//$('#loc').val();
    var eventtype = $('.select2-selection__rendered').text();//$('span.current').text();
    var guests = $('#guests').val();
    var date = $('#datetimepicker1').val();
    if (location == '') {
        alert('Enter Location');
        $('#loc').focus();
    }
    else if (eventtype == '' || eventtype == null || eventtype == 'Select Event') {
        alert('Select Event Type');
    }
    else if (guests == '') {
        alert('Enter Guests');
        $('#guests').focus();
    }
    else if (date == '') {
        alert('Select Event date');
        $('#datetimepicker1').focus();
    }
    else {
        window.localStorage.setItem("guests1", guests);
        window.localStorage.setItem("location1", location);
        window.localStorage.setItem("eventtype1", eventtype);
        window.localStorage.setItem("eventdate1", date);
        window.location.href = '/vdb/Index?eventtype=' + eventtype + '&&count=' + guests + '&&date=' + date;
    }
});

//Calendar Onchange
$(".vendorcalendar").on("change", function () {
    $(".kscbook").css('display', 'none');
    var date = new Date($(this).val());
    var yr = date.getFullYear();
    var day = date.getDate() < 10 ? '0' + date.getDate() : date.getDate();
    var month = date.getMonth() + 1;
    var selected = yr + '-' + month + '-' + day;
    var vsid = $(this).parents("div#vcal").children('#venusid').val();
    // alert(selected + '~' + vsid);
    vcal = $("#vcalender").val();
    selected1 = selected + '~' + vsid;
    var vcal1 = vcal + "," + selected1;
    //   alert(vcal1);
    $("#vcalender").val(vcal1);
    // $(this).parents("div#vcal").next("div.kscbook").css('display', 'block');
});

//initialising Datepickers
$('#datetimepicker1,datetimepicker2').datepicker({
    dateFormat: 'dd/mm/yy',
    minDate: new Date()
});

//Booking part
$('.btnbook').on('click', function () {
    window.localStorage.removeItem("vpbook", select);
    window.localStorage.removeItem("vpbookdate", date);
    window.localStorage.removeItem("vpbooktimeslot", timeslotlist);
    window.localStorage.removeItem("vpbookpid", packageid);
    //   getValueUsingClass();
    /* declare an checkbox array */
    var chkArray = [];

    /* look for all checkboes that have a class 'chk' attached to it and check if it was checked */
    $(".kspackage:checked").each(function () {
        chkArray.push($(this).val());
    });

    /* we join the array separated by the comma */
    var selected;
    selected = chkArray.join(',');

    /* check if there is selected checkboxes, by default the length is 1 as it contains one single comma */
    if (selected.length > 0) {
        //  alert("You have selected " + selected);
    } else {
        alert("Please at least check one of the checkbox");
    }
    var chkArray1 = [];

    /* look for all checkboes that have a class 'chk' attached to it and check if it was checked */
    $(".kstimeslot:checked").each(function () {
        chkArray1.push($(this).val());
    });

    /* we join the array separated by the comma */
    var timeslot;
    timeslot = chkArray1.join(',');

    /* check if there is selected checkboxes, by default the length is 1 as it contains one single comma */
    if (timeslot.length > 0) {
        //  alert("You have selected " + selected);
    } else {
        alert("Please at least check timeslot of the checkbox");
    }
    // var calendar = $("#vcalender").val();
    //var calendar = $("input[id='vcalender']")
    //    .map(function () { return $(this).val(); }).get();
    var packageid = selected;
    window.localStorage.removeItem("vpbook");
    var timeslotlist = timeslot;
    var location = 'Hyderabad';//$('#loc').val();
    //var eventtype = $('.current').text().replace('1', '');
    //var guests = $('#guests').val();
    var eventtype = $('.select2-selection__rendered').text();//$('.search-box2').find(':selected');//'wedding';
    //alert(eventtype);
    var guests = $('#guests').val();//'35';
    var date = $('#vcalender').val();
    // if (date == '') date = $('#datetimepicker1').val();

    if (eventtype == '' || eventtype == null || eventtype == 'Select Event') {
        alert('Select Event Type');
    }
    else if (guests == '') {
        alert('Enter Guests');
        $('#guests').focus();
    }
    else if (date == '') {
        alert('Select date');
        $('#datetimepicker1').focus();
    }
    else {
        var select = location + ',' + guests + "," + eventtype;
        window.localStorage.setItem("vpbook", select);
        window.localStorage.setItem("vpbookdate", date);
        window.localStorage.setItem("vpbooktimeslot", timeslotlist);
        window.localStorage.setItem("vpbookpid", packageid);
        //  alert(select +","+ packageid +","+timeslotlist);
        window.location.href = '/ManageUser/Index';
        //alert('Location = '+location+',Event Type = '+eventtype+',Guests = '+guests+',Event Date = '+date+'');
    }
});
