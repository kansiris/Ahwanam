var pkgheadings = 'Welcome Drinks,Starters,Rice,Bread,Curries,Fry/Dry,Salads,Soups,Tandoori/Kababs,Deserts,Beverages,Fruits';
var pkgdesc = '';
$(function () {
    //var pkgheadings = 'Welcome Drinks,Starters,Rice,Bread,Curries,Fry/Dry,Salads,Soups,Tandoori/Kababs,Deserts,Beverages,Fruits';
    var cvalue = '';
    for (var i = 0; i < pkgheadings.split(',').length; i++) {
        cvalue = window.localStorage.getItem(pkgheadings.split(',')[i].toLowerCase());
        if (cvalue != null) {
            if ($(".col-md-2-10 button:contains(" + pkgheadings.split(',')[i] + ")").replaceWith('<button class="button pkgitem">' + pkgheadings.split(',')[i] + '(' + cvalue.split('!').length + ')</button>'));
            if (pkgdesc != '') {
                pkgdesc = pkgdesc + ',' + pkgheadings.split(',')[i] + '(' + cvalue + ')';
            }
            else {
                pkgdesc = pkgheadings.split(',')[i] + '(' + cvalue + ')';
            }
        }
    }
});
$('.addpkg').click(function () {
    var packagename = $(this).parent('div').parent('.col-md-2-10').prev('.col-md-2-9').children('form').children('table').find('tr').find('td:first').find('input');
    var pkgcategory = $("input[name='checkradio']:checked").val();
    var dayssection = $(this).parent('div').parent('.col-md-2-10').prev('.col-md-2-9').find('table').find('tr.pricestr');
    var peakdays = dayssection.find('td').find('input#pdays').val();
    var normaldays = dayssection.find('td').find('input#ndays').val();
    var holidays = dayssection.find('td').find('input#holidays').val();
    var choicedays = dayssection.find('td').find('input#cdays').val();
    if (packagename.val() == '') {
        $(packagename).focus();
        alert("Enter Package Name");
    }
    else if (pkgcategory == undefined) {
        alert("Select Package Category");
    }
    else if (peakdays == '') {
        $(peakdays).focus();
        alert("Enter Peak Days Price");
    }
    else if (normaldays == '') {
        $(normaldays).focus();
        alert("Enter Normal Days Price");
    }
    else if (holidays == '') {
        $(holidays).focus();
        alert("Enter Holidays Price");
    }
    else if (choicedays == '') {
        $(choicedays).focus();
        alert("Enter Choice Days Price");
    }
    else {
        var package = {
            PackageName: packagename.val(),
            Category: pkgcategory,
            PackageDescription: pkgdesc,
            peakdays: peakdays,
            normaldays: normaldays,
            holidays: holidays,
            choicedays: choicedays
        }
        $.ajax({
            url: '/VDashboard/AddPackage',
            type: 'post',
            datatype: 'json',
            data: package,
            success: function (data) {
                alert("Package Added Successfully");
            }
        });
    }

});

function Addpkgitems() {
    var type = $('#pkgmodal .selectedpkg').text();
    var selecteditems = '';
    var itemstable = $('#pkgmodal').find('table.menuitems');
    $(itemstable).find('tr').each(function () {
        if ($(this).find("input[type='checkbox']").is(':checked')) {
            var checkText = $(this).find('label').text();
            if (selecteditems != '')
                selecteditems = selecteditems + '!' + checkText;
            else
                selecteditems = checkText;
        }
    });
    window.localStorage.setItem(type.toLowerCase(), selecteditems);
    if ($(".col-md-2-10 button:contains(" + type + ")").replaceWith('<button class="button pkgitem">' + type + '(' + selecteditems.split('!').length + ')</button>'));
}

function orderemail(val) {
    //  alert(val);
    $('#Querymodel .custemail').text(val);
    $('#Querymodel #txtmsg').val('');
    $('#Querymodel').modal('show');
}

$(document).on('click', '.pkgitem', function () {
    var checkbox = $("input[name='checkradio']:checked").val();
    if (checkbox == undefined) {
        alert("Select Menu Type")
    }
    else {
        var val = $(this).text().split('(')[0];
        var exists = $(this).text().split('(')[1].split(')')[0];
        if (exists > 0) {
            filteritems(checkbox, val, '1');
        }
        else {
            filteritems(checkbox, val, '0');
        }
    }
});
//$('.writemsg.modal').modal('attach events', '.writemsg.button', 'show');

