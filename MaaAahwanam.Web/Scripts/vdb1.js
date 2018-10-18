var category = $('#category').val();
var subcategory = $('#subcategory').val();
var subid = $('#vendorsubid').val();
var id = $('#vendorid').val();

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

//Packages Section

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
    var btn = '<button id="pillbutton" class="button extraitem" value="">New Course(0)</button>';
    $("#extracourse").append(btn);
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
        if ($(".col-md-2-10 button[value = " + ctype.replace('/','_') + "]").replaceWith('<button class="button pkgitem" value='+ ctype +'>' + type + '(' + maxcount + ')</button>'));
    }
    else
    {
        if ($(".col-md-2-10 #extracourse button:contains(" + mtype + ")").replaceWith('<button class="button extraitem" value='+ type +'>' + type + '(' + maxcount + ')</button>'));
    }
}

// Loading Package Menu
$(document).on('click', '.pkgitem', function () {
    $('.overlay').show();
    $('#loadermsg').text("Fetching Menu Items");
    var checkbox = $("input[name='checkradio']:checked").val();
    var pkgid = $(this).parent("div.col-md-2-10").find("div.pkgsave").find("input.packageid").val();
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
    var pkgid = $(this).parent("div.col-md-2-10").find("div.pkgsave").find("input.packageid").val();
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
    var allbuttons = $(".col-md-2-10 button").text();
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
    var allbuttons = $(".col-md-2-10 button").text();
    var list=[];
    for (var i = 0; i < allbuttons.split(')').length; i++) {
        if(allbuttons.split(')')[i] != '' && allbuttons.split(')')[i].split('(')[1]!=0)
            list.push(allbuttons.split(')')[i]+')');
    }
    var allVal = '';
    $(".col-md-2-10 button").each(function() {
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
