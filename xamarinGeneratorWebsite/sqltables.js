function stripString(x)
{
    return x.replace(/^\s+|\s+$/gm,'');
}

function addToTable()
{
  var TableName = document.getElementById("newTableName").value;
  var isVisible = document.getElementById("newTableVisibility").checked;
  TableName = stripString(TableName)
  TableName = TableName.replace(" ", "_");
  console.log(TableName, isVisible);
  if(TableName != "")
  {
    var table = {}
    table.TableName = TableName;
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
    table.fields[1].refList = ["singleTable"]

    table.fields[2] = {}
    table.fields[2].type = "Ref"
    table.fields[2].name = "Name of long ref"
    table.fields[2].refList = ["firstTable", "secondTable", "ThirdTable"]

    table.fields[3] = {}
    table.fields[3].type = "DateTime"
    table.fields[3].name = "Name of Date Time Entry"
    table.fields[3].uiType = "label"

    table.fields[4] = {}
    table.fields[4].type = "Integer"
    table.fields[4].name = "Name of Integer Entry"
    table.fields[4].uiType = "textbox"
    // TEMP */

    listOfTables.push( table );

    var listString = "<li class=\"collection-item\" id="+ TableName +"_tableID onclick=\"tableSelected('" + TableName + "')\">";
    listString += TableName;
    listString += "</li>";
    $(listString).appendTo("#tableListing");

    var x = document.getElementById("refConnections");
    var option = document.createElement("option");
    option.value= TableName;
    option.text = TableName;
    var sel = x.options[x.length];
    x.add(option, sel);
    console.log(x)
    console.log(option)
    $('select').material_select();

  }
  else
  {
    Materialize.toast('Please add a table name', 4000);
  }

  // reset footer-modal for next table addition
  document.getElementById("newTableName").value = "";
  document.getElementById("newTableVisibility").checked = "on";
}

function cleanFieldListing()
{
  // delete all elements currently listed in previous table
  var root=document.getElementById('fieldListing');
  while(root.hasChildNodes())
  {
    root.removeChild(root.childNodes[0])
  }
}

function populateFieldListing(TableName)
{
  var myTable = {}
  for(var i = 0; i < listOfTables.length; i+=1)
  {
    if(listOfTables[i].TableName == TableName)
    {
      myTable = listOfTables[i];
    }
  }
  populateWithNewTable(myTable.fields);
}

function tableSelected(TableName)
{
  console.log(TableName + " was selected");
  console.log(listOfTables)

  // removing the disabled part of the button
  document.getElementById('addFieldButton').className = "waves-effect waves-light btn modal-trigger";

  // change table name in title of next row
  document.getElementById('tableInDisplay').innerHTML = "Table: " + TableName;

  for (var i = 0; i < listOfTables.length; i++)
  {
    //console.log((listOfTables[i].TableName + "_tableID"))
    document.getElementById(listOfTables[i].TableName + "_tableID").className = "collection-item";
  }
  document.getElementById(TableName + "_tableID").className += " active";

  cleanFieldListing();
  populateFieldListing(TableName);
}

function populateWithNewTable(fields) // fields is an array of objects
{
  for(var i = 0; i < fields.length; i+=1)
  {
    var field = fields[i];
    appendToList(field);
  }
}

function appendToList(field) // field is an object
{
  var listString = "";
  listString += "<li class=\"collection-item\">"
  listString += '<div class="row">'

  switch(field.type)
  {
    case "String":
      listString += '<div class="col s12 m6 l3">'
      listString += 'Name: ' + field.name;
      listString += '</div>'
      listString += '<div class="col s12 m6 l3">'
      listString += 'UI Type: ' + field.uiType;
      listString += '</div>'
      listString += '<div class="col s12 m6 l3">'
      listString += 'Default Value: ' + field.defaultValue;
      listString += '</div>'
      listString += '<div class="col s12 m6 l3">'
      listString += 'Regex: ' + field.regex;
      listString += '</div>'
      listString += '<div class="col s12 m6 l3">'
      listString += 'isAuth: ' + field.isAuth;
      listString += '</div>'
      break;

    case "Ref":
      listString += '<div class="col s12 m6 l3">'
      listString += 'Name: ' + field.name;
      listString += '</div>'
      listString += '<div class="col s12 m6 l3">'
      listString += 'ref list: ' + field.refList;
      listString += '</div>'
      break;

    case "DateTime":
        listString += '<div class="col s12 m6 l3">'
        listString += 'Name: ' + field.name;
        listString += '</div>'
        listString += '<div class="col s12 m6 l3">'
        listString += 'UI Type: ' + field.uiType;
        listString += '</div>'
      break;

    case "Integer":
        listString += '<div class="col s12 m6 l3">'
        listString += 'Name: ' + field.name;
        listString += '</div>'
        listString += '<div class="col s12 m6 l3">'
        listString += 'UI Type: ' + field.uiType;
        listString += '</div>'
      break;
  }
  listString += '</div>'
  listString += "</li>"

  $(listString).appendTo("#fieldListing");
}


