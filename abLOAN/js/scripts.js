var Prefix = "cphPage_";
var DateFormat = 'dd/MM/yyyy';
var DateFormatWatermark = 'DD/MM/YYYY';
var ErrorMessage = "Your session may be expired or an error occured while retrieving data, Please refresh the page and try again.";

function OnGetFailure(error) {
    alert("Error: " + error._statusCode + " - " + error._message);
}
function SetDatePicker(controlId, minDate, maxDate, daysOfWeekDisabled) {
    $("#" + controlId).attr("data-mask", "99/99/9999");
    $("#" + controlId).attr("placeholder", DateFormatWatermark);
    $("#" + controlId).attr("autocomplete", "off");
    $("#" + controlId).datetimepicker({
        format: DateFormatWatermark,
        useCurrent: false,
        daysOfWeekDisabled: daysOfWeekDisabled
    }).on("dp.show", function () {
        if ($("#" + controlId).val() == "") {
            $("#" + controlId).data("DateTimePicker").date(null);
        }
    });

    if (minDate != null) {
        minDate.setHours(0, 0, 0, 0);
        $("#" + controlId).data("DateTimePicker").minDate(minDate);
    }
    if (maxDate != null) {
        maxDate.setHours(23, 59, 59, 999);
        $("#" + controlId).data("DateTimePicker").maxDate(maxDate);
    }
}
function SetMonthPicker(controlId, minDate, maxDate) {
    $("#" + controlId).attr("data-mask", "99/9999");
    $("#" + controlId).attr("placeholder", "MM/YYYY");
    $("#" + controlId).attr("autocomplete", "off");
    $("#" + controlId).datetimepicker({
        format: "MM/YYYY",
        useCurrent: false
    }).on("dp.show", function () {
        if ($("#" + controlId).val() == "") {
            $("#" + controlId).data("DateTimePicker").date(null);
        }
    });

    if (minDate != null) {
        minDate.setHours(0, 0, 0, 0);
        $("#" + controlId).data("DateTimePicker").minDate(minDate);
    }
    if (maxDate != null) {
        maxDate.setHours(23, 59, 59, 999);
        $("#" + controlId).data("DateTimePicker").maxDate(maxDate);
    }
}
function SetRangeDatePicker(fromControlId, toControlId, minDate, maxDate) {
    try {

        $("#" + fromControlId).attr("data-mask", "99/99/9999");
        $("#" + fromControlId).attr("placeholder", DateFormatWatermark);
        $("#" + fromControlId).attr("autocomplete", "off");
        $("#" + toControlId).attr("data-mask", "99/99/9999");
        $("#" + toControlId).attr("placeholder", DateFormatWatermark);
        $("#" + toControlId).attr("autocomplete", "off");

        $("#" + fromControlId).datetimepicker({
            format: DateFormatWatermark,
            useCurrent: false
        }).on("dp.show", function () {
            if ($("#" + fromControlId).val() == "") {
                $("#" + fromControlId).data("DateTimePicker").date(null);
            }
        });
        $("#" + toControlId).datetimepicker({
            format: DateFormatWatermark,
            useCurrent: false
        }).on("dp.show", function () {
            if ($("#" + toControlId).val() == "") {
                $("#" + toControlId).data("DateTimePicker").date(null);
            }
        });

        if (minDate != null) {
            minDate.setHours(0, 0, 0, 0);
            $("#" + fromControlId).data("DateTimePicker").minDate(minDate);
        }
        if (maxDate != null) {
            maxDate.setHours(23, 59, 59, 999);
            $("#" + fromControlId).data("DateTimePicker").maxDate(maxDate);
        }

        $("#" + toControlId).data("DateTimePicker").minDate(GetStringToDate($("#" + fromControlId).val()));

        $("#" + fromControlId).on("dp.change", function (e) {
            $("#" + toControlId).data("DateTimePicker").minDate(e.date);
            if (GetStringToDate($("#" + toControlId).val()) < e.date) {
                $("#" + toControlId).val($("#" + fromControlId).val());
            }
        });
        $("#" + toControlId).on("dp.change", function (e) {
            $("#" + fromControlId).data("DateTimePicker").maxDate(e.date);
        });
    }
    catch (ex) {
        alert(ex);
    }
}
function SetTimePicker(controlId) {
    //$("#" + controlId).attr("data-mask", "99:99");
    $("#" + controlId).attr("autocomplete", "off");
    $("#" + controlId).datetimepicker({
        format: 'hh:mm A',
        useCurrent: false
    }).on("dp.show", function () {
        if ($("#" + controlId).val() == "") {
            $("#" + controlId).data("DateTimePicker").date("12:00 A");
            $("#" + controlId).data("DateTimePicker").date(null);
        }
    });
}
function SetTimePicker24Hour(controlId) {
    $("#" + controlId).attr("data-mask", "99:99");
    $("#" + controlId).attr("placeholder", "HH:MM");
    $("#" + controlId).attr("autocomplete", "off");
    $("#" + controlId).datetimepicker({
        format: 'HH:mm',
        useCurrent: false
    }).on("dp.show", function () {
        if ($("#" + controlId).val() == "") {
            $("#" + controlId).data("DateTimePicker").date("00:00");
            $("#" + controlId).data("DateTimePicker").date(null);
        }
    });
}
function SetColorPicker(controlId) {
    $("#" + controlId).colorpicker({
        format: 'hex',
        colorSelectors: { 'black': '#000000', 'gray': '#808080', 'silver': '#C0C0C0', 'white': '#FFFFFF', 'primary': '#2196F3', 'info': '#00BCD4', 'success': '#4CAF50', 'warning': '#FF9800', 'danger': '#F44336' }
    }).on("changeColor", function () {
        $("#" + controlId.replace("txt", "lbl")).css("color", $("#" + controlId).val());
    }).on("showPicker", function () {
        if ($("#" + controlId).val() == "") {
            $("#" + controlId).colorpicker("setValue", "");
        }
    });
}
function SetSlider(controlId, minValue, maxValue, stepValue) {
    if ($("#" + controlId).val() == "") {
        $("#" + controlId).val(0);
    }
    var value = $("#" + controlId).val();
    var slider = $("#" + controlId).slider({
        step: stepValue,
        min: minValue,
        max: maxValue
    });
    slider.slider("setValue", value);
    $("#" + controlId.replace("txt", "lbl")).text(value);
    slider.on("slide", function (slideEvt) {
        $("#" + controlId.replace("txt", "lbl")).text(slideEvt.value);
    });
}
function SetRatingById(controlId) {
    var Target = controlId.replace("pnl", "hdn");
    $("#" + controlId).raty({
        readOnly: false,
        half: true,
        score: function () {
            return $(this).attr('data-score');
        },
        target: '#' + Target,
        targetType: 'score',
        targetKeep: true,
        starHalf: 'https://cdnjs.cloudflare.com/ajax/libs/raty/2.7.1/images/star-half.png',
        starOff: 'https://cdnjs.cloudflare.com/ajax/libs/raty/2.7.1/images/star-off.png',
        starOn: 'https://cdnjs.cloudflare.com/ajax/libs/raty/2.7.1/images/star-on.png'
    });
}
function SetRatingByClass(controlClass) {
    $("." + controlClass).raty({
        readOnly: true,
        score: function () {
            return $(this).attr('data-score');
        },
        starHalf: 'https://cdnjs.cloudflare.com/ajax/libs/raty/2.7.1/images/star-half.png',
        starOff: 'https://cdnjs.cloudflare.com/ajax/libs/raty/2.7.1/images/star-off.png',
        starOn: 'https://cdnjs.cloudflare.com/ajax/libs/raty/2.7.1/images/star-on.png'
    });
}
function SetFileUpload(controlId) {
    $("#" + controlId).fileinput({
        browseClass: "btn btn-default",
        browseLabel: "",
        removeLabel: "",
        showUpload: false,
        showPreview: false
    });
}
function SetImageUpload(controlId, isRequired) {
    $("#" + controlId).fileinput({
        browseClass: "btn btn-default",
        showUpload: false,
        showPreview: true,
        showClose: false,
        showCaption: false,
        browseOnZoneClick: true,
        defaultPreviewContent: '<img src="img/xs_NoImage.png" class="' + controlId + ' file-defaultpreview-img img_resize_fit">',
        frameClass: 'filepreview-frame img_resize_fit',
        layoutTemplates: { footer: '' }
    }).on("fileclear", function (event) {
        $("#" + controlId.replace("fu", "hdn")).val("");
        if (isRequired == true) {
            $("#" + controlId.replace("fu", "rfv")).prop("enabled", isRequired);
        }
    });
}
function InitBasicTinyMCE(controlId) {
    tinymce.init({
        selector: "#" + controlId,
        theme: "modern",
        menubar: false,
        statusbar: false,
        verify_html: false,
        relative_urls: false,
        content_css: 'https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.3.7/css/bootstrap.min.css',
        plugin_preview_width: screen.width - 200,
        plugins: [
            "advlist autolink link image lists charmap print preview hr anchor pagebreak",
            "searchreplace wordcount visualblocks visualchars code fullscreen insertdatetime media nonbreaking",
            "save table contextmenu directionality emoticons paste textcolor"
        ],
        toolbar: "undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image | forecolor backcolor | code",
        setup: function (editor) {
            editor.on("change", function () {
                editor.save();
            });
        }
    });
}
function GetDateToString(date) {
    if (date == null) {
        return "";
    }
    return date.format(DateFormat);
}
function GetTimeToString(time) {
    if (time == null) {
        return "";
    }
    var hour = time.Hours;
    var minute = time.Minutes;
    var tt;
    if (hour >= 12) {
        if (hour > 12) {
            hour = hour - 12;
        }
        tt = 'PM';
    }
    else {
        tt = 'AM';
    }

    return ToTwoDigit(hour) + ':' + ToTwoDigit(minute) + ' ' + tt;
}
function GetStringToDate(stringDate) {
    if (stringDate == "" || stringDate == null) {
        return;
    }
    var temp = stringDate.split('/');
    return new Date(temp[2], temp[1] - 1, temp[0]);
}
function ToTwoDigit(d) {
    d = parseInt(d.toString());
    return (d < 10) ? "0" + d.toString() : d.toString();
}
function GetRandomString(prefix, length, isAlpha, isNumeric) {
    var text = prefix;
    var chars = "";
    if (isAlpha) {
        var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    }
    if (isNumeric) {
        chars += "012345678901234567890123456789";
    }
    while (true) {
        if (text.length >= length) {
            break;
        }
        text += chars.charAt(Math.floor(Math.random() * chars.length));
        if (text == "0") {
            text = "";
        }
    }
    return text;
}
function ValidateTimeDefference(fromControlId, toControlId) {
    var start = $("#" + fromControlId).val();
    var end = $("#" + toControlId).val();

    if (start != "" && end != "") {
        var starttime = new Date("01/01/1753 " + start);
        var endtime = new Date("01/01/1753 " + end);

        if (starttime > endtime) {
            return false;
        }
    }
    return true;
}
function GetTimeDefference(fromControlId, toControlId) {
    var start = $("#" + fromControlId).val();
    var end = $("#" + toControlId).val();

    if (start != "" && end != "") {
        if (ValidateTimeDefference() == true) {
            var starttime = new Date("01/01/1753 " + start);
            var endtime = new Date("01/01/1753 " + end);

            var diff = endtime.getTime() - starttime.getTime();
            var hours = Math.floor(diff / 1000 / 60 / 60);
            diff -= hours * 1000 * 60 * 60;
            var minutes = Math.floor(diff / 1000 / 60);

            // If using time pickers with 24 hours format, add the below line get exact hours
            if (hours < 0)
                hours = hours + 24;

            var totalhours = (hours <= 9 ? "0" : "") + hours + ":" + (minutes <= 9 ? "0" : "") + minutes;
            return totalhours;
        }
    }
    else {
        return "";
    }
}
function ShowMessage() {
    var AlertMessage = $("#pnlMessage");
    if (AlertMessage.length != 0) {
        var Message = $("#lblMessage").text();
        var Icon = $("#lblIcon").attr("class");
        var Type = "";
        var Placement = "top";
        if (AlertMessage.attr("class").indexOf("info") != -1) {
            Type = "info";
        } if (AlertMessage.attr("class").indexOf("warning") != -1) {
            Type = "warning";
        } else if (AlertMessage.attr("class").indexOf("danger") != -1) {
            Type = "danger";
        } else if (AlertMessage.attr("class").indexOf("success") != -1) {
            //Placement = "bottom";
            Type = "success";
        }
        $.notify({
            // options
            icon: Icon,
            message: Message
        }, {
            // settings
            type: Type,
            allow_dismiss: true,
            placement: {
                from: Placement
            },
            template: '<div data-notify="container" class="col-xs-11 col-sm-3 notify alert alert-{0}" role="alert">' +
		        '<button type="button" aria-hidden="true" class="close" data-notify="dismiss">&times;</button>' +
		        '<span class="notify-icon" data-notify="icon"></span> ' +
		        '<span data-notify="title"><div class="notify-title">{1}</div></span> ' +
		        '<span data-notify="message"><div class="notify-message">{2}</div></span>' +
		        '<div class="progress" data-notify="progressbar">' +
			        '<div class="progress-bar progress-bar-{0}" role="progressbar" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100" style="width: 0%;"></div>' +
		        '</div>' +
		        '<a href="{3}" target="{4}" data-notify="url"></a>' +
	        '</div>'
        });
    }
}
function BlockElement() {
    $.blockUI.defaults.css = {};
    $(".blockui").block({
        message: "<div class='message' style='width: 150px;'><img src='../img/loader.gif' title='Please wait...'></img>&nbsp;&nbsp;&nbsp;<b>Please wait...</b></div>",
        overlayCSS: { backgroundColor: '#FFFFFF' }
    });
}
function UnblockElement() {
    $(".blockui").unblock();
}
function BlockElementById(elementid, message) {
    $.blockUI.defaults.css = {};
    var Message = "<div class='message' style='width: 150px;'><img src='../img/loader.gif' title='Please wait...'></img>&nbsp;&nbsp;&nbsp;<b>Please wait...</b></div>";
    if (message != null) {
        Message = "<div class='message'><b>" + message + "</b></div>";
    }
    $("#" + elementid).block({
        message: Message,
        overlayCSS: { backgroundColor: '#FFFFFF' }
    });
}
function UnblockElementById(elementid) {
    $("#" + elementid).unblock();
}
function SetFilter() {
    ShowHideFilter("divFilter", true);
}
function ShowHideFilter(elementid, isReset) {
    try {
        var hdnFilter = $("#hdnFilter");
        var aFilter = $("#aFilter");
        if (isReset) {
            if (hdnFilter.val() == 1) {
                $("#" + elementid).css("display", "block");
                var FilterText = aFilter.html().replace("<span class=\"caret\"></span>", "<span class=\"dropup\"><span class=\"caret\"></span></span>");
                aFilter.html(FilterText);
            }
            return false;
        }
        if (hdnFilter.val() == 0) {
            $("#" + elementid).slideDown();
            hdnFilter.val(1);
            var FilterText = aFilter.html().replace("<span class=\"caret\"></span>", "<span class=\"dropup\"><span class=\"caret\"></span></span>");
            aFilter.html(FilterText);
        }
        else {
            $("#" + elementid).slideUp();
            hdnFilter.val(0);
            var FilterText = aFilter.html().replace("<span class=\"dropup\"><span class=\"caret\"></span></span>", "<span class=\"caret\"></span>");
            aFilter.html(FilterText);
        }
        return false;
    }
    catch (ex) {
        alert(ex);
        return false;
    }
}
function SetOption() {
    ShowHideOption("divOption", true);
}
function ShowHideOption(elementid, isReset) {
    try {
        var hdnFilter = $("#hdnOption");
        var aFilter = $("#aOption");
        if (isReset) {
            if (hdnFilter.val() == 1) {
                if ($("#" + elementid).length != 0) {
                    $("#" + elementid).css("display", "block");
                }
                aFilter.html('<span class="dropup"><span class="caret"></span></span>');
            }
            return false;
        }
        if (hdnFilter.val() == 0) {
            $("#" + elementid).slideDown();
            hdnFilter.val() = 1;
            aFilter.html('<span class="dropup"><span class="caret"></span></span>');
        }
        else {
            $("#" + elementid).slideUp();
            hdnFilter.val() = 0;
            aFilter.html('<span class="caret"></span>');
        }
        return false;
    }
    catch (ex) {
        alert(ex);
        return false;
    }
}
function ShowHideElement(obj, elementid) {
    var hdnShowHideObject = $("#hdnShowHideObject");
    var hdnShowHideElementId = $("#hdnShowHideElementId");
    if (obj != null && $("#" + elementid).css("display") == "none") {
        $("#" + elementid).show("slow");
        $("#" + obj.id).html('<i class="fa fa-minus-square-o fa-lg"></i>');
        hdnShowHideObject.val(obj.id);
        hdnShowHideElementId.val(elementid);
    }
    else if (obj == null && hdnShowHideObject.val() != "") {
        $("#" + hdnShowHideElementId.val()).show("slow");
        $("#" + hdnShowHideObject.val()).html('<i class="fa fa-minus-square-o fa-lg"></i>');
    }
    else {
        if (obj != null) {
            $("#" + elementid).hide();
            $("#" + obj.id).html('<i class="fa fa-plus-square-o fa-lg"></i>');
        }
        hdnShowHideObject.val("");
        hdnShowHideElementId.val("");
    }
    return false;
}
function CheckSpecialKeys(e) {
    if (e.keyCode != 8 && e.keyCode != 46 && e.keyCode != 37 && e.keyCode != 38 && e.keyCode != 39 && e.keyCode != 40) {
        return false;
    }
    else {
        return true;
    }
}
function CheckTextAreaMaxLength(textbox, e, length) {
    var maxlength = parseInt(length);

    if (!CheckSpecialKeys(e)) {
        if (textbox.value.length > maxlength - 1) {
            if (window.event) {
                e.returnValue = false;
            }
            else {
                e.preventDefault();
            }
        }
    }
}
function ListHeaderCheckChanged(object, controlId, headerControlId, selectControlId) {
    try {
        if (headerControlId == null) {
            headerControlId = "chkHeader";
        }
        if (selectControlId == null) {
            selectControlId = "chkSelect";
        }
        var TargetBaseControl = $("#" + object.id.replace(headerControlId, controlId));
        var Inputs = TargetBaseControl.find("input");

        for (var i = 0; i < Inputs.length; i++) {
            if (Inputs[i].type == 'checkbox' && Inputs[i].disabled == false) {
                if (selectControlId == null || Inputs[i].id.indexOf(selectControlId + "_") != -1) {
                    Inputs[i].checked = object.checked;
                }
            }
        }
    }
    catch (ex) {
        alert(ex);
    }
}
function ListSelectCheckChanged(object, controlId, headerControlId, selectControlId) {
    try {
        var TargetBaseControl = "";
        if (headerControlId == null) {
            headerControlId = "chkHeader";
        }
        if (selectControlId == null) {
            selectControlId = "chkSelect";
        }
        var HeaderControlId = "";
        if (selectControlId == null) {
            var prefix = object.id.substring(0, object.id.indexOf(selectControlId));
            TargetBaseControl = $("#" + prefix + controlId);
            HeaderControlId = prefix + headerControlId;
        }
        else {
            TargetBaseControl = $("#" + object.id.replace(selectControlId, controlId));
            HeaderControlId = object.id.replace(selectControlId, headerControlId);
        }

        var Inputs = TargetBaseControl.find("input");

        var AllSelectCheckBoxChecked = true;
        for (var i = 0; i < Inputs.length; i++) {
            if (Inputs[i].id.indexOf(headerControlId + "_") == -1 && Inputs[i].type == 'checkbox' && Inputs[i].checked == false && Inputs[i].disabled == false) {
                AllSelectCheckBoxChecked = false;
            }
        }

        $("#" + HeaderControlId).prop("checked", AllSelectCheckBoxChecked);
    }
    catch (ex) {
        alert(ex);
    }
}
function ListSelectRowCheckBoxes(object, rowId, selectControlId) {
    try {
        if (selectControlId == null) {
            selectControlId = "chkSelect";
        }

        var TargetBaseControl = $("#" + object.id.replace(selectControlId, rowId));
        var Inputs = TargetBaseControl.find("input");

        for (var i = 0; i < Inputs.length; i++) {
            if (Inputs[i].id != object.id && Inputs[i].type == 'checkbox' && Inputs[i].disabled == false) {
                Inputs[i].checked = object.checked;
            }
        }
    }
    catch (ex) {
        alert(ex);
    }
}
function ListEnableDisableRowCheckBoxes(object, rowId, selectControlId) {
    try {
        if (selectControlId == null) {
            selectControlId = "chkSelect";
        }

        var TargetBaseControl = $("#" + object.id.replace(selectControlId, rowId));
        var Inputs = TargetBaseControl.find("input");

        for (var i = 0; i < Inputs.length; i++) {
            if (Inputs[i].id != object.id && Inputs[i].type == 'checkbox') {
                if (object.checked == false) {
                    Inputs[i].checked = false
                    Inputs[i].disabled = true;
                }
                else {
                    Inputs[i].disabled = false;
                }
            }
        }
    }
    catch (ex) {
        alert(ex);
    }
}
function ListEnableDisableRowTextBoxes(object, rowId, selectControlId) {
    
    try {
        if (selectControlId == null) {
            selectControlId = "chkSelect";
        }

        var TargetBaseControl = $("#" + object.id.replace(selectControlId, rowId));
        var Inputs = TargetBaseControl.find("input");
        var InputsMultiple = TargetBaseControl.find("textarea");
        var Selects = TargetBaseControl.find("select");

        for (var i = 0; i < Inputs.length; i++) {
            if (Inputs[i].id != object.id  ) {
                if (object.checked == false) {                   
                    Inputs[i].disabled = true;                    
                   // Inputs[i].value = "";

                    //Selects[i].attr('disabled', true);
                }
                else {
                    Inputs[i].disabled = false;
                    //Selects[i].attr('disabled', false);
                }
            }
        }
        for (var i = 0; i < InputsMultiple.length; i++) {
            if (InputsMultiple[i].id != object.id) {
                if (object.checked == false) {
                    InputsMultiple[i].disabled = true;                   
                }
                else {
                    InputsMultiple[i].disabled = false;                    
                }
            }
        }
    }
    catch (ex) {
        alert(ex);
    }
}

