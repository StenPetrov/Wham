function stripString(x) {
    return x.replace(/^\s+|\s+$/gm, "");
}

function addToTable() {
    var tableName = document.getElementById("newTableName").value;
    var isVisible = document.getElementById("newTableVisibility").checked;
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

        var listString = "<li class=\"collection-item\" id=\"" + tableName + "\""
            + "_tableID onclick=\"tableSelected('" + tableName + "')\">";
        listString += tableName;
        listString += "</li>";
        $(listString).appendTo("#tableListing");

        var x = document.getElementById("refConnection");
        var option = document.createElement("option");
        option.value = tableName;
        option.text = tableName;
        var sel = x.options[x.length];
        x.add(option, sel);
        console.log(x);
        console.log(option);
        $("select").material_select();
    }
    else {
        Materialize.toast("Please add a table name", 4000);
    }

    // reset footer-modal for next table addition
    document.getElementById("newTableName").value = "";
    document.getElementById("newTableVisibility").checked = "on";
}

function cleanFieldListing() {
    // delete all elements currently listed in previous table
    var root = document.getElementById("fieldListing");
    while (root.hasChildNodes()) {
        root.removeChild(root.childNodes[0]);
    }
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
    document.getElementById("addFieldButton").className = "waves-effect waves-light btn modal-trigger";

    // change table name in title of next row
    document.getElementById("tableInDisplay").innerHTML = "Table: " + tableName;

    for (var i = 0; i < listOfTables.length; i++) {
        //console.log((listOfTables[i].TableName + "_tableID"))
        document.getElementById(listOfTables[i].TableName + "_tableID").className = "collection-item";
    }
    document.getElementById(tableName + "_tableID").className += " active";

    cleanFieldListing();
    populateFieldListing(tableName);
}

function populateWithNewTable(fields) // fields is an array of objects
{
    for (var i = 0; i < fields.length; i += 1) {
        var field = fields[i];
        appendToList(field);
    }
}

function appendToList(field) // field is an object
{
    var listString = "";
    listString += "<li class='collection-item'>";
    listString += "<div class='row'>";
    switch (field.type) {
        case "string":
            listString += "<div class='col s12 m6 l3'>";
            listString += "Name: " + field.name;
            listString += "</div>";
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
            listString += "Name: " + field.name;
            listString += "</div>";
            listString += "<div class='col s12 m6 l3'>";
            listString += "ref table: " + field.refTable;
            listString += "</div>";
            break;

        case "DateTime":
            listString += "<div class='col s12 m6 l3'>";
            listString += "Name: " + field.name;
            listString += "</div>";
            listString += "<div class='col s12 m6 l3'>";
            listString += "UI Type: " + field.uiType;
            listString += "</div>";
            break;

        case "int":
            listString += "<div class='col s12 m6 l3'>";
            listString += "Name: " + field.name;
            listString += "</div>";
            listString += "<div class='col s12 m6 l3'>";
            listString += "UI Type: " + field.uiType;
            listString += "</div>";

        case "double":
            listString += "<div class='col s12 m6 l3'>";
            listString += "Name: " + field.name;
            listString += "</div>";
            listString += "<div class='col s12 m6 l3'>";
            listString += "UI Type: " + field.uiType;
            listString += "</div>";
            break;
    }
    listString += "</div>";
    listString += "</li>";
    $(listString).appendTo("#fieldListing");
}


function typeChanged() {
    console.log("type changed");
    var type = document.getElementById("fieldType").value;
    console.log(type);
    var formsToShow = "";
    switch (type) {
        case "String":
            document.getElementById("div_isAuth").style.display = "block";
            document.getElementById("div_field_name").style.display = "block";
            document.getElementById("div_defaultValue").style.display = "block";
            document.getElementById("div_regex").style.display = "block";
            document.getElementById("div_UI_Type").style.display = "block";
            document.getElementById("div_refConnection").style.display = "none";
            break;
        case "Ref":
            document.getElementById("div_isAuth").style.display = "none";
            document.getElementById("div_field_name").style.display = "block";
            document.getElementById("div_defaultValue").style.display = "none";
            document.getElementById("div_regex").style.display = "none";
            document.getElementById("div_UI_Type").style.display = "none";
            document.getElementById("div_refConnection").style.display = "block";
            break;
        case "DateTime":
            document.getElementById("div_isAuth").style.display = "block";
            document.getElementById("div_field_name").style.display = "block";
            document.getElementById("div_defaultValue").style.display = "none";
            document.getElementById("div_regex").style.display = "none"; // அபிஷேக்
            document.getElementById("div_UI_Type").style.display = "block";
            document.getElementById("div_refConnection").style.display = "none";
            break;
        case "Integer":
            document.getElementById("div_isAuth").style.display = "block";
            document.getElementById("div_field_name").style.display = "block";
            document.getElementById("div_defaultValue").style.display = "block";
            document.getElementById("div_regex").style.display = "none";
            document.getElementById("div_UI_Type").style.display = "block";
            document.getElementById("div_refConnection").style.display = "none";
            break;
    }
    $(formsToShow).appendTo("#inputForms");
}

function cleanFieldInputModal() {
    // post processing - clean up functions
    document.getElementById("div_isAuth").style.display = "none";
    document.getElementById("div_field_name").style.display = "none";
    document.getElementById("div_defaultValue").style.display = "none";
    document.getElementById("div_regex").style.display = "none";
    document.getElementById("div_UI_Type").style.display = "none";
    document.getElementById("div_refConnection").style.display = "none";
}

function addThisField() {
    var type = document.getElementById("fieldType").value;
    var fieldName = document.getElementById("field_name").value;
    var isAuth = document.getElementById("isAuth").value;
    var uiType = document.getElementById("UI_Type").value;
    var defaultValue = document.getElementById("defaultValue").value;
    var isCollection = document.getElementById("field_isCollection").value;
    var regex = document.getElementById("regex").value;
    var refC = document.getElementById("refConnections");
    var refConnection = "";
    for (var i = 0; i < refC.options.length; i++) {
        if (refC.options[i].selected == true) {
            refConnection = refC.options[i].value;
            break;
        }
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
            rowData.isAuth = isAuth;
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
            Materialize.toast("No field was added", 4000);
            cleanFieldInputModal();
            return;
    }
    var tableName = document.getElementById("tableInDisplay").innerHTML;
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