function filteritems(type, selectedpkg, checked) {
    var selpkg = selectedpkg.replace(' ', '_').replace('/', '_').toLowerCase();
    var welcome_drinks = ""; var starters = ""; var rice = ""; var bread = ""; var curries = "";
    var fry_dry = ""; var salads = ""; var soups = ""; var tandoori_kababs = ""; var deserts = ""; var beverages = ""; var fruits = "";
    var tabletr = $('#pkgmodal .pkgitemsection table.menuitems'); var commondiv = "";
    var itemsdiv = $('#pkgmodal .pkgitemsection table.menuitems tr#pkgitemstr');
    if (type == 'Veg') {
        if (selpkg == 'welcome_drinks') {
            welcome_drinks = "Hot Badam Milk,Cold Badam Milk,Mosambi Juice,Pineapple Juice,Water Melon,Black Grape,Orange Juice,Apple Juice,Fruit Juice,Lychee Punch,Mango Blossom,Orange Blossom,Sweet Lassi,Salted Lassi,Pink Lady,Strawberry Surprise,Kiwi Kiss,Passion Punch,Blue Passion,Misty Mint,Virgin Mojito,200 ML Water Bottle (Branded),Tea or Coffee,Any Soft Drinks (Branded)";
            commondiv = welcome_drinks;
        }
        if (selpkg == 'starters') {
            starters = "Hot Badam Milk,Cold Badam Milk,Mosambi Juice,Pineapple Juice,Water Melon,Black Grape,Orange Juice,Apple Juice,Fruit Juice,Lychee Punch,Mango Blossom,Orange Blossom,Sweet Lassi,Salted Lassi,Pink Lady,Strawberry Surprise,Kiwi Kiss,Passion Punch,Blue Passion,Misty Mint,Virgin Mojito,200 ML Water Bottle (Branded),Tea or Coffee,Any Soft Drinks (Branded)";
            commondiv = starters;
        }
        if (selpkg == 'rice') {
            rice = "Hot Badam Milk,Cold Badam Milk,Mosambi Juice,Pineapple Juice,Water Melon,Black Grape,Orange Juice,Apple Juice,Fruit Juice,Lychee Punch,Mango Blossom,Orange Blossom,Sweet Lassi,Salted Lassi,Pink Lady,Strawberry Surprise,Kiwi Kiss,Passion Punch,Blue Passion,Misty Mint,Virgin Mojito,200 ML Water Bottle (Branded),Tea or Coffee,Any Soft Drinks (Branded)";
            commondiv = rice;
        }
        if (selpkg == 'bread') {
            bread = "Hot Badam Milk,Cold Badam Milk,Mosambi Juice,Pineapple Juice,Water Melon,Black Grape,Orange Juice,Apple Juice,Fruit Juice,Lychee Punch,Mango Blossom,Orange Blossom,Sweet Lassi,Salted Lassi,Pink Lady,Strawberry Surprise,Kiwi Kiss,Passion Punch,Blue Passion,Misty Mint,Virgin Mojito,200 ML Water Bottle (Branded),Tea or Coffee,Any Soft Drinks (Branded)";
            commondiv = bread;
        }
        if (selpkg == 'curries') {
            curries = "Hot Badam Milk,Cold Badam Milk,Mosambi Juice,Pineapple Juice,Water Melon,Black Grape,Orange Juice,Apple Juice,Fruit Juice,Lychee Punch,Mango Blossom,Orange Blossom,Sweet Lassi,Salted Lassi,Pink Lady,Strawberry Surprise,Kiwi Kiss,Passion Punch,Blue Passion,Misty Mint,Virgin Mojito,200 ML Water Bottle (Branded),Tea or Coffee,Any Soft Drinks (Branded)";
            commondiv = curries;
        }
        if (selpkg == 'fry_dry') {
            fry_dry = "Hot Badam Milk,Cold Badam Milk,Mosambi Juice,Pineapple Juice,Water Melon,Black Grape,Orange Juice,Apple Juice,Fruit Juice,Lychee Punch,Mango Blossom,Orange Blossom,Sweet Lassi,Salted Lassi,Pink Lady,Strawberry Surprise,Kiwi Kiss,Passion Punch,Blue Passion,Misty Mint,Virgin Mojito,200 ML Water Bottle (Branded),Tea or Coffee,Any Soft Drinks (Branded)";
            commondiv = fry_dry;
        }
        if (selpkg == 'salads') {
            salads = "Carrot Salad,Green Salad,Ceaser Salad,Barley Salad,Sprouts Salad,Onion Salad,Green Bean Salad,Leafy Salad with nuts,Lintel Salad";
            commondiv = salads;
        }
        if (selpkg == 'soups') {
            soups = "Hot Badam Milk,Cold Badam Milk,Mosambi Juice,Pineapple Juice,Water Melon,Black Grape,Orange Juice,Apple Juice,Fruit Juice,Lychee Punch,Mango Blossom,Orange Blossom,Sweet Lassi,Salted Lassi,Pink Lady,Strawberry Surprise,Kiwi Kiss,Passion Punch,Blue Passion,Misty Mint,Virgin Mojito,200 ML Water Bottle (Branded),Tea or Coffee,Any Soft Drinks (Branded)";
            commondiv = soups;
        }
        if (selpkg == 'tandoori_kababs') {
            tandoori_kababs = "Hot Badam Milk,Cold Badam Milk,Mosambi Juice,Pineapple Juice,Water Melon,Black Grape,Orange Juice,Apple Juice,Fruit Juice,Lychee Punch,Mango Blossom,Orange Blossom,Sweet Lassi,Salted Lassi,Pink Lady,Strawberry Surprise,Kiwi Kiss,Passion Punch,Blue Passion,Misty Mint,Virgin Mojito,200 ML Water Bottle (Branded),Tea or Coffee,Any Soft Drinks (Branded)";
            commondiv = tandoori_kababs;
        }
        if (selpkg == 'deserts') {
            deserts = "Hot Badam Milk,Cold Badam Milk,Mosambi Juice,Pineapple Juice,Water Melon,Black Grape,Orange Juice,Apple Juice,Fruit Juice,Lychee Punch,Mango Blossom,Orange Blossom,Sweet Lassi,Salted Lassi,Pink Lady,Strawberry Surprise,Kiwi Kiss,Passion Punch,Blue Passion,Misty Mint,Virgin Mojito,200 ML Water Bottle (Branded),Tea or Coffee,Any Soft Drinks (Branded)";
            commondiv = deserts;
        }
        if (selpkg == 'beverages') {
            beverages = "Hot Badam Milk,Cold Badam Milk,Mosambi Juice,Pineapple Juice,Water Melon,Black Grape,Orange Juice,Apple Juice,Fruit Juice,Lychee Punch,Mango Blossom,Orange Blossom,Sweet Lassi,Salted Lassi,Pink Lady,Strawberry Surprise,Kiwi Kiss,Passion Punch,Blue Passion,Misty Mint,Virgin Mojito,200 ML Water Bottle (Branded),Tea or Coffee,Any Soft Drinks (Branded)";
            commondiv = beverages;
        }
        if (selpkg == 'fruits') {
            fruits = "Hot Badam Milk,Cold Badam Milk,Mosambi Juice,Pineapple Juice,Water Melon,Black Grape,Orange Juice,Apple Juice,Fruit Juice,Lychee Punch,Mango Blossom,Orange Blossom,Sweet Lassi,Salted Lassi,Pink Lady,Strawberry Surprise,Kiwi Kiss,Passion Punch,Blue Passion,Misty Mint,Virgin Mojito,200 ML Water Bottle (Branded),Tea or Coffee,Any Soft Drinks (Branded)";
            commondiv = fruits;
        }
        tabletr.empty();
        var getlist = window.localStorage.getItem(selectedpkg.toLowerCase());
        alert(getlist);
        for (var i = 0; i < commondiv.split(',').length; i++) {
            var row = $("<tr/>");
            if (getlist.includes(commondiv.split(',')[i])) {
                row.append('<td class="menuitems"><input type="checkbox" class="icheck" checked/><label>' + commondiv.split(',')[i] + '</label></td>');
            }
            else {
                row.append('<td class="menuitems"><input type="checkbox" class="icheck" /><label>' + commondiv.split(',')[i] + '</label></td>');
            }
            tabletr.append(row);
        }
        $('#pkgmodal .selectedpkg').text(selectedpkg);
        $('#pkgmodal').modal('show');
    }
}