var appJson = {}

function submitClicked()
{
  appJson.AppOptions = getAppOptions();
  appJson.Owner = getOwner();
  appJson.Database = getDB();
  appJson.Authentication = getAuth();
  appJson.DataModel = getDataModel();
  console.log(JSON.stringify(appJson, null, 4))
}

function getAppOptions()
{
  var AppOptions = {}

  var AppName = document.getElementById("appName").value;
  AppOptions.AppName = AppName;

  var Platform = document.getElementById("targetPlatform").value;
  AppOptions.Platform = Platform;

  var Theme = document.getElementById("theme").value;
  AppOptions.Theme = Theme;

  return AppOptions;
}

function getOwner()
{
  var Owner = {};

  var firstName = document.getElementById("firstName").value;
  var lastName = document.getElementById("lastName").value;
  Owner.Name = firstName + " " + lastName;

  var Company = document.getElementById("company").value;
  Owner.Company = Company;

  var email = document.getElementById("email").value;
  Owner.email = email;

  return Owner;
}

function getDB()
{
  var Database = {};

  var type = document.getElementById("db_type").value;
  Database.Type = type;

  var db_url = document.getElementById("db_url").value;
  Database.ConnectionString = db_url;

  return Database;
}

function getAuth()
{
  var Auth = {};

  var auth_type = document.getElementById("auth_type").value;
  Auth.Type = auth_type;

  var auth_id = document.getElementById("auth_id").value;
  Auth.ClientId = auth_id;

  return Auth;
}

function getDataModel()
{
  return listOfTables;
}