function ConfirmDelete(object, title) {
    try {
        var disabled = object.getAttribute("disabled");
        if (disabled == "disabled") {
            return false;
        }
        if (title != null && title.length > 0) {
            return confirm("Delete '" + title + "'?");
        }
        else {
            var msg = '<%= GetGlobalResourceObject("Resource", "btnDelete") %>';
            return confirm(msg);
        }
        return false;
    }
    catch (ex) {
        alert(ex);
        return false;
    }
}
function ConfirmDeleteSelected(object, ListId) {
    try {
        var disabled = object.getAttribute("disabled");
        if (disabled == "disabled") {
            return false;
        }
        var ControlId = Prefix + ListId + "_itemPlaceholderContainer";
        var SelectedCheckboxCount = CountSelectedCheckbox(ControlId);
        if (SelectedCheckboxCount == 0) {
            alert("Please select atleast one checkbox from list.");
            return false;
        }
        return confirm("Delete " + SelectedCheckboxCount + " selected record(s)?");
    }
    catch (ex) {
        alert(ex);
        return false;
    }
}
function ConfirmSelected(object, ListId) {
    try {
        var disabled = object.getAttribute("disabled");
        if (disabled == "disabled") {
            return false;
        }
        var ControlId = Prefix + ListId + "_itemPlaceholderContainer";
        var SelectedCheckboxCount = CountSelectedCheckbox(ControlId);
        if (SelectedCheckboxCount == 0) {
            alert("Please select atleast one checkbox from list.");
            return false;
        }
        return true;
    }
    catch (ex) {
        alert(ex);
        return false;
    }
}
function CountSelectedCheckbox(controlId) {
    try {
        var TargetBaseControl = $("#" + controlId);
        var TargetChildControl = "chkSelect";

        var SelectCheckBoxChecked = 0;

        var Inputs = TargetBaseControl.find("input");
        for (var i = 0; i < Inputs.length; i++) {
            if (Inputs[i].id.indexOf("chkHeader") == -1 && Inputs[i].type == 'checkbox' && Inputs[i].checked == true) {
                SelectCheckBoxChecked++;
            }
        }
        return SelectCheckBoxChecked;
    }
    catch (ex) {
        alert(ex);
        return 0;
    }
}
function CountTotalCheckbox(controlId) {
    try {
        var TargetBaseControl = $("#" + controlId);
        var TargetChildControl = "chkSelect";

        var SelectCheckBox = 0;

        var Inputs = TargetBaseControl.find("input");
        for (var i = 0; i < Inputs.length; i++) {
            if (Inputs[i].id.indexOf("chkHeader") == -1 && Inputs[i].type == 'checkbox') {
                SelectCheckBox++;
            }
        }
        return SelectCheckBox;
    }
    catch (ex) {
        alert(ex);
        return 0;
    }
}
function ValidateCheckBoxList(checkBoxListId) {
    var CheckBoxList = $("#" + checkBoxListId);
    var Inputs = CheckBoxList.find("input");
    var IsValid = false;
    for (var i = 0; i < Inputs.length; i++) {
        if (Inputs[i].type == 'checkbox' && Inputs[i].disabled == false && Inputs[i].checked) {
            IsValid = true;
            break;
        }
    }
    return IsValid;
}
function ValidateCheckBoxListGroup(controlClass) {
    var CheckBoxList = $("." + controlClass);
    var Inputs = CheckBoxList.find("input");
    var IsValid = false;
    for (var i = 0; i < Inputs.length; i++) {
        if (Inputs[i].type == 'checkbox' && Inputs[i].disabled == false && Inputs[i].checked) {
            IsValid = true;
            break;
        }
    }
    return IsValid;
}
function CheckUncheckCheckBoxList(checkBoxListId, value) {
    var CheckBoxList = $("#" + checkBoxListId);
    var Inputs = CheckBoxList.find("input");

    for (var i = 0; i < Inputs.length; i++) {
        if (Inputs[i].type == 'checkbox' && Inputs[i].disabled == false) {
            Inputs[i].checked = value;
        }
    }
}
function CheckUncheckCheckBoxListGroup(controlClass, value) {

    var CheckBoxList = $("." + controlClass);
    var Inputs = CheckBoxList.find("input");

    for (var i = 0; i < Inputs.length; i++) {
        if (Inputs[i].type == 'checkbox' && Inputs[i].disabled == false) {
            Inputs[i].checked = value;
        }
    }
}
function EnableDisableCheckBoxList(checkBoxListId, value) {
    var CheckBoxList = $("#" + checkBoxListId);
    var Inputs = CheckBoxList.find("input");

    for (var i = 0; i < Inputs.length; i++) {
        if (Inputs[i].type == 'checkbox') {
            if (value == false) {
                Inputs[i].checked = false
                Inputs[i].disabled = true;
            }
            else {
                Inputs[i].disabled = false;
            }
        }
    }
}
function ValidateImage(controlId) {
    if (!ValidateFile(controlId)) {
        return false;
    }
    var fuControl = $("#" + controlId);
    var imageExtension = ['jpeg', 'jpg', 'png', 'gif', 'bmp'];
    if ($.inArray($("#" + controlId).val().split('.').pop().toLowerCase(), imageExtension) == -1) {
        return false;
    }
    else {
        return true;
    }
}
function ValidateFile(controlId) {
    var FileUploadLimit = 8388608;
    var fuControl = $("#" + controlId);
    if (fuControl[0].files[0].size > FileUploadLimit) {
        alert("Maximum file upload size is " + (FileUploadLimit / 1024) / 1024 + " MBs.");
        return false;
    }
    return CheckInvalidExtension($("#" + controlId).val().split('.').pop());
}
function ValidateRating(controlId) {
    var Target = controlId.replace("pnl", "hdn");
    if ($("#" + Target).val() == 0) {
        return false;
    }
    else {
        return true;
    }
}
function CheckInvalidExtension(extension) {
    var fileExtension = ['ADE', 'ADP', 'BAT', 'CHM', 'CMD', 'COM', 'CPL', 'DLL', 'EXE', 'HTA', 'INS', 'ISP', 'JAR', 'JS', 'JSE'
    , 'LIB', 'LNK', 'MDE', 'MSC', 'MSI', 'MSP', 'MST', 'NSH', 'PIF', 'SCR', 'SCT', 'SHB', 'SYS', 'VB', 'VBE', 'VBS', 'VXD', 'WSC', 'WSF', 'WSH'];
    if ($.inArray(extension.toUpperCase(), fileExtension) > -1) {
        return false;
    }
    else {
        return true;
    }
}
function TreeviewExpandCollapseAll(treeViewId, isExpand) {
    var displayState = (isExpand == true ? "none" : "block");
    var treeView = document.getElementById(treeViewId);
    var treeLinks = treeView.getElementsByTagName("a");
    var nodeCount = treeLinks.length;
    var flag = true; for (i = 0; i < nodeCount; i++) {
        if (treeLinks[i].firstChild.tagName) {
            if (treeLinks[i].firstChild.tagName.toLowerCase() == "img") {
                var node = treeLinks[i];
                var level = parseInt(treeLinks[i].id.substr(treeLinks[i].id.length - 1), 10);
                var childContainer = GetTreeviewParentByTagName("table", node).nextSibling;
                if (flag) {
                    if (childContainer.style.display == displayState) {
                        TreeView_ToggleNode(eval(treeViewId + "_Data"), level, node, 'r', childContainer);
                        if (isExpand) {
                            treeLinks[i].firstChild.src = "img/minus.png";
                        }
                        else {
                            treeLinks[i].firstChild.src = "img/plus.png";
                        }
                    }
                    flag = false;
                }
                else {
                    if (childContainer.style.display == displayState) {
                        TreeView_ToggleNode(eval(treeViewId + "_Data"), level, node, 'l', childContainer);
                        if (isExpand) {
                            treeLinks[i].firstChild.src = "img/minus.png";
                        }
                        else {
                            treeLinks[i].firstChild.src = "img/plus.png";
                        }
                    }
                }
            }
        } //for loop ends
    }
} //utility function to get the container of an element by tagname
function GetTreeviewParentByTagName(parentTagName, childElementObj) {
    var parent = childElementObj.parentNode;
    while (parent.tagName.toLowerCase() != parentTagName.toLowerCase()) {
        parent = parent.parentNode;
    }
    return parent;
}
function SelectTreeviewCheckbox(TreeViewId, value, isCheck, isDisable) {
    var divTreeView = document.getElementById(Prefix + TreeViewId);
    var Inputs = divTreeView.getElementsByTagName("input");
    for (var i = 0; i < Inputs.length; i++) {
        if (Inputs[i].type == "checkbox") {
            var aPrefix = Inputs[i].id.substring(Inputs[i].id.indexOf(TreeViewId) + TreeViewId.length + 1, Inputs[i].id.indexOf("CheckBox"));
            var node = document.getElementById(Prefix + TreeViewId + "t" + aPrefix);
            var nodeValue = "";
            var nodePath = node.href.substring(node.href.indexOf(",") + 2, node.href.length - 2);
            var nodeValues = nodePath.split("\\");
            if (nodeValues.length > 1) {
                nodeValue = nodeValues[nodeValues.length - 1];
            }
            else {
                nodeValue = nodeValues[0].substr(1);
            }
            if (nodeValue == value) {
                if (isDisable != null) {
                    Inputs[i].disabled = isDisable;
                }
                if (Inputs[i].disabled == false) {
                    if (isCheck == null) {
                        Inputs[i].checked = !Inputs[i].checked;
                    }
                    else {
                        Inputs[i].checked = isCheck;
                    }
                }
                return;
            }
        }
    }
}
function GetNodeValue(TreeViewId, node) {
    var nodeValue = "";
    var nodePath = node.href.substring(node.href.indexOf(",") + 2, node.href.length - 2);
    var nodeValues = nodePath.split("\\");
    if (nodeValues.length > 1)
        nodeValue = nodeValues[nodeValues.length - 1];
    else
        nodeValue = nodeValues[0].substr(1);
    return nodeValue;
}