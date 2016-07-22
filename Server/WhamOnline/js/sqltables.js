function stripString(x) {
    return x.replace(/^\s+|\s+$/gm, "");
}

function addToTable() {
    var tableName = $("#newTableName").val();
    var isVisible = $("#newTableVisibility").val();
    tableName = stripString(tableName);
    tableName = tableName.replace(" ", "_");
    console.log(tableName, isVisible);
    if (tableName != "") {
        var table = {};
        table.TableName = tableName;
        table.isVisible = isVisible;
        table.fields = [];

        /* TEMP - Testing data by prepopulating elements
        table.fields[0] = {}
        table.fields[0].type = "String"
        table.fields[0].name = "First String"
        table.fields[0].uiType = "textbox"
        table.fields[0].defaultValue = "default string"
        table.fields[0].regex = "\\s+"
        table.fields[0].isAuth = "true"

        table.fields[1] = {}
        table.fields[1].type = "Ref"
        table.fields[1].name = "Name of single ref"
        table.fields[1].refTable = "singleTable"

        table.fields[3] = {}
        table.fields[3].type = "DateTime"
        table.fields[3].name = "Name of Date Time Entry"
        table.fields[3].uiType = "label"

        table.fields[4] = {}
        table.fields[4].type = "Integer"
        table.fields[4].name = "Name of Integer Entry"
        table.fields[4].uiType = "textbox"
        // TEMP */

        listOfTables.push(table);

        var listString = "<li class=\"collection-item\"" +
            "id=\"" + tableName + "_tableID\" " +
            "onclick=\"tableSelected('" + tableName + "')\">";
        listString += tableName;
        listString += "</li>";

        var x = document.getElementById("refConnection");
        var option = document.createElement("option");
        option.value = tableName;
        option.text = tableName;
        var sel = x.options[x.length];

        $(listString).appendTo("#tableListing");
        x.add(option, sel);

        console.log(x);
        console.log(option);
        $("select").material_select();
    }
    else {
        Materialize.toast("Please add a table name", 4000);
    }

    // reset footer-modal for next table addition
    $("#newTableName").val("");
    $("#newTableVisibility").val("on");
}

function cleanFieldListing() {
    // delete all elements currently listed in previous table
    $("#fieldListing").empty();
}

function populateFieldListing(tableName) {
    var myTable = {};
    for (var i = 0; i < listOfTables.length; i += 1) {
        if (listOfTables[i].TableName == tableName) {
            myTable = listOfTables[i];
        }
    }
    populateWithNewTable(myTable.fields);
}

function tableSelected(tableName) {
    console.log(tableName + " was selected");
    console.log(listOfTables); // removing the disabled part of the button
    $("#addFieldButton").show();
    $("#addFieldButton").removeClass("disabled");

    // change table name in title of next row
    $("#tableInDisplay").html("Table: " + tableName);

    for (var i = 0; i < listOfTables.length; i++) {
        //console.log((listOfTables[i].TableName + "_tableID"))
        $("#" + listOfTables[i].TableName + "_tableID").addClass("collection-item");
    }
    $("#" + tableName + "_tableID").addClass("active");

    cleanFieldListing();
    populateFieldListing(tableName);
}

function populateWithNewTable(fields) // fields is an array of objects
{
    for (var i = 0; i < fields.length; i += 1) {
        var field = fields[i];
        appendFieldToList(field);
    }
}

function appendFieldToList(field) // field is an object
{
    var listString = "";
    listString += "<li class='collection-item'>";
    listString += "<div class='row'>";

    listString += "<div class='col s12'>";
    listString += "<h5>" + field.name + "</h5>";
    listString += "</div>";

    switch (field.type) {
        case "string":
            listString += "<div class='col s12 m6 l3'>";
            listString += "UI Type: " + field.uiType;
            listString += "</div>";
            listString += "<div class='col s12 m6 l3'>";
            listString += "Default Value: " + field.defaultValue;
            listString += "</div>";
            listString += "<div class='col s12 m6 l3'>";
            listString += "Regex: " + field.regex;
            listString += "</div>";
            listString += "<div class='col s12 m6 l3'>";
            listString += "isAuth: " + field.isAuth;
            listString += "</div>";
            break;

        case "Ref":
            listString += "<div class='col s12 m6 l3'>";
            listString += "ref table: " + field.refTable;
            listString += "</div>";
            break;

        case "DateTime":
            listString += "<div class='col s12 m6 l3'>";
            listString += "UI Type: " + field.uiType;
            listString += "</div>";
            break;

        case "int":
            listString += "<div class='col s12 m6 l3'>";
            listString += "UI Type: " + field.uiType;
            listString += "</div>";

        case "double":
            listString += "<div class='col s12 m6 l3'>";
            listString += "UI Type: " + field.uiType;
            listString += "</div>";
            break;

        case "bool":
            listString += "<div class='col s12 m6 l3'>";
            listString += "UI Type: " + field.uiType;
            listString += "</div>";
            listString += "<div class='col s12 m6 l3'>";
            listString += "Default Value: " + field.defaultValue;
            listString += "</div>";
            break;
    }
    listString += "</div>";
    listString += "</li>";
    $(listString).appendTo("#fieldListing");
}


