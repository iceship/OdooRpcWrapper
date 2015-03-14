# Odoo - C# API
===============
This library contains basic code to interact with Odoo from a C# application.


## Examples

Create credential object:

> OdooConnectionCredentials creds = new OdooConnectionCredentials("https://your.instance", "database", "admin", "password");

Log in:

>  OdooAPI api = new OdooAPI(creds);

## Initialize model

Define what model you want to use:

>  OdooModel productModel = api.GetModel("product.template");

Define what fields to fetch by default:

>  productModel.AddField("name");
>  productModel.AddField("default_code");

## Create

Create new objects by calling the model. New objects need to be saved.

>  OdooRecord record = productModel.CreateNew();

>  record.SetValue("name", "my new product!")

>  record.Save()

## Search

>  object[] filter = new object[1];

>  filter[0] = new object[3] { "name", "my new product", "" };

>  List<OdooRecord> records = productModel.Search(filter);

>  foreach(OdooRecord record in records)

>  {

>      Console.WriteLine(String.Format("[{0}] {1}", record.GetValue("default_code"), record.GetValue("name")));

>  }

## Modify

Modify C# objects, and call 'Save'

>  OdooRecord record = records[0]

>  record.SetValue("name", "my old product")

>  record.Save()