
// item format: ("string sent in json", "string shown to user", "selected", "disabled")
var optionList_targetPlatform = [
  [".Net", ".NET/Xamarin", 1, 0],
  ["Ionic", "js/Ionic", 0, 1]
];

var optionList_theme = [
   ["Basic", "Basic Theme", 1, 0],
   ["Stylish", "Stylish Theme", 0, 1]
];

var optionList_db = [
  ["", "No Database", 1, 0],
  ["SQL", "SQL Server", 0, 0],
  ["DocumentDB", "DocumentDB", 0, 1]
];

var optionList_auth = [
  ["", "No Authentication", 1, 0],
  ["AzureAD", "AzureAD Authentication", 0, 0],
  ["Social", "Social Authentication (Google, FB, Twitter)", 0, 1]
];

var listOfTables = [];

// init all drop-downs
generateDropdownOptions("targetPlatform", optionList_targetPlatform);
generateDropdownOptions("theme", optionList_theme);
generateDropdownOptions("db_type", optionList_db);
generateDropdownOptions("auth_type", optionList_auth);

function generateDropdownOptions(dropdownId, optionList) {
    var dropDown = document.getElementById(dropdownId);
    for (var i = 0; i < optionList.length; i += 1) {
        var option = document.createElement("option");
        option.value = optionList[i][0];
        option.text = optionList[i][1];
        option.selected = optionList[i][2] & !optionList[i][3];
        option.disabled = optionList[i][3];
        var sel = dropDown.options[dropDown.length];
        dropDown.add(option, sel);
    }
}