function typeChanged() {
    var type = $("#fieldType").val();
    console.log("Field type changed: " + type);

    $("#div_isAuth").hide();
    $("#div_field_name").hide();
    $("#div_defaultValue").hide();
    $("#div_regex").hide();
    $("#div_UI_Type").hide();
    $("#div_refConnection").hide();
    $("#div_field_isCollection").hide();

    if (type && type !== "") {
        $("#div_field_name").show();
        $("#div_defaultValue").show();
        $("#div_UI_Type").show();
        $("#div_field_isCollection").show();
    }

    switch (type) {
        case "string":
            $("#div_isAuth").show();
            $("#div_regex").show();
            break;
        case "Ref":
            $("#div_field_name").show();
            $("#div_defaultValue").hide();
            $("#div_UI_Type").hide();
            $("#div_refConnection").show();
            break;
        case "DateTime":
            break;
        case "int":
            break;
        case "double":
            break;
        case "bool":
            break;
    }
}

function cleanFieldInputModal() {
    // clean options' values
    $("#fieldType").val("");
    $("#field_name").val("");
    $("#isAuth").val(false);
    $("#UI_Type").val("");
    $("#defaultValue").val("");
    $("#field_isCollection").val(false);
    $("#regex").val("");
    $("#refConnection").val("");
    // hide all options
    $("#div_isAuth").hide();
    $("#div_field_name").hide();
    $("#div_defaultValue").hide();
    $("#div_regex").hide();
    $("#div_UI_Type").hide();
    $("#div_refConnection").hide();
}

function addThisField() {
    var type = $("#fieldType").val();
    var fieldName = $("#field_name").val();
    var isAuth = $("#isAuth").val();
    var uiType = $("#UI_Type").val();
    var defaultValue = $("#defaultValue").val();
    var isCollection = $("#field_isCollection").val();
    var regex = $("#regex").val();
    var refC = $("#refConnection")[0];
    var refConnection = "";

    for (var i = 0; i < refC.options.length; i++) {
        if (refC.options[i].selected == true) {
            refConnection = refC.options[i].value;
            break;
        }
    }

    if (!(fieldName) || fieldName.length === 0) {
        Materialize.toast("No field was added, unknown type: " + type, 3000);
        cleanFieldInputModal();
        return;
    }

    var rowData = {};

    rowData.type = type;
    rowData.name = fieldName;
    rowData.uiType = uiType;
    rowData.defaultValue = defaultValue;
    rowData.isCollection = isCollection;
    delete rowData.isAuth;
    delete rowData.refTable;
    // this switch is over Constants.DataTypes in the API Models folder
    switch (type) {
        case "bool":
            // TODO validation
            break;
        case "string":
            // TODO validation
            rowData.regex = regex;
            rowData.isAuth = isAuth == "true" || isAuth === "on";
            break;
        case "Ref":
            // TODO validation
            rowData.refTable = refConnection;
            break;
        case "DateTime":
            // TODO validation
            break;
        case "int":
            // TODO validation
            break;
        case "double":
            // TODO validation
            break;
        default:
            Materialize.toast("No field was added, unknown type: " + type, 3000);
            cleanFieldInputModal();
            return;
    }

    var tableName = $("#tableInDisplay").html();
    if (tableName.includes("Table:")) {
        tableName = tableName.split(" ")[1];
        console.log(tableName);
        for (var i = 0; i < listOfTables.length; i += 1) {
            if (listOfTables[i].TableName == tableName) {
                listOfTables[i].fields.push(rowData);
                cleanFieldListing(tableName);
                populateFieldListing(tableName);
            }
        }
    }
    cleanFieldInputModal();
}
