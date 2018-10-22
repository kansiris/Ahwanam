﻿var category = $('#category').val();
var subcategory = $('#subcategory').val();
var subid = $('#vendorsubid').val();
var id = $('#vendorid').val();

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
                //var url = '/vdashboard/profilepic';
                //$('#profilepipccontent').empty().load(url);
            }
        });
    }
});

// Add/Update Services Section
$(document).on('click', '.subcatclose', function () {
    var val1 = $(this).attr('value').split(',');
    var text = val1[1];
    var vsid = val1[0];
    var type = val1[3];
    var r = confirm("Do you want to delete" + text);
    if (r == true) {
        $.ajax({
            url: '/vdb/deleteservice?vsid=' + vsid + '&&type=' + type,
            type: 'POST',
            contentType: 'application/json',
            success: function (result) {
                if (result = 'success') {
                    alert("service removed");
                } else { alert("error occured"); }
                var url1 = document.referrer;
                window.location.href = url1;
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
        SaveImages(thisimage, subid, id, 'Slider');
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
        SaveImages(thisimage, subid, id, 'image');
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

//Amenities Section
var validateForm = function () {
    var checks = $('input[type="checkbox"]:checked').map(function () {
        return $(this).val();
    }).get()
    var hdname = $('#hdname').val();
    var Dimentions = $('#Dimentions').val();
    var Minimumseatingcapacity = $('#Minimumseatingcapacity').val();
    var Maximumcapacity = $('#Maximumcapacity').val();
    var selectedamenuities = checks;
    var Address = $('#Address').val();
    var Landmark = $('#Landmark').val();
    var City = $('#City').val();
    var ZipCode = $('#ZipCode').val();
    var Description = $('#mainDescription').val();
    $.ajax({
        url: '/vdb/UpdateAmenities?selectedamenities=' + selectedamenuities + '&&id=' + id + '&&vid=' + subid + '&&hdname=' + hdname + '&&Dimentions=' + Dimentions + '&&Minimumseatingcapacity=' + Minimumseatingcapacity + '&&Maximumcapacity=' + Maximumcapacity + '&&Description=' + Description + '&&Address=' + Address + '&&Landmark=' + Landmark + '&&City=' + City + '&&ZipCode=' + ZipCode + '&&command=' + 'two',
        type: 'post',
        contentType: 'application-json',
        success: function (data) {
            alert(data);
        },
        error: function (er) {
            alert("System Encountered Internal Error!!! Try Again After Some Time");
        }
    });
    return false;
}

//Packages Section

//Add Package
$("#anp").click(function () {
    var div = document.getElementById("pkgsdiv");
    var childdiv = "<div class='row' id='pkgsdiv'><div class='allpkgs'><div class='container-fluid'><div class='row'><div class='col-md-6 col-xs-6'><input type='text' class='form-control' id='pkgname' placeholder='Package Name' value=''></div><div class='col-md-6 col-xs-6'><div class='row' id='sectiontimeslot'><div class='col-md-4 col-xs-6'><input type='radio' name='checkradio' id='Veg' value='Veg' /><label for='Veg'>Veg</label><br /><input type='radio' name='checkradio' id='NonVeg' value='NonVeg' /><label for='NonVeg'>Non-Veg</label></div><input type='hidden' id='pkgslots' value='' /><div class='col-md-4 col-xs-4'><input type='checkbox' class='slotcheck' name='slot' id='Break_Fast' value='Break Fast' /><label for='breakfast'>Break Fast</label><br /><input type='checkbox' class='slotcheck' name='slot' id='Lunch' value='Lunch' /><label for='lunch'>Lunch</label></div><div class='col-md-4 col-xs-4'><input type='checkbox' class='slotcheck' name='slot' id='Dinner' value='Dinner' /><label for='dinner'>Dinner</label><br /><input type='checkbox' class='slotcheck' name='slot' id='High_Tea' value='High Tea' /><label for='hightea'>High Tea</label></div></div></div></div><div class='row' id='pkgpricesection'><div class='col-md-3 col-xs-3'><span>Normal Days ₹</span><span><input type='number' id='ndays' name='ndays' placeholder='₹100' style='width:75%' value='' /></span></div><div class='col-md-3 col-xs-3'><span>Peak Days ₹</span><span><input type='number' id='pdays' name='pdays' placeholder='₹100' style='width:75%' value='' /></span></div><div class='col-md-3 col-xs-3'><span>Holidays ₹</span><span><input type='number' id='holidays' name='holidays' placeholder='₹100' style='width:75%' value='' /></span></div><div class='col-md-3 col-xs-3'><span>Choice Days ₹</span><span><input type='number' id='cdays' name='cdays' placeholder='₹100' style='width:75%' value='' /></span></div></div><div class='row' align='center' style='padding-top:10px'><textarea id='pkgdesc' class='form-control' name='pkgdesc' placeholder='Enter Package Description' cols='3'></textarea></div></div></div><div class='package-list' ><button id='pillbutton' class='button pkgitem' value='Welcome_Drinks'>Welcome Drinks(0) </button><button id='pillbutton' class='button pkgitem' value='Starters'>Starters(0)</button><button id='pillbutton' class='button pkgitem' value='Rice'>Rice(0)</button><button id='pillbutton' class='button pkgitem' value='Bread'>Bread(0)</button><button id='pillbutton' class='button pkgitem' value='Curries'>Curries(0)</button><button id='pillbutton' class='button pkgitem' value='Fry_Dry'>Fry/Dry(0)</button><button id='pillbutton' class='button pkgitem' value='Salads'>Salads(0)</button><button id='pillbutton' class='button pkgitem' value='Soups'>Soups(0)</button><button id='pillbutton' class='button pkgitem' value='Deserts'>Deserts(0)</button><button id='pillbutton' class='button pkgitem' value='Beverages'>Beverages(0)</button><button id='pillbutton' class='button pkgitem' value='Fruits'>Fruits(0)</button></div><div align='right' class='pkgsave'><input type='hidden' class='pkgmenuitems' value='' /><input type='hidden' class='selpkgitems' /><input type='hidden' class='availablepkgitems' value='' /><a class='addcourse' style='padding: 17px;margin-top: 3px; color:#dc3545;cursor:pointer'>Add Course</a><a class='viewmenu' style='padding: 17px;margin-top: 3px; color:#dc3545;cursor:pointer'>View Menu</a><a class='pkgclone' style='padding: 17px;margin-top: 3px; color:#dc3545;cursor:pointer'>Duplicate</a><a class='addpkg' style='padding: 17px;margin-top: 3px; color:#dc3545; cursor: pointer;'>Publish</a><a href='#' style='padding: 17px;margin-top: 3px; color:#dc3545;cursor:pointer'>Remove</a></div></div></div>";
    $('#pkgsdiv').append(childdiv);
});

//Package Checkbox Selection
$('.slotcheck').on('change', function () {
    var thisdiv = $(this).parents("div.row");
    if (thisdiv.find("input[type='checkbox']").is(':checked')) {
        var favorite = [];
        $.each($("input[type='checkbox']:checked"), function () {
            favorite.push($(this).val());
            timeslot = favorite.join(", ");
        })
        $('#pkgslots').val(favorite);
    }
});

//Loading Menu on Load
var pkgheadings = 'Welcome Drinks,Starters,Rice,Bread,Curries,Fry/Dry,Salads,Soups,Deserts,Beverages,Fruits';
var totallist = '';//$('.pkgmenuitems').val();

//Add Course Button
$('.addcourse').click(function(){
    var parentdiv = $(this).parent('div.pkgsave').prev('div.package-list').find('div#extracourse');
    var btn = '<button id="pillbutton" class="button extraitem" value="">New Course(0)</button>';
    parentdiv.append(btn);
});

//Adding Selected Lists to Course
function Addpkgitems(mtype) {
    var type = $('#pkgmodal .selectedpkg').val();
    var maxcount = $('#pkgmodal .maxcount').val();
    var selecteditems = '';
    var itemstable = $('#pkgmodal').find('table.menuitems');
    var ctype = $('#pkgmodal .pkgitemsection p#ctype').text();
    $(itemstable).find('tr').each(function () {
        if ($(this).find("input[type='checkbox']").is(':checked')) {
            var checkText = $(this).find('label').text();
            if (selecteditems != '')
                selecteditems = selecteditems + '!' + checkText;
            else
                selecteditems = checkText;
        }
    });
    var availableitems = $('.availablepkgitems').val();
    if(availableitems == '' || availableitems == undefined) availableitems = $('.selpkgitems').val();
    var finallist=totallist.split(',');
    if ($('.pkgmenuitems').val() != '') {
        if (availableitems.indexOf(ctype) != -1) {
            for (var i = 0; i < availableitems.split(',').length; i++) {
                if (availableitems.split(',')[i] == ctype) {
                    finallist.push(ctype+'('+selecteditems+')');
                }
            }
            totallist = finallist.join(',');
        }
        else {
            finallist.push(ctype+'('+selecteditems+')');
            totallist= totallist = finallist.join(',');
        }
    }
    else {
        if (availableitems.indexOf(ctype) != -1) {
            for (var i = 0; i < availableitems.split(',').length; i++) {
                if (availableitems.split(',')[i] == ctype) {
                    finallist.push(ctype+'('+selecteditems+')');
                }
            }
            totallist = finallist.join(',');
        }
        else {
            finallist.push(ctype+'('+selecteditems+')');
            totallist= totallist +','+ finallist.join(',');
        }
    }
    $('.selpkgitems').val(totallist);
    if(mtype == 'old')
    {
        if ($(".package-list button[value = " + ctype.replace('/', '_') + "]").replaceWith('<button id="pillbutton" class="button pkgitem" value=' + ctype + '>' + type + '(' + maxcount + ')</button>'));
    }
    else
    {
        if ($(".package-list #extracourse button:contains(" + mtype + ")").replaceWith('<button id="pillbutton" class="button extraitem" value=' + type + '>' + type + '(' + maxcount + ')</button>'));
    }
}

// Loading Package Menu
$(document).on('click', '.pkgitem', function () {
    $('.overlay').show();
    $('#loadermsg').text("Fetching Menu Items");
    var checkbox = $("input[name='checkradio']:checked").val();
    var pkgid = $(this).parent("div#pkgsdiv").find("div.pkgsave").find("input.packageid").val();
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
        $('#pkgmodal .add').attr('onclick','Addpkgitems("old")');
        var exists = $(this).text().split('(')[1].split(')')[0];
        filteritems(checkbox, val.replace('_',' '),pkgid);
    }
});

// Loading Extra Menus
$(document).on('click', '.extraitem', function () {
    $('.overlay').show();
    $('#loadermsg').text("Fetching Menu Items");
    var checkbox = $("input[name='checkradio']:checked").val();
    var pkgid = $(this).parent("div#pkgsdiv").find("div.pkgsave").find("input.packageid").val();
    if (checkbox == undefined) {
        alert("Select Menu Type")
    }
    else {
        var val = $(this).text().split('(')[0];
        //var pkgid = $(this).text(); // Course name
        $('#pkgmodal .pkgitemsection #ctype').text(val);
        $('#pkgmodal .maxcount').val($(this).text().split('(')[1].split(')')[0]);
        $('#pkgmodal .selectedpkg').val(val);
        $('#pkgmodal .add').attr('onclick','Addpkgitems("'+val+'")');
        var exists = $(this).text().split('(')[1].split(')')[0];
        filteritems(checkbox, val,'New');
    }
});
//$('.writemsg.modal').modal('attach events', '.writemsg.button', 'show');

//Filtering Menu and loading menu items
function filteritems(type, selectedpkg,pkgid) {
    var selpkg = selectedpkg.replace(' ', '_').replace('/', '_').toLowerCase();
    var welcome_drinks = ""; var starters = ""; var rice = ""; var bread = ""; var curries = "";
    var fry_dry = ""; var salads = ""; var soups = ""; var tandoori_kababs = ""; var deserts = ""; var beverages = ""; var fruits = "";
    var tabletr = $('#pkgmodal .pkgitemsection table.menuitems'); var commondiv = "";
    var itemsdiv = $('#pkgmodal .pkgitemsection table.menuitems tr#pkgitemstr');
    tabletr.empty();

    $.get("/VDashboard/GetPackagemenuItem?menuitem=" + selectedpkg + "&&category=" + type + "&&vsid=" +subid+"",
        function (data) {
            commondiv = data;
            var getlist =  $('.selpkgitems').val() + ',' + $('.pkgmenuitems').val();
            //if(getlist == '' || getlist == undefined) getlist =$('.pkgmenuitems').val();
            for (var i = 0; i < totallist.split(',').length; i++) {
                if(totallist.split(',')[i] == type)
                    getlist = totallist.split(',')[i].split('(')[1].split(')')[0];
            }
            var newitem = '';
            if(pkgid == 'New')
                newitem='<tr class="newtr"><td><input type="textbox" class="form-control newitem" /><div align="center"><button class="btn btn-success sni">+ New Item</button>  <button class="btn btn-success saveall1">Save All</button></div></td></tr>';
            else
                newitem='<tr class="newtr"><td><input type="textbox" class="form-control newitem" /><div align="center"><button class="btn btn-success sni">+ New Item</button>  <button class="btn btn-success saveall">Save All</button></div></td></tr>';

            for (var i = 0; i < commondiv.split('!').length; i++) {
                var row = $("<tr/>");
                if (getlist != null && getlist!= ',') {
                    if (getlist.includes(commondiv.split('!')[i])) {
                        row.append('<td class="menuitems" style="padding-top:0"> <a title="Remove '+commondiv.split('!')[i]+'?" class="removemitem" style="color:red;cursor:pointer">X</a> <input type="checkbox" class="icheck" style="margin-left:5px;" checked id="'+commondiv.split('!')[i]+'"/>  <label for="'+commondiv.split('!')[i]+'">' + commondiv.split('!')[i] + '</label></td>');
                    }
                    else {
                        row.append('<td class="menuitems" style="padding-top:0"> <a title="Remove '+commondiv.split('!')[i]+'?" class="removemitem" style="color:red;cursor:pointer">X</a> <input type="checkbox" class="icheck" style="margin-left:5px;" id="'+commondiv.split('!')[i]+'"/>  <label for="'+commondiv.split('!')[i]+'">' + commondiv.split('!')[i] + '</label></td>');
                    }
                }
                else if(commondiv != ''){
                    row.append('<td class="menuitems" style="padding-top:0"> <a title="Remove '+commondiv.split('!')[i]+'?" class="removemitem" style="color:red;cursor:pointer">X</a> <input type="checkbox" style="margin-left:5px;" id="'+commondiv.split('!')[i]+'"/>  <label for="'+commondiv.split('!')[i]+'">' + commondiv.split('!')[i] + '</label></td>');
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
    var pkgcategory = $("input[name='checkradio']:checked").val();
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

    $.ajax({
        url:'/VDashboard/UpdateMenuItems',
        type: 'POST',
        data: JSON.stringify({ PackageMenu: PackageMenu, type: type }),
        dataType: 'json',
        contentType: 'application/json',
        success:function(data){
            $('.overlay').hide();
            alert(data + '!!!Select items and click Add');
            //$('#pkgmodal').modal('hide');
        }
    });
});

//Save All for Extra Items
$(document).on('click','.saveall1',function(){
    $('.overlay').show();
    $('#loadermsg').text("Updating items list...");
    var type = $('#pkgmodal .selectedpkg').val();
    var pkgcategory = $("input[name='checkradio']:checked").val();
    var totalitems = '';var favorite = [];
    $.each($('#pkgmodal .pkgitemsection table.menuitems input[type=checkbox]'), function () {
        favorite.push($(this).next("label").text());
        totalitems = favorite.join('!');
    });
    totalitems = type + '(' + totalitems + ')';
    //alert(totalitems);
    var PackageMenu = {
        VendorID: subid,
        VendorMasterID: id,
        Category: pkgcategory,
        Extra_Menu_Items:totalitems,
    }

    $.ajax({
        url:'/VDashboard/NewCourse',
        type: 'POST',
        data: JSON.stringify({ PackageMenu: PackageMenu, type: type }),
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
$('.viewmenu').click(function () {
    var packagename = $('#pkgname').val();
    var pkgcategory = $("input[name='checkradio']:checked").val();
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
$('.addpkg').click(function () {
    //validation('AddPackage');
    $('.overlay').show();
    $('#loadermsg').text("Saving Package...");
    var allbuttons = $(".package-list button").text();
    var list=[];
    for (var i = 0; i < allbuttons.split(')').length; i++) {
        if(allbuttons.split(')')[i] != '' && allbuttons.split(')')[i].split('(')[1]!=0)
            list.push(allbuttons.split(')')[i]+')');
    }
    var packagename = $('#pkgname').val();
    var pkgcategory = $("input[name='checkradio']:checked").val();
    var peakdays = $('#pdays').val();
    var normaldays = $('#ndays').val();
    var holidays = $('#holidays').val();
    var choicedays = $('#cdays').val();
    var timeslot = $('#pkgslots').val();
    var pkgdescription = $('#pkgdesc').val();
    var menuitems = $(this).parent('div').find('input.selpkgitems').val();//pkgdesc;
    var menu = list.join(','); //$(this).parent('div').find('input.selpkgitems').val();
    //alert(pkgdesc);
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
            url: '/VDashboard/AddPackage',
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
$('.updatepkg').click(function () {
    var menuitems='';
    //validation('UpdatePackage');
    $('.overlay').show();
    $('#loadermsg').text("Updating Package...");
    var allbuttons = $(".package-list button").text();
    var list=[];
    for (var i = 0; i < allbuttons.split(')').length; i++) {
        if(allbuttons.split(')')[i] != '' && allbuttons.split(')')[i].split('(')[1]!=0)
            list.push(allbuttons.split(')')[i]+')');
    }
    var allVal = '';
    $(".package-list button").each(function () {
        if($(this).text().split('(')[1].split(')')[0] != 0)
            allVal += ',' + $(this).val();
    });
    var allbuttonsvals = allVal.substring(1, allVal.length);
    var newlists = $('.selpkgitems').val().substring(1,$('.selpkgitems').val().length) +',' + $('.pkgmenuitems').val();
    var dblist = $('.pkgmenuitems').val();
    var selectedlist = $('.availablepkgitems').val();
    var splitteddblist = dblist.split(',');
    var a =[];var b=[];
    for (var i = 0; i < newlists.split(',').length; i++) {
        var thisval = newlists.split(',')[i].split('(')[0]
        if(thisval != '')
            a.push(thisval);
    }
    var availableitems = a;
    for (var i = 0; i < availableitems.length; i++) {
        var value = $.inArray(availableitems[i].replace('/','_'),selectedlist.split(','));
        if (b.indexOf(value) == -1) {
            if (value != -1) {
                b.push(value);
                splitteddblist[value] = newlists.split(',')[i];
            }
            else {
                splitteddblist.push(newlists.split(',')[i]);
            }
        }
    }
    menuitems =splitteddblist.join(',');
    var pkgid = $(this).prev('input.packageid').val();
    var packagename = $('#pkgname').val();
    var pkgcategory = $("input[name='checkradio']:checked").val();
    var peakdays = $('#pdays').val();
    var normaldays = $('#ndays').val();
    var holidays = $('#holidays').val();
    var choicedays = $('#cdays').val();
    var timeslot = $('#pkgslots').val();
    var pkgdescription = $('#pkgdesc').val();
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
    else {
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
        //alert(menuitems);
        $.ajax({
            url: '/VDashboard/UpdatePackage',
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

// Cloning Package
$(document).on('click', '.pkgclone', function () {
    $('.overlay').show();
    $('#loadermsg').text("Duplicating Package...");
    var totaldiv = $(this).parents('div.allpkgs');
    var allbuttons = totaldiv.find(".package-list button").text();
    var list = [];
    for (var i = 0; i < allbuttons.split(')').length; i++) {
        if (allbuttons.split(')')[i] != '' && allbuttons.split(')')[i].split('(')[1] != 0)
            list.push(allbuttons.split(')')[i] + ')');
    }
    var packagename = totaldiv.find('input#pkgname').val(); //$('#pkgname').val();
    var pkgcategory = totaldiv.find("input[name='checkradio']:checked").val();
    var peakdays = totaldiv.find('#pdays').val();
    var normaldays = totaldiv.find('#ndays').val();
    var holidays = totaldiv.find('#holidays').val();
    var choicedays = totaldiv.find('#cdays').val();
    var timeslot = totaldiv.find('#pkgslots').val();
    var pkgdescription = totaldiv.find('#pkgdesc').val();
    var menuitems = totaldiv.find('input.selpkgitems').val();//pkgdesc;
    if (menuitems == '') {
        menuitems = totaldiv.find('input.pkgmenuitems').val();
    }
    var menu = list.join(',');
    var thisdiv = $(this);
    var pkgid = '';
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
        menuitems: menuitems,
        menu: menu
    }
    $.ajax({
        url: '/vdb/DuplicatePackage',
        type: 'post',
        datatype: 'json',
        data: package,
        success: function (data) {
            if (data != '0') {
                pkgid = data;
                alert("Package Duplicated SuccessFully!!!");
                duplicatepkg(thisdiv, pkgid);
            }
            else { alert(data); }
            $('.overlay').hide();
        }
    });
});

function duplicatepkg(thisdiv, pkgid) {
    var parentdiv = document.getElementById("pkgsdiv");
    thisdiv.parents('div#pkgsdiv').clone().find("input.packageid").attr('value', pkgid).end().insertAfter(parentdiv);
}

// Removing Item from list
$(document).on('click','.removemitem',function(){
    $('.overlay').show();
    $('#loadermsg').text("Removing item from list...");
    var item = $(this).next("input").next("label").text();
    var selpkg = $('#pkgmodal .selectedpkg').val();
    var totalitems = '';
    var favorite = [];
    var pkgcategory = $("input[name='checkradio']:checked").val();
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
        url:'/VDashboard/UpdateMenuItems',
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
