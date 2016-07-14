
// item format: ("string sent in json", "string shown to user", "selected", "disabled")
var optionList_targetPlatform = [
  [".Net", ".NET/Xamarin", 1, 0],
  ["Cordova", "nodejs/Cordova", 0, 1], 
  ["Android", "Android", 0, 1]
];

var optionList_theme = [
   ["Basic","Basic Theme", 1, 0],
   ["Stylish", "Stylish Theme", 0, 1]
];

var optionList_db = [
  ["","No Database", 1, 0]
  ["SQL","SQL Database", 1, 0]
];

var optionList_auth = [
  ["","No Authentication", 1, 0]
  ["AzureAD","AzureAD Authentication", 1, 0]
  ["Social","Social Authentication (Google, FB, Twitter)", 1, 0]
];

var optionList_company = [
  ["Microsoft", "Microsoft", 1, 0],
  ["Other", "Other", 0, 0]
];

var listOfTables = []

var types = {};
types.str = {}
types.int = {}
types.ref = {}
types.datetime = {}
types.money = {}


generateTargetPlatformOptions()
generateThemeOptions()
generateDatabaseOptions()
generateAuthenticationOptions()
generateCompanyOptions()

function generateCompanyOptions()
{
  var x = document.getElementById("company");
  for(var i = 0; i < optionList_company.length; i+=1)
  {
    var option = document.createElement("option");
    option.value=optionList_company[i][0];
    option.text = optionList_company[i][1];
    option.selected = optionList_company[i][2] & !optionList_company[i][3];
    option.disabled = optionList_company[i][3];
    var sel = x.options[x.length];
    x.add(option, sel);
  }
}

function generateTargetPlatformOptions()
{
  var x = document.getElementById("targetPlatform");
  for(var i = 0; i < optionList_targetPlatform.length; i+=1)
  {
    var option = document.createElement("option");
    option.value=optionList_targetPlatform[i][0];
    option.text = optionList_targetPlatform[i][1];
    option.selected = optionList_targetPlatform[i][2] & !optionList_targetPlatform[i][3];
    option.disabled = optionList_targetPlatform[i][3];
    var sel = x.options[x.length];
    x.add(option, sel);
  }
}

function generateThemeOptions()
{
  var x = document.getElementById("theme");
  for(var i = 0; i < optionList_theme.length; i+=1)
  {
    var option = document.createElement("option");
    option.value=optionList_theme[i][0];
    option.text = optionList_theme[i][1];
    option.selected = optionList_theme[i][2] & !optionList_theme[i][3];
    option.disabled = optionList_theme[i][3];
    var sel = x.options[x.length];
    x.add(option, sel);
  }
}


function generateDatabaseOptions()
{
  var x = document.getElementById("db_type");
  for(var i = 0; i < optionList_db.length; i+=1)
  {
    var option = document.createElement("option");
    option.value=optionList_db[i][0];
    option.text = optionList_db[i][1];
    option.selected = optionList_db[i][2] & !optionList_db[i][3];
    option.disabled = optionList_db[i][3];
    var sel = x.options[x.length];
    x.add(option, sel);
  }
}

function generateAuthenticationOptions()
{
  var x = document.getElementById("auth_type");
  for(var i = 0; i < optionList_auth.length; i+=1)
  {
    var option = document.createElement("option");
    option.value=optionList_auth[i][0];
    option.text = optionList_auth[i][1];
    option.selected = optionList_auth[i][2] & !optionList_auth[i][3];
    option.disabled = optionList_auth[i][3];
    var sel = x.options[x.length];
    x.add(option, sel);
  }
}