function typeChanged()
{
  console.log('type changed')
  var type = document.getElementById('fieldType').value
  console.log(type)

  var formsToShow = "";
  switch(type)
  {
    case "String":
      document.getElementById('div_isAuth').style.display = 'block'
      document.getElementById('div_field_name').style.display = 'block'
      document.getElementById('div_defaultValue').style.display = 'block'
      document.getElementById('div_regex').style.display = 'block'
      document.getElementById('div_UI_Type').style.display = 'block'
      document.getElementById('div_refConnections').style.display = 'none'
      break;
    case "Ref":
      document.getElementById('div_isAuth').style.display = 'none'
      document.getElementById('div_field_name').style.display = 'block'
      document.getElementById('div_defaultValue').style.display = 'none'
      document.getElementById('div_regex').style.display = 'none'
      document.getElementById('div_UI_Type').style.display = 'none'
      document.getElementById('div_refConnections').style.display = 'block'
      break;
    case "DateTime":
      document.getElementById('div_isAuth').style.display = 'block'
      document.getElementById('div_field_name').style.display = 'block'
      document.getElementById('div_defaultValue').style.display = 'none'
      document.getElementById('div_regex').style.display = 'none' // அபிஷேக்
      document.getElementById('div_UI_Type').style.display = 'block'
      document.getElementById('div_refConnections').style.display = 'none'
      break;
    case "Integer":
      document.getElementById('div_isAuth').style.display = 'block'
      document.getElementById('div_field_name').style.display = 'block'
      document.getElementById('div_defaultValue').style.display = 'block'
      document.getElementById('div_regex').style.display = 'none'
      document.getElementById('div_UI_Type').style.display = 'block'
      document.getElementById('div_refConnections').style.display = 'none'
      break;
  }
  $(formsToShow).appendTo("#inputForms");
}

function cleanFieldInputModal()
{
  // post processing - clean up functions
  document.getElementById('div_isAuth').style.display = 'none'
  document.getElementById('div_field_name').style.display = 'none'
  document.getElementById('div_defaultValue').style.display = 'none'
  document.getElementById('div_regex').style.display = 'none'
  document.getElementById('div_UI_Type').style.display = 'none'
  document.getElementById('div_refConnections').style.display = 'none'
}

function addThisField()
{
  var type = document.getElementById('fieldType').value

  var fieldName = document.getElementById('field_name').value
  var isAuth = document.getElementById('isAuth').value
  var UI_Type = document.getElementById('UI_Type').value
  var defaultValue = document.getElementById('defaultValue').value
  var regex = document.getElementById('regex').value

  var refC = document.getElementById("refConnections");
  var refConnections = []
  for (var i = 0; i < refC.options.length; i++) {
     if(refC.options[i].selected ==true){
          refConnections.push(refC.options[i].value);
      }
  }


  console.log(typeof refConnections)

  var rowData = {}

  switch(type)
  {
    case "String":
      // TODO validation
      rowData.type = type
      rowData.name = fieldName
      rowData.uiType = UI_Type
      rowData.defaultValue = defaultValue
      rowData.regex = regex
      rowData.isAuth = isAuth
      break;
    case "Ref":
      // TODO validation
      rowData.type = type
      rowData.name = fieldName
      rowData.refList = refConnections
      break;
    case "DateTime":
      // TODO validation
      rowData.type = type
      rowData.name = fieldName
      rowData.uiType = UI_Type
      rowData.isAuth = isAuth
      break;
    case "Integer":
      // TODO validation
      rowData.type = type
      rowData.name = fieldName
      rowData.uiType = UI_Type
      rowData.defaultValue = defaultValue
      rowData.isAuth = isAuth
      break;
    default:
      Materialize.toast('No field was added', 4000);
      cleanFieldInputModal()
      return;
  }
  var TableName = document.getElementById('tableInDisplay').innerHTML
  if(TableName.includes("Table:"))
  {
    TableName = TableName.split(" ")[1]
    console.log(TableName)
    for(var i = 0; i < listOfTables.length; i+=1)
    {
      if(listOfTables[i].TableName == TableName)
      {
        listOfTables[i].fields.push(rowData)
        cleanFieldListing(TableName)
        populateFieldListing(TableName)
      }
    }
  }
  cleanFieldInputModal();
}
